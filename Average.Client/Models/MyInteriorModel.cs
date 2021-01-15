using System.Collections.Generic;

namespace Client.Models
{
    public class MyInteriorModel
    {
        public int Id { get; set; }
        public string Set { get; set; }
        public bool Enable { get; set; }
    }

    public class MyInteriorModelConfig
    {
        public List<MyInteriorModel> Interiors { get; set; }
    }
}
