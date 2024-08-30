using AnimalAllies.API.Extensions;
using AnimalAllies.Application.FileProvider;
using Microsoft.AspNetCore.Mvc;
using AnimalAllies.Application.Providers;


namespace AnimalAllies.API.Controllers;

public class FileController : ApplicationController
{
    private readonly string BUCKET_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    
    public FileController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();

        //TODO: Возможно какая-то валидация
        var fileData = new FileData(stream, BUCKET_NAME, Guid.NewGuid().ToString());

        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }
        
        return Ok(result.Value);
    }

    [HttpDelete("{objectName:guid}")]
    public async Task<IActionResult> RemoveFile([FromRoute] Guid objectName, CancellationToken cancellationToken)
    {
        //TODO: Валидация

        var fileMetadata = new FileMetadata(BUCKET_NAME, objectName.ToString());

        var result = await _fileProvider.DeleteFile(fileMetadata, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }
        
        return Ok(result.Value);
    }
    
    
    [HttpGet("{objectName:guid}")]
    public async Task<IActionResult> GetFileById([FromRoute] Guid objectName, CancellationToken cancellationToken)
    {

        //TODO: Валидация

        var fileMetadata = new FileMetadata(BUCKET_NAME, objectName.ToString());

        var result = await _fileProvider.GetFileByObjectName(fileMetadata, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }
        
        return Ok(result.Value);
    }
}