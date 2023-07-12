using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Features.Commands.User.CreateUser;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result = await _userManager.CreateAsync(new User()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Username,
            Email = model.Email,
            NameSurname = model.NameSurname
        }, model.Password);

        CreateUserResponse response = new() { Succeeded = result.Succeeded };
            
        if (result.Succeeded)
            response.Message = "User created successfully";
        else
            foreach (var error in result.Errors)
                response.Message += $"{error.Code} - {error.Description}\n";
        return response;
    }
}