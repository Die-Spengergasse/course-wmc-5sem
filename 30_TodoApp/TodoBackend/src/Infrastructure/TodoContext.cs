using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using TodoBackend.Models;
using System.Linq;

namespace TodoBackend.Infrastructure
{
    public class TodoContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(c => c.Priority).HasConversion<string>();
            modelBuilder.Entity<TodoItem>().HasIndex(nameof(TodoItem.Title), "CategoryId").IsUnique(true);

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
            var createdAt = new DateTime(2024, 1, 24, 12, 0, 0, DateTimeKind.Utc);
            var updatedAt = new DateTime(2024, 1, 24, 12, 0, 0, DateTimeKind.Utc);
            // Add 5 Categories
            var categories = new Category[]
            {
                new Category("Work", "Tasks related to your professional or business activities.", true, Priority.High, "guest")
                { Guid = new Guid("00000000-0000-0000-0000-000000000001"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Personal", "Tasks related to personal life, not connected to work.", true, Priority.Medium, "guest")
                { Guid = new Guid("00000000-0000-0000-0000-000000000002"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Health", "Tasks focused on health, fitness, or medical appointments.", true, Priority.Medium, "guest")
                { Guid = new Guid("00000000-0000-0000-0000-000000000003"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Home", "Tasks related to household chores, repairs, or home improvement projects.", true, Priority.Low, "guest")
                { Guid = new Guid("00000000-0000-0000-0000-000000000004"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
                new Category("Finance", "Tasks concerning personal or business finances and budgeting.", true, Priority.Low, "guest")
                { Guid = new Guid("00000000-0000-0000-0000-000000000005"), CreatedAt = createdAt, UpdatedAt = updatedAt  },
            };

            Categories.AddRange(categories);
            var categoryWithoutItems = new Category("Without Items", "Category without items", true, Priority.Low, "guest")
            {
                Guid = new Guid("00000000-0000-0000-0000-000000000006"),
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
            SaveChanges();
            Categories.Add(categoryWithoutItems);
            SaveChanges();

            var todoItemWithoutTasks = new TodoItem("Without Tasks", "TodoItem without tasks", categories[0], false, null)
            {
                Guid = new Guid("00000000-0000-0000-0001-000000000001"),
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
            TodoItems.Add(todoItemWithoutTasks);
            SaveChanges();

            // A fixed seed generates the same GUIDs and times on every start.
            // The dates are relative to today, so the data always contains upcoming and overdue items.
            var random = new Random(808);
            var now = DateTime.UtcNow;
            var today = now.Date;
            var categoriesByName = categories.ToDictionary(c => c.Name);
            var seedItems = TodoSeedData.TodoItems;
            for (var i = 0; i < seedItems.Length; i++)
            {
                var data = seedItems[i];
                // One item per day, so ordering by CreatedAt returns the items in the order of the seed data.
                var itemCreatedAt = today.AddDays(-70 + i).AddHours(random.Next(8, 18)).AddMinutes(random.Next(60));
                DateTime? dueDate = data.DueInDays is int days ? today.AddDays(days).AddHours(random.Next(9, 18)) : null;
                // A completed item was last changed when it was finished, an open item at any time until now.
                var lastChange = data.IsCompleted && dueDate < now ? dueDate.Value : now;
                var todoItem = new TodoItem(data.Title, data.Description, categoriesByName[data.Category], data.IsCompleted, dueDate)
                {
                    Guid = data.Guid ?? NewGuid(random),
                    CreatedAt = itemCreatedAt,
                    UpdatedAt = RandomBetween(random, itemCreatedAt, lastChange)
                };
                TodoItems.Add(todoItem);

                // The due dates of the tasks are spread evenly up to the due date of the item.
                // They use the time of day of the item's due date, so no task is due at night.
                var taskDueDates = Enumerable.Range(1, data.Tasks.Length)
                    .Select(n => dueDate.HasValue
                        ? (itemCreatedAt + (dueDate.Value - itemCreatedAt) * n / data.Tasks.Length).Date + dueDate.Value.TimeOfDay
                        : (DateTime?)null)
                    .ToArray();
                var doneTasks = data.IsCompleted
                    ? data.Tasks.Length
                    : CountDoneTasks(random, taskDueDates, now);
                for (var j = 0; j < data.Tasks.Length; j++)
                {
                    var taskDueDate = taskDueDates[j];
                    var taskCreatedAt = itemCreatedAt.AddMinutes(2 * (j + 1));
                    TodoTasks.Add(new TodoTask(todoItem, data.Tasks[j], j < doneTasks, taskDueDate)
                    {
                        Guid = NewGuid(random),
                        CreatedAt = taskCreatedAt,
                        UpdatedAt = RandomBetween(random, taskCreatedAt, lastChange)
                    });
                }
            }
            SaveChanges();
        }

        /// <summary>
        /// Creates a GUID from the given random generator, so the GUIDs are reproducible.
        /// </summary>
        private static Guid NewGuid(Random random)
        {
            var bytes = new byte[16];
            random.NextBytes(bytes);
            return new Guid(bytes);
        }

        /// <summary>
        /// Returns a random point in time between from and to with whole minutes.
        /// </summary>
        private static DateTime RandomBetween(Random random, DateTime from, DateTime to)
        {
            var minutes = (int)(to - from).TotalMinutes;
            return from.AddMinutes(random.Next(Math.Max(minutes, 1)));
        }

        /// <summary>
        /// Returns how many tasks of an open item are done. Tasks are done in their order.
        /// Tasks due in the past are usually done, sometimes the last of them is still open (overdue).
        /// Without due dates a random number of tasks is done. At least one task stays open.
        /// </summary>
        private static int CountDoneTasks(Random random, DateTime?[] taskDueDates, DateTime now)
        {
            var done = taskDueDates.All(d => d.HasValue)
                ? taskDueDates.Count(d => d < now) - (random.Next(3) == 0 ? 1 : 0)
                : random.Next(taskDueDates.Length);
            return Math.Clamp(done, 0, taskDueDates.Length - 1);
        }
    }
}
