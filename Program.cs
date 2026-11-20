using System.Reflection;
using DbUp;
using GaNe.Repositories;
using GaNe.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var connectionString =
    config.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Не найдена настройка ConnectionStrings:DefaultConnection");
}

builder.Services.AddSingleton<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

// Update migrations
var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();


if (!result.Successful)
{
    throw new InvalidOperationException("Не удалось применить миграции", result.Error);
}

//Registers
// builder.Services.AddScoped<IExpeditionsRepository, ExpeditionRepository>();
builder.Services.AddScoped<IAdventurerRepository, AdventurerRepository>();
//
// builder.Services.AddScoped<IExpeditionsService, ExpeditionsService>();
 builder.Services.AddScoped<IAdventurerService, AdventurerService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();