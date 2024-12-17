using MySql.Data.MySqlClient;

namespace backend.Database;

public class DatabaseCreator
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseCreator> _logger;

    public DatabaseCreator(string connectionString, ILogger<DatabaseCreator> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public void CreateDatabase()
    {

        var builder = new MySqlConnectionStringBuilder(_connectionString) { Database = "" };

        var databaseName = new MySqlConnectionStringBuilder(_connectionString).Database;

        try
        {
            using var connection = new MySqlConnection(builder.ToString());

            connection.Open();

            if (!DatabaseExists(connection, databaseName))
            {
                var createDatabaseSql = $"CREATE DATABASE {databaseName}";

                using var createCommand = new MySqlCommand(createDatabaseSql, connection);

                createCommand.ExecuteNonQuery();

                _logger.LogInformation($"Banco de dados '{databaseName}' criado com sucesso.");
            }
            else
            {
                _logger.LogInformation($"O banco de dados '{databaseName}' já existe.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Houve um erro ao iniciar ou criar o banco de dados.");
            throw;
        }

    }

    private bool DatabaseExists(MySqlConnection connection, string databaseName)
    {
        try
        {
            var checkDatabaseSql = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";

            using var checkCommand = new MySqlCommand(checkDatabaseSql, connection);

            return checkCommand.ExecuteScalar() != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Houve um erro ao verificar se o banco de dados existe.");
            throw;
        }
    }

    public void InitializeTables()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);

            connection.Open();

            var createTablesSql = @"
                CREATE TABLE IF NOT EXISTS categories (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    description TEXT NULL,
                    date_added DATETIME NULL
                );

                CREATE TABLE IF NOT EXISTS products (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    price DECIMAL(10, 2) NOT NULL,
                    description TEXT NULL,
                    is_deleted BOOLEAN DEFAULT FALSE,
                    category_id INT NOT NULL,
                    date_added DATETIME NULL,
                    FOREIGN KEY (category_id) REFERENCES categories(id)
                );

                CREATE TABLE IF NOT EXISTS suppliers (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    cnpj VARCHAR(20) NOT NULL,
                    telephone VARCHAR(15) NULL,
                    address TEXT NULL,
                    date_added DATETIME NULL
                );

                CREATE TABLE IF NOT EXISTS product_suppliers (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    product_id INT NOT NULL,
                    supplier_id INT NOT NULL,
                    date_added DATETIME NULL,
                    FOREIGN KEY (product_id) REFERENCES Products(id),
                    FOREIGN KEY (supplier_id) REFERENCES Suppliers(id)
                );";

            using var command = new MySqlCommand(createTablesSql, connection);

            command.ExecuteNonQuery();

            _logger.LogInformation("Tabelas criadas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Houve um erro ao criar as tabelas.");
            throw;
        }
    }

}
