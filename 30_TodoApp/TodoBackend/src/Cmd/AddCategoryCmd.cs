namespace TodoBackend.Cmd
{
    public record AddCategoryCmd(
        string Name, string Description, bool IsVisible, string Priority);
}
