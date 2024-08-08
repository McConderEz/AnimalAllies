using CSharpFunctionalExtensions;
using ToDo.Domain.Constraints;

namespace ToDo.Domain.Models;

public class ToDoItem 
{
    private ToDoItem(){}
    private ToDoItem(int id, string title, DateTime creationDate)
    {
        Id = id;
        Title = title;
        CreationDate = creationDate;
    }
    
    public int Id { get; }
    public string Title { get; }
    public DateTime CreationDate { get; }

    public static Result<ToDoItem> Create(int id, string title, DateTime creationTime)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Contraints.MAX_TITLE_LENGTH)
        {
            return Result.Failure<ToDoItem>($"Title cannot be null or length more than {Contraints.MAX_TITLE_LENGTH}");
        }

        var todoItem = new ToDoItem(id, title, creationTime);
        
        return Result.Success<ToDoItem>(todoItem);
    }
}