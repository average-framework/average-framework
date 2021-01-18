using CitizenFX.Core;
using Client.Core;
using Client.Core.Controllers;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Client.Core.UI.HitMenu;
using Client.Core.UI.Menu;
using System;
using System.Collections.Generic;

namespace Client
{
    public class Main : BaseScript
    {
        protected List<Script> scripts = new List<Script>();

        public static EventHandlerDictionary Handlers { get; private set; }

        public Main()
        {
            GC.SuppressFinalize(this);
            
            Handlers = EventHandlers;

            Constant.Config = Configuration.Parse("config");
            Lang.Init();
            Settings.Init();

            // Start all script in same instance of plugin
            Init();
        }

        private void Init()
        {
            LoadScript(new TaskManager(this));
            LoadScript(new UserManager(this));
            LoadScript(new Menu(this));
            LoadScript(new PermissionManager(this));
            LoadScript(new CharacterManager(this));
            LoadScript(new PlayerController(this));
            LoadScript(new HitMenu(this));
            LoadScript(new BlipManager(this));
            LoadScript(new NpcManager(this));
            LoadScript(new SpawnManager(this));
            LoadScript(new DoorManager(this));
        }

        #region DO NOT TOUCH THIS

        public bool ScriptIsStarted<T>() => GetScript<T>() != null;
        public bool ScriptIsStarted(string scriptName) => GetScript(scriptName) != null;
        public T GetScript<T>() => (T)Convert.ChangeType(scripts.Find(x => x.GetType() == typeof(T)), typeof(T));
        public Script GetScript(string scriptName) => scripts.Find(x => x.Name == scriptName);
        public void LoadScript(Script script)
        {
            if (!scripts.Exists(x => x.Name == script.Name))
            {
                Log.WriteLog(Lang.Current["Client.Main.LoadScriptSuccess"], script.Name);

                scripts.Add(script);
                RegisterScript(script);
            }
            else
            {
                Log.WriteLog(Lang.Current["Client.Main.LoadScriptError"], script.Name);
            }
        }
        public void UnloadScript(Script script)
        {
            if (scripts.Exists(x => x.Name == script.Name))
            {
                Log.WriteLog(Lang.Current["Client.Main.UnloadScriptSuccess"], script.Name);

                UnregisterScript(script);
                scripts.Remove(script);
            }
            else
            {
                Log.WriteLog(Lang.Current["Client.Main.UnloadScriptError"], script.Name);
            }
        }

        #endregion
    }
}
