#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DrivingExamBackend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Topic : Entity<int>
    {
        public Topic(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public List<Question> Questions { get; } = new();
    }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.