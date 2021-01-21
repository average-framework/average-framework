using CitizenFX.Core;
using Server.Core.Internal;
using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Core.Managers
{
    public class DoorManager : Script
    {
        public DoorManager(Main main) : base(main) => Init();

        private void Init()
        {
            Constant.DoorsInfos = Configuration<List<Door>>.Parse("config/my_doors");

            Event(Events.Door.GetDoors).On((message, callback) => callback(Constant.DoorsInfos.ToArray()));
        }

        #region Events

        [EventHandler(Events.Door.SetDoorState)]
        private void SetDoorStateEvent(Vector3 position)
        {
            var door = Constant.DoorsInfos.Find(x =>
                Math.Round(x.Position.X) == Math.Round(position.X) &&
                Math.Round(x.Position.Y) == Math.Round(position.Y) &&
                Math.Round(x.Position.Z) == Math.Round(position.Z));

            if (door == null)
                return;
            
            door.IsLocked = !door.IsLocked;
            TriggerClientEvent(Events.Door.SetDoorState, door.Position, door.IsLocked);   
        }

        [EventHandler(Events.Door.SetDefaultDoorState)]
        private void SetDefaultDoorStateEvent(Vector3 position, bool isLocked)
        {
            var door = Constant.DoorsInfos.Find(x =>
                Math.Round(x.Position.X) == Math.Round(position.X) &&
                Math.Round(x.Position.Y) == Math.Round(position.Y) &&
                Math.Round(x.Position.Z) == Math.Round(position.Z));

            if (door == null)
                return;
            
            door.IsLocked = isLocked;
            TriggerClientEvent(Events.Door.SetDoorState, door.Position, isLocked);
        }

        #endregion
    }
}