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

namespace DrivingExamBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        public record TopicDto(Guid Guid, string Name, int QuestionCount);
        public record NewTopicDto([StringLength(255, MinimumLength = 1)] string Name);
        public record EditTopicDto(Guid Guid, [StringLength(255, MinimumLength = 1)] string Name);
        private readonly DrivingExamContext _db;

        public TopicsController(DrivingExamContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType<List<TopicDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TopicDto>>> GetTopics([FromQuery] Guid? assignedModule)
        {
            var topicsSet = _db.Questions
                .Where(q => !assignedModule.HasValue || q.Module.Guid == assignedModule.Value)
                .Select(q => q.Topic.Guid)
                .ToHashSet();

            var topics = await _db.Topics
                .Where(t => topicsSet.Contains(t.Guid))
                .Select(t => new TopicDto(
                    t.Guid, t.Name, t.Questions.Count()))
                .ToListAsync();
            return Ok(topics);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTopic([FromBody] NewTopicDto cmd)
        {
            var topic = new Topic(cmd.Name);
            _db.Topics.Add(topic);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.InnerException?.Message ?? e.Message, statusCode: 400);
            }
            return CreatedAtAction(nameof(CreateTopic), new { topic.Guid });
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditTopic(Guid guid, [FromBody] EditTopicDto cmd)
        {
            if (cmd.Guid != guid) return Problem("Invalid GUID", statusCode: 400);
            var topic = await _db.Topics.FirstOrDefaultAsync(t => t.Guid == guid);
            if (topic is null) return Problem("Topic not found", statusCode: 404);
            topic.Name = cmd.Name;
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
        public async Task<IActionResult> DeleteTopic(Guid guid, [FromQuery] bool removeQuestions = false)
        {
            var questionsPresent = await _db.Questions.AnyAsync(q => q.Topic.Guid == guid);
            if (questionsPresent && !removeQuestions)
                return Problem("Topic has questions.", statusCode: 400);
            try
            {
                await _db.Answers.Where(a => a.Question.Topic.Guid == guid).ExecuteDeleteAsync();
                await _db.Questions.Where(q => q.Topic.Guid == guid).ExecuteDeleteAsync();
                await _db.Topics.Where(t => t.Guid == guid).ExecuteDeleteAsync();
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
