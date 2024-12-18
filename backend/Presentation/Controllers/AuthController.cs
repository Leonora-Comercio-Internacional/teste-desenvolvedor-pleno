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

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRequest userRegister)
    {
        var user = new UserRequest
        {
            Username = userRegister.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password)
        };

        await _userRepository.Register(user);

        return Ok(new { message = "Usuário registrado com sucesso." });
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] UserRequest request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username ?? "");

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new { message = "Credenciais inválidas." });
        }

        var token = GenerateToken(user.Username ?? "");
        return Ok(new { token });
    }

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