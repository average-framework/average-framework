using Client.Core.Managers;

namespace Client.Models
{
    public class HorseSeller
    {
        public string Name { get; set; }
        public Interact Interact { get; set; }
        public CameraInfo Buy { get; set; }
        public CameraInfo Customization { get; set; }
        public Blip Blip { get; set; }
        public Npc Npc { get; set; }
        public bool IsNear { get; set; }
        public NpcManager.Npc NPCsInstance { get; set; }  
    }
}