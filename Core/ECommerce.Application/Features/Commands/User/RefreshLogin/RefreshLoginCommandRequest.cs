using MediatR;

namespace ECommerce.Application.Features.Commands.User.RefreshLogin;

public class RefreshLoginCommandRequest : IRequest<RefreshLoginCommandResponse>
{
    public string RefreshToken { get; set; }
}