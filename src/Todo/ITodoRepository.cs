using ApiWithDapper.Helpers;

namespace ApiWithDapper.Todo;

public interface ITodoRepository {
    public Task<PageData<Todo>> GetAllAsync(bool? completed, string? contains, int? limit, int? page);
    public Task<Todo?> GetByIdAsync(int id);
    public Task<int> CreateAsync(Todo input);
    public Task<int> UpdateAsync(Todo input);
    public Task<int> DeleteAsync(int id);
    public Task DeleteAllAsync();
}