using Backend.Domain.Interfaces;
using Backend.Repositories;
using Backend.Database;
using Backend.Utils;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();

builder.Services.AddApiAuthentication();
builder.Services.AddApiAuthorization();
builder.Services.AddApiSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var mySqlConnection = builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");

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
