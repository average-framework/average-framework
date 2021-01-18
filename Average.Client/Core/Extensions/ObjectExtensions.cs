using Newtonsoft.Json;

namespace Client.Core.Extensions
{
    public static class ObjectExtentions
    {
        public static T Convert<T>(this object source)
        {
            return JsonConvert.DeserializeObject<T>(source.ToString());
        }
    }
}
