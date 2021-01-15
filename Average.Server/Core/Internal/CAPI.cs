using CitizenFX.Core;
using System;

namespace Server.Core.Internal
{
    public static class CAPI
    {
        public static string RandomString()
        {
            var g = Guid.NewGuid();
            var guid = Convert.ToBase64String(g.ToByteArray());
            guid = guid.Replace("=", "");
            guid = guid.Replace("+", "");
            guid = guid.Replace("/", "");

            return guid;
        }

        public static string GetSteamId(Player player) => player.Identifiers["steam"];
        public static string GetRockstarId(Player player) => player.Identifiers["license"];
        public static bool HasSteamStarted(Player player) => !string.IsNullOrEmpty(GetSteamId(player));
        public static bool HasRockstarStarted(Player player) => !string.IsNullOrEmpty(GetRockstarId(player));
    }
}
