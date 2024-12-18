using backend.Repositories;
using backend.Interfaces;
using backend.Database;
using backend.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiAuthentication();
builder.Services.AddApiAuthorization();
builder.Services.AddApiSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json");

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IProductRepository>(provider => new ProductRepository(mySqlConnection));
builder.Services.AddScoped<IUserRepository>(provider => new UserRepository(mySqlConnection));


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

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
