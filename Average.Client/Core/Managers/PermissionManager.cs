using Client.Core.Extensions;
using Shared.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Core.Managers
{
    public class PermissionManager : Script
    {
        protected User user;

        protected List<PermissionData> permissions = new List<PermissionData>();

        public PermissionManager(Main main) : base(main)
        {
            user = Main.GetScript<User>();

            Task.Factory.StartNew(async () =>
            {
                Event(Events.Permission.OnGetPermissions).On((message) => message.Payloads.ForEach((v) => permissions.Add(v.Convert<PermissionData>()))).Emit();

                await IsReady();
                await user.IsReady();
            });
        }

        public async Task IsReady()
        {
            while (permissions == null)
            {
                await Delay(0);
            }
        }

        public bool Exists(string name) => permissions.Exists(x => x.Name == name);
        public bool Exists(int level) => permissions.Exists(x => x.Level == level);
        public bool HasPermission(string name)
        {
            if (Exists(name))
            {
                var permissionLevel = permissions.Find(x => x.Name == name).Level;
                return user.Data.Permission.Level >= permissionLevel;
            }
            else
            {
                return false;
            }
        }
        public bool HasPermission(int level)
        {
            if (Exists(level))
            {
                return user.Data.Permission.Level >= level;
            }
            else
            {
                return false;
            }
        }
    }
}
