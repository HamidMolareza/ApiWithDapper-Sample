using ApiWithDapper.Todo;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiWithDapper.Helpers;

public static class DatabaseInitializer {
    public static Task CreateDatabaseAsync(this SqlConnection sqlConnection) {
        const string script = """
                              if not exists(select * from sys.databases where name = 'ApiWithDapper')
                              begin
                                  create database ApiWithDapper;
                              end;
                              """;
        return sqlConnection.ExecuteAsync(script);
    }

    public static async Task SeedDatabaseAsync(IServiceScope scope) {
        var todoRepository = scope.ServiceProvider.GetRequiredService<ITodoRepository>();

        var count = await todoRepository.CountAsync();
        if (count > 0) return;

        var items = new List<Todo.Todo> {
            new() {
                Title = "Task 1",
                Completed = false
            },
            new() {
                Title = "Task 2",
                Completed = true
            },
            new() {
                Title = "Task 3",
                Completed = false
            },
            new() {
                Title = "Task 4",
                Completed = false
            },
        };
        var effected = await todoRepository.CreateManyAsync(items);
        if (effected != items.Count) {
            throw new Exception(
                $"It was expected that {items.Count} items would be added, but {effected} items were added.");
        }
    }
}