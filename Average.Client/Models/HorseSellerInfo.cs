using CitizenFX.Core;
using Client.Core.Managers;

namespace Client.Models
{
    public class HorseSellerInfo
    {
        public NpcManager.Npc Npc { get; private set; }
        public BlipManager.Blip Blip { get; private set; }
        public Vector3 InteractPosition { get; private set; }
        public Vector3 BuyPosition { get; private set; }
        public float BuyHeading { get; private set; }
        public Vector3 BuyCameraPosition { get; private set; }
        public Vector3 CustomizationPosition { get; private set; }
        public float CustomizationHeading { get; private set; }
        public Vector3 CustomizationCameraPosition { get; private set; }

        public HorseSellerInfo(NpcManager.Npc npcInstance, BlipManager.Blip blipInstance, Vector3 interactPosition, Vector3 buyPosition, float buyHeading, Vector3 buyCameraPosition, Vector3 customizationPosition, float customizationHeading, Vector3 cameraCustomizationPosition)
        {
            Npc = npcInstance;
            Blip = blipInstance;
            InteractPosition = interactPosition;

            BuyPosition = buyPosition;
            BuyHeading = buyHeading;
            BuyCameraPosition = buyCameraPosition;

            CustomizationPosition = customizationPosition;
            CustomizationHeading = customizationHeading;
            CustomizationCameraPosition = cameraCustomizationPosition;
        }
    }
}