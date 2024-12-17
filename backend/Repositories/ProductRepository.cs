using MySql.Data.MySqlClient;
using backend.Interfaces;
using backend.Models;
using System.Data;
using backend.DTOs;

namespace backend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductAsync()
    {
        var query = "SELECT * FROM products";

        var products = new List<ProductResponse>();

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new ProductResponse
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Price = reader.GetDecimal("price"),
                    Description = reader.GetString("description"),
                    CategoryId = reader.GetInt32("category_id"),
                    IsDeleted = reader.GetBoolean("is_deleted"),
                    DateAdded = reader.GetDateTime("date_added")
                });
            }
        }
        catch
        {
            Console.WriteLine("Houve um erro ao buscar os produtos.");
        }

        return products;

    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        var query = "SELECT * FROM products WHERE id = @id AND is_deleted = FALSE";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ProductResponse
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Price = reader.GetDecimal("price"),
                    Description = reader.GetString("description"),
                    CategoryId = reader.GetInt32("category_id"),
                    IsDeleted = reader.GetBoolean("is_deleted"),
                    DateAdded = reader.GetDateTime("date_added")
                };
            }

        }
        catch
        {
            Console.WriteLine("Houve um erro ao buscar o produto.");
        }

        return null!;
    }

    public async Task<int?> AddProductAsync(ProductCreate product)
    {
        var query = @"
            INSERT INTO products (name, price, description, category_id, is_deleted)
            VALUES (@name, @price, @description, @category_id, FALSE);
            SELECT LAST_INSERT_ID()";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@price", product.Price);
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@category_id", product.CategoryId);

            var result = await command.ExecuteScalarAsync();

            if (result != null && int.TryParse(result.ToString(), out int id))
            {
                return id;
            }
        }
        catch
        {
            Console.WriteLine("Houve um erro ao adicionar o produto.");
        }

        return null!;
    }

    public async Task UpdateProductAsync(int id, ProductUpdate product)
    {
        var query = @"
            UPDATE products 
            SET name = @name, price = @price, description = @description, category_id = @category_id 
            WHERE id = @id AND is_deleted = FALSE";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@price", product.Price);
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@category_id", product.CategoryId);
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Houve um erro ao atualizar o produto.");
        }

    }

    public async Task DeleteProductAsync(int id)
    {
        var query = "UPDATE products SET is_deleted = TRUE WHERE id = @id";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();


            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Houve um erro ao deletar o produto.");
        }

    }
}
