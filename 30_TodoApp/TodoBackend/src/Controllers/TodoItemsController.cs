﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class TodoItemsController : ControllerBase
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
            var username = HttpContext.User.Identity?.Name;
            var todoItems = await _db.TodoItems
                .Where(t =>
                    t.Category.Owner == username
                    && (string.IsNullOrEmpty(category) ? true : t.Category.Name.ToLower() == category.Trim().ToLower())
                    && (!isCompleted.HasValue ? true : t.IsCompleted == isCompleted.Value))
                .OrderBy(t => t.CreatedAt)
                .Select(t => new AllTodoItemsDto(
                    t.Guid, t.Title, t.Description, t.Category.Guid, t.Category.Name, t.Category.Priority.ToString(), t.Category.IsVisible,
                    t.IsCompleted, t.DueDate, t.CreatedAt, t.UpdatedAt))
                .ToListAsync();
            return Ok(todoItems);
        }
        [HttpGet("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTodoItem(Guid guid)
        {
            var username = HttpContext.User.Identity?.Name;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner == username && t.Guid == guid)
                .Select(t => new TodoItemDto(
                    t.Guid, t.Title, t.Description, t.Category.Name, t.Category.Priority.ToString(),
                    t.IsCompleted, t.DueDate, t.CreatedAt, t.UpdatedAt,
                    t.TodoTasks.Select(ti => new TodoTaskDto(ti.Guid, ti.Title, ti.IsCompleted, ti.DueDate, ti.CreatedAt, ti.UpdatedAt)).ToList()))
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTodoItem(AddTodoItemCmd cmd)
        {
            var username = HttpContext.User.Identity?.Name;
            var category = await _db.Categories
                .Where(c => c.Owner == username && c.Guid == cmd.CategoryGuid)
                .FirstOrDefaultAsync();
            if (category == null)
                return BadRequest($"Category {cmd.CategoryGuid} not found");

            var todoItem = new TodoItem(cmd.Title, cmd.Description, category, false, cmd.DueDate);
            _db.TodoItems.Add(todoItem);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }

            return CreatedAtAction(nameof(AddTodoItem), new { guid = todoItem.Guid });
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditTodoItem(Guid guid, EditTodoItemCmd cmd)
        {
            if (guid != cmd.Guid) return BadRequest();
            var username = HttpContext.User.Identity?.Name;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner == username && t.Guid == guid)
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();

            var category = await _db.Categories
                .Where(c => c.Owner == username && c.Guid == cmd.CategoryGuid)
                .FirstOrDefaultAsync();
            if (category == null)
                return BadRequest($"Category {cmd.CategoryGuid} not found");

            todoItem.Title = cmd.Title;
            todoItem.Description = cmd.Description;
            todoItem.Category = category;
            todoItem.IsCompleted = cmd.IsCompleted;
            todoItem.DueDate = cmd.DueDate;
            todoItem.UpdatedAt = DateTime.UtcNow;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException e) { return BadRequest(e.InnerException?.Message ?? e.Message); }
            return NoContent();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTodoItem(Guid guid, [FromQuery] bool deleteTasks = false)
        {
            var username = HttpContext.User.Identity?.Name;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner == username && t.Guid == guid)
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NoContent();

            var todoTasks = _db.TodoTasks.Where(t => t.TodoItem.Id == todoItem.Id).ToList();
            if (!deleteTasks && todoTasks.Any())
                return BadRequest("TodoItem has tasks.");
            _db.TodoTasks.RemoveRange(todoTasks);

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException e) { return BadRequest(e.InnerException?.Message ?? e.Message); }
            _db.TodoItems.Remove(todoItem);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException e) { return BadRequest(e.InnerException?.Message ?? e.Message); }

            return NoContent();
        }
    }
}
