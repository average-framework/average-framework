using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Internal
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
                Log.WriteLog((string)Lang.Current["Client.Config.FileNotFound"]);
                throw new FileNotFoundException();
            }
        }

        public static dynamic ParseToDynamic(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JsonConvert.DeserializeObject(json);
            }
            catch
            {
                Log.WriteLog((string)Lang.Current["Client.Config.FileNotFound"]);
                throw new FileNotFoundException();
            }
        }

        public static Dictionary<string, string> ParseToDictionary(string filePath)
        {
            try
            {
                var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch
            {
                Log.WriteLog((string)Lang.Current["Client.Config.FileNotFound"]);
                throw new FileNotFoundException();
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
                Log.WriteLog((string)Lang.Current["Client.Config.FileNotFound"]);
                throw new FileNotFoundException();
            }
        }
    }

    public static class Configuration<T>
    {
        public static T Parse(string filePath)
        {
#if DEBUG
            Log.WriteLog((string)Lang.Current["Client.Config.FileLoaded"], filePath);
#endif
            var json = LoadResourceFile(GetCurrentResourceName(), filePath + ".json");
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
