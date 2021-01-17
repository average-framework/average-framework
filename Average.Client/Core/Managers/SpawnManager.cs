using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core.Internal;
using Client.Core.UI;
using Client.Scripts;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static Client.Core.Internal.CAPI;

namespace Client.Core.Managers
{
    public class SpawnManager : Script
    {
        protected CharacterManager character;
        protected Vector3 defaultSpawnPosition = Vector3.Zero;
        protected uint defaultModel = (uint)GetHashKey("mp_male");
        protected float defaultSpawnHeading = 0f;
        protected bool? characterExist = false;

        public SpawnManager(Main main) : base(main, "spawn_manager")
        {
            defaultSpawnPosition = new Vector3((float)Config["DefaultSpawnPosition"]["X"], (float)Config["DefaultSpawnPosition"]["Y"], (float)Config["DefaultSpawnPosition"]["Z"]);
            defaultSpawnHeading = (float)Config["DefaultSpawnHeading"];

            Task.Factory.StartNew(async () =>
            {
                character = Main.GetScript<CharacterManager>();

                characterExist = await character.IsCharacterExist();

                if ((bool)characterExist)
                {
                    character.Load();
                }
                else
                {
#if DEBUG
                    CreateCharacter();
#endif
                }
            });
        }

        private async void CreateCharacter()
        {
            NetworkStartSoloTutorialSession();
            SetPlayerModel(defaultModel);
            SetPedOutfitPreset(PlayerPedId(), 0);

            while (!OutfitFullyLoaded(PlayerPedId()))
            {
                await Delay(250);
            }

            UpdatePedVariation();

            await Delay(2500);

            SetModelAsNoLongerNeeded(defaultModel);
            RequestCollisionAtCoord(defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z);

            var time = GetGameTimer();

            while (!HasCollisionLoadedAroundEntity(PlayerPedId()) && (GetGameTimer() - time) < 5000)
            {
                await Delay(250);
            }

            SetEntityCoordsNoOffset(PlayerPedId(), defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z, false, false, false);
            SetEntityHeading(PlayerPedId(), defaultSpawnHeading);

            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z, defaultSpawnHeading, true, true, false);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, PlayerPedId());
            Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, PlayerPedId());

            ClearPlayerWantedLevel(PlayerId());

            SetPedComponentDisabled(PlayerPedId(), 0x3F1F01E5, 0);
            SetPedComponentDisabled(PlayerPedId(), 0xDA0E2C55, 0);
            UpdatePedVariation();

            Main.LoadScript(new CharacterCreator(Main));

            var charCreator = Main.GetScript<CharacterCreator>();
            charCreator.Init(defaultSpawnPosition, defaultSpawnHeading);

            NUI.Visibility("hit-menu", 0, false);

            if (IsLoadingScreenActive())
            {
                ShutdownLoadingScreen();
                while (IsLoadingScreenActive())
                {
                    await Delay(250);
                }
            }

            Dispose();
        }

        public async void SpawnPlayer()
        {
            await NUI.FadeOut(0);

            while (!IsScreenFadedOut())
            {
                await Delay(250);
            }

            await character.IsReady();

            var model = character.Data.SexType == 0 ? (uint)GetHashKey("mp_male") : (uint)GetHashKey("mp_female");

            SetPlayerModel(model);
            SetPedOutfitPreset(PlayerPedId(), 0);

            while (!OutfitFullyLoaded(PlayerPedId()))
            {
                await Delay(250);
            }

            UpdatePedVariation();

            await Delay(5000);

            SetModelAsNoLongerNeeded(model);
            RequestCollisionAtCoord(defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z);

            var time = GetGameTimer();

            while (!HasCollisionLoadedAroundEntity(PlayerPedId()) && (GetGameTimer() - time) < 5000)
            {
                await Delay(250);
            }

            SetEntityCoordsNoOffset(PlayerPedId(), defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z, false, false, false);
            SetEntityHeading(PlayerPedId(), defaultSpawnHeading);

            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, defaultSpawnPosition.X, defaultSpawnPosition.Y, defaultSpawnPosition.Z, defaultSpawnHeading, true, true, false);
            Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, PlayerPedId());
            Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, PlayerPedId());

            ClearPlayerWantedLevel(PlayerId());

            if (IsLoadingScreenActive())
            {
                ShutdownLoadingScreen();
                while (IsLoadingScreenActive())
                {
                    await Delay(250);
                }
            }

            await NUI.FadeIn(500);

            while (!IsScreenFadedIn())
            {
                await Delay(250);
            }

            character.SetPedBody();
            character.SetPedFaceFeatures();
            character.SetPedBodyComponents();
            character.UpdateOverlay();
            character.SetPedClothes();

            while (!HasPedComponentLoaded())
            {
                await Delay(250);
#if DEBUG
                Log.Warn("Loading ped components..");
#endif
                break;
            }

            Dispose();
        }

        #region Events

        [EventHandler(Events.Map.OnClientMapStart)]
        private async void OnClientMapStart(string resourceName)
        {
            if (!Main.ScriptIsStarted<MapManager>())
            {
                Main.LoadScript(new MapManager(Main));
            }

            var map = Main.GetScript<MapManager>();

            map.Load();

            await Delay(15000);

            while (!character.IsComponentsReady)
            {
                await Delay(250);
            }

            if ((bool)!characterExist)
            {
                CreateCharacter();
            }
            else
            {
                SpawnPlayer();
            }

            map.Dispose();
        }

        #endregion
    }
}