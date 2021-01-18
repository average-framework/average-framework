using CitizenFX.Core;

namespace Client.Models
{
    public class MapObject
    {
        public int Entity { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public int Alpha { get; set; }
        public uint Model { get; set; }
        public bool Gravity { get; set; }
        public bool Visible { get; set; }
        public bool Superposition { get; set; }
        public bool Collision { get; set; }
        public bool PlaceOnGround { get; set; }

        public MapObject(int entity, uint model, Vector3 position, Vector3 rotation, int alpha, bool gravity, bool visible, bool superposition, bool collision, bool placeOnGround)
        {
            Entity = entity;
            Model = model;
            Position = position;
            Rotation = rotation;
            Alpha = alpha;
            Gravity = gravity;
            Visible = visible;
            Superposition = superposition;
            Collision = collision;
            PlaceOnGround = placeOnGround;
        }
    }
}
