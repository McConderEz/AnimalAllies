using ToDo.Application.Common;
using ToDo.Domain.Models;

namespace ToDo.Application.Repositories;

public interface ITodoItemRepository: ICrudRepository<ToDoItem>
{
    
}