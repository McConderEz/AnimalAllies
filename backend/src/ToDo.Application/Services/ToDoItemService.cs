using ToDo.Application.Abstractions;
using ToDo.Application.Repositories;
using ToDo.Domain.Models;

namespace ToDo.Application.Services;

public class ToDoItemService: ITodoItemService
{
    private readonly ITodoItemRepository _repository;

    public ToDoItemService(ITodoItemRepository repository)
    {
        _repository = repository;
    }
    
    public async Task CreateAsync(ToDoItem entity, CancellationToken? cancellationToken)
    {
        await _repository.CreateAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(ToDoItem entity, CancellationToken? cancellationToken)
    {
        await _repository.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken? cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<ToDoItem> GetByIdAsync(int id, CancellationToken? cancellationToken)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<List<ToDoItem>> GetAsync(CancellationToken? cancellationToken)
    {
        return await _repository.GetAsync(cancellationToken);
    }
}