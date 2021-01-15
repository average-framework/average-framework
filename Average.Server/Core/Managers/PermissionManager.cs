using Server.Core.Data;
using Server.Core.Internal;
using Shared.DataModels;
using System.Collections.Generic;

namespace Server.Core.Managers
{
    public class PermissionManager : Script
    {
        protected List<PermissionData> permissions = new List<PermissionData>();

        public PermissionManager(Main main) : base(main)
        {
            permissions = MySQL.SelectMultiples<PermissionData>("SELECT * FROM permissions");
            Log.WriteLog((string)Lang.Current["Server.Permission.Loaded"]);

            Event(Events.Permission.OnGetPermissions).On((message, callback) => callback(permissions.ToArray()));
        }
    }
}
