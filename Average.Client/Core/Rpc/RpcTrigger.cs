using CitizenFX.Core;
using Shared.Core.Interfaces;
using Shared.Core.Rpc;

namespace Client.Core.Rpc
{
    public class RpcTrigger : IRpcTrigger
    {
        public void Trigger(RpcMessage message)
        {
            BaseScript.TriggerServerEvent(message.Event, message.ToJson());
        }
    }
}
