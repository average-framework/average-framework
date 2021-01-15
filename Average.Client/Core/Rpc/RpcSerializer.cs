using Newtonsoft.Json;
using Shared.Core.Interfaces;

namespace Client.Core.Rpc
{
    public class RpcSerializer : IRpcSerializer
    {
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);
        public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
    }
}
