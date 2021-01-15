using Client.Core.Extensions;
using Shared.DataModels;
using System.Threading.Tasks;

namespace Client.Core
{
    public class User : Script
    {
        public UserData Data { get; private set; }

        public User(Main main) : base(main) => Load();

        public async Task IsReady()
        {
            while (Data == null) await Delay(0);
        }

        public void Load() => Event(Events.User.OnGetUserData).On((message) => Data = message.Payloads[0].Convert<UserData>()).Emit();
    }
}
