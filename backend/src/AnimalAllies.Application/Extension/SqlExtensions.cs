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
        if (!string.IsNullOrWhiteSpace(sortBy) || !string.IsNullOrWhiteSpace(sortDirection))
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
    
}