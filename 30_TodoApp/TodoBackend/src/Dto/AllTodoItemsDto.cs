using System;

namespace TodoBackend.Dto
{
    public record class AllTodoItemsDto(
        Guid guid, string Title, string Description, 
        string CategoryName, string CategoryPriority,
        bool IsCompleted, DateTime? DueDate,
        DateTime CreatedAt, DateTime UpdatedAt);
}
