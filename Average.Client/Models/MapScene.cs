using System.Collections.Generic;
using Client.Core.UI.Menu;

namespace Client.Models
{
    public class MapScene
    {
        public string Name { get; set; }
        public MenuContainer Container { get; set; }
        public List<MapObject> Objects { get; set; }

        public MapScene(string name, MenuContainer menuContainer, List<MapObject> objects)
        {
            Name = name;
            Container = menuContainer;
            Objects = objects;
        }
    }
}