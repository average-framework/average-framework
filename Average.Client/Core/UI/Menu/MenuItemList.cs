using Client.Core.Interfaces;
using Client.Core.Internal;
using System;
using System.Collections.Generic;

namespace Client.Core.UI.Menu
{
    public class MenuItemList : IMenuItem
    {
        public class KeyValue<T>
        {
            public string Key { get; }
            public T Value { get; }

            public KeyValue(string key, T value)
            {
                Key = key;
                Value = value;
            }
        }

        protected string name;
        protected string text;
        protected int index;
        protected bool visible;

        public string Name { get { return name; } set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public int Index { get { return index; } set { index = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public List<KeyValue<object>> Values { get; set; } = new List<KeyValue<object>>();
        public MenuContainer ParentContainer { get; set; }
        public Action<int, KeyValue<object>> Action { get; set; }

        public MenuItemList(string text, int index, List<KeyValue<object>> values, Action<int, KeyValue<object>> action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Values = values;
            Index = index;
            Action = action;
            Visible = visible;
        }

        void UpdateRender() => NUI.Execute(new
        {
            request = "menu.updateItemRender",
            type = "menu_list_item",
            name = Name,
            text = Text,
            itemName = Values.Count == 0 ? "Default" : Values[Index].Value,
            visible = Visible
        });
    }
}
