using Microsoft.AspNetCore.Authorization;
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
    public class TodoTasksController : ControllerBase
    {
        private readonly TodoContext _db;

        public TodoTasksController(TodoContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<TodoTaskWithItemDto>> GetAllTodoTasks()
        {
            var username = HttpContext.User.Identity?.Name;
            var todoTasks = await _db.TodoTasks.Where(t => t.TodoItem.Category.Owner == username)
                .OrderBy(t => t.CreatedAt)
                .Select(t => new TodoTaskWithItemDto(
                    t.TodoItem.Guid, t.TodoItem.Title, t.TodoItem.IsCompleted, t.TodoItem.DueDate,
                    t.Guid, t.Title, t.IsCompleted, t.DueDate, t.CreatedAt, t.UpdatedAt))
                .ToListAsync();
            return Ok(todoTasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodoTask(AddTodoTaskCmd cmd)
        {
            var username = HttpContext.User.Identity?.Name;
            var todoItem = await _db.TodoItems
                .Where(t => t.Category.Owner == username && t.Guid == cmd.TodoItemGuid)
                .FirstOrDefaultAsync();
            if (todoItem == null)
                return NotFound();
            var todoTask = new TodoTask(todoItem, cmd.Title, cmd.IsCompleted, cmd.DueDate);
            _db.TodoTasks.Add(todoTask);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(AddTodoTask), new { guid = todoTask.Guid });
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> EditTodoTask(Guid guid, EditTodoTaskCmd cmd)
        {
            if (guid != cmd.Guid) return BadRequest();
            var username = HttpContext.User.Identity?.Name;
            var todoTask = await _db.TodoTasks
                .Where(t => t.TodoItem.Category.Owner == username && t.Guid == cmd.Guid)
                .FirstOrDefaultAsync();
            if (todoTask == null)
                return NotFound();
            todoTask.Title = cmd.Title;
            todoTask.IsCompleted = cmd.IsCompleted;
            todoTask.DueDate = cmd.DueDate;
            todoTask.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteTodoTask(Guid guid)
        {
            var username = HttpContext.User.Identity?.Name;
            var todoTask = await _db.TodoTasks
                .Where(t => t.TodoItem.Category.Owner == username && t.Guid == guid)
                .FirstOrDefaultAsync();
            if (todoTask == null)
                return NoContent();
            _db.TodoTasks.Remove(todoTask);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
