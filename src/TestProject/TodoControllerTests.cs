using ApiWithDapper.Helpers;
using ApiWithDapper.Todo;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject {
    public class TodoControllerTests {
        private readonly Mock<ITodoRepository> _mockRepo;
        private readonly TodoController _controller;

        public TodoControllerTests() {
            _mockRepo = new Mock<ITodoRepository>();
            _controller = new TodoController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithPageData() {
            // Arrange
            var pageData = new PageData<Todo> {
                Page = 1,
                TotalPages = 1,
                HasNextPage = false,
                HasPreviousPage = false,
                TotalCount = 1,
                Data = [new Todo() { Id = 1, Title = "Test", Completed = false }]
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(null, null, 10, 1)).ReturnsAsync(pageData);

            // Act
            var result = await _controller.GetAll(null, null, 10, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PageData<Todo>>(okResult.Value);
            Assert.Equal(pageData, returnValue);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithTodo() {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", Completed = false };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todo);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(todo, returnValue);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundResult_WhenTodoNotFound() {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Todo)null!);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithTodo() {
            // Arrange
            var todo = new Todo { Title = "Test", Completed = false };
            _mockRepo.Setup(repo => repo.CreateAsync(todo)).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(todo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Todo>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult_WhenTodoIsUpdated() {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", Completed = false };
            _mockRepo.Setup(repo => repo.UpdateAsync(todo)).ReturnsAsync(1);

            // Act
            var result = await _controller.Update(1, todo);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequestResult_WhenIdMismatch() {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", Completed = false };

            // Act
            var result = await _controller.Update(2, todo);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFoundResult_WhenTodoNotFound() {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", Completed = false };
            _mockRepo.Setup(repo => repo.UpdateAsync(todo)).ReturnsAsync(0);

            // Act
            var result = await _controller.Update(1, todo);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenTodoIsDeleted() {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(1);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenTodoNotFound() {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(0);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAll_ReturnsNoContentResult_WhenAllTodosAreDeleted() {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAllAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}