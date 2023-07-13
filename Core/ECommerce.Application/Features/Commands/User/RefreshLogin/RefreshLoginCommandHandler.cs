using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Tokens;
using MediatR;

namespace ECommerce.Application.Features.Commands.User.RefreshLogin;

public class RefreshLoginCommandHandler : IRequestHandler<RefreshLoginCommandRequest, RefreshLoginCommandResponse>
{
    private readonly IAuthService _authService;

    public RefreshLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<RefreshLoginCommandResponse> Handle(RefreshLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
        Token token = await _authService.RefreshLoginAsync(request.RefreshToken);
        return new() { Token = token };
    }
}