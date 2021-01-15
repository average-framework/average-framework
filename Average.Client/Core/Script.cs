using CitizenFX.Core;
using Client.Core.Internal;
using Client.Core.Rpc;
using System;

namespace Client.Core
{
    public abstract class Script : BaseScript, IDisposable
    {
        public Main Main { get; }
        public string Name { get; }
        public dynamic Config { get; }

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
