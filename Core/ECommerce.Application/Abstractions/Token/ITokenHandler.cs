namespace ECommerce.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Tokens.Token CreateAccessToken(int minute);
    }
}
