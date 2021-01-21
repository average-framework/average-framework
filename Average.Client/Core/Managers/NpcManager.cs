using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client.Core.Internal;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Managers
{
    public class NpcManager : Script
    {
        public class Npc
        {
            public int Handle { get; set; }

            public Vector3 Position
            {
                get => GetEntityCoords(Handle, true, true);
                set => SetEntityCoords(Handle, value.X, value.Y, value.Z, true, true, true, false);
            }

            public float Heading
            {
                get => GetEntityHeading(Handle);
                set => SetEntityHeading(Handle, value);
            }

            public Npc(int handle)
            {
                Handle = handle;
            }
        }
        
        private readonly List<Npc> npcs = new List<Npc>();

        public NpcManager(Main main) : base(main){}

        public async Task<Npc> Create(uint model, int variation, Vector3 position, float heading, bool isNetwork = false, bool netMissionEntity = false)
        {
            if (!HasModelLoaded(model))
            {
                CAPI.RequestModel(model);

                while (!HasModelLoaded(model)) await Delay(250);
            }

            var handle = CreatePed(model, position.X, position.Y, position.Z, heading, isNetwork, netMissionEntity,
                false, false);
            
            SetBlockingOfNonTemporaryEvents(handle, true);
            SetEntityVisible(handle, true);
            SetEntityInvincible(handle, true);
            SetPedCanBeTargetted(handle, false);
            SetPedCanPlayGestureAnims(handle, 1, 1);
            SetPedCanRagdoll(handle, false);
            FreezeEntityPosition(handle, true);
            CAPI.SetPedOutfitPreset(handle, variation);

            var npc = new Npc(handle);
            npcs.Add(npc);

            return npc;
        }

        public Npc Get(int handle)
        {
            return npcs.Find(x => x.Handle == handle);
        }

        public bool Exists(int handle)
        {
            return npcs.Exists(x => x.Handle == handle);
        }

        public void Delete(int handle)
        {
            if (Exists(handle))
            {
                if (DoesEntityExist(handle)) DeleteEntity(ref handle);

                npcs.Remove(Get(handle));
            }
        }

        #region Events

        [EventHandler(Events.CFX.OnResourceStop)]
        private void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName)
                foreach (var npc in npcs)
                    Delete(npc.Handle);
        }

        #endregion
    }
}