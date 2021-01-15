using Client.Core.Interfaces;
using Client.Core.Internal;
using System.Collections.Generic;

namespace Client.Core.UI.Menu
{
    public class MenuContainer
    {
        public List<IMenuItem> Items { get; } = new List<IMenuItem>();
        public string Name { get; private set; }
        public string Title { get; private set; }

        public MenuContainer(string title)
        {
            Name = CAPI.RandomString();
            Title = title;
        }

        public bool ItemExists(IMenuItem menuItem) => Items.Contains(menuItem);

        public void AddItem(IMenuItem menuItem)
        {
            if (!ItemExists(menuItem))
            {
                menuItem.ParentContainer = this;
                Items.Add(menuItem);
            }
        }

        public void RemoveItem(IMenuItem menuItem)
        {
            if (ItemExists(menuItem))
                Items.Remove(menuItem);
        }

        public IMenuItem GetItem(string name) => Items.Find(x => x.Name == name);
    }
}
