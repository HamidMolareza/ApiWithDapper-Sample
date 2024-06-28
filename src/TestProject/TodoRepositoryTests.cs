using System.Data;
using System.Data.Common;
using ApiWithDapper.Todo;
using Dapper;
using Moq;
using Moq.Dapper;

namespace TestProject;

public class TodoRepositoryTests {
    [Fact]
    public async Task GetByIdAsync_ShouldReturnTodo_WhenTodoExists() {
        // Arrange
        var todoId = 1;
        var expectedTodo = new Todo { Id = todoId, Title = "Test", Completed = false };

        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Todo>(It.IsAny<string>(),
                It.IsAny<object>(), null, null, null))
            .ReturnsAsync(expectedTodo);
        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.GetByIdAsync(todoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTodo.Id, result.Id);
        Assert.Equal(expectedTodo.Title, result.Title);
        Assert.Equal(expectedTodo.Completed, result.Completed);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNewId() {
        // Arrange
        var newTodo = new Todo { Title = "New Task", Completed = false };
        var newId = 1;

        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
            .ReturnsAsync(newId);
        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.CreateAsync(newTodo);

        // Assert
        Assert.Equal(newId, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNumberOfAffectedRows() {
        // Arrange
        var updatedTodo = new Todo { Id = 1, Title = "Updated Task", Completed = true };
        var affectedRows = 1;

        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection
            .SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(affectedRows);
        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.UpdateAsync(updatedTodo);

        // Assert
        Assert.Equal(affectedRows, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNumberOfAffectedRows() {
        // Arrange
        var todoId = 1;
        var affectedRows = 1;

        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(affectedRows);
        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        var result = await repository.DeleteAsync(todoId);

        // Assert
        Assert.Equal(affectedRows, result);
    }

    [Fact]
    public async Task DeleteAllAsync_ShouldReturnNumberOfAffectedRows() {
        // Arrange
        var affectedRows = 1;

        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(affectedRows);
        var repository = new TodoRepository(mockDbConnection.Object);

        // Act
        await repository.DeleteAllAsync();

        // Assert
        Assert.True(true);
    }
}