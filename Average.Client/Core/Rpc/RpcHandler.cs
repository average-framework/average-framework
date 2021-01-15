using Shared.Core.Interfaces;
using System;

namespace Client.Core.Rpc
{
    public class RpcHandler : IRpcHandler
    {
        public void Attach(string @event, Delegate callback) => Main.Handlers[@event] += callback;
        public void Detach(string @event, Delegate callback) => Main.Handlers[@event] -= callback;
        public void Detach(string @event) => Main.Handlers.Remove(@event);
    }
}
