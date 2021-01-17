using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using static CitizenFX.Core.Native.API;

namespace Server.Core.Internal
{
    public static class Configuration
    {
        public static JObject Parse(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JObject.Parse(json);
            }
            catch
            {
                throw new FileNotFoundException((string)Lang.Current["Server.Config.FileNotFound"]);
            }
        }

        public static dynamic ParseToDynamic(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JsonConvert.DeserializeObject<dynamic>(json);
            }
            catch
            {
                throw new FileNotFoundException((string)Lang.Current["Server.Config.FileNotFound"]);
            }
        }

        public static Dictionary<string, dynamic> ParseToDictionary(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            }
            catch
            {
                throw new FileNotFoundException((string)Lang.Current["Server.Config.FileNotFound"]);
            }
        }

        public static JArray ParseToArray(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JArray.Parse(json);
            }
            catch
            {
                throw new FileNotFoundException((string)Lang.Current["Server.Config.FileNotFound"]);
            }
        }
    }

    public static class Configuration<T>
    {
        public static T Parse(string filePath)
        {
            Log.WriteLog((string)Lang.Current["Server.Config.FileLoaded"], filePath);
            var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
