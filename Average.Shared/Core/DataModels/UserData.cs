namespace Shared.DataModels
{
    public class UserData
    {
        public string RockstarId { get; set; }
        public string Name { get; set; }
        public PermissionData Permission { get; set; } = new PermissionData();
        public int IsBanned { get; set; }
        public int IsWhitelisted { get; set; }
        public int IsConnected { get; set; }
        public string FirstConnection { get; set; }
        public string LastConnection { get; set; }
    }
}
