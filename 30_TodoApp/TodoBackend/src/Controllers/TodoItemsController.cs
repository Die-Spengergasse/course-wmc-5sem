﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoBackend.Cmd;
using TodoBackend.Dto;
using TodoBackend.Infrastructure;
using TodoBackend.Models;

namespace TodoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : TodoControllerBase
    {
        private readonly TodoContext _db;

        public TodoItemsController(TodoContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTodoItems([FromQuery] string? category, [FromQuery] bool? isCompleted)
        {
            var username = Username;
            var todoItems = await _db.TodoItems
                .Where(t =>
                    t.Category.Owner.Name == username
                    && (string.IsNullOrEmpty(category) ? true : t.Category.Name.ToLower() == category.Trim().ToLower())
                    && (!isCompleted.HasValue ? true : t.IsCompleted == isCompleted.Value))
                .OrderBy(t => t.CreatedAt)
                .Select(t => new AllTodoItemsDto(
                    t.Guid, t.Title, t.Description, t.Category.Name, t.Category.Priority.ToString(),
                    t.IsCompleted, t.DueDate, t.CreatedAt, t.UpdatedAt))
                .ToListAsync();
            return Ok(todoItems);
        }
        [HttpGet("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTodoItem(Guid guid)
        {
            var username = Username;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner.Name == username && t.Guid == guid)
                .Select(t => new TodoItemDto(
                    t.Guid, t.Title, t.Description, t.Category.Name, t.Category.Priority.ToString(),
                    t.IsCompleted, t.DueDate, t.CreatedAt, t.UpdatedAt,
                    t.TodoItems.Select(ti => new TodoTaskDto(ti.Guid, ti.Title, ti.IsCompleted, ti.DueDate, ti.CreatedAt, ti.UpdatedAt)).ToList()))
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodoItem(AddTodoItemCmd cmd)
        {
            var username = Username;
            var category = await _db.Categories
                .Where(c => c.Owner.Name == username && c.Guid == cmd.CategoryGuid)
                .FirstOrDefaultAsync();
            if (category == null)
                return BadRequest($"Category {cmd.CategoryGuid} not found");

            var todoItem = new TodoItem(cmd.Title, cmd.Description, category, false, cmd.DueDate);
            _db.TodoItems.Add(todoItem);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { guid = todoItem.Guid });
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> EditTodoItem(Guid Guid, EditTodoItemCmd cmd)
        {
            var username = Username;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner.Name == username && t.Guid == Guid)
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();

            var category = await _db.Categories
                .Where(c => c.Owner.Name == username && c.Guid == cmd.CategoryGuid)
                .FirstOrDefaultAsync();
            if (category == null)
                return BadRequest($"Category {cmd.CategoryGuid} not found");

            todoItem.Title = cmd.Title;
            todoItem.Description = cmd.Description;
            todoItem.IsCompleted = cmd.IsCompleted;
            todoItem.Category = category;
            todoItem.DueDate = cmd.DueDate;
            todoItem.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteTodoItem(Guid guid)
        {
            var username = Username;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner.Name == Username && t.Guid == guid)
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();

            _db.TodoTasks.RemoveRange(_db.TodoTasks.Where(t => t.TodoItem.Guid == guid));
            await _db.SaveChangesAsync();
            _db.TodoItems.Remove(todoItem);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}