using ToDo.Application.Common;
using ToDo.Domain.Models;

namespace ToDo.Application.Abstractions;

public interface ITodoItemService: ICrudService<ToDoItem>
{
    
}