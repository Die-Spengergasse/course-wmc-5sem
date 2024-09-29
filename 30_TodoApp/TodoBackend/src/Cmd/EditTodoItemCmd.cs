using System;

namespace TodoBackend.Cmd
{
    public record EditTodoItemCmd(
        Guid guid, string Title, string Description, bool IsCompleted, Guid CategoryGuid, DateTime? DueDate);
}
