using System.Collections.Generic;

namespace Client.Models
{
    public class HorseComponentInfo
    {
        public string Category { get; private set; }
        public List<string> Hashes { get; private set; } = new List<string>();
        public decimal Cost { get; private set; }

        public HorseComponentInfo() { }

        public HorseComponentInfo(string category, List<string> hashes, decimal cost)
        {
            Category = category;
            Hashes = hashes;
            Cost = cost;
        }
    }
}