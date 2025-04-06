using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace TodoBackend.Models
{
    public class Category : Entity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected Category() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Category(string name, string description, bool isVisible, Priority priority, string owner)
        {
            Name = name;
            Description = description;
            IsVisible = isVisible;
            Priority = priority;
            Owner = owner;
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public Priority Priority { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TodoItem> TodoItems { get; } = new();
    }
}
