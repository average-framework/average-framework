using System;

namespace Client.Core.UI
{
    public static class Hud
    {
        public static void SetVisibility(bool value) => NUI.Execute(new { request = "hud_set_visibility", value });
        public static void SetHealth(int value) => NUI.Execute(new { request = "hud_set_health", value });
        public static void SetHunger(int value) => NUI.Execute(new { request = "hud_set_hunger", value });
        public static void SetThirst(int value) => NUI.Execute(new { request = "hud_set_thirst", value });
        public static void SetHorseHealth(int value) => NUI.Execute(new { request = "hud_set_horse_health", value });
        public static void SetHorseStamina(int value) => NUI.Execute(new { request = "hud_set_horse_stamina", value });

        public static void SetTime(TimeSpan value) =>
            NUI.Execute(new { request = "hud_set_time", value = value.ToString(@"hh\:mm\:ss") });

        public static void SetMessage(string left, string key, string right) =>
            NUI.Execute(new { request = "hud_set_message", left, key, right });

        public static void SetPlayerVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "player", value, duration });

        public static void SetHorseVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "horse", value, duration });

        public static void SetHelpTextVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "helptext", value, duration });

        public static void SetDeathScreenVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "deathScreen", value, duration });

        public static void SetCooldownVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "cooldown", value, duration });

        public static void SetContainerVisible(bool value, int duration = 500) =>
            NUI.Execute(new { request = "hud_element_visibility", element = "container", value, duration });
    }
}