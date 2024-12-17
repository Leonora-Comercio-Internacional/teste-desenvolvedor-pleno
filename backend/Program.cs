using backend.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

var logger = loggerFactory.CreateLogger("Program");

logger.LogInformation("Iniciando configuração do banco de dados...");

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

#region CreateAndInitializeDatabase
var databaseCreator = new DatabaseCreator(mySqlConnection, loggerFactory.CreateLogger<DatabaseCreator>());

databaseCreator.CreateDatabase();
databaseCreator.InitializeTables();
#endregion

#region PopulateDatabase
var databaseSeeder = new DatabasePopulate(mySqlConnection, loggerFactory.CreateLogger<DatabasePopulate>());

databaseSeeder.Populating();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
