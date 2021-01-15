using Client.Core.Interfaces;
using Client.Core.Internal;
using System;

namespace Client.Core.UI.Menu
{
    public class MenuItemCheckbox : IMenuItem
    {
        protected string name;
        protected string text;
        protected bool check;
        protected bool visible;

        public string Name { get { return name; } private set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public bool Checked { get { return check; } set { check = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public MenuContainer ParentContainer { get; set; }
        public Action<bool> Action { get; private set; }

        public MenuItemCheckbox(string text, bool @checked, Action<bool> action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Checked = @checked;
            Action = action;
            Visible = visible;
        }

        void UpdateRender() => NUI.Execute(new
        {
            request = "menu.updateItemRender",
            type = "menu_checkbox_item",
            name = Name,
            text = Text,
            isChecked = Checked,
            visible = Visible
        });
    }
}
