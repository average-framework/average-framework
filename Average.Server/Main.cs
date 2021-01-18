using CitizenFX.Core;
using Server.Core;
using Server.Core.Data;
using Server.Core.Internal;
using Server.Core.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Server.Scripts;

namespace Server
{
    public class Main : BaseScript
    {
        private List<Script> scripts = new List<Script>();

        public static EventHandlerDictionary Handlers { get; private set; }
        public new static PlayerList Players { get; private set; }

        public Main()
        {
            GC.SuppressFinalize(this);

            Handlers = EventHandlers;
            Players = base.Players;

            Console.Clear();

            Watermark();

            Constant.Config = Configuration.ParseToDynamic("config");
            Lang.Init();

            var mysql = Constant.Config["MySQL"];
            MySQL.Connect((string)mysql["Host"], (int)mysql["Port"], (string)mysql["Database"], (string)mysql["Username"], (string)mysql["Password"]);

            if (MySQL.IsOpen)
            {
                Init();
            }
        }

        private void Init()
        {
            LoadScript(new TaskManager(this));
            LoadScript(new UserManager(this));
            LoadScript(new CfxManager(this));
            LoadScript(new PermissionManager(this));
            LoadScript(new CharacterManager(this));
            LoadScript(new DoorManager(this));
            LoadScript(new MapEditor(this));
        }

        #region DO NOT TOUCH THIS

        private void Watermark()
        {
            Log.WriteLine("");
            Log.WriteLine("");
            Log.WriteLine("^11             AAAAAAAA ^15 VVVVV       VVVVV ^12 GGGGGGGGGGGGGG");
            Log.WriteLine("^11            AAAAAAAAA ^15 VVVVV      VVVVV ^12 GGGGGGGGGGGGGGGGGGG");
            Log.WriteLine("^11           AAAAAAAAAA ^15 VVVVV     VVVVV ^12 GGGGGGG      GGGGGGGG");
            Log.WriteLine("^11          AAAAA AAAAA ^15 VVVVV    VVVVV ^12 GGGGGGG         GGGGGGG");
            Log.WriteLine("^11         AAAAA  AAAAA ^15 VVVVV   VVVVV ^12 GGGGGGGGGGG      GGGGGGG");
            Log.WriteLine("^11        AAAAA   AAAAA ^15 VVVVV  VVVVV ^12 GGGGGGGGGGGGG     GGGGGGG");
            Log.WriteLine("^11       AAAAA    AAAAA ^15 VVVVV VVVVV ^12                  GGGGGGGGG");
            Log.WriteLine("^11      AAAAA     AAAAA ^15 VVVVVVVVVV ^12       GGGGGGGGGGGGGGGGGGGG");
            Log.WriteLine("^11     AAAAA      AAAAA ^15 VVVVVVVVV ^12       GGGGGGGGGGGGGGGGGGGG");
            Log.WriteLine("^11    AAAAA       AAAAA ^15 VVVVVVVV ^12       GGGGGGGGGGGGGGGGG");
            Log.WriteLine("   --------------------------------------------------------");

#if DEBUG
            Log.WriteLine($"   | ^15 VERSION ^11 {Assembly.GetExecutingAssembly().GetName().Version} ^15 | STABLE | EARLY | ^14 DEBUG MODE        ^15 |");
#else
            Log.WriteLine($"   | ^15 VERSION {Assembly.GetExecutingAssembly().GetName().Version} | ^11 STABLE ^15 | EARLY | ^12 RELEASE MODE     ^15 |");
#endif

            Log.WriteLine("   --------------------------------------------------------");
            Log.WriteLine("");
            Log.WriteLine("");
        }

        public bool ScriptIsStarted<T>() => GetScript<T>() != null;
        public bool ScriptIsStarted(string scriptName) => GetScript(scriptName) != null;
        public T GetScript<T>() => (T)Convert.ChangeType(scripts.Find(x => x.GetType() == typeof(T)), typeof(T));
        public Script GetScript(string scriptName) => scripts.Find(x => x.Name == scriptName);
        public void LoadScript(Script script)
        {
            if (!scripts.Exists(x => x.Name == script.Name))
            {
                Log.WriteLog((string)Lang.Current["Server.Main.LoadScriptSuccess"], script.Name);

                scripts.Add(script);
                RegisterScript(script);
            }
            else
            {
                Log.WriteLog((string)Lang.Current["Server.Main.LoadScriptError"]);
            }
        }
        public void UnloadScript(Script script)
        {
            if (scripts.Contains(script))
            {
                Log.WriteLog((string)Lang.Current["Server.Main.UnloadScriptSuccess"], script.Name);

                UnregisterScript(script);
                scripts.Remove(script);
            }
            else
            {
                Log.WriteLog((string)Lang.Current["Server.Main.UnloadScriptError"], script.Name);
            }
        }
        #endregion
    }
}
