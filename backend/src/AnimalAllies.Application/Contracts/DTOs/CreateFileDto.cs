namespace AnimalAllies.Application.Contracts.DTOs;

public record CreateFileDto(Stream Content,string FileName,string ContentType);