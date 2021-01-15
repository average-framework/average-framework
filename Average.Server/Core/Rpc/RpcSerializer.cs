using Newtonsoft.Json;
using Shared.Core.Interfaces;

namespace Server.Core.Rpc
{
    public class RpcSerializer : IRpcSerializer
    {
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);
        public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
    }
}
