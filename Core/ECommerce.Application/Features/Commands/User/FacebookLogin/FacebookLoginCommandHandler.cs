using System.Text.Json;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs.Facebook;
using ECommerce.Application.DTOs.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using N = ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Features.Commands.User.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    private readonly UserManager<N.User> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly HttpClient _httpClient;
    public FacebookLoginCommandHandler(UserManager<N.User> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
    {
        string accessToken = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/oauth/access_token?client_id=546631843676576&client_secret=d3438100dc962c8f34a765e7d7deef3c&grant_type=client_credentials");
        
        FacebookTokenResponse? tokenResponse = JsonSerializer.Deserialize<FacebookTokenResponse>(accessToken);

        string userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={tokenResponse.AccessToken}");

        FacebookUserValidation? validation =
            JsonSerializer.Deserialize<FacebookUserValidation>(userAccessTokenValidation);

        if (validation.Data.IsValid)
        {
            string userInfoResponse =
                await _httpClient.GetStringAsync(
                    $"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");
            FacebookUserInfoResponse response = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            N.User? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(response.Email);
                if (user is null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = response.Email,
                        UserName = response.Email,
                        NameSurname = response.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                return new() 
                    { Token = _tokenHandler.CreateAccessToken(10) };
            }
        }
        throw new Exception("Invalid external authentication");
    }
}