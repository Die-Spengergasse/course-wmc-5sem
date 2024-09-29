using System;

namespace TodoBackend.Cmd
{
    public record EditTodoTaskCmd(
        Guid Guid, string Title, bool IsCompleted, DateTime? DueDate);
}
