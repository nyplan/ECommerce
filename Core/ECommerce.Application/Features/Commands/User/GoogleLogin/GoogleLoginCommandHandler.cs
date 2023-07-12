using ECommerce.Application.Abstractions.Services.Authentications;
using MediatR;
using N = ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly IExternalAuthentication _externalAuthentication;

        public GoogleLoginCommandHandler(IExternalAuthentication externalAuthentication)
        {
            _externalAuthentication = externalAuthentication;
        }
        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _externalAuthentication.GoogleLoginAsync(request.IdToken, 15);
            return new() { Token = token };
        }
    }
}
