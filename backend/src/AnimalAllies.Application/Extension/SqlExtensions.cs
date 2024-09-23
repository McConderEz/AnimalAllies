using System.Text;

namespace AnimalAllies.Application.Extension;

public static class SqlExtensions
{
    public static void ApplySorting(
        this StringBuilder sqlBuilder,
        string? sortBy,
        string? sortDirection,
        params string[] validSortBy)
    {
        if (!string.IsNullOrWhiteSpace(sortBy) && !string.IsNullOrWhiteSpace(sortDirection))
        {
            var validSortDirections = new[]{"asc", "desc"};

            if (validSortDirections.Contains(sortDirection?.ToLower()) && validSortBy.Contains(sortBy?.ToLower()))
            {
                sqlBuilder.Append($"\norder by {sortBy} {sortDirection}");
            }
            else
            {
                throw new ArgumentException("Invalid sort parameters");
            }
        }
    }
    
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        int page,
        int pageSize)
    {
        sqlBuilder.Append($"\nlimit {pageSize} offset {(page - 1) * pageSize}");
    }
    
    public static void ApplyBetweenFilter<TValue>(
        this StringBuilder sqlBuilder,
        string? propertyName,
        TValue? valueFrom,
        TValue? valueTo)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            sqlBuilder.Append($"\nwhere {propertyName} between {valueFrom} and {valueTo}");
        }
    }
    
    public static void ApplyFilterByValueFrom<TValue>(
        this StringBuilder sqlBuilder,
        string? propertyName,
        TValue? valueFrom)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            sqlBuilder.Append($"\nwhere {propertyName} >= {valueFrom}");
        }
    }
    
    public static void ApplyFilterByValueTo<TValue>(
        this StringBuilder sqlBuilder,
        string? propertyName,
        TValue? valueTo)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            sqlBuilder.Append($"\nwhere {propertyName} <= {valueTo}");
        }
    }
    
    public static void ApplyFilterByString(
        this StringBuilder sqlBuilder,
        Dictionary<string, string> properties)
    {
        foreach (var property in properties)
        {
            if (!string.IsNullOrWhiteSpace(property.Key)  && !string.IsNullOrWhiteSpace(property.Value))
            {
                sqlBuilder.Append($"\nwhere {property.Key} like '%{property.Value}%'");
            }
        }
    }
    
}