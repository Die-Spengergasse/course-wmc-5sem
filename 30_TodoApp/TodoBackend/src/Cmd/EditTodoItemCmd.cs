using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Cmd
{
    public record EditTodoItemCmd(
        Guid Guid,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        string Title,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 255 characters.")]
        string Description,
        Guid CategoryGuid,
        bool IsCompleted,
        DateTime? DueDate) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DueDate.HasValue && DueDate.Value < DateTime.Now)
            {
                yield return new ValidationResult("Due date must be in the future.", new[] { nameof(DueDate) });
            }
        }
    }
}
