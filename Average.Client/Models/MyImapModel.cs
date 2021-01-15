using System.Collections.Generic;

namespace Client.Models
{
    public class MyImapModel
    {
        public string Hash { get; set; }
        public bool Enabled { get; set; }
    }

    public class MyImapModelConfig
    {
        public List<MyImapModel> Imaps { get; set; }
    }
}
