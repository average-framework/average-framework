using Server.Core.Interfaces;
using Server.Core.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Core.Managers
{
    public class TaskManager : Script
    {
        public class CAction : ITask
        {
            public string Id { get; } = CAPI.RandomString();
            public int Delay { get; private set; }
            public bool DestroyOnFinish { get; private set; }
            public int CurrentRepeat { get; set; }
            public int Repeat { get; private set; }
            public Func<Task> Task { get; set; }
            public Action Action { get; private set; }

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

        protected Dictionary<string, CAction> actions = new Dictionary<string, CAction>();

        public ITask GetActionById(string id) => actions[id];
        public bool Exists(string id) => actions.ContainsKey(id);

        public void Add(CAction task)
        {
            Func<Task> func = null;
            func = new Func<Task>(async () =>
            {
                if (task.Delay >= 0)
                {
                    await Delay(task.Delay);
                }

                if (Exists(task.Id))
                {
                    if (task.Repeat <= 0)
                    {
                        task.Action.Invoke();

                        if (task.DestroyOnFinish)
                        {
                            Delete(task);
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
                                Delete(task);
                            }
                        }
                    }
                }
                else
                {
                    Tick -= func;
                }
            });

            if (!Exists(task.Id))
            {
                actions.Add(task.Id, task);
                Tick += func;
            }
        }

        public void Delete(CAction task)
        {
            if (Exists(task.Id))
            {
                actions.Remove(task.Id);
            }
        }
    }
}
