using System.Collections.Generic;

namespace Client.Models
{
    public class InteriorSetModel
    {
        public int Id { get; set; }
        public string HashString { get; set; }
        public string Set { get; set; }
    }

    public class InteriorSetModelConfig
    {
        public List<InteriorSetModel> InteriorsSet { get; set; }
    }
}
