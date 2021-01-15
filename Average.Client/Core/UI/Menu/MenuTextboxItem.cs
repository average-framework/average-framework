using Client.Core.Interfaces;
using Client.Core.Internal;
using System;

namespace Client.Core.UI.Menu
{
    public class MenuTextboxItem : IMenuItem
    {
        protected string name;
        protected string text;
        protected string placeHolder;
        protected string pattern;
        protected int minLength;
        protected int maxLength;
        protected object value;
        protected bool visible;

        public string Name { get { return name; } private set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public string Placeholder { get { return placeHolder; } set { placeHolder = value; UpdateRender(); } }
        public string Pattern { get { return pattern; } set { pattern = value; UpdateRender(); } }
        public int MinLength { get { return minLength; } set { minLength = value; UpdateRender(); } }
        public int MaxLength { get { return maxLength; } set { maxLength = value; UpdateRender(); } }
        public object Value { get { return value; } set { this.value = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public MenuContainer ParentContainer { get; set; }
        public Action<object> Action { get; private set; }

        public MenuTextboxItem(string text, object value, string placeholder, string pattern, int minLength, int maxLength, Action<object> action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Value = value;
            Placeholder = placeholder;
            Pattern = pattern;
            MinLength = minLength;
            MaxLength = maxLength;
            Action = action;
            Visible = visible;
        }

        void UpdateRender() => NUI.Execute(new
        {
            request = "menu.updateItemRender",
            type = "menu_textbox_item",
            name = Name,
            text = Text,
            value = Value,
            placeholder = Placeholder,
            pattern = Pattern,
            minLength = MinLength,
            maxLength = MaxLength,
            visible = Visible
        });
    }
}
