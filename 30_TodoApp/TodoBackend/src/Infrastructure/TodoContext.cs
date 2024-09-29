using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using TodoBackend.Models;
using Bogus;

namespace TodoBackend.Infrastructure
{
    public class TodoContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

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
            Randomizer.Seed = new Random(808);

            var user = User.GenerateUserWithPassword("guest", "guest");
            user.Guid = new Guid("00000000-0000-0000-0000-000000000001");
            Users.Add(user);
            SaveChanges();

            var createdAt = new DateTime(2024, 1, 24, 12, 0, 0, DateTimeKind.Utc);
            var updatedAt = new DateTime(2024, 1, 24, 12, 0, 0, DateTimeKind.Utc);
            // Add 5 Categories
            var categories = new Category[]
            {
                new Category("Work", "Tasks related to your professional or business activities.", true, Priority.High, user)
                { Guid = new Guid("00000000-0000-0000-0000-000000000001"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Personal", "Tasks related to personal life, not connected to work.", true, Priority.Medium, user)
                { Guid = new Guid("00000000-0000-0000-0000-000000000002"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Health", "Tasks focused on health, fitness, or medical appointments.", true, Priority.Medium, user)
                { Guid = new Guid("00000000-0000-0000-0000-000000000003"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Home", "Tasks related to household chores, repairs, or home improvement projects.", true, Priority.Low, user)
                { Guid = new Guid("00000000-0000-0000-0000-000000000004"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Finance", "Tasks concerning personal or business finances and budgeting.", true, Priority.Low, user)
                { Guid = new Guid("00000000-0000-0000-0000-000000000005"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
            };

            Categories.AddRange(categories);
            SaveChanges();

            var todoItems = new Faker<TodoItem>().CustomInstantiator(f =>
            {
                var category = f.PickRandom(categories);
                var dueDate = f.Date.Future().OrNull(f, 0.2f);
                var createdAt = f.Date.Between(category.CreatedAt, DateTime.UtcNow);
                var updatedAt = f.Date.Between(createdAt, DateTime.UtcNow);
                return new TodoItem(f.Lorem.Sentence(), f.Lorem.Paragraph(), category, f.Random.Bool(), dueDate)
                {
                    Guid = f.Random.Guid(),
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt
                };
            })
            .Generate(50);
            TodoItems.AddRange(todoItems);
            SaveChanges();

            var todoTasks = new Faker<TodoTask>().CustomInstantiator(f =>
            {
                var todoItem = f.PickRandom(todoItems);
                var dueDate = todoItem.DueDate.HasValue 
                    ? f.Date.Between(DateTime.Now, todoItem.DueDate.Value).OrNull(f, 0.2f) 
                    : f.Date.Future().OrNull(f, 0.2f);
                var createdAt = f.Date.Between(todoItem.CreatedAt, DateTime.UtcNow);
                var updatedAt = f.Date.Between(createdAt, DateTime.UtcNow);
                return new TodoTask(todoItem, f.Lorem.Sentence(), f.Random.Bool(), dueDate)
                {
                    Guid = f.Random.Guid(),
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt
                };
            })
            .Generate(500);
            TodoTasks.AddRange(todoTasks);
            SaveChanges();
        }
    }
}
