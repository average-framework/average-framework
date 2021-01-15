using CitizenFX.Core;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;
using static Client.Core.Internal.CAPI;

namespace Client.Core.Managers
{
    public class BlipManager : Script
    {
        public class Blip
        {
            protected int handle;
            protected int sprite;
            protected float scale;
            protected string text;
            protected Vector3 position;

            public int Handle
            {
                get { return handle; }
                set { handle = value; }
            }

            public string Text
            {
                get { return text; }
                set
                {
                    text = value;
                    SetBlipNameFromPlayerString(Handle, value);
                }
            }

            public Vector3 Position
            {
                get { return position; }
                set
                {
                    position = value;
                    SetBlipCoords(Handle, value.X, value.Y, value.Z);
                }
            }

            public float Scale
            {
                get { return scale; }
                set
                {
                    scale = value;
                    SetBlipScale(Handle, value);
                }
            }

            public int Sprite
            {
                get { return sprite; }
                set
                {
                    sprite = value;
                    SetBlipSprite(Handle, value, 1);
                }
            }

            public int AssignedToEntity { get; set; }

            public Blip(int handle, int sprite, string text, float scale, Vector3 position, int assignedToEntity = -1)
            {
                Handle = handle;
                Sprite = sprite;
                Text = text;
                Scale = scale;
                Position = position;
                AssignedToEntity = assignedToEntity;
            }
        }

        protected List<Blip> blips = new List<Blip>();

        public BlipManager(Main main) : base(main) { }

        public Blip Create(int sprite, string text, float scale, Vector3 position, int assignedToEntity = -1)
        {
            var handle = CreateBlip(sprite, text, scale, position);
            var blip = new Blip(handle, sprite, text, scale, position, assignedToEntity);

            blips.Add(blip);

            return blip;
        }
        public Blip GetBlipByAssignation(int entity) => ExistsOnAssignedEntity(entity) ? blips.Find(x => x.AssignedToEntity == entity) : null;
        public Blip GetBlip(int handle) => Exists(handle) ? blips.Find(x => x.Handle == handle) : null;
        public bool Exists(int handle) => blips.Exists(x => x.Handle == handle);
        public bool ExistsOnAssignedEntity(int assignedToEntity) => blips.Exists(x => x.AssignedToEntity == assignedToEntity);
        public void Delete(int handle)
        {
            if (Exists(handle))
            {
                if (DoesBlipExist(handle))
                {
                    RemoveBlip(ref handle);
                }

                blips.Remove(GetBlip(handle));
            }
        }

        #region Events

        [EventHandler(Events.CFX.OnResourceStop)]
        private void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName)
            {
                foreach (var blip in blips)
                {
                    Delete(blip.Handle);
                }
            }
        }

        #endregion
    }
}