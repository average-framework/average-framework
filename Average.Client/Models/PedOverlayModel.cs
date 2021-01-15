namespace Client.Models
{
    public class PedOverlayModel
    {
        public string Name { get; private set; }
        public uint Id { get; private set; }
        public uint Albedo { get; private set; }
        public uint Normal { get; private set; }
        public uint Ma { get; private set; }

        public PedOverlayModel(string name, uint id, uint albedo, uint normal, uint ma)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
            Normal = normal;
            Ma = ma;
        }

        public PedOverlayModel(string name, uint id, uint albedo, uint normal)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
            Normal = normal;
        }

        public PedOverlayModel(string name, uint id, uint albedo)
        {
            Name = name;
            Id = id;
            Albedo = albedo;
        }
    }
}
