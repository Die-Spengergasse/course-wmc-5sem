#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivingExamBackend.Models
{
    /// <summary>
    /// Name des Prüfungsmoduls (Grundwissen, AM, A, B, ...)
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class Module
    {
        public Module(int number, string name)
        {
            Number = number;
            Name = name;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; } = new();
    }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.