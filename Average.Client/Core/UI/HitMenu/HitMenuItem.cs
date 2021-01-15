using Client.Core.UI.HitMenu;
using Newtonsoft.Json;
using System;
using static Client.Core.Internal.CAPI;

namespace Client.Core.UI
{
    public class HitMenuItem
    {
        public string Id { get; private set; } = RandomString();
        public string Text { get; set; }
        public string Emoji { get; set; }
        public bool CloseMenuOnAction { get; set; }
        public HitMenuContainer Parent { get; set; }

        [JsonIgnore]
        public Action<RaycastHit> Action { get; private set; }

        public HitMenuItem() { }

        public HitMenuItem(string text, string emoji, bool closeMenuOnAction, Action<RaycastHit> action)
        {
            Text = text;
            Emoji = emoji;
            CloseMenuOnAction = closeMenuOnAction;
            Action = action;
        }
    }
}
