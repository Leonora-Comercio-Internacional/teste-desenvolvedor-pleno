using backend.Database;
using backend.Interfaces;
using backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IProductRepository>(provider => new ProductRepository(mySqlConnection));


#region CreateAndInitializeDatabase
var databaseCreator = new DatabaseCreator(mySqlConnection);

databaseCreator.CreateDatabase();
databaseCreator.InitializeTables();
#endregion

#region PopulateDatabase
var databaseSeeder = new DatabasePopulate(mySqlConnection);

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
