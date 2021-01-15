using System.Collections.Generic;

namespace Client.Models
{
    public class PedInfoModel
    {
        public string HashString { get; set; }
        public int Variation { get; set; }
    }

    public class PedInfoModelConfig
    {
        public List<PedInfoModel> Peds { get; set; }
    }
}
