using System.Data.Common;
using ApiWithDapper.Todo;
using Dapper;
using Moq;
using Moq.Dapper;

namespace TestProject;

public class TodoRepositoryTests {
    [Fact]
    public async Task GetByIdAsync_ReturnsTodo_WhenTodoExists() {
        // Arrange
        var mockDbConnection = new Mock<DbConnection>();
        var todo = new Todo { Id = 1, Title = "Test Todo" };
        mockDbConnection.SetupDapperAsync(db => db.QueryFirstOrDefaultAsync<Todo>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
            .ReturnsAsync(todo);

        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(todo.Id, result.Id);
        Assert.Equal(todo.Title, result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTodoDoesNotExist() {
        // Arrange
        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(db => db.QueryFirstOrDefaultAsync<Todo>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
            .ReturnsAsync((Todo?)null);

        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }
    
    
}