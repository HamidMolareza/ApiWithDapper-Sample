using System.Data;
using System.Text;
using Dapper;

namespace ApiWithDapper.Helpers;

public static class PaginationHelpers {
    public static async Task<(PageData<T> pageData, string pageFilterQuery)>
        GetPageDataAsync<T>(
            IDbConnection db,
            StringBuilder baseSql,
            DynamicParameters parameters,
            int? limit,
            int? page) {
        var countSql = new StringBuilder($"select count(*) from ({baseSql}) as CountQuery");

        var totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);
        var totalPages = limit is > 0 ? (int)Math.Ceiling(totalCount / (double)limit.Value) : 1;

        var pageFilterQuery = "";
        if (limit.HasValue && page.HasValue) {
            var offset = (page.Value - 1) * limit.Value;
            pageFilterQuery = " offset @Offset rows fetch next @Limit rows only";
            parameters.Add("Offset", offset);
            parameters.Add("Limit", limit.Value);
        }

        var pageData = new PageData<T> {
            Page = page ?? 1,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Limit = limit
        };

        return (pageData, pageFilterQuery);
    }
}