namespace ApiWithDapper.Todo;

public class Todo {
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public bool Completed { get; set; }
}