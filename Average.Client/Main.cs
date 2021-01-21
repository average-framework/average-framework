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
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Client.Models;
using Client.Scripts;
using HorseSeller = Client.Scripts.HorseSeller;
using Stable = Client.Scripts.Stable;

namespace Client
{
    public class Main : BaseScript
    {
        protected List<Script> scripts = new List<Script>();

        public static EventHandlerDictionary Handlers { get; private set; }
        public static Func<Task> tick;
        
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
            LoadScript(new MapEditor(this));
            LoadScript(new Stable(this));
            LoadScript(new HorseSeller(this));
        }

#if DEBUG
        [Command("debug.gotow")]
        private async void GotowCommand()
        {
            var ped = API.PlayerPedId();
            var pos = API.GetWaypointCoords();

            await NUI.FadeOut(250);

            for (int i = (int)API.GetHeightmapBottomZForPosition(pos.X, pos.Y) - 10; i < 1000; i++)
            {
                API.FreezeEntityPosition(ped, true);
                API.SetEntityCoords(ped, pos.X, pos.Y, i, true, true, true, false);
                var rayHandle = API.StartShapeTestRay(pos.X, pos.Y, i, pos.X, pos.Y, i - 2f, -1, ped, 0);
                var hitd = false;
                var endCoords = Vector3.Zero;
                var surfaceNormal = Vector3.Zero;
                var entityHit = 0;

                API.GetShapeTestResult(rayHandle, ref hitd, ref endCoords, ref surfaceNormal, ref entityHit);

                if (hitd)
                {
                    API.SetEntityCoords(ped, pos.X, pos.Y, i, true, true, true, false);
                    API.FreezeEntityPosition(ped, false);
                    break;
                }

                await Delay(0);
            }

            await NUI.FadeIn(250);
        }        
#endif
        
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
