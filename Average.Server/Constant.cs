using Newtonsoft.Json.Linq;
using Server.Configs;
using System.Collections.Generic;

namespace Server
{
    public static class Constant
    {
        #region Configs

        public static dynamic Config { get; set; }
        public static DoorConfig DoorsInfos { get; set; }

        #endregion
    }
}
