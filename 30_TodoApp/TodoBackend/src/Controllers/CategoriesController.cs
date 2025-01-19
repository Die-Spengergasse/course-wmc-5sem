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
    public class CategoriesController : TodoControllerBase
    {

        private readonly TodoContext _db;

        public CategoriesController(TodoContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AllCategoriesDto>> GetAllCategories()
        {
            var username = Username;
            var categories = await _db.Categories
                .Where(c => c.Owner.Name == username)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new AllCategoriesDto(c.Guid, c.Name, c.Description, c.IsVisible, c.Priority.ToString(), c.Owner.Name))
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory(AddCategoryCmd cmd)
        {
            var username = Username;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user == null) return Forbid();
            var category = new Category(
                name: cmd.Name,
                description: cmd.Description,
                isVisible: cmd.IsVisible,
                priority: Enum.Parse<Priority>(cmd.Priority),
                owner: user
            );
            _db.Categories.Add(category);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }
            return CreatedAtAction(nameof(AddCategory), new { guid = category.Guid });
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditCategory(Guid guid, EditCategoryCmd cmd)
        {
            if (guid != cmd.Guid) return BadRequest();
            var username = Username;
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Owner.Name == username && c.Guid == guid);
            if (category == null) return NotFound();
            category.Name = cmd.Name;
            category.Description = cmd.Description;
            category.IsVisible = cmd.IsVisible;
            category.Priority = Enum.Parse<Priority>(cmd.Priority);
            category.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }
            return NoContent();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(Guid guid)
        {
            var username = Username;
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Owner.Name == username && c.Guid == guid);
            if (category == null) return NoContent();
            if (_db.TodoItems.Any(t => t.Category.Id == category.Id))
                return BadRequest("Category has tasks.");
            _db.Categories.Remove(category);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }
            return NoContent();
        }
    }
}
