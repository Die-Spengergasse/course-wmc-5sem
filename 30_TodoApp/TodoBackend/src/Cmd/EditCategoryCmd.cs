using System;
using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Cmd
{
    public record EditCategoryCmd(
        Guid Guid,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        string Name,
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 255 characters.")]
        string Description,
        bool IsVisible,
        [RegularExpression("Low|Medium|High", ErrorMessage = "Priority must be Low, Medium or High.")]
        string Priority);
}
