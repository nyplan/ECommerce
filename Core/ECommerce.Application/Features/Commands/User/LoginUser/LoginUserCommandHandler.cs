using ECommerce.Application.Abstractions.Services.Authentications;
using MediatR;
using N = ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IInternalAuthentication _internalAuthentication;

        public LoginUserCommandHandler(IInternalAuthentication internalAuthentication)
        {
            _internalAuthentication = internalAuthentication;
        }
        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _internalAuthentication.LoginAsync(request.UsernameOrEmail, request.Password, 15);
            return new LoginUserSuccessCommandResponse() { Token = token };
        }
    }
}
