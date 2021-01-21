using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Client.Core.Enums;
using Client.Core.Extensions;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Client.Core.UI.Menu;
using Newtonsoft.Json;
using Shared.Core.DataModels;
using static CitizenFX.Core.Native.API;

namespace Client.Scripts
{
    public class Stable : Script
    {
        private readonly Menu menu;
        private readonly BlipManager blip;
        private MenuContainer mainMenu;
        private Models.Stable currentStable;

        public List<HorseData> Data { get; private set; }

        public bool IsOpen { get; private set; }
        
        public Stable(Main main) : base(main)
        {
            menu = Main.GetScript<Menu>();
            blip = Main.GetScript<BlipManager>();

            Constant.Stables = Configuration<List<Models.Stable>>.Parse("config/stables");

            Task.Factory.StartNew(async () =>
            {
                Load();
                await IsReady();
            
                RegisterStables();
                CreateNonStoredHorse();

                await Delay(0);
                
                Tick += Update;
                Tick += Update3;
                
                var task = Main.GetScript<TaskManager>();
                task.Add(new TaskManager.CAction(1000, 0, false, Update4));
                task.Add(new TaskManager.CAction(60000, 0, false, Save));
            });
        }

        #region Ticks

        private async Task Update()
        {
            var pos = GetEntityCoords(PlayerPedId(), true, true);

            foreach (var stable in Constant.Stables)
            {
                var interactPosition = new Vector3(stable.Interact.Position.X, stable.Interact.Position.Y,
                    stable.Interact.Position.Z);

                if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, interactPosition.X, interactPosition.Y,
                    interactPosition.Z, false) <= stable.Interact.Radius)
                {
                    if (!stable.IsNear)
                    {
                        stable.IsNear = true;

                        Hud.SetMessage("Appuyer sur", "X", "pour sortir un cheval");
                        Hud.SetVisibility(true);
                        Hud.SetContainerVisible(true);
                        Hud.SetHelpTextVisible(true);
                        Hud.SetPlayerVisible(false, 0);
                        Hud.SetHorseVisible(false, 0);
                        Hud.SetDeathScreenVisible(false, 0);
                        Hud.SetCooldownVisible(false, 0);
                    }

                    if (IsControlJustReleased(0, (uint) Keys.X))
                    {
                        IsOpen = true;
                        
                        currentStable = stable;

                        InitMainMenu();

                        menu.OpenMenu(mainMenu);
                        NUI.Focus(true, true);
                    }
                }
                else
                {
                    if (stable.IsNear)
                    {
                        stable.IsNear = false;

                        Hud.SetVisibility(false);
                        Hud.SetContainerVisible(false);
                        Hud.SetHelpTextVisible(false);
                        Hud.SetPlayerVisible(true);
                    }

                    await Delay(250);
                }
            }
        }

        private async Task Update3()
        {
            await Delay(1000);

            for (var i = 0; i < Data.Count; i++)
                if (Data[i].IsStored == 0)
                    if (DoesEntityExist(Data[i].Entity))
                        if (IsPedDeadOrDying(Data[i].Entity, true))
                        {
                            Data[i].IsStored = 1;

                            await Delay(1000 * 60 * 2);

                            var horse = Data[i].Entity;
                            DeletePed(ref horse);
                            DeleteEntity(ref horse);
                        }
        }

        private void Update4()
        {
            var ped = PlayerPedId();
            var pos = GetEntityCoords(ped, true, true);

            for (var i = 0; i < Data.Count; i++)
                if (Data[i].IsStored == 0)
                    if (DoesEntityExist(Data[i].Entity))
                    {
                        var horsePos = GetEntityCoords(Data[i].Entity, true, true);

                        if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, horsePos.X, horsePos.Y, horsePos.Z, true) <= 4f)
                            if (blip.ExistsOnAssignedEntity(Data[i].Entity))
                            {
                                var entBlip = blip.GetBlipByAssignation(Data[i].Entity);
                                blip.Delete(entBlip.Handle);
                            }
                    }
        }

        public void Save()
        {
            for (var i = 0; i < Data.Count; i++)
            {
                var horse = Data[i];

                if (horse.IsStored == 0)
                {
                    var pos = GetEntityCoords(horse.Entity, true, true);
                    var heading = GetEntityHeading(horse.Entity);
                    horse.Position = new CharacterData.PositionData(pos.X, pos.Y, pos.Z, heading);
                }

                Save(horse);
            }
        }

        #endregion

        private async void RegisterStables()
        {
            var npc = Main.GetScript<NpcManager>();

            foreach (var stable in Constant.Stables)
            {
                var npcPosition = new Vector3(stable.Npc.Position.X, stable.Npc.Position.Y, stable.Npc.Position.Z);
                var npcHeading = stable.Npc.Position.W;
                var blipPosition = new Vector3(stable.Blip.Position.X, stable.Blip.Position.Y,
                    stable.Blip.Position.Z);

                stable.NPCsInstance = await npc.Create((uint) GetHashKey(stable.Npc.Model), stable.Npc.ModelVariation,
                    npcPosition, npcHeading);
                blip.Create(stable.Blip.Sprite, stable.Blip.Label, stable.Blip.Scale, blipPosition);
            }

            StartScenario();
        }

        #region Menus

        private void InitMainMenu()
        {
            mainMenu = new MenuContainer(Lang.Current["MyHorses"].ToUpper());
            menu.CanCloseMenu = true;
            menu.CreateSubMenu(mainMenu);

            for (var i = 0; i < Data.Count; i++) InitMyHorseMenu(Data[i], mainMenu);
        }

        private void InitMyHorseMenu(HorseData horse, MenuContainer container)
        {
            var horseMenu = new MenuContainer(horse.Name.ToUpper());
            menu.CreateSubMenu(horseMenu);

            var horseStateItem = new MenuItem(Lang.Current["Client.Stable.HorseIsStored"].Replace("{0}", horse.IsStored == 1 ? Lang.Current["Client.Stable.IsStored"] : Lang.Current["Client.Stable.IsNotStored"]));
            horseMenu.AddItem(horseStateItem);

            var horseItem = new MenuItem(horse.Name, () =>
            {
                menu.UpdateRender(horseMenu);
                menu.OpenMenu(horseMenu);
            });
            container.AddItem(horseItem);

            MenuItem changeHorseStateItem = null;
            changeHorseStateItem = new MenuItem(horse.IsStored == 0 ? Lang.Current["Client.Stable.IsNotStored2"] : Lang.Current["Client.Stable.IsStored2"],
                async () =>
                {
                    if (horse.IsStored == 1)
                    {
                        IsOpen = false;
                        
                        var spawnPoint =
                            currentStable.SpawnPoints[
                                new Random(Environment.TickCount).Next(0, currentStable.SpawnPoints.Count - 1)];
                        var position = new Vector3(spawnPoint.X, spawnPoint.Y, spawnPoint.Z);
                        var heading = (float) new Random(Environment.TickCount + 1).Next(0, 360);
                        var tempHorse = await CreateHorse(horse, position, heading);
                        horse.Entity = tempHorse.Entity;
                        horse.OwnerServerId = tempHorse.OwnerServerId;
                        horse.IsStored = 0;
                        horse.Position = tempHorse.Position;
                        NetworkRegisterEntityAsNetworked(horse.Entity);

                        //Save(horse);

                        menu.CloseMenu();
                        NUI.Focus(false, false);
                    }
                    else
                    {
                        if (currentStable != null)
                        {
                            var position = new Vector3(currentStable.BringHorse.Position.X,
                                currentStable.BringHorse.Position.Y, currentStable.BringHorse.Position.Z);

                            if (horse.IsStored == 0)
                                if (DoesEntityExist(horse.Entity))
                                {
                                    var hPos = GetEntityCoords(horse.Entity, true, true);
                                    var hHeading = GetEntityHeading(horse.Entity);

                                    if (GetDistanceBetweenCoords(position.X, position.Y, position.Z, hPos.X, hPos.Y,
                                        hPos.Z, true) <= currentStable.BringHorse.Radius)
                                    {
                                        IsOpen = false;
                                        
                                        // Store horses in database
                                        var ent = horse.Entity;

                                        horse.IsStored = 1;
                                        horse.Position =
                                            new CharacterData.PositionData(hPos.X, hPos.Y, hPos.Z, hHeading);

                                        DeletePed(ref ent);
                                        DeleteEntity(ref ent);

                                        //Save(horse);

                                        menu.CloseMenu();
                                        NUI.Focus(false, false);
                                    }
                                }
                        }
                    }

                    changeHorseStateItem.Text = horse.IsStored == 0
                        ? Lang.Current["Client.Stable.IsNotStored2"]
                        : Lang.Current["Client.Stable.IsStored2"];
                });
            horseMenu.AddItem(changeHorseStateItem);
        }

        #endregion

        #region My Horses Methods

        public async Task IsReady()
        {
            while (Data == null) await Delay(0);
        }

        public void Load()
        {
            Log.WriteLog("Loading horse(s)..");

            Event(Events.Stable.OnGetPlayerHorses).On(message =>
            {
                Data = message.Payloads[0].Convert<List<HorseData>>();
            }).Emit();

            Log.WriteLog("Horse(s) loaded");
        }

        private void Save(HorseData horse)
        {
            TriggerServerEvent(Events.Stable.OnSaveMyHorse, JsonConvert.SerializeObject(horse));
        }

        private async void CreateNonStoredHorse()
        {
            for (var i = 0; i < Data.Count; i++)
                if (Data[i].IsStored == 0)
                {
                    // Create horse around last position
                    var radius = 150;
                    var xPos = (float) new Random(Environment.TickCount).Next(-radius / 2, radius / 2);
                    var yPos = (float) new Random(Environment.TickCount + 1).Next(-radius / 2, radius / 2);
                    var newPos = new Vector3(Data[i].Position.X + xPos, Data[i].Position.Y + yPos, Data[i].Position.Z);
                    var groundZ = 0f;
                    var rndHeading = (float) new Random(Environment.TickCount + 2).NextDouble() * 360f;

                    while (!CAPI.GetGroundZFor3DCoord(newPos.X, newPos.Y, newPos.Z, ref groundZ, false))
                    {
                        xPos = new Random(Environment.TickCount).Next(-radius / 2, radius / 2);
                        yPos = new Random(Environment.TickCount + 1).Next(-radius / 2, radius / 2);
                        newPos = new Vector3(Data[i].Position.X + xPos, Data[i].Position.Y + yPos, Data[i].Position.Z);

                        await Delay(0);
                    }

                    var tempHorse = await CreateHorse(Data[i], new Vector3(newPos.X, newPos.Y, groundZ), rndHeading);
                    Data[i].Entity = tempHorse.Entity;
                    Data[i].OwnerServerId = GetPlayerServerId(PlayerPedId());

                    // Create blip for horse
                    blip.Create(-1715189579, Data[i].Name, 1f, newPos, tempHorse.Entity);
                }
        }

        private async Task<HorseData> CreateHorse(HorseData horse, Vector3 spawnPosition, float spawnHeading)
        {
            await CAPI.LoadModel(horse.Model);
            var handle = CreatePed(horse.Model, spawnPosition.X, spawnPosition.Y, spawnPosition.Z, spawnHeading, true,
                true, false, false);
            SetEntityVisible(handle, true);
            //            var groupHash = (uint) GetHashKey("PLAYER");
            //            CAPI.SetPedRelationshipGroupDefaultHash(handle, groupHash);
            CAPI.SetAnimalMood(handle, 1);

            // ----
            //N_0xa691c10054275290(PlayerPedId(), handle, 0);
            //N_0x931b241409216c1f(PlayerPedId(), handle, 0);
            //N_0xed1c764997a86d5a(PlayerPedId(), handle);
            //N_0xb8b6430ead2d2437(handle, 130495496);
            //N_0xdf93973251fb2ca5(GetEntityModel(handle), 1);
            //N_0xaeb97d84cdf3c00b(handle, 0);

            //CAPI.SetPedConfigFlag(handle, 211, true);
            //CAPI.SetPedConfigFlag(handle, 208, true);
            //CAPI.SetPedConfigFlag(handle, 209, true);
            //CAPI.SetPedConfigFlag(handle, 400, true);
            //CAPI.SetPedConfigFlag(handle, 297, true);
            //CAPI.SetPedConfigFlag(handle, 136, false);
            CAPI.SetPedConfigFlag(handle, 312, false); // Horse not flee when player shoots
            //CAPI.SetPedConfigFlag(handle, 113, false);
            //CAPI.SetPedConfigFlag(handle, 301, false);
            //CAPI.SetPedConfigFlag(handle, 277, true);
            //CAPI.SetPedConfigFlag(handle, 319, true);
            //CAPI.SetPedConfigFlag(handle, 6, true);
            // ----

            CAPI.SetAnimalTuningBoolParam(handle, 25, false);
            CAPI.SetAnimalTuningBoolParam(handle, 24, false);
            CAPI.SetAnimalTuningBoolParam(handle, 48, false);

            N_0xa691c10054275290(handle, PlayerId(), 431);
            N_0x6734f0a6a52c371c(PlayerId(), 431);
            N_0x024ec9b649111915(handle, 1);

            Function.Call((Hash) 0xEB8886E1065654CD, handle, 10, "ALL", 10f);

            CAPI.SetPedOutfitPreset(handle, 0);

            switch (horse.Gender)
            {
                case 0:
                    CAPI.SetPedFaceFeature(handle, 0xA28B, 0.0f);
                    break;
                case 1:
                    CAPI.SetPedFaceFeature(handle, 0xA28B, 1.0f);
                    break;
            }

            horse.Entity = handle;
            horse.OwnerServerId = GetPlayerServerId(PlayerId());

            ApplyHorseComponents(handle, horse);

            TriggerServerEvent(Events.Stable.OnPlayerDisconnect, NetworkGetNetworkIdFromEntity(handle));

            return horse;
        }

        private void ApplyHorseComponents(int horseHandle, HorseData horse)
        {
            CAPI.UpdatePedVariation(horseHandle);

            var horseInstance = Data.Find(x => x.Id == horse.Id);

            if (horseInstance.Components.Count > 0)
                foreach (var component in horseInstance.Components)
                {
                    var comp = uint.Parse(component.Value.ToString("X"), NumberStyles.AllowHexSpecifier);

                    CAPI.SetPedComponentEnabled(horseHandle, comp, true, true, true);
                    CAPI.UpdatePedVariation(horseHandle);
                }
        }

        #endregion

        #region Other Methods

        private void StartScenario()
        {
            foreach (var stable in Constant.Stables)
            {
                var handle = stable.NPCsInstance.Handle;
                CAPI.PlayScenarioInPlace(handle, stable.Npc.Scenario);
            }
        }

        #endregion

        #region Events

        [EventHandler(Events.CFX.OnResourceStop)]
        private void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName)
                for (var i = 0; i < Data.Count; i++)
                    if (Data[i].IsStored == 0)
                    {
                        var ent = Data[i].Entity;

                        DeletePed(ref ent);
                        DeleteEntity(ref ent);
                    }
        }

        [EventHandler(Events.Stable.OnPlayerDisconnect)]
        private void OnPlayerDisconnectEvent(int netId)
        {
            if (NetworkDoesNetworkIdExist(netId))
            {
                var ent = NetworkGetEntityFromNetworkId(netId);

                NetworkRequestControlOfEntity(ent);
                SetEntityAsMissionEntity(ent, true, true);

                DeletePed(ref ent);
                DeleteEntity(ref ent);
            }
        }

        #endregion
    }
}