using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.DTOs;
using ToDo.Application.Abstractions;
using ToDo.Domain.Models;

namespace ToDo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemController: ControllerBase
{
    private readonly ITodoItemService _service;

    public TodoItemController(ITodoItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        var todoItems = await _service.GetAsync(token);
        return Ok(todoItems);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken token)
    {
        var todoItem = await _service.GetByIdAsync(id, token);
        if (todoItem == null)
            return NotFound();

        return Ok(todoItem);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), 201)]
    [ProducesResponseType(typeof(TodoItemDto), 400)]
    public async Task<IActionResult> Create([FromBody,Required]TodoItemDto dto, CancellationToken token)
    {
        var newTodoItem = ToDoItem.Create(0, dto.title,DateTime.UtcNow);

        
        if (newTodoItem.IsFailure)
            return BadRequest();

        await _service.CreateAsync(newTodoItem.Value, token);

        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,[FromBody,Required] TodoItemDto dto, CancellationToken token)
    {
        var existingTodoItem = await _service.GetByIdAsync(id, token);
        if (existingTodoItem == null)
            return NotFound();

        var updateItem = ToDoItem.Create(existingTodoItem.Id, dto.title, existingTodoItem.CreationDate);

        if (updateItem.IsFailure)
            return BadRequest();

        await _service.UpdateAsync(updateItem.Value, token);
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var existingTodoItem = await _service.GetByIdAsync(id, token);
        if (existingTodoItem == null)
            return NotFound();
        
        await _service.DeleteAsync(id, token);
        return Ok();
    }
    
}