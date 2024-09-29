using System;

namespace TodoBackend.Models
{
    public class TodoTask : Entity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected TodoTask() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public TodoTask(TodoItem todoItem, string title, bool isCompleted, DateTime? dueDate = null)
        {
            TodoItem = todoItem;
            Title = title;
            IsCompleted = isCompleted;
            DueDate = dueDate;
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        public TodoItem TodoItem { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
