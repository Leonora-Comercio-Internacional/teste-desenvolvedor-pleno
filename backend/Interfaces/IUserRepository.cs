using backend.DTOs;
using backend.Models;

namespace backend.Interfaces;

public interface IUserRepository
{
    Task Register(UserRequest user);
    Task<User?> GetUserByUsernameAsync(string username);
}
