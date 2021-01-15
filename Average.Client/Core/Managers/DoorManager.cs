using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core.Enums;
using Client.Core.Extensions;
using Client.Core.Internal;
using Client.Core.UI;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Managers
{
    public class DoorManager : Script
    {
        public class Door
        {
            public Vector3 Position { get; }
            public bool IsLocked { get; set; }
            public float Range { get; }
            public string JobName { get; }
            public bool IsNear { get; set; }

            public Door(Vector3 position, bool isLocked, float range, string jobName)
            {
                Position = position;
                IsLocked = isLocked;
                Range = range;
                JobName = jobName;
            }
        }

        protected PermissionManager permission;
        protected CharacterManager character;

        protected List<Door> doors = new List<Door>();

        public DoorManager(Main main) : base(main)
        {
            Constant.DoorsInfos = Configuration<DoorInfoModelConfig>.Parse("utils/doors_infos");

            permission = Main.GetScript<PermissionManager>();
            character = Main.GetScript<CharacterManager>();

            Init();
        }

        private void Init()
        {
            Task.Factory.StartNew(async () =>
            {
                Load();

                await IsReady();
                await character.IsReady();

                var task = Main.GetScript<TaskManager>();
                task.Add(new TaskManager.CAction(-1, 0, false, Update));
            });
        }

        public async Task IsReady()
        {
            while (doors == null)
            {
                await Delay(0);
            }
        }

        private void Load()
        {
            Event(Events.Door.GetDoors).On((message) =>
            {
                foreach (var arg in message.Payloads)
                {
                    var door = arg.Convert<Door>();
                    doors.Add(door);
                    SetDefaultDoorState(door);
                }
            }).Emit();
        }

        private void SetDoorState(Vector3 position)
        {
            TriggerServerEvent(Events.Door.SetDoorState, position);
        }

        private void SetDefaultDoorState(Door door)
        {
            TriggerServerEvent(Events.Door.SetDefaultDoorState, door.Position, door.IsLocked);
        }

        private bool IsAuthorizedToSetDoorState(string jobName, Door door)
        {
            if (permission.HasPermission("owner"))
            {
                return true;
            }
            else
            {
                return jobName == door.JobName;
            }
        }

        private DoorInfoModel GetDoorModel(Vector3 position)
        {
            return Constant.DoorsInfos.Doors.Find(x =>
              Math.Round(x.X) == Math.Round(position.X) &&
              Math.Round(x.Y) == Math.Round(position.Y) &&
              Math.Round(x.Z) == Math.Round(position.Z));
        }

        private void Update()
        {
            var pos = GetEntityCoords(PlayerPedId(), true, true);

            for (int i = 0; i < doors.Count; i++)
            {
                if (GetDistanceBetweenCoords(doors[i].Position.X, doors[i].Position.Y, doors[i].Position.Z, pos.X, pos.Y, pos.Z, true) <= doors[i].Range)
                {
                    if (IsAuthorizedToSetDoorState(character.Data.Job.Name, doors[i]))
                    {
                        if (!doors[i].IsNear)
                        {
                            doors[i].IsNear = true;

                            Hud.SetMessage(Lang.Current["Client.DoorManager.IsAuthorizedLeft"], Lang.Current["Client.DoorManager.IsAuthorizedKey"], Lang.Current["Client.DoorManager.IsAuthorizedRight"].ToString().Replace("{0}", doors[i].IsLocked ? Lang.Current["Client.DoorManager.IsNotLocked"] : Lang.Current["Client.DoorManager.IsLocked"]));
                            Hud.SetVisibility(true);
                            Hud.SetContainerVisible(true);
                            Hud.SetHelpTextVisible(true);
                            Hud.SetPlayerVisible(false, 0);
                            Hud.SetHorseVisible(false, 0);
                        }

                        if (IsControlJustReleased(0, (uint)Keys.X))
                        {
                            SetDoorState(doors[i].Position);
                        }
                    }
                }
                else
                {
                    if (doors[i].IsNear)
                    {
                        doors[i].IsNear = false;

                        Hud.SetContainerVisible(false);
                        Hud.SetHelpTextVisible(false);
                    }
                }
            }
        }

        #region Events

        [EventHandler(Events.Door.SetDoorState)]
        private void SetDoorStateEvent(Vector3 position, bool isLocked)
        {
            var door = doors.Find(x =>
               Math.Round(x.Position.X) == Math.Round(position.X) &&
               Math.Round(x.Position.Y) == Math.Round(position.Y) &&
               Math.Round(x.Position.Z) == Math.Round(position.Z));

            door.IsLocked = isLocked;
            Hud.SetMessage(Lang.Current["Client.DoorManager.IsAuthorizedLeft"], Lang.Current["Client.DoorManager.IsAuthorizedKey"], Lang.Current["Client.DoorManager.IsAuthorizedRight"].ToString().Replace("{0}", isLocked ? Lang.Current["Client.DoorManager.IsNotLocked"] : Lang.Current["Client.DoorManager.IsLocked"]));

            Function.Call((Hash)0xD99229FE93B46286, uint.Parse(GetDoorModel(position).Hash), 1, 0, 0, 0, 0, 0);
            CAPI.DoorSystemSetDoorState(uint.Parse(GetDoorModel(position).Hash), isLocked);
        }

        #endregion
    }
}