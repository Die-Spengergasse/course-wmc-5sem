using System;

namespace TodoBackend.Dto
{
    public record TodoTaskDto(
        Guid guid, string Title, bool IsCompleted, DateTime? DueDate,
        DateTime CreatedAt, DateTime UpdatedAt);
}
