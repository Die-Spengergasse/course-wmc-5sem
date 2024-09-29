using System;

namespace TodoBackend.Cmd
{
    public record AddTodoItemCmd(
        string Title, string Description, Guid CategoryGuid, DateTime? DueDate);
}
