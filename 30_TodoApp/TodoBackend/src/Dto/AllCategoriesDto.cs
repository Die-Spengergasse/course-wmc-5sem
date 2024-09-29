using System;

namespace TodoBackend.Dto
{
    public record AllCategoriesDto(
        Guid Guid, string Name, string Description, bool IsVisible,
        string Priority, string OwnerName);
}
