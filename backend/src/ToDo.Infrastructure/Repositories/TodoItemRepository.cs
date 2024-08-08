using Microsoft.EntityFrameworkCore;
using ToDo.Application.Repositories;
using ToDo.Domain.Models;

namespace ToDo.Infrastructure.Repositories;

public class TodoItemRepository: ITodoItemRepository
{
    private readonly TodoDbContext _context;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public TodoItemRepository(TodoDbContext context)
    {
        _context = context;
    }
    
    
    public async Task CreateAsync(ToDoItem entity, CancellationToken? cancellationToken)
    {
        _semaphore.WaitAsync();
        try
        {
            await _context.ToDoItems.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task UpdateAsync(ToDoItem entity, CancellationToken? cancellationToken)
    {
        _semaphore.WaitAsync();
        try
        {
            await _context.ToDoItems
                .Where(x => x.Id == entity.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Title, entity.Title));

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DeleteAsync(int id, CancellationToken? cancellationToken)
    {
        _semaphore.WaitAsync();
        try
        {
            await _context.ToDoItems
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<ToDoItem> GetByIdAsync(int id, CancellationToken? cancellationToken)
    {
        _semaphore.WaitAsync();
        try
        {
            return await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<ToDoItem>> GetAsync(CancellationToken? cancellationToken)
    {
        _semaphore.WaitAsync();
        try
        {
            return await _context.ToDoItems.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}