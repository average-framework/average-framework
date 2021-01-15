using Client.Core.Interfaces;
using Client.Core.Internal;
using System;

namespace Client.Core.UI.Menu
{
    public class MenuItem : IMenuItem
    {
        protected string name;
        protected string text;
        protected bool visible;

        public string Name { get { return name; } set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public MenuContainer ParentContainer { get; set; }
        public MenuContainer TargetContainer { get; set; }
        public Action Action { get; set; }

        public MenuItem(string text, MenuContainer targetMenuContainer, Action action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            TargetContainer = targetMenuContainer;
            Action = action;
            Visible = visible;
        }

        public MenuItem(string text, MenuContainer targetMenuContainer, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            TargetContainer = targetMenuContainer;
            Visible = visible;
        }

        public MenuItem(string text, Action action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Action = action;
            Visible = visible;
        }

        public MenuItem(string text, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Visible = visible;
        }

        void UpdateRender() => NUI.Execute(new
        {
            request = "menu.updateItemRender",
            type = "menu_item",
            name = Name,
            text = Text,
            visible = Visible
        });
    }
}
