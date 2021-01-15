using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Core.Data;
using Server.Core.Internal;
using Shared.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Server.Core.Internal.CAPI;

namespace Server.Core.Managers
{
    public class CharacterManager : Script
    {
        protected bool readyForUpdate = true;
        protected Dictionary<string, CharacterData> characters { get; } = new Dictionary<string, CharacterData>();

        public CharacterManager(Main main) : base(main)
        {
            Event(Events.Character.OnExist).On((message, callback) => callback(Exists(Players[message.Target])));
            Event(Events.Character.OnLoad).On((message, callback) => callback(Get(Players[message.Target])));

            LoadChararacters();

            Main.GetScript<TaskManager>().Add(new TaskManager.CAction(60000, 0, false, Save));
        }

        private void LoadChararacters()
        {
            foreach (var character in MySQL.SelectMultiples<CharacterData>("SELECT * FROM characters"))
            {
                characters.Add(character.CharacterId, character);
            }

            Log.WriteLog((string)Lang.Current["Server.Character.Loaded"]);
        }

        public bool Exists(Player player) => characters.Values.ToList().Find(x => x.RockstarId == GetRockstarId(player)) == null ? false : true;
        public CharacterData Get(Player player) => characters.ToList().Find(x => x.Value.RockstarId == GetRockstarId(player)).Value;

        private void Save()
        {
            if (readyForUpdate)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    var character = characters.ElementAt(i).Value;
                    MySQL.Update("characters", $"CharacterId='{character.CharacterId}'", character);
                }

                Log.WriteLog((string)Lang.Current["Server.Character.OnSave"]);
            }
        }

        private void CreateCharacter(CharacterData data) => MySQL.Insert("characters", data);

        #region Events

        [EventHandler(Events.Character.OnSave)]
        private async void OnSaveEvent(string json)
        {
            readyForUpdate = false;

            var character = JsonConvert.DeserializeObject<CharacterData>(json);

            if (characters.ContainsKey(character.CharacterId))
            {
                characters[character.CharacterId] = character;
            }
            else
            {
                characters.Add(character.CharacterId, character);
                CreateCharacter(character);
            }

            await Delay(0);
            readyForUpdate = true;
        }

        [EventHandler(Events.Character.OnSetMoney)]
        private void OnSetMoneyEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnSetMoney, amount);

        [EventHandler(Events.Character.OnSetBank)]
        private void OnSetBankEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnSetBank, amount);

        [EventHandler(Events.Character.OnAddMoney)]
        private void OnAddMoneyEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnAddMoney, amount);

        [EventHandler(Events.Character.OnAddBank)]
        private void OnAddBankEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnAddBank, amount);

        [EventHandler(Events.Character.OnRemoveMoney)]
        private void OnRemoveMoneyEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnRemoveMoney, amount);

        [EventHandler(Events.Character.OnRemoveBank)]
        private void OnRemoveBankEvent(int player, decimal amount) => Players[player].TriggerEvent(Events.Character.OnRemoveBank, amount);

        #endregion
    }
}
