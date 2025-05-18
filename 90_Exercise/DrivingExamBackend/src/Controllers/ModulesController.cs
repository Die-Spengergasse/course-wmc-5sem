using DrivingExamBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoBackend.Infrastructure;
using static DrivingExamBackend.Controllers.QuestionsController;

namespace DrivingExamBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        public record ModuleDto(int Number, Guid Guid, string Name);
        public record NewModuleCmd([Range(1, 999_999)] int Number, [StringLength(255, MinimumLength = 1)] string Name);
        public record EditModuleCmd(Guid Guid, [StringLength(255, MinimumLength = 1)] string Name);
        private readonly DrivingExamContext _db;

        public ModulesController(DrivingExamContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType<List<ModuleDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ModuleDto>>> GetAllModules()
        {
            var modules = await _db.Modules
                .OrderBy(m => m.Number)
                .Select(m => new ModuleDto(m.Number, m.Guid, m.Name))
                .ToListAsync();
            return Ok(modules);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateModule([FromBody] NewModuleCmd cmd)
        {
            var module = new Module(cmd.Number, cmd.Name);
            _db.Modules.Add(module);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.InnerException?.Message ?? e.Message, statusCode: 400);
            }
            return CreatedAtAction(nameof(CreateModule), new { module.Guid });
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditModule(Guid guid, [FromBody] EditModuleCmd cmd)
        {
            if (cmd.Guid != guid) return Problem("Invalid GUID", statusCode: 400);
            var module = await _db.Modules.FirstOrDefaultAsync(m => m.Guid == guid);
            if (module is null) return Problem("Module not found", statusCode: 404);
            module.Name = cmd.Name;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.InnerException?.Message ?? e.Message, statusCode: 400);
            }
            return NoContent();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteModule(Guid guid, [FromQuery] bool removeQuestions = false)
        {
            var questionsPresent = await _db.Questions.AnyAsync(q => q.Module.Guid == guid);
            if (questionsPresent && !removeQuestions)
                return Problem("Module has questions.", statusCode: 400);
            try
            {
                await _db.Answers.Where(a => a.Question.Module.Guid == guid).ExecuteDeleteAsync();
                await _db.Questions.Where(q => q.Module.Guid == guid).ExecuteDeleteAsync();
                await _db.Modules.Where(m => m.Guid == guid).ExecuteDeleteAsync();
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.InnerException?.Message ?? e.Message, statusCode: 400);
            }
            return NoContent();
        }
    }
}
