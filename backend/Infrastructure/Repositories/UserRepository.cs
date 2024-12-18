using MySql.Data.MySqlClient;
using Backend.Models;
using System.Data;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;

namespace Backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Register(UserRequest user)
    {
        var query = "INSERT INTO users (username, password) VALUES (@username, @password)";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);

            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Houve um erro ao registrar o usuário.");
        }
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT * FROM users WHERE username = @username LIMIT 1";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", username);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                Username = reader.GetString("username"),
                Password = reader.GetString("password"),
                DateAdded = reader.GetDateTime("date_added")
            };
        }

        return null;
    }
}
