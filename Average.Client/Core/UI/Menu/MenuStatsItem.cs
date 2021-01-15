using Client.Core.Interfaces;
using Client.Core.Internal;

namespace Client.Core.UI.Menu
{
    public class MenuStatsItem : IMenuItem
    {
        protected string name;
        protected string text;
        protected int step;
        protected int value;
        protected bool visible;

        public string Name { get { return name; } set { name = value; UpdateRender(); } }
        public string Text { get { return text; } set { text = value; UpdateRender(); } }
        public int Step { get { return step; } set { step = value; UpdateRender(); } }
        public int Value { get { return value; } set { this.value = value; UpdateRender(); } }
        public bool Visible { get { return visible; } set { visible = value; UpdateRender(); } }
        public MenuContainer ParentContainer { get; set; }
        public MenuStatsItem(string text, int step, int value, bool visible = true)
        {
            Name = CAPI.RandomString();
            Text = text;
            Step = step;
            Value = value;
            Visible = visible;
        }

        void UpdateRender() => NUI.Execute(new
        {
            request = "menu.updateItemRender",
            type = "menu_stats_item",
            name = name,
            text = text,
            step = step,
            value = value,
            visible = visible
        });
    }
}
