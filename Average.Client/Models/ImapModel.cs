using System.Collections.Generic;

namespace Client.Models
{
    public class ImapModel
    {
        public string Hash { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float H { get; set; }
    }

    public class ImapModelConfig
    {
        public List<ImapModel> Imaps { get; set; }
    }
}
