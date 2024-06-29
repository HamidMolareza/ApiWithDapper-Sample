using ApiWithDapper.Helpers;

namespace ApiWithDapper.Todo;

public interface ITodoRepository {
    public Task<PageData<Todo>> GetAllAsync(bool? completed = null, string? contains = null, int? limit = null,
        int? page = null);

    public Task<Todo?> GetByIdAsync(int id);
    public Task<int> UpdateAsync(Todo input);
    public Task<int> CreateAsync(Todo input);
    public Task<int> CreateManyAsync(List<Todo> inputs);
    public Task<int> DeleteAsync(int id);
    public Task DeleteAllAsync();
    public Task<int> CountAsync(bool? completed = null, string? contains = null);
}