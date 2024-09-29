using System;
using System.Collections.Generic;

namespace TodoBackend.Models
{
    public class TodoItem : Entity<int>
    {
        public TodoItem(string title, string description, Category category, bool isCompleted, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            Category = category;
            IsCompleted = isCompleted;
            DueDate = dueDate;
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected TodoItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TodoItem> TodoItems { get; } = new();
    }
}
