using System;

namespace TodoBackend.Cmd
{
    public record EditCategoryCmd(
       Guid Guid, string Name, string Description, bool IsVisible, string Priority);
}
