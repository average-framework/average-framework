using System.Collections.Generic;
using CitizenFX.Core;
using Client.Core.Managers;

namespace Client.Models
{
    public class Stable
    {
        public string Name { get; set; }
        public Interact Interact { get; set; }
        public Interact BringHorse { get; set; }
        public Blip Blip { get; set; }
        public Npc Npc { get; set; }
        public List<Vector3> SpawnPoints { get; set; }
        public bool IsNear { get; set; }
        // public NPC.NPCData NPCsInstance { get; set; }
        public NpcManager.Npc NPCsInstance { get; set; }
    }
}