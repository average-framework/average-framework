using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Core.Rpc
{
    public class RpcMessage
    {
        public string Event { get; set; }
        public List<object> Payloads { get; set; }
        public int Target { get; set; } = -1;
        public DateTime Created { get; set; } = DateTime.Now;

        public RpcMessage() { }
        public RpcMessage(params object[] args) => Payloads = args.ToList();

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
