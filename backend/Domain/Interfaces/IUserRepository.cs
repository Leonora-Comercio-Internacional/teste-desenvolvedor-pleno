using Backend.Application.DTOs;
using Backend.Models;

namespace Backend.Domain.Interfaces;

public interface IUserRepository
{
    Task SignUp(UserRequest user);
    Task<User?> GetUserByUsernameAsync(string username);
}
