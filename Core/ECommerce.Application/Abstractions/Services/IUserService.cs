using ECommerce.Application.DTOs.User;
using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
    Task UpdateRefreshToken(string refreshToken, User user, DateTime accessTokenExpires, int refreshTokenLifetime);
}