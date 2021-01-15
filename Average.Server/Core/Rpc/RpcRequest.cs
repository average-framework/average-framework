using CitizenFX.Core;
using Shared.Core.Interfaces;
using Shared.Core.Rpc;
using System;
using System.Collections.Generic;

namespace Server.Core.Rpc
{
    public class RpcRequest
    {
        protected readonly RpcMessage message = new RpcMessage();
        protected readonly IRpcHandler handler;
        protected readonly IRpcTrigger trigger;
        protected readonly IRpcSerializer serializer;

        public delegate object RpcCallback(params object[] args);

        public RpcRequest(string @event, IRpcHandler handler, IRpcTrigger trigger, IRpcSerializer serializer)
        {
            message.Event = @event;
            this.handler = handler;
            this.trigger = trigger;
            this.serializer = serializer;
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

        public void On(Action<RpcMessage, RpcCallback> callback)
        {
            var c = new Action<string>((request) =>
            {
                var message = serializer.Deserialize<RpcMessage>(request);

                if (message.Payloads == null)
                {
                    message.Payloads = new List<object>();
                }

                handler.Detach(message.Event);

                callback(message, new RpcCallback((args) =>
                {
                    message.Payloads = new List<object>();
                    foreach (var arg in args) message.Payloads.Add(arg);
                    trigger.Trigger(message);

                    return args;
                }));
            });

            handler.Attach(message.Event, c);
        }

        public RpcRequest Target(Player player)
        {
            message.Target = int.Parse(player.Handle);
            return this;
        }

        public void Emit(RpcMessage message) => trigger.Trigger(message);

        public void Emit(params object[] args)
        {
            foreach (var arg in args) message.Payloads.Add(arg);
            trigger.Trigger(message);
        }
    }
}
