using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userRegister">User details including username and password.</param>
    /// <returns>Returns a success message if the user is registered successfully.</returns>
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] UserRequest userRegister)
    {
        if (string.IsNullOrWhiteSpace(userRegister.Username) || string.IsNullOrWhiteSpace(userRegister.Password))
        {
            return BadRequest(new { message = "Username e password são obrigatórios." });
        }

        if (userRegister.Username.Length < 3 || userRegister.Username.Length > 20)
        {
            return BadRequest(new { message = "O username deve ter entre 3 e 20 caracteres." });
        }

        if (userRegister.Password.Length < 6 || userRegister.Password.Length > 30)
        {
            return BadRequest(new { message = "A senha deve ter entre 6 e 30 caracteres." });
        }

        var existingUser = await _userRepository.GetUserByUsernameAsync(userRegister.Username);

        if (existingUser != null)
        {
            return BadRequest(new { message = "Usuário já registrado." });
        }

        var user = new UserRequest
        {
            Username = userRegister.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password)
        };

        await _userRepository.SignUp(user);

        return Ok(new { message = "Usuário registrado com sucesso." });
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token.
    /// </summary>
    /// <param name="request">User credentials including username and password.</param>
    /// <returns>Returns a JWT token if the authentication is successful.</returns>
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] UserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username e password são obrigatórios." });
        }

        var user = await _userRepository.GetUserByUsernameAsync(request.Username ?? "");

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new { message = "Credenciais inválidas." });
        }

        var token = GenerateToken(user.Username ?? "");
        return Ok(new { token });
    }

    /// <summary>
    /// Generates a JWT token for the authenticated user.
    /// </summary>
    /// <param name="username">The username for which the token is generated.</param>
    /// <returns>A JWT token string.</returns>
    private static string GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}