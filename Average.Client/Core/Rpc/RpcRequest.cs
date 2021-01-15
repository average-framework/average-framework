using CitizenFX.Core.Native;
using Shared.Core.Interfaces;
using Shared.Core.Rpc;
using System;

namespace Client.Core.Rpc
{
    public class RpcRequest
    {
        protected readonly RpcMessage message = new RpcMessage();
        protected readonly IRpcHandler handler;
        protected readonly IRpcTrigger trigger;
        protected readonly IRpcSerializer serializer;

        public RpcRequest(string @event, RpcHandler handler, IRpcTrigger trigger, IRpcSerializer serializer)
        {
            message.Event = @event;
            this.handler = handler;
            this.trigger = trigger;
            this.serializer = serializer;
        }

        public RpcRequest Target(int playerId)
        {
            message.Target = API.GetPlayerServerId(playerId);
            return this;
        }

        public void Emit() => trigger.Trigger(message);

        public void Emit(params object[] args)
        {
            foreach (var arg in args) message.Payloads.Add(arg);
            trigger.Trigger(message);
        }

        public void On(Action action) => handler.Attach(message.Event, action);
        public void On<T1>(Action<T1> action) => handler.Attach(message.Event, action);
        public void On<T1, T2>(Action<T1, T2> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3>(Action<T1, T2, T3> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) => handler.Attach(message.Event, action);
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) => handler.Attach(message.Event, action);

        public RpcRequest On(Action<RpcMessage> action)
        {
            var c = new Action<string>((response) =>
            {
                var message = serializer.Deserialize<RpcMessage>(response);
                action(message);

                handler.Detach(message.Event);
            });

            if (message.Target == -1)
            {
                message.Target = API.GetPlayerServerId(API.PlayerId());
            }

            handler.Attach(message.Event, c);
            return this;
        }
    }
}
