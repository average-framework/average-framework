using System.Collections.Generic;
using System.IO;
using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Core;
using Server.Models;
using static CitizenFX.Core.Native.API;
using static Server.Core.Internal.Log;

namespace Server.Scripts
{
    public class MapEditor : Script
    {
        private readonly string directory;

        public MapEditor(Main main) : base(main)
        {
            directory = GetResourcePath(GetCurrentResourceName()).Replace("//", "/") + "/scenes/";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Init();
        }

        private void Init()
        {
            InitOnGetAllScenesNameCallback();
        }

        private void InitOnGetAllScenesNameCallback()
        {
            Event(Events.MapEditor.OnGetAllScenesName).On((message, callback) =>
            {
                var scenes = new Dictionary<string, List<MapObject>>();

                foreach (var file in GetSceneFiles())
                {
                    var content = File.ReadAllText(file);
                    var result = JsonConvert.DeserializeObject<Dictionary<string, List<MapObject>>>(content);

                    foreach (var res in result)
                        scenes.Add(res.Key, res.Value);
                }

                callback(scenes);
            });
        }

        private string[] GetSceneFiles()
        {
            return Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories);
        }

        #region Events

        [EventHandler(Events.MapEditor.OnSaveScene)]
        private void OnSaveScene([FromSource] Player player, string sceneName, string json)
        {
            if (File.Exists(directory + sceneName + ".json"))
                File.Delete(directory + sceneName + ".json");

            File.WriteAllText(directory + sceneName + ".json", json);

            Warn($"[MAP EDITOR] Mise a jour de la scene: \"{sceneName}\"");
        }

        [EventHandler(Events.MapEditor.OnDeleteScene)]
        private void OnDeleteScene([FromSource] Player player, string sceneName)
        {
            if (File.Exists(directory + sceneName + ".json"))
                File.Delete(directory + sceneName + ".json");

            Warn($"[MAP EDITOR] Suppression de la scene: \"{sceneName}\"");
        }

        #endregion
    }
}