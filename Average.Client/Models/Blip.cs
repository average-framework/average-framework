using CitizenFX.Core;

namespace Client.Models
{
    public class Blip
    {
        public Vector4 Position { get; set; }
        public int Sprite { get; set; }
        public string Label { get; set; }
        public float Scale { get; set; }
    }
}