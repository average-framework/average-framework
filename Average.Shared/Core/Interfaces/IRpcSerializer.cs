namespace Shared.Core.Interfaces
{
    public interface IRpcSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string data);
    }
}
