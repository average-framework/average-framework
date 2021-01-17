using CitizenFX.Core;

namespace Client.Models
{
    public class Door
    {
        public Vector3 Position { get; }
        public bool IsLocked { get; set; }
        public float Range { get; }
        public string JobName { get; }
        public bool IsNear { get; set; }

        public Door(Vector3 position, bool isLocked, float range, string jobName)
        {
            Position = position;
            IsLocked = isLocked;
            Range = range;
            JobName = jobName;
        }
    }
}
