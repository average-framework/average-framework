using CitizenFX.Core;

namespace Server.Models
{
    public class DoorModel
    {
        public Vector3 Position { get; set; }
        public bool IsLocked { get; set; }
        public float Range { get; set; }
        public string JobName { get; set; }

        public DoorModel(Vector3 position, bool isLocked, float range, string jobName)
        {
            Position = position;
            IsLocked = isLocked;
            Range = range;
            JobName = jobName;
        }
    }
}
