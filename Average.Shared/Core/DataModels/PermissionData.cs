namespace Shared.DataModels
{
    public class PermissionData
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public PermissionData() { }

        public PermissionData(string name, int level)
        {
            Name = name;
            Level = level;
        }
    }
}
