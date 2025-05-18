using Bogus;
using DrivingExamBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
namespace TodoBackend.Infrastructure
{
    public class DrivingExamContext : DbContext
    {
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();


        public DrivingExamContext(DbContextOptions<DrivingExamContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Generic config for all entities
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        public void Initialize(bool deleteDatabase = false)
        {
            if (deleteDatabase) Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1942);
            var faker = new Faker("de");
            Dictionary<int, Module> modules = new Dictionary<int, Module>()
            {
                {  1, new Module(1, "Grundwissen") {Guid = faker.Random.Guid() } },
                {  2, new Module(2, "A spezifisch") {Guid = faker.Random.Guid() } },
                {  3, new Module(3, "B spezifisch") {Guid = faker.Random.Guid() } },
                {  4, new Module(4, "C spezifisch") {Guid = faker.Random.Guid() } },
                {  5, new Module(5, "D spezifisch") {Guid = faker.Random.Guid() } },
                {  6, new Module(6, "E spezifisch") {Guid = faker.Random.Guid() } },
                {  7, new Module(7, "F spezifisch") {Guid = faker.Random.Guid() } },
                {  8, new Module(8, "AM spezifisch") {Guid = faker.Random.Guid() }},
                {  10, new Module(10, "Fahrlehrerausbildung") {Guid = faker.Random.Guid() }}
            };
            Dictionary<string, Topic> topics = new Dictionary<string, Topic>(10);
            List<Question> questions = new List<Question>(5000);
            List<Answer> answers = new List<Answer>(20000);

            var questionsStreamReader = new StreamReader("questions.json", new UTF8Encoding(false));
            var questionsDocument = JsonDocument.Parse(questionsStreamReader.BaseStream).RootElement;

            foreach (var question in questionsDocument.EnumerateArray())
            {
                var number = question.GetProperty("questionNumber").GetInt32();
                var text = question.GetProperty("questionText").GetString();
                if (text is null)
                    throw new ApplicationException($"No questionText for Question {number}.");
                var topic = question.GetProperty("path").EnumerateArray().Select(p => p.GetString()).Last();
                if (topic is null)
                    throw new ApplicationException($"No element in path array for Question {number}.");
                var module = question.GetProperty("classes").EnumerateArray().First().GetInt32();

                var imageUrl = question.TryGetProperty("imageUrl", out var imageUrlProp)
                    ? imageUrlProp.GetString() : null;
                var correctAnswersText = question.GetProperty("correctAnswers")
                    .EnumerateArray().Select(q => q.GetString()).Where(q => !string.IsNullOrEmpty(q)).ToList();
                if (correctAnswersText.Count == 0)
                    throw new ApplicationException($"No correct answers found for Question {number}.");
                var wrongAnswersText = question.GetProperty("wrongAnswers")
                    .EnumerateArray().Select(q => q.GetString()).Where(q => !string.IsNullOrEmpty(q)).ToList();

                if (!modules.TryGetValue(module, out var moduleEntity))
                    throw new ApplicationException($"Invalid class for Question {number}.");
                if (!topics.TryGetValue(topic, out var topicEntity))
                {
                    topicEntity = new Topic(topic) { Guid = faker.Random.Guid() };
                    topics.Add(topic, topicEntity);
                }
                var questionEntity = new Question(
                    number, text, 1, moduleEntity, topicEntity, imageUrl)
                { Guid = faker.Random.Guid() };
                questions.Add(questionEntity);
                var answersEntities = correctAnswersText
                    .Select(t => new Answer(questionEntity, t!, true) { Guid = faker.Random.Guid() })
                    .Concat(wrongAnswersText
                        .Select(t => new Answer(questionEntity, t!, false) { Guid = faker.Random.Guid() }));
                answers.AddRange(answersEntities);
            }
            Modules.AddRange(modules.Values);
            Topics.AddRange(topics.Values);
            Questions.AddRange(questions);
            Answers.AddRange(answers);

            SaveChanges();
        }
    }
}
