using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Core;
using Server.Core.Data;
using Server.Core.Internal;
using Server.Core.Managers;
using Shared.Core.DataModels;

namespace Server.Scripts
{
    public class Stable : Script
    {
        private readonly bool readyForUpdate = true;
        private readonly Dictionary<string, List<HorseData>> horses = new Dictionary<string, List<HorseData>>();
        private readonly Dictionary<string, List<int>> horsesToDeleteOnDisconnect = new Dictionary<string, List<int>>();

        public Stable(Main main) : base(main)
        {
            Event(Events.Stable.OnGetPlayerHorses).On((message, callback) =>
            {
                var rockstarId = Players[message.Target].Identifiers["license"];

                if (horses.ContainsKey(rockstarId))
                    callback(horses[rockstarId]);
                else
                    callback("[]");
            });

            LoadHorses();
            
            var task = Main.GetScript<TaskManager>();
            task.Add(new TaskManager.CAction(60000, 0, false, Save));
        }

        private async void Save()
        {
            while (!readyForUpdate) await Delay(0);

            for (var i = 0; i < horses.Count; i++)
            {
                var player = horses.ElementAt(i);
                
                for (var o = 0; o < player.Value.Count; o++)
                {
                    var horse = player.Value.ElementAt(o);
                    MySQL.InsertOrUpdate("horses", horse);
                }
            }

            Log.WriteLog("Horses Saved");
        }

        private void LoadHorses()
        {
            foreach (var horse in MySQL.SelectMultiples<HorseData>("SELECT * FROM horses"))
                if (!horses.ContainsKey(horse.RockstarId))
                    horses.Add(horse.RockstarId, new List<HorseData> {horse});
                else
                    horses[horse.RockstarId].Add(horse);

            Log.WriteLine("Horses Loaded");
        }
        
        // void InitOnGetPlayerHorsesToDeleteCallback()
        // {
        //     Event(Events.Stable.OnGetPlayerHorsesToDelete).On((message, callback) =>
        //     {
        //         var rockstarId = Players[message.Target].Identifiers["steam"];
        //         callback(horsesToDeleteOnDisconnect[rockstarId]);
        //     });
        //     
        //     // Player tempPlayer = null;
        //     //
        //     // EventCallback.Register(Events.Stable.OnGetPlayerHorsesToDelete,
        //     //     player => tempPlayer = Players[player],
        //     //     () => JsonConvert.SerializeObject(horsesToDeleteOnDisconnect[tempPlayer.Identifiers["steam"]]));
        // }

        public void DeleteHorseForAllPlayer(Player player)
        {
            var rockstarId = player.Identifiers["license"];
            
            if (horsesToDeleteOnDisconnect.ContainsKey(rockstarId))
            {
                var horses = horsesToDeleteOnDisconnect[rockstarId];

                for (var i = 0; i < horses.Count; i++)
                {
                    var horse = horses[i];
                    TriggerClientEvent(Events.Stable.OnPlayerDisconnect, horse);
                }

                horsesToDeleteOnDisconnect.Remove(rockstarId);
            }
        }

        #region Events

        [EventHandler(Events.Stable.OnSaveMyHorse)]
        private async void OnSaveEvent(string json)
        {
            var horseData = JsonConvert.DeserializeObject<HorseData>(json);

            if (horses.ContainsKey(horseData.RockstarId))
            {
                var horse = horses[horseData.RockstarId].Find(x => x.Id == horseData.Id);

                if (horse != null)
                {
                    // Si le cheval existe, on le supprime l'ancien et ajoute le nouveau 
                    horses[horseData.RockstarId].Remove(horse);
                    horses[horseData.RockstarId].Add(horseData);
                }
                else
                {
                    // Si le cheval n'existe pas, on l'ajoute
                    horses[horseData.RockstarId].Add(horseData);
                }
            }
            else
            {
                horses.Add(horseData.RockstarId, new List<HorseData> {horseData});
            }

            await Delay(0);
        }

        [EventHandler(Events.Stable.OnPlayerDisconnect)]
        private void OnPlayerDisconnectEvent([FromSource] Player player, int netId)
        {
            var rockstarId = player.Identifiers["license"];

            if (horsesToDeleteOnDisconnect.ContainsKey(rockstarId))
                horsesToDeleteOnDisconnect[rockstarId].Add(netId);
            else
                horsesToDeleteOnDisconnect.Add(rockstarId, new List<int> {netId});
        }
        
        [EventHandler(Events.CFX.OnPlayerDisconnecting)]
        private void OnPlayerDisconnecting([FromSource] Player player, string reason)
        {
            DeleteHorseForAllPlayer(player);
        }

        #endregion
    }
}