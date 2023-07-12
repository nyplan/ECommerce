namespace ECommerce.Application.Abstractions.Services.Authentications;

public interface IExternalAuthentication
{
    Task<DTOs.Tokens.Token> GoogleLoginAsync(string idToken, int lifetime);
    Task<DTOs.Tokens.Token> FacebookLoginAsync(string authToken, int lifetime);
}