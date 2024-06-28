using System.Data.Common;
using System.Text;
using ApiWithDapper.Helpers;
using Dapper;
using Moq;
using Moq.Dapper;

namespace TestProject;

public class PaginationHelpersTests {
    [Fact]
    public async Task GetPageDataAsync_ReturnsCorrectPageData_WhenLimitAndPageAreProvided() {
        // Arrange
        var mockDbConnection = new Mock<DbConnection>();
        var baseSql = new StringBuilder("select * from Todos");
        var parameters = new DynamicParameters();
        const int limit = 10;
        const int page = 2;

        mockDbConnection.SetupDapperAsync(db => db.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
            .ReturnsAsync(50);

        // Act
        var (pageData, pageFilterQuery) = await PaginationHelpers.GetPageDataAsync<object>(
            mockDbConnection.Object,
            baseSql,
            parameters,
            limit,
            page);

        // Assert
        Assert.Equal(2, pageData.Page);
        Assert.Equal(5, pageData.TotalPages); // 50 / 10 = 5
        Assert.True(pageData.HasNextPage);
        Assert.True(pageData.HasPreviousPage);
        Assert.Equal(50, pageData.TotalCount);
        Assert.Equal(limit, pageData.Limit);
        Assert.Equal(" offset @Offset rows fetch next @Limit rows only", pageFilterQuery);
    }

    [Fact]
    public async Task GetPageDataAsync_ReturnsCorrectPageData_WhenLimitAndPageAreNotProvided() {
        // Arrange
        var mockDbConnection = new Mock<DbConnection>();
        var baseSql = new StringBuilder("select * from Todos");
        var parameters = new DynamicParameters();

        mockDbConnection.SetupDapperAsync(db => db.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
            .ReturnsAsync(50); // totalCount

        // Act
        var (pageData, pageFilterQuery) = await PaginationHelpers.GetPageDataAsync<object>(
            mockDbConnection.Object,
            baseSql,
            parameters,
            null,
            null);

        // Assert
        Assert.Equal(1, pageData.Page);
        Assert.Equal(1, pageData.TotalPages); // default to 1 page
        Assert.False(pageData.HasNextPage);
        Assert.False(pageData.HasPreviousPage);
        Assert.Equal(50, pageData.TotalCount);
        Assert.Null(pageData.Limit);
        Assert.Equal("", pageFilterQuery);
    }
}