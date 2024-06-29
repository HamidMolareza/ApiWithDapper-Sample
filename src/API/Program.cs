using System.Data;
using ApiWithDapper.Helpers;
using ApiWithDapper.Todo;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Register IDbConnection
var dbConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddTransient<IDbConnection>(_ =>
    new SqlConnection(dbConnectionString));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure FluentMigrator
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSqlServer()
        .WithGlobalConnectionString(dbConnectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

var app = builder.Build();

// Apply migration
using (var scope = app.Services.CreateScope()) {
    await using var masterConnection = new SqlConnection(builder.Configuration.GetConnectionString("Master"));

    await masterConnection.CreateDatabaseAsync();

    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();

    await DatabaseInitializer.SeedDatabaseAsync(scope);
}

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();