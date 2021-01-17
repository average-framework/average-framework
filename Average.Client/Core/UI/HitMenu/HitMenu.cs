using CitizenFX.Core;
using Client.Core.Enums;
using Client.Core.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static Client.Core.Internal.CAPI;

namespace Client.Core.UI.HitMenu
{
    public class HitMenu : Script
    {
        protected UserManager user;
        protected CharacterManager character;
        protected RaycastHit entity;
        protected HitMenuContainer currentMenuContainer;
        protected List<HitMenuContainer> containers = new List<HitMenuContainer>();
        protected bool isFocusActive;

        public bool IsOpen { get; private set; }
        public float TargetRange { get; set; } = 6f;

        public HitMenu(Main main) : base(main)
        {
            user = Main.GetScript<UserManager>();
            character = Main.GetScript<CharacterManager>();

            Init();
        }

        void Init()
        {
            NUI.RegisterNUICallback(Events.HitMenu.OnContextMenu, OnContextMenu);
            NUI.RegisterNUICallback(Events.HitMenu.OnCloseMenu, OnCloseMenu);
            NUI.RegisterNUICallback(Events.HitMenu.OnKeyUp, OnKeyUp);

            Task.Factory.StartNew(async () =>
            {
                await user.IsReady();
                await character.IsReady();

                currentMenuContainer = containers.Find(x => x.EntityType == 0 && x.Job == user.Data.Permission.Name);

                var task = Main.GetScript<TaskManager>();
                task.Add(new TaskManager.CAction(250, 0, false, Update));
                task.Add(new TaskManager.CAction(-1, 0, false, UpdateKeyboard));
            });
        }

        void Update()
        {
            var ped = PlayerPedId();
            entity = GetTarget(ped, TargetRange);

            if (entity.EntityHit != 0)
            {
                if (!isFocusActive)
                {
                    isFocusActive = true;

                    NUI.Execute(new
                    {
                        request = "hitmenu.crosshair",
                        crosshair = true
                    });
                }
            }
            else
            {
                if (isFocusActive)
                {
                    isFocusActive = false;

                    NUI.Execute(new
                    {
                        request = "hitmenu.crosshair",
                        crosshair = false
                    });
                }
            }

            if (!entity.Hit)
            {
                currentMenuContainer = containers.Find(x => x.EntityType == 0 && x.Job == "default_" + user.Data.Permission.Name);
            }
            else
            {
                if (entity.EntityHit != 0)
                {
                    currentMenuContainer = containers.Find(x => (int)x.EntityType == entity.EntityType && x.Job == character.Data.Job.Name);
                }
            }
        }

        void UpdateKeyboard()
        {
            if (currentMenuContainer != null)
            {
                if (IsControlJustReleased(0, (uint)Keys.N))
                {
                    Open(currentMenuContainer);
                }
            }
        }

        public void CreateMenu(HitMenuContainer menu) => containers.Add(menu);

        public void DeleteMenu(HitMenuContainer menu)
        {
            var idx = containers.IndexOf(menu);
            containers.RemoveAt(idx);
        }

        public void Open(HitMenuContainer menu)
        {
            IsOpen = true;

            NUI.Execute(new
            {
                request = "hitmenu.open",
                options = menu.Items
            });
            NUI.Focus(true, true);
        }

        public void Close()
        {
            IsOpen = false;

            NUI.Execute(new
            {
                request = "hitmenu.close"
            });
            NUI.Focus(false, false);
        }

        #region NUI Callbacks

        CallbackDelegate OnContextMenu(IDictionary<string, object> data, CallbackDelegate result)
        {
            var menuid = data["menu_id"].ToString();
            var id = data["id"].ToString();
            var menu = containers.Find(x => x.Id == menuid);

            if (menu != null)
            {
                var option = menu.GetItem(id);

                if (option != null)
                {
                    if (option.Action != null)
                    {
                        option.Action.Invoke(entity);

                        if (option.CloseMenuOnAction)
                        {
                            Close();
                        }
                    }
                }
            }

            return result;
        }

        CallbackDelegate OnCloseMenu(IDictionary<string, object> data, CallbackDelegate result)
        {
            NUI.Focus(false, false);
            return result;
        }

        CallbackDelegate OnKeyUp(IDictionary<string, object> data, CallbackDelegate result)
        {
            var key = int.Parse(data["key"].ToString());

            if (key == 27)
            {
                if (IsOpen)
                {
                    Close();
                }
            }

            return result;
        }

        #endregion
    }
}
