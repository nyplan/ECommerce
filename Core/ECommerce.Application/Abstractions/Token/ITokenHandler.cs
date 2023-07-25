using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Tokens.Token CreateAccessToken(int minute, User user);
        string CreateRefreshToken(DateTime accessTokenExpires);
    }
}
