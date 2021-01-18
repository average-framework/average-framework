using Client.Core.Internal;
using Client.Core.UI;

namespace Client
{
    public static class Settings
    {
        public static void Init()
        {
#if DEBUG
            NUI.EnableConsole(true);
#else
            NUI.EnableConsole(false);
#endif
            
            NUI.Focus(false, false);

            // Delete info in right corner in tab menu
            CAPI.SetHudPreset("HUD_CTX_IN_MINIGAME_POKER_OUTRO");
            // Delete minimap and money in hud
            CAPI.SetHudPreset("HUD_CTX_MP_OUT_OF_AREA_BOUNDS");
        }
    }
}
