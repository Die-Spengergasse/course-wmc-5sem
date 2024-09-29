using System;

namespace TodoBackend.Dto
{
    public record TodoTaskWithItemDto(
        Guid TodoItemGuid, string TodoItemTitle, bool TodoItemIsCompleted, DateTime? TodoItemDueDate,
        Guid guid, string Title, bool IsCompleted, DateTime? DueDate,
        DateTime CreatedAt, DateTime UpdatedAt);        
}
