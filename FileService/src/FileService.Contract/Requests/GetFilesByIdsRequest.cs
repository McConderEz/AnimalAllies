namespace FileService.Contract.Requests;

public record GetFilesByIdsRequest(
    IEnumerable<Guid> FileIds);