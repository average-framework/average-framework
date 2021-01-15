using System;
using System.Threading.Tasks;

namespace Client.Core.Interfaces
{
    public interface ITask
    {
        public string Id { get; }
        public bool DestroyOnFinish { get; }
        public Func<Task> Task { get; }
    }
}
