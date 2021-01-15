namespace Client.Models
{
    public class PedOverlay
    {
        public string Name { get; private set; }
        public uint Id { get; private set; }
        public uint Albedo { get; private set; }
        public uint Normal { get; private set; }
        public uint Ma { get; private set; }

        public PedOverlay(string name, uint id, uint albedo, uint normal, uint ma)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
            Normal = normal;
            Ma = ma;
        }

        public PedOverlay(string name, uint id, uint albedo, uint normal)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
            Normal = normal;
        }

        public PedOverlay(string name, uint id, uint albedo)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
        }
    }
}
