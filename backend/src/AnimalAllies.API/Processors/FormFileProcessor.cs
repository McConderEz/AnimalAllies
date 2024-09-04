using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.API.Processors;

public class FormFileProcessor: IAsyncDisposable
{
    private readonly List<CreateFileDto> _fileDtos = [];

    public List<CreateFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileDto(stream, file.FileName);
            _fileDtos.Add(fileDto);
        }

        return _fileDtos;
    }
    
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _fileDtos)
        {
            await file.Content.DisposeAsync();
        }
    }
}