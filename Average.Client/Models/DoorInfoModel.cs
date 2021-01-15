using System.Collections.Generic;

namespace Client.Models
{
    public class DoorInfoModel
    {
        public string Hash { get; set; }
        public string ModelString { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class DoorInfoModelConfig
    {
        public List<DoorInfoModel> Doors { get; set; }
    }
}
