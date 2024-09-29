using System;

namespace TodoBackend.Cmd
{
    public record AddTodoTaskCmd(
        Guid TodoItemGuid, string Title, bool IsCompleted, DateTime? DueDate);
}
