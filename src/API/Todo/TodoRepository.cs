using System.Data;
using System.Text;
using ApiWithDapper.Helpers;
using Dapper;

namespace ApiWithDapper.Todo;

public class TodoRepository(IDbConnection db) : ITodoRepository {
    private const string TableName = "Todos";

    public async Task<PageData<Todo>> GetAllAsync(bool? completed = null, string? contains = null, int? limit = null,
        int? page = null) {
        var sql = new StringBuilder($"select * from {TableName} where 1=1");
        var parameters = new DynamicParameters();

        if (completed.HasValue) {
            sql.Append(" and Completed = @Completed");
            parameters.Add("Completed", completed.Value);
        }

        if (!string.IsNullOrEmpty(contains)) {
            sql.Append(" and Title like @Title");
            parameters.Add("Title", $"%{contains}%");
        }

        var (pageData, pageFilterQuery) = await PaginationHelpers.GetPageDataAsync<Todo>
            (db, sql, parameters, limit, page);

        sql.Append(" order by Id desc")
            .Append(pageFilterQuery);

        pageData.Data = (await db.QueryAsync<Todo>(sql.ToString(), parameters)).ToList();

        return pageData;
    }

    public async Task<Todo?> GetByIdAsync(int id) {
        var item = await db.QueryFirstOrDefaultAsync<Todo>($"select * from {TableName} where Id=@Id", new { Id = id });
        return item;
    }

    public async Task<int> CreateAsync(Todo input) {
        const string sql = $"""
                                insert into {TableName} (Title, Completed)
                                output inserted.Id
                                values (@Title, @Completed);
                            """;
        var entityId = await db.ExecuteScalarAsync<int>(sql, new { input.Title, input.Completed });
        return entityId;
    }

    public async Task<int> CreateManyAsync(List<Todo> inputs) {
        if (inputs.Count == 0) return 0;

        var sql = new StringBuilder();
        sql.Append("insert into Todos (Title, Completed) values ");

        var parameters = new DynamicParameters();
        for (var i = 0; i < inputs.Count; i++) {
            var paramIndex = i + 1;
            sql.Append($"(@Title{paramIndex}, @Completed{paramIndex}),");
            parameters.Add($"@Title{paramIndex}", inputs[i].Title);
            parameters.Add($"@Completed{paramIndex}", inputs[i].Completed);
        }

        // Remove the trailing comma
        sql.Length--;

        return await db.ExecuteAsync(sql.ToString(), parameters);
    }

    public Task<int> UpdateAsync(Todo input) {
        return db.ExecuteAsync($"update {TableName} set Title=@Title, Completed=@Completed where Id=@Id",
            new { input.Title, input.Completed, input.Id });
    }

    public Task<int> DeleteAsync(int id) {
        return db.ExecuteAsync($"delete from {TableName} where Id=@Id", new { id });
    }

    public Task DeleteAllAsync() {
        return db.ExecuteAsync($"truncate table {TableName}");
    }

    public Task<int> CountAsync(bool? completed = null, string? contains = null) {
        var sql = new StringBuilder($"select count(*) from {TableName} where 1=1");
        var parameters = new DynamicParameters();

        if (completed.HasValue) {
            sql.Append(" and Completed = @Completed");
            parameters.Add("Completed", completed.Value);
        }

        if (!string.IsNullOrEmpty(contains)) {
            sql.Append(" and Title like @Title");
            parameters.Add("Title", $"%{contains}%");
        }

        return db.ExecuteScalarAsync<int>(sql.ToString(), parameters);
    }
}