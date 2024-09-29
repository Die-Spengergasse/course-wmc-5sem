using System;
using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Models
{
    public abstract class Entity<T> where T : struct
    {
        public T Id { get; private set; }
        public Guid Guid { get; set; }
    }
}
