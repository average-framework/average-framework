using Client.Core.Interfaces;
using Client.Core.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Core.Managers
{
    public class TaskManager : Script
    {
        public class CAction : ITask
        {
            public string Id { get; } = CAPI.RandomString();
            public int Delay { get; }
            public bool DestroyOnFinish { get; }
            public int CurrentRepeat { get; set; }
            public int Repeat { get; }
            public Func<Task> Task { get; set; }
            public Action Action { get; }

            public CAction(int delay, int repeat, bool destroyOnFinish, Action action)
            {
                Delay = delay;
                DestroyOnFinish = destroyOnFinish;
                CurrentRepeat = 0;
                Repeat = repeat;
                Action = action;
            }
        }

        public TaskManager(Main main) : base(main) { }

        private Dictionary<string, CAction> actions = new Dictionary<string, CAction>();

        public ITask GetActionById(string id) => actions[id];
        public bool ActionExists(string id) => actions.ContainsKey(id);

        public void Add(CAction task)
        {
            Func<Task> func = null;
            func = async () =>
            {
                if (task.Delay >= 0)
                {
                    await Delay(task.Delay);
                }

                if (ActionExists(task.Id))
                {
                    if (task.Repeat <= 0)
                    {
                        task.Action.Invoke();

                        if (task.DestroyOnFinish)
                        {
                            DeleteAction(task);
                        }
                    }
                    else if (task.Repeat > 0)
                    {
                        if (task.CurrentRepeat < task.Repeat)
                        {
                            task.CurrentRepeat++;
                            task.Action.Invoke();

                            if (task.CurrentRepeat >= task.Repeat)
                            {
                                DeleteAction(task);
                            }
                        }
                    }
                }
                else
                {
                    Tick -= func;
                }
            };

            if (!ActionExists(task.Id))
            {
                actions.Add(task.Id, task);
                Tick += func;
            }
        }

        public void DeleteAction(CAction task)
        {
            if (ActionExists(task.Id))
            {
                actions.Remove(task.Id);
            }
        }
    }
}
