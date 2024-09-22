using System.Text;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

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
    
    public static void ApplyFilterByNumber<TValue>(
        this StringBuilder sqlBuilder,
        string? propertyName,
        TValue? valueFrom,
        TValue? valueTo,
        params string[] validProperties)
    {
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            if (validProperties.Contains(propertyName?.ToLower()))
            {
                sqlBuilder.Append($"\nwhere {propertyName} between {valueFrom} and {valueTo}");
            }
            else
            {
                throw new ArgumentException("Invalid filter parameters");
            }
        }
    }
    
    public static void ApplyFilterByString(
        this StringBuilder sqlBuilder,
        string? propertyName,
        string? value,
        params string[] validProperties)
    {
        if (!string.IsNullOrWhiteSpace(propertyName)  && !string.IsNullOrWhiteSpace(value))
        {
            if (validProperties.Contains(propertyName?.ToLower()))
            {
                sqlBuilder.Append($"\nwhere {propertyName} = {value}");
            }
            else
            {
                throw new ArgumentException("Invalid filter parameters");
            }
        }
    }
    
}