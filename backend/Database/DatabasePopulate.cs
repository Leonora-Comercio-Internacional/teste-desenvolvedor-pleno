using MySql.Data.MySqlClient;

namespace backend.Database;

public class DatabasePopulate
{
    private readonly string _connectionString;

    public DatabasePopulate(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Populating()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            if (IsTablePopulate(connection, "categories") && IsTablePopulate(connection, "suppliers"))
            {
                Console.WriteLine("Os dados iniciais já foram populados.");
                return;
            }

            var insertCategories = @"
                    INSERT INTO categories (name, description, date_added) VALUES
                    ('Eletrônicos', 'Produtos como celulares, video games, TVs e computadores', NOW()),
                    ('Móveis', 'Mesas, cadeiras e armários', NOW()),
                    ('Alimentos', 'Produtos alimentícios e bebidas', NOW()),
                    ('Roupas', 'Vestuário em geral', NOW()),
                    ('Eletrodomésticos', 'Geladeiras, fogões e máquinas de lavar', NOW())
                    ON DUPLICATE KEY UPDATE name = name;";

            var insertSuppliers = @"
                    INSERT INTO suppliers (name, cnpj, telephone, address, date_added) VALUES
                    ('Fornecedor A', '12.345.678/0001-90', '1234-5678', 'Rua Exemplo, 123', NOW()),
                    ('Fornecedor B', '98.765.432/0001-21', '9876-5432', 'Avenida Exemplo, 456', NOW()),
                    ('Fornecedor C', '55.444.333/0001-12', '5544-3321', 'Praça Exemplo, 789', NOW())
                    ON DUPLICATE KEY UPDATE name = name;";
            
            var password = BCrypt.Net.BCrypt.HashPassword("leonora");

            var insertUser = $@"
                    INSERT INTO users (username, password) VALUES
                    ('Grupo Leonora', '{password}')
                    ON DUPLICATE KEY UPDATE username = username;";

            ExecuteCommand(connection, insertCategories);
            ExecuteCommand(connection, insertSuppliers);
            ExecuteCommand(connection, insertUser);

            Console.WriteLine("Dados iniciais populados com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao inserir os dados iniciais: {ex.Message}");
            throw;
        }
    }

    private static bool IsTablePopulate(MySqlConnection connection, string tableName)
    {
        var query = $"SELECT COUNT(*) FROM {tableName}";
        try
        {
            using var command = new MySqlCommand(query, connection);

            return (long)command.ExecuteScalar() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao verificar se a tabela '{tableName}' está preenchida: {ex.Message}");
            throw;
        }
    }

    private static void ExecuteCommand(MySqlConnection connection, string sql)
    {
        try
        {
            using var command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao executar o comando SQL: {ex.Message}");
            throw;
        }
    }
}
