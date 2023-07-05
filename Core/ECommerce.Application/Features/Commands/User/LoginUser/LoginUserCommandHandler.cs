using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using N = ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly UserManager<N.User> _userManager;
        private readonly SignInManager<N.User> _signInManager;
        public LoginUserCommandHandler(UserManager<N.User> userManager, SignInManager<N.User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            N.User? user = (await _userManager.FindByNameAsync(request.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail)) ?? throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                // Authorization will be here
            }
            return new();
        }
    }
}
