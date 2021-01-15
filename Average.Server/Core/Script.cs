using CitizenFX.Core;
using Server.Core.Internal;
using Server.Core.Rpc;
using System;

namespace Server.Core
{
    public abstract class Script : BaseScript, IDisposable
    {
        public Main Main { get; }
        public string Name { get; private set; }
        public dynamic Config { get; private set; }

        public Script(Main main, string configFile = "")
        {
            Main = main;
            Name = GetType().Name;

            if (!string.IsNullOrEmpty(configFile))
            {
                Config = Configuration.ParseToDynamic("config/" + configFile);
            }
        }

        public RpcRequest Event(string @event) => new RpcRequest(@event, new RpcHandler(), new RpcTrigger(), new RpcSerializer());

        public void Dispose() => Main.UnloadScript(this);
    }
}
