using Client.Core.Enums;
using Client.Core.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Core.UI.HitMenu
{
    public class HitMenuContainer
    {
        [JsonIgnore]
        public List<HitMenuItem> Items { get; } = new List<HitMenuItem>();

        public string Id { get; } = CAPI.RandomString();
        public string Job { get; private set; }
        public EntityType EntityType { get; private set; }

        public HitMenuContainer(EntityType entityType, string job = "all")
        {
            EntityType = entityType;
            Job = job;
        }

        public bool ItemExists(HitMenuItem menuItem) => Items.Contains(menuItem);

        public void AddItem(HitMenuItem menuItem)
        {
            if (!ItemExists(menuItem))
            {
                menuItem.Parent = this;
                Items.Add(menuItem);
            }
        }

        public void RemoveItem(HitMenuItem menuItem)
        {
            if (ItemExists(menuItem))
            {
                Items.Remove(menuItem);
            }
        }

        public HitMenuItem GetItem(string id) => Items.Find(x => x.Id == id);
    }
}
