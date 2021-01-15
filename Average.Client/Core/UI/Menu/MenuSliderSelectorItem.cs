using Client.Core.Interfaces;
using Client.Core.Internal;
using System;

namespace Client.Core.UI.Menu
{
    public class MenuSliderSelectorItem<T> : IMenuItem
    {
        protected string name;
        protected string text;
        protected bool visible;
        protected T minValue;
        protected T maxValue;
        protected T value;
        protected T step;

        public string Name { get { return name; } set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public T MinValue { get { return minValue; } set { minValue = value; UpdateRender(); } }
        public T MaxValue { get { return maxValue; } set { maxValue = value; UpdateRender(); } }
        public T Value { get { return value; } set { this.value = value; UpdateRender(); } }
        public T Step { get { return step; } set { step = value; UpdateRender(); } }
        public MenuContainer ParentContainer { get; set; }
        public Action<T> Action { get; set; }

        public MenuSliderSelectorItem(string text, T minValue, T maxValue, T defaultValue, T step, Action<T> action, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = defaultValue;
            Step = step;
            Action = action;
            Visible = visible;
        }

        void UpdateRender()
        {
            NUI.Execute(new
            {
                request = "menu.updateItemRender",
                type = "menu_slider_selector_item",
                name = Name,
                text = Text,
                min = MinValue,
                max = MaxValue,
                step = Step,
                value = Value,
                visible = Visible
            });
        }
    }
}
