using Newtonsoft.Json.Linq;

namespace Server.Core.Internal
{
    public static class Lang
    {
        public static string Language { get; private set; }
        public static JObject Current { get; private set; }

        public static void Init()
        {
            Language = (string)Constant.Config["Language"];
            Current = Configuration.Parse("languages/" + Language);
        }
    }
}
