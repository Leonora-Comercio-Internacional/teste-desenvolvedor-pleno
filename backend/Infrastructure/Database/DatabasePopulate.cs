using MySql.Data.MySqlClient;

namespace Backend.Database;

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
                    ('Eletrônicos', 'Aparelhos como smartphones, notebooks, tablets e câmeras digitais', NOW()),
                    ('Móveis', 'Móveis para casa e escritório, como sofás, mesas, cadeiras e estantes', NOW()),
                    ('Alimentos', 'Produtos como cereais, laticínios, bebidas e alimentos processados', NOW()),
                    ('Roupas', 'Vestuário masculino, feminino e infantil, incluindo acessórios', NOW()),
                    ('Eletrodomésticos', 'Itens como geladeiras, aspiradores, fornos de micro-ondas e lavadoras', NOW())
                    ON DUPLICATE KEY UPDATE name = name;";

            var insertSuppliers = @"
                    INSERT INTO suppliers (name, cnpj, telephone, address, date_added) VALUES
                    ('Comercial Santos LTDA', '23.456.789/0001-12', '(11) 3124-5678', 'Rua das Flores, 145, São Paulo - SP', NOW()),
                    ('Distribuidora Nova Era', '87.654.321/0001-65', '(21) 9876-5432', 'Avenida Atlântica, 890, Rio de Janeiro - RJ', NOW()),
                    ('Alimentos União S.A.', '45.123.987/0001-34', '(31) 9987-1122', 'Praça Central, 255, Belo Horizonte - MG', NOW()),
                    ('Móveis e Design LTDA', '56.789.123/0001-44', '(41) 3322-5566', 'Alameda das Árvores, 678, Curitiba - PR', NOW()),
                    ('Tech Solutions Inc.', '12.345.678/0001-99', '(51) 3344-7788', 'Rua da Inovação, 321, Porto Alegre - RS', NOW())
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
