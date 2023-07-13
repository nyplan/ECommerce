namespace ECommerce.Application.Abstractions.Services.Authentications;

public interface IInternalAuthentication
{
    Task<DTOs.Tokens.Token> LoginAsync(string usernameOrEmail, string password, int lifetime);
    Task<DTOs.Tokens.Token> RefreshLoginAsync(string refreshToken);
}