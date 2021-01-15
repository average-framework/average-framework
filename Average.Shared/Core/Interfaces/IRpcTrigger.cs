using Shared.Core.Rpc;

namespace Shared.Core.Interfaces
{
    public interface IRpcTrigger
    {
        void Trigger(RpcMessage message);
    }
}
