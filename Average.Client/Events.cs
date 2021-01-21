namespace Client
{
    public static class Events
    {
        public static class CFX
        {
            public const string OnResourceStart = "onResourceStart";
            public const string OnResourceStop = "onResourceStop";
            public const string OnClientResourceStart = "onClientResourceStart";
            public const string OnClientResourceStop = "onClientResourceStop";
            public const string OnPopulationPedCreating = "populationPedCreating";
            public const string OnPlayerActivated = "playerActivated";
            public const string OnSessionInitialized = "sessionInitialized";
        }

        public static class Session
        {
            public const string OnPlayerActivated = "playerActivated";
            public const string OnSessionInitialized = "sessionInitialized";
        }

        public static class Map
        {
            public const string OnClientMapStart = "onClientMapStart";
            public const string OnClientMapStop = "onClientMapStop";
            public const string OnClientGameTypeStart = "onClientGameTypeStart";
            public const string OnClientGameTypeStop = "onClientGameTypeStop";
            public const string OnGetMapDirectives = "getMapDirectives";
        }

        public static class Chat
        {
            public const string OnChatMessage = "chatMessage";
            public const string OnAddMessage = "chat:addMessage";
            public const string OnAddTemplate = "chat:addTemplate";
            public const string OnAddSuggestion = "chat:addSuggestion";
            public const string OnRemoveSuggestion = "chat:removeSuggestion";
            public const string OnClear = "chat:clear";
        }

        public static class Menu
        {
            public const string OnItemClicked = "OnItemClicked";
            public const string OnPrevious = "OnPrevious";
        }

        public static class UI
        {
            public const string OnKeyDown = "OnKeyDown";
            public const string OnKeyUp = "OnKeyUp";
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

        public static class HitMenu
        {
            public const string OnContextMenu = "hitmenu/context";
            public const string OnCloseMenu = "hitmenu/close";
            public const string OnKeyUp = "hitmenu/OnKeyUp";
        }
        
        public class MapEditor
        {
            public const string OnGetAllScenesName = "MapEditor.OnGetAllScenesName";
            public const string OnSaveScene = "MapEditor.OnSaveScene";
            public const string OnDeleteScene = "MapEditor.OnDeleteScene";
        }
        
        public class HorseSeller
        {
            public const string OnBuyHorse = "HorseSeller.OnBuyHorse";
            public const string OnBuyHorseComponents = "HorseSeller.OnBuyHorseComponents";
        }
        
        public class Stable
        {
            public const string OnGetPlayerHorses = "Stable.OnGetPlayerHorses";
            public const string OnSaveMyHorse = "Stable.OnSaveMyHorse";
            public const string OnPlayerDisconnect = "Stable.OnPlayerDisconnect";
        }
    }
}
