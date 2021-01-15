using System;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface ITask
    {
        public string Id { get; }
        public bool DestroyOnFinish { get; }
        public Func<Task> Task { get; }
    }
}
