using System.Collections.Generic;

namespace Client.Core.Internal
{
    public static class Lang
    {
        public static string Language { get; private set; }
        public static Dictionary<string, string> Current { get; private set; }

        public static void Init()
        {
            Language = (string)Constant.Config["Language"];
            Current = Configuration.ParseToDictionary("languages/" + Language);
        }
    }
}
