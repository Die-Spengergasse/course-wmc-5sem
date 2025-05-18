using DrivingExamBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoBackend.Infrastructure;

namespace DrivingExamBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        public record QuestionDto(
            Guid Guid, int Number, string Text, int Points, string? imageUrl,
            Guid moduleGuid, Guid topicGuid, List<AnswerDto> Answers);
        public record AnswerDto(Guid Guid, string Text);
        public record CheckAnswersCmd(List<CheckAnswerCmd> CheckedAnswers);
        public record CheckAnswerCmd(Guid Guid, bool IsChecked);
        public record CheckAnswersDto(int PointsReachable, int PointsReached, Dictionary<Guid, bool> CheckResult);
        public record NewQuestionCmd(
            [Range(1, 999_999)] int Number, [StringLength(255, MinimumLength = 1)] string Text,
            [Range(1, 99)] int Points, Guid ModuleGuid, Guid TopicGuid, string? ImageUrl,
            List<NewAnswerCmd> Answers);
        public record NewAnswerCmd([StringLength(255, MinimumLength = 1)] string Text, bool IsCorrect);
        private readonly DrivingExamContext _db;

        public QuestionsController(DrivingExamContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType<List<QuestionDto>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions([FromQuery] Guid moduleGuid, [FromQuery] Guid topicGuid)
        {
            var questions = await _db.Questions
                .Where(q => q.Module.Guid == moduleGuid && q.Topic.Guid == topicGuid)
                .Select(q => new QuestionDto(
                    q.Guid, q.Number, q.Text, q.Points, q.ImageUrl, q.Module.Guid, q.Topic.Guid,
                    q.Answers.Select(a => new AnswerDto(a.Guid, a.Text)).ToList()))
                .ToListAsync();
            return Ok(questions);
        }

        [HttpPost("{guid}/checkanswers")]
        [ProducesResponseType<List<CheckAnswersDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CheckAnswersDto>>> CheckAnswers([FromRoute] Guid guid, [FromBody] CheckAnswersCmd cmd)
        {
            var questionWithAnswers = await _db.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Guid == guid);
            if (questionWithAnswers is null) return Problem("Question not found", statusCode: 404);

            var answers = questionWithAnswers.Answers.ToDictionary(a => a.Guid, a => a.IsCorrect);
            if (answers.Count != cmd.CheckedAnswers.Select(c => c.Guid).Distinct().Count())
                return Problem("Number of answers does not match number of answers in question.", statusCode: 400);
            if (cmd.CheckedAnswers.Any(a => !answers.ContainsKey(a.Guid)))
                return Problem("Invalid answer GUIDs for question.", statusCode: 400);

            var checkDict = cmd.CheckedAnswers
                .Select(c => new
                {
                    c.Guid,
                    IsCorrectAnswer = c.IsChecked == answers[c.Guid]
                })
                .ToDictionary(c => c.Guid, c => c.IsCorrectAnswer);

            return Ok(new CheckAnswersDto(
                questionWithAnswers.Points,
                checkDict.Values.All(v => v) ? questionWithAnswers.Points : 0, checkDict));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddQuestion([FromBody] NewQuestionCmd cmd)
        {
            if (cmd.Answers.Count == 0 || cmd.Answers.Count > 10)
                return Problem("Invalid answer count.");
            if (!cmd.Answers.Any(a => a.IsCorrect))
                return Problem("No correct answer provided.");

            var module = await _db.Modules.FirstOrDefaultAsync(m => m.Guid == cmd.ModuleGuid);
            if (module is null) return Problem("Invalid Module GUID.", statusCode: 400);
            var topic = await _db.Topics.FirstOrDefaultAsync(t => t.Guid == cmd.TopicGuid);
            if (topic is null) return Problem("Invalid Topic GUID.", statusCode: 400);

            var question = new Question(cmd.Number, cmd.Text, cmd.Points, module, topic, cmd.ImageUrl);
            var answers = cmd.Answers.Select(a => new Answer(question, a.Text, a.IsCorrect)).ToList();
            _db.Questions.Add(question);
            _db.Answers.AddRange(answers);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Problem(e.InnerException?.Message ?? e.Message, statusCode: 400);
            }
            return CreatedAtAction(nameof(AddQuestion), new
            {
                question.Guid,
                Answers = answers.Select(a => a.Guid).ToList()
            });
        }
    }
}
