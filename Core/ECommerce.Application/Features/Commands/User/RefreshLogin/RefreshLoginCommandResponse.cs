using ECommerce.Application.DTOs.Tokens;

namespace ECommerce.Application.Features.Commands.User.RefreshLogin;

public class RefreshLoginCommandResponse
{
    public Token Token { get; set; }
}