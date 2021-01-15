using System;

namespace Shared.Core.Interfaces
{
    public interface IRpcHandler
    {
        void Attach(string @event, Delegate callback);
        void Detach(string @event, Delegate callback);
        void Detach(string @event);
    }
}
