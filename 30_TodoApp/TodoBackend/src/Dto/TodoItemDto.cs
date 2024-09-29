using System;
using System.Collections.Generic;

namespace TodoBackend.Dto
{
    public record class TodoItemDto(
        Guid guid, string Title, string Description,
        string CategoryName, string CategoryPriority,
        bool IsCompleted, DateTime? DueDate,
        DateTime CreatedAt, DateTime UpdatedAt,
        List<TodoTaskDto> TodoTasks);
}
