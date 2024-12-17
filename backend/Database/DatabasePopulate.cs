using MySql.Data.MySqlClient;

namespace backend.Database;

public class DatabasePopulate
{
    private readonly string _connectionString;
    private readonly ILogger<DatabasePopulate> _logger;

    public DatabasePopulate(string connectionString, ILogger<DatabasePopulate> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public void Populating()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);

            connection.Open();

            var insertCategories = @"
                    INSERT INTO categories (name, description, date_added) VALUES
                    ('Eletrônicos', 'Produtos como celulares, video games, TVs e computadores', NOW()),
                    ('Móveis', 'Mesas, cadeiras e armários', NOW()),
                    ('Alimentos', 'Produtos alimentícios e bebidas', NOW()),
                    ('Roupas', 'Vestuário em geral', NOW()),
                    ('eletrodomésticos', 'Geladeiras, fogões e máquinas de lavar', NOW())
                    ON DUPLICATE KEY UPDATE name = name;";

            var insertSuppliers = @"
                    INSERT INTO suppliers (name, cnpj, telephone, address, date_added) VALUES
                    ('Fornecedor A', '12.345.678/0001-90', '1234-5678', 'Rua Exemplo, 123', NOW()),
                    ('Fornecedor B', '98.765.432/0001-21', '9876-5432', 'Avenida Exemplo, 456', NOW()),
                    ('Fornecedor C', '55.444.333/0001-12', '5544-3321', 'Praça Exemplo, 789', NOW())
                    ON DUPLICATE KEY UPDATE name = name;";

            ExecuteCommand(connection, insertCategories);

            ExecuteCommand(connection, insertSuppliers);

            _logger.LogInformation("Dados iniciais populados com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Houve um erro ao inserir os dados iniciais: {ex.Message}");
            throw;
        }
    }

    private void ExecuteCommand(MySqlConnection connection, string sql)
    {
        try
        {
            using var command = new MySqlCommand(sql, connection);

            command.ExecuteNonQuery();
        } catch (Exception ex)
        {
            _logger.LogError($"Houve um erro ao executar o comando SQL: {ex.Message}");
            throw;
        }
    }
}
