using Client.Core.Enums;

namespace Client.Models
{
    public class HorseInfo
    {
        public PedHash Hash { get; private set; }
        public string Category { get; private set; }
        public string Coat { get; private set; }
        public string Type { get; private set; }
        public HorseStats Stats { get; private set; } = new HorseStats();
        public decimal Cost { get; private set; }

        public HorseInfo(PedHash hash, string category, string coat, string type, HorseStats stats, decimal cost)
        {
            Hash = hash;
            Category = category;
            Coat = coat;
            Type = type;
            Stats = stats;
            Cost = cost;
        }
    }
}