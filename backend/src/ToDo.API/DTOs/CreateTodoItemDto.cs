using System.ComponentModel.DataAnnotations;

namespace ToDo.API.DTOs;

public record TodoItemDto([MaxLength(300)] string title);
