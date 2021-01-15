using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core.Internal;
using Client.Models;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Managers
{
    public class MapManager : Script
    {
        protected PermissionManager permission;

        protected List<uint> scannedImaps = new List<uint>();

        public MapManager(Main main) : base(main)
        {
            Constant.Imaps = Configuration<List<Imap>>.Parse("utils/imaps_infos");
            Constant.MyImaps = Configuration<List<MyImap>>.Parse("config/my_imaps");
            Constant.Interiors = Configuration<List<Interior>>.Parse("utils/interiors_infos");
            Constant.InteriorsSet = Configuration<List<InteriorSet>>.Parse("utils/interiors_set_infos");
            Constant.MyInteriors = Configuration<List<MyInterior>>.Parse("config/my_interiors");

            permission = Main.GetScript<PermissionManager>();
        }

        public void Load()
        {
            foreach (var imap in Constant.MyImaps)
            {
                var hash = (uint)long.Parse(imap.Hash);

                if (imap.Enabled)
                {
                    if (!IsImapActive(hash))
                    {
                        RequestImap(hash);
                    }
                }
                else
                {
                    if (IsImapActive(hash))
                    {
                        RemoveImap(hash);
                    }
                }
            }

            foreach (var interior in Constant.MyInteriors)
            {
                if (interior.Enable)
                {
                    Load(interior.Id, interior.Set);
                }
                else
                {
                    Unload(interior.Id, interior.Set);
                }
            }
        }

        private void UnloadAll()
        {
            foreach (var imap in Constant.Imaps)
            {
                var hash = uint.Parse(imap.Hash, System.Globalization.NumberStyles.AllowHexSpecifier);

                if (IsImapActive(hash))
                {
                    RemoveImap(hash);
                }
            }
        }

        public void Load(int interior, string entitySetName)
        {
            if (!IsInteriorEntitySetActive(interior, entitySetName))
            {
                Function.Call((Hash)0x174D0AAB11CED739, interior, entitySetName);
            }
        }
        public void Unload(int interior, string entitySetName)
        {
            if (IsInteriorEntitySetActive(interior, entitySetName))
            {
                Function.Call((Hash)0x33B81A2C07A51FFF, interior, entitySetName, true);
            }
        }

        #region Commands

#if DEBUG
        [Command("imap.loadall")]
        private void LoadAllCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("owner"))
            {
                foreach (var imap in Constant.Imaps)
                {
                    var hash = uint.Parse(imap.Hash, System.Globalization.NumberStyles.AllowHexSpecifier);

                    if (!IsImapActive(hash))
                    {
                        RequestImap(hash);
                    }
                }
            }
        }

        [Command("imap.removeall")]
        private void RemoveAllCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("owner"))
            {
                foreach (var imap in Constant.Imaps)
                {
                    var hash = uint.Parse(imap.Hash, System.Globalization.NumberStyles.AllowHexSpecifier);

                    if (IsImapActive(hash))
                    {
                        RemoveImap(hash);
                    }
                }
            }
        }

        [Command("imap.scanproximity")]
        private void ScanProximityCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("owner"))
            {
                scannedImaps.Clear();

                var distance = float.Parse(args[0].ToString());
                var pos = GetEntityCoords(PlayerPedId(), true, true);

                Vector3 position = Vector3.Zero;
                float heading = 0f;
                bool imapExist = Function.Call<bool>((Hash)0x9C77964B0E07B633, 2040843256, position, heading);
                Log.WriteLog("imap info: " + position + ", " + heading + ", " + imapExist);

                UnloadAll();
            }
        }
#endif

        #endregion
    }
}
