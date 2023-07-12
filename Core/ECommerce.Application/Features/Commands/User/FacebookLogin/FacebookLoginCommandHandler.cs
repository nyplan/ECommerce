using ECommerce.Application.Abstractions.Services.Authentications;
using MediatR;

namespace ECommerce.Application.Features.Commands.User.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    private readonly IExternalAuthentication _externalAuthentication;

    public FacebookLoginCommandHandler(IExternalAuthentication externalAuthentication)
    {
        _externalAuthentication = externalAuthentication;
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _externalAuthentication.FacebookLoginAsync(request.AuthToken, 15);
        return new() { Token = token };
    }
}