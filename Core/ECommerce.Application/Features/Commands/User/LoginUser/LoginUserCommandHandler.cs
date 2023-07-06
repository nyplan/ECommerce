using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs.Tokens;
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
        private readonly ITokenHandler _tokenHandler;
        public LoginUserCommandHandler(UserManager<N.User> userManager, SignInManager<N.User> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            N.User? user = (await _userManager.FindByNameAsync(request.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail)) ?? throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(10);
                return new LoginUserSuccessCommandResponse() { Token = token };
            }
            throw new AuthenticationException();
        }
    }
}
