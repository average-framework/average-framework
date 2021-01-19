using CitizenFX.Core;
using Server.Core.Internal;
using static Server.Core.Internal.CAPI;

namespace Server.Core.Managers
{
    public class CfxManager : Script
    {
        private UserManager user;

        public CfxManager(Main main) : base(main) => user = Main.GetScript<UserManager>();

        [EventHandler(Events.CFX.OnPlayerConnecting)]
        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            await Delay(0);

            Log.WriteLog((string)Lang.Current["Server.Cfx.OnPlayerConnecting"], playerName);

            if (!HasSteamStarted(player))
            {
                deferrals.done((string)Lang.Current["Server.Cfx.SteamNotStarted"]);
                return;
            }

            if (!HasRockstarStarted(player))
            {
                deferrals.done((string)Lang.Current["Server.Cfx.RockstarNotStarted"]);
                return;
            }

            if (!user.Exists(player))
            {
                await Delay(0);
                user.CreateAccount(player);
                return;
            }
            else
            {
                var userData = user.Get(player);
                await Delay(0);
                user.UpdateLastConnectionTime(userData);

                if (userData.IsBanned == 1)
                {
                    await Delay(0);
                    deferrals.done((string)Lang.Current["Server.Cfx.PlayerIsBanned"]);
                    return;
                }
                
                if ((bool) Constant.Config["UseWhitelistSystem"])
                {
                    if (userData.IsWhitelisted == 0)
                    {
                        await Delay(0);
                        deferrals.done((string)Lang.Current["Server.Cfx.PlayerIsNotWhitelisted"]);
                        return;
                    }
                    else
                    {
                        await Delay(0);
                        user.UpdateConnectionState(userData, true);
                        await Delay(0);
                        
                        deferrals.done();
                    }   
                }
                else
                {
                    await Delay(0);
                    user.UpdateConnectionState(userData, true);
                    await Delay(0);
                    
                    deferrals.done();
                }
            }
        }

        #region Events

        [EventHandler(Events.CFX.OnPlayerDisconnecting)]
        private void OnPlayerDisconnecting([FromSource] Player player, string reason)
        {
            var userData = user.Get(player);

            user.UpdateLastConnectionTime(userData);
            user.UpdateConnectionState(userData, false);

            Log.WriteLog((string)Lang.Current["Server.Cfx.OnPlayerDisconnecting"], player.Name, reason);
        }

        #endregion
    }
}