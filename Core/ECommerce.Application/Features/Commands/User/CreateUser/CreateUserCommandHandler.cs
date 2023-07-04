using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.User> _userManager;
        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new Domain.Entities.Identity.User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.NameSurname
            }, request.Password);

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };
            
            if (result.Succeeded)
                response.Message = "User created successfully";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}";
            return response;
        }
    }
}
