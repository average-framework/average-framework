using Shared.Core.Interfaces;
using System;

namespace Server.Core.Rpc
{
    public class RpcHandler : IRpcHandler
    {
        public void Attach(string @event, Delegate callback) => Main.Handlers[@event] += callback;
        public void Detach(string @event, Delegate callback) => Main.Handlers[@event] -= callback;
        public void Detach(string @event) => Main.Handlers.Remove(@event);
    }
}
