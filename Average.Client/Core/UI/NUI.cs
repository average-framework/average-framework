using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core.UI
{
    public static class NUI
    {
        public static void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            RegisterNuiCallbackType(msg);

            Main.Handlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) => callback.Invoke(body, resultCallback));
        }

        public static async void Execute(object obj)
        {
            await BaseScript.Delay(0);
            SendNuiMessage(JsonConvert.SerializeObject(obj));
        }

        public static async Task FadeOut(int duration = 1000)
        {
            DoScreenFadeOut(duration);

            while (!IsScreenFadedOut())
            {
                await BaseScript.Delay(0);
            }
        }

        public static async Task FadeIn(int duration = 1000)
        {
            DoScreenFadeIn(duration);

            while (!IsScreenFadedIn())
            {
                await BaseScript.Delay(0);
            }
        }

        public static async void Focus(bool cursor, bool focus)
        {
            await BaseScript.Delay(0);
            SetNuiFocus(focus, cursor);
        }

        public static void Visibility(string name, int duration, bool visible) => Execute(new
        {
            request = "ui.visibility",
            name,
            duration,
            visible
        });

        public static void EnableConsole(bool enable) => Execute(new
        {
            request = "ui.enableConsole",
            enable
        });
    }
}
