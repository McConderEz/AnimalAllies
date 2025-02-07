namespace VolunteerRequests.Contracts.Requests;

public record GetFilteredRequestsByUserIdRequest(
    string? RequestStatus,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize);