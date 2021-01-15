using CitizenFX.Core;
using Server.Core.Data;
using Shared.DataModels;
using System;
using static Server.Core.Internal.CAPI;

namespace Server.Core
{
    public class UserManager : Script
    {
        public UserManager(Main main) : base(main, "user")
        {
            Event(Events.User.OnGetUserData).On((message, callback) => callback(Get(Players[message.Target])));
        }

        #region Methods

        public UserData Get(Player player) => MySQL.Select<UserData>($"SELECT * FROM users WHERE RockstarId=\"{GetRockstarId(player)}\"");
        public bool Exists(Player player) => MySQL.Exists($"SELECT * FROM users WHERE RockstarId='{GetRockstarId(player)}'");
        public void CreateAccount(Player player) => MySQL.Insert("users", new UserData
        {
            RockstarId = GetRockstarId(player),
            Name = player.Name,
            IsBanned = 0,
            IsWhitelisted = (int)Config["DefaultPlayerIsWhitelisted"],
            IsConnected = 0,
            FirstConnection = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
            LastConnection = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
            Permission = new PermissionData((string)Config["DefaultPlayerPermissionName"], (int)Config["DefaultPlayerPermissionLevel"])
        });
        public void UpdateLastConnectionTime(UserData data)
        {
            data.LastConnection = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
            Save(data);
        }
        public void UpdateConnectionState(UserData data, bool isConnected)
        {
            data.IsConnected = isConnected ? 1 : 0;
            Save(data);
        }
        public void Save(UserData data) => MySQL.Update("users", $"RockstarId='{data.RockstarId}'", data);
        public DateTime GetLastConnectionTime(Player player) => DateTime.Parse(Get(player).LastConnection).ToLocalTime();

        #endregion
    }
}
