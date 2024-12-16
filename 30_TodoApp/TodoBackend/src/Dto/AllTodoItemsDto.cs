using System;

namespace TodoBackend.Dto
{
    public record class AllTodoItemsDto(
        Guid guid, string Title, string Description, 
        Guid CategoryGuid,
        string CategoryName, string CategoryPriority, bool CategoryIsVisible,
        bool IsCompleted, DateTime? DueDate,
        DateTime CreatedAt, DateTime UpdatedAt);
}
