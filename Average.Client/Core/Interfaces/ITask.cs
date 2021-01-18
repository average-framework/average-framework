using System;
using System.Threading.Tasks;

namespace Client.Core.Interfaces
{
    public interface ITask
    {
         string Id { get; }
         bool DestroyOnFinish { get; }
         Func<Task> Task { get; }
    }
}
