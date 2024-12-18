using MySql.Data.MySqlClient;
using Backend.Models;
using System.Data;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;

namespace Backend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductAsync()
    {
        var query = @"
        SELECT 
            p.id AS product_id, 
            p.name, 
            p.price, 
            p.description, 
            p.category_id, 
            p.is_deleted, 
            p.date_added, 
            s.id AS supplier_id, 
            s.name AS supplier_name
        FROM 
            products p
        LEFT JOIN 
            product_suppliers ps ON p.id = ps.product_id
        LEFT JOIN 
            suppliers s ON ps.supplier_id = s.id
        WHERE 
            p.is_deleted = FALSE";

        var productDict = new Dictionary<int, ProductResponse>();

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var productId = reader.GetInt32("product_id");

                if (!productDict.ContainsKey(productId))
                {
                    productDict[productId] = new ProductResponse
                    {
                        Id = productId,
                        Name = reader.GetString("name"),
                        Price = reader.GetDecimal("price"),
                        Description = reader.GetString("description"),
                        CategoryId = reader.GetInt32("category_id"),
                        IsDeleted = reader.GetBoolean("is_deleted"),
                        DateAdded = reader.GetDateTime("date_added"),
                        SupplierId = new List<int>()
                    };
                }

                if (!reader.IsDBNull(reader.GetOrdinal("supplier_id")))
                {
                    productDict[productId].SupplierId.Add(reader.GetInt32("supplier_id"));
                }
            }

            return productDict.Values;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar os produtos: {ex.Message}");
            return Enumerable.Empty<ProductResponse>();
        }

    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        var query = @"
        SELECT 
            p.id, 
            p.name, 
            p.price, 
            p.description, 
            p.category_id, 
            p.is_deleted, 
            p.date_added, 
            s.id AS supplier_id
        FROM 
            products p
        LEFT JOIN 
            product_suppliers ps ON p.id = ps.product_id
        LEFT JOIN 
            suppliers s ON ps.supplier_id = s.id
        WHERE 
            p.id = @id AND p.is_deleted = FALSE";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();

            ProductResponse? product = null;
            var suppliers = new List<int>();

            while (await reader.ReadAsync())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("supplier_id")))
                {
                    suppliers.Add(reader.GetInt32("supplier_id"));
                }

                if (product == null)
                {
                    product = new ProductResponse
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Price = reader.GetDecimal("price"),
                        Description = reader.GetString("description"),
                        CategoryId = reader.GetInt32("category_id"),
                        IsDeleted = reader.GetBoolean("is_deleted"),
                        DateAdded = reader.GetDateTime("date_added"),
                        SupplierId = suppliers
                    };
                }
            }

            return product;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar o produto: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> AddProductAsync(ProductCreate product)
    {
        var productInsertQuery = @"
        INSERT INTO products (name, description, price, category_id)
        VALUES (@name, @description, @price, @categoryId);
        SELECT LAST_INSERT_ID();";

        var productSupplierInsertQuery = @"
        INSERT INTO product_suppliers (product_id, supplier_id)
        VALUES (@productId, @supplierId);";

        int productId;

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await using var productCommand = new MySqlCommand(productInsertQuery, connection, transaction);
            productCommand.Parameters.AddWithValue("@name", product.Name);
            productCommand.Parameters.AddWithValue("@description", product.Description);
            productCommand.Parameters.AddWithValue("@price", product.Price);
            productCommand.Parameters.AddWithValue("@categoryId", product.CategoryId);

            productId = Convert.ToInt32(await productCommand.ExecuteScalarAsync());

            if (product.SupplierIds != null && product.SupplierIds.Any())
            {
                foreach (var supplierId in product.SupplierIds)
                {
                    await using var supplierCommand = new MySqlCommand(productSupplierInsertQuery, connection, transaction);
                    supplierCommand.Parameters.AddWithValue("@productId", productId);
                    supplierCommand.Parameters.AddWithValue("@supplierId", supplierId);
                    await supplierCommand.ExecuteNonQueryAsync();
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return productId;
    }

    public async Task UpdateProductAsync(int id, ProductUpdate product)
    {
        var updateProductQuery = @"
        UPDATE products 
        SET name = @name, price = @price, description = @description, category_id = @category_id 
        WHERE id = @id AND is_deleted = FALSE";

        var getExistingSuppliersQuery = @"
        SELECT supplier_id 
        FROM product_suppliers 
        WHERE product_id = @productId";

        var insertProductSupplierQuery = @"
        INSERT INTO product_suppliers (product_id, supplier_id) 
        VALUES (@productId, @supplierId)";

        var deleteProductSupplierQuery = @"
        DELETE FROM product_suppliers 
        WHERE product_id = @productId AND supplier_id = @supplierId";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await using var updateProductCommand = new MySqlCommand(updateProductQuery, connection, transaction);
                updateProductCommand.Parameters.AddWithValue("@name", product.Name);
                updateProductCommand.Parameters.AddWithValue("@price", product.Price);
                updateProductCommand.Parameters.AddWithValue("@description", product.Description);
                updateProductCommand.Parameters.AddWithValue("@category_id", product.CategoryId);
                updateProductCommand.Parameters.AddWithValue("@id", id);
                await updateProductCommand.ExecuteNonQueryAsync();

                var existingSupplierIds = new HashSet<int>();
                await using (var getExistingSuppliersCommand = new MySqlCommand(getExistingSuppliersQuery, connection, transaction))
                {
                    getExistingSuppliersCommand.Parameters.AddWithValue("@productId", id);
                    await using var reader = await getExistingSuppliersCommand.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        existingSupplierIds.Add(reader.GetInt32("supplier_id"));
                    }
                }

                var newSupplierIds = product.SupplierIds ?? new List<int>();
                var suppliersToAdd = newSupplierIds.Except(existingSupplierIds);
                var suppliersToRemove = existingSupplierIds.Except(newSupplierIds);

                foreach (var supplierId in suppliersToAdd)
                {
                    await using var insertCommand = new MySqlCommand(insertProductSupplierQuery, connection, transaction);
                    insertCommand.Parameters.AddWithValue("@productId", id);
                    insertCommand.Parameters.AddWithValue("@supplierId", supplierId);
                    await insertCommand.ExecuteNonQueryAsync();
                }

                foreach (var supplierId in suppliersToRemove)
                {
                    await using var deleteCommand = new MySqlCommand(deleteProductSupplierQuery, connection, transaction);
                    deleteCommand.Parameters.AddWithValue("@productId", id);
                    deleteCommand.Parameters.AddWithValue("@supplierId", supplierId);
                    await deleteCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao atualizar o produto: {ex.Message}");
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
