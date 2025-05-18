using System;

namespace DrivingExamBackend.Models
{
    public abstract class Entity<T> where T : struct
    {
        public T Id { get; private set; }
        public Guid Guid { get; set; }
    }

}
