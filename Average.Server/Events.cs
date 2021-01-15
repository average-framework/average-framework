namespace Server
{
    public static class Events
    {
        public static class CFX
        {
            public const string OnPlayerConnecting = "playerConnecting";
            public const string OnPlayerDisconnecting = "playerDropped";
            public const string OnRconCommand = "rconCommand";
            public const string OnResourceListRefresh = "onResourceListRefresh";
        }

        public static class Character
        {
            public const string OnSave = "Character.OnSave";
            public const string OnExist = "Character.OnExist";
            public const string OnLoad = "Character.OnLoad";
            public const string OnSetMoney = "Character.OnSetMoney";
            public const string OnSetBank = "Character.OnSetBank";
            public const string OnAddMoney = "Character.OnAddMoney";
            public const string OnAddBank = "Character.OnAddBank";
            public const string OnRemoveMoney = "Character.OnRemoveMoney";
            public const string OnRemoveBank = "Character.OnRemoveBank";
        }

        public static class User
        {
            public const string OnGetUserData = "User.OnGetUserData";
        }

        public static class Permission
        {
            public const string OnGetPermissions = "Permission.OnGetPermissions";
        }

        public static class Door
        {
            public const string SetDoorState = "Door.OnSetDoorState";
            public const string SetDefaultDoorState = "Door.OnSetDefaultDoorState";
            public const string GetDoors = "Door.OnGetDoors";
        }
    }
}
