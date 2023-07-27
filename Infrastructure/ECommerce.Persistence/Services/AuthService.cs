using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs.Facebook;
using ECommerce.Application.DTOs.Tokens;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserService _userService;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        UserManager<User> userManager, ITokenHandler tokenHandler, SignInManager<User> signInManager, IUserService userService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
        _httpClient = httpClientFactory.CreateClient();
    }
    private async Task<Token> CreateUserExternalAsync(User? user, string email, string name, UserLoginInfo info, int lifetime)
    {
        bool result = user != null;
        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = email,
                    NameSurname = name
                };
                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }

        if (!result) throw new Exception("Invalid external authentication");
        await _userManager.AddLoginAsync(user, info);
        Token token = _tokenHandler.CreateAccessToken(lifetime, user);
        await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
        return token;
    }

    public async Task<Token> FacebookLoginAsync(string authToken, int lifetime)
    {
        string accessToken = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:ClientID"]}&client_secret={_configuration["Facebook:ClientSecret"]}&grant_type=client_credentials");

        FacebookTokenResponse? tokenResponse = JsonSerializer.Deserialize<FacebookTokenResponse>(accessToken);

        string userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={tokenResponse?.AccessToken}");

        FacebookUserValidation? validation =
            JsonSerializer.Deserialize<FacebookUserValidation>(userAccessTokenValidation);

        if (validation?.Data.IsValid == null) throw new Exception("Invalid external authentication");
        string userInfoResponse =
            await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
        FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

        var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
        User? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        return await CreateUserExternalAsync(user, userInfo!.Email, userInfo.Name, info, lifetime);
    }

    public async Task<Token> LoginAsync(string usernameOrEmail, string password, int lifetime)
    {
        User? user =
            (await _userManager.FindByNameAsync(usernameOrEmail) ??
             await _userManager.FindByEmailAsync(usernameOrEmail)) ?? throw new NotFoundUserException();

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) throw new AuthenticationException();
        Token token = _tokenHandler.CreateAccessToken(15, user);
        await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
        return token;
    }

    public async Task<Token> RefreshLoginAsync(string refreshToken)
    {
        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        
        if (user == null || user?.RefreshTokenExpires < DateTime.UtcNow)
            throw new NotFoundUserException();
        
        Token token = _tokenHandler.CreateAccessToken(15, user!);
        await _userService.UpdateRefreshToken(token.RefreshToken, user!, token.Expiration, 15);
        return token;
    }

    public async Task<Token> GoogleLoginAsync(string idToken, int lifetime)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>
                { "bu frontdan gelir gencayda, sen bunun backda servisin yazmalisan - Video 43 - appsettingse yaz" }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

        User? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, lifetime);
    }
    
    public string DecodeRefreshToken(string encodedToken, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            tokenHandler.ValidateToken(encodedToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var refreshToken = jwtToken.Claims.First(x => x.Type == "refreshToken").Value;

            return refreshToken;
        }
        catch
        {
            // Token validation failed
            return null!;
        }
    }

}