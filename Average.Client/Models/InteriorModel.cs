using System.Collections.Generic;

namespace Client.Models
{
    public class InteriorModel
    {
        public int Id { get; set; }
        public string HashString { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class InteriorModelConfig
    {
        public List<InteriorModel> Interiors { get; set; }
    }
}
