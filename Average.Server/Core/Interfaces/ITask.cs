using System;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface ITask
    {
         string Id { get; }
         bool DestroyOnFinish { get; } 
         Func<Task> Task { get; }
    }
}
