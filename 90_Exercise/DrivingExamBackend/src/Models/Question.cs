#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DrivingExamBackend.Models
{
    [Index(nameof(Number), IsUnique = true)]
    public class Question : Entity<int>
    {
        public Question(int number, string text, int points, Module module, Topic topic, string? imageUrl)
        {
            Number = number;
            Text = text;
            Points = points;
            Module = module;
            Topic = topic;
            ImageUrl = imageUrl;
        }

        protected Question() { }

        public int Number { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public Module Module { get; set; }
        public Topic Topic { get; set; }
        public string? ImageUrl { get; set; }
        public List<Answer> Answers { get; } = new();
    }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.