using CitizenFX.Core;
using Shared.Core.Interfaces;
using Shared.Core.Rpc;

namespace Server.Core.Rpc
{
    public class RpcTrigger : IRpcTrigger
	{
		public void Trigger(RpcMessage message)
		{
			if(message.Target != -1)
            {
				Main.Players[message.Target].TriggerEvent(message.Event, message.ToJson());
            }
            else
            {
				BaseScript.TriggerClientEvent(message.Event, message.ToJson());
            }
		}
	}
}
