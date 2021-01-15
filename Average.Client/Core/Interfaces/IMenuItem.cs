using Client.Core.UI.Menu;

namespace Client.Core.Interfaces
{
    public interface IMenuItem
    {
        string Name { get; }
        string Text { get; set; }
        bool Visible { get; set; }
        MenuContainer ParentContainer { get; set; }
    }
}
