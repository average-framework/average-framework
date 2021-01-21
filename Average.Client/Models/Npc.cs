using CitizenFX.Core;

namespace Client.Models
{
    public class Npc
    {
        public Vector4 Position { get; set; }
        public string Model { get; set; }
        public int ModelVariation { get; set; }
        public string Scenario { get; set; }
    }
}