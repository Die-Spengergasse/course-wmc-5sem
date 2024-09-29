using System;

namespace TodoBackend.Cmd
{
    public record EditTodoItemCmd(
        Guid Guid, string Title, string Description, bool IsCompleted, Guid CategoryGuid, DateTime? DueDate);
}
