﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core.Controllers;
using Client.Core.Enums;
using Client.Core.Extensions;
using Client.Core.Internal;
using Client.Models;
using Newtonsoft.Json;
using Shared.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static Client.Constant;
using static Client.Core.Internal.Log;
using static Client.Core.Managers.TaskManager;
using static Client.Core.Internal.CAPI;

namespace Client.Core.Managers
{
    public class CharacterManager : Script
    {
        protected PermissionManager permission;
        protected int textureId = -1;

        public CharacterData Data { get; private set; }
        public bool IsComponentsReady { get; private set; }

        public CharacterManager(Main main) : base(main, "character_manager")
        {
            Clothes = Configuration<Clothing>.Parse("utils/clothes");

            permission = Main.GetScript<PermissionManager>();
            var task = Main.GetScript<TaskManager>();

            Task.Factory.StartNew(async () =>
            {
                await RemoveInvalidClothes();
                await permission.IsReady();
                await IsReady();

                task.Add(new CAction(5000, 0, false, () => CAPI.SetPedScale(PlayerPedId(), Data.Scale)));
                task.Add(new CAction(60000, 0, false, () => Save()));
            });
        }

        public async Task IsReady()
        {
            while (Data == null)
            {
                await Delay(0);
            }
        }

        public async Task<bool?> IsCharacterExist()
        {
            bool? wait = null;

            Event(Events.Character.OnExist).On((message) => wait = (bool?)message.Payloads[0]).Emit();

            while (wait == null)
            {
                await Delay(0);
            }

            return wait;
        }

        private async Task RemoveInvalidClothes()
        {
            if ((bool)Config["RemoveInvalidCloth"])
            {
                for (int i = 0; i < 9; i++)
                {
                    InitComponents();

                    while (!IsComponentsReady)
                    {
                        await Delay(100);
                    }
                }
            }
            else
            {
                IsComponentsReady = true;
            }
        }

        public async Task Load()
        {
            Data = null;

            Event(Events.Character.OnLoad).On(message => Data = message.Payloads[0].Convert<CharacterData>()).Emit();

            while (Data == null)
            {
                await Delay(0);
            }

            SetPedBody();
            await SetPedFaceFeatures();
            await SetPedFaceFeatures();
            SetPedBodyComponents();
            UpdateOverlay();
            await SetPedClothes();

            SetPedComponentDisabled(PlayerPedId(), 0x3F1F01E5, 0);
            SetPedComponentDisabled(PlayerPedId(), 0xDA0E2C55, 0);
            UpdatePedVariation();

            if (Main.ScriptIsStarted<PlayerController>())
            {
                Main.GetScript<PlayerController>().CoreSystemEnabled = true;
            }
        }

        private void Save() => TriggerServerEvent(Events.Character.OnSave, JsonConvert.SerializeObject(Data));
        public void CreateCharacter(CharacterData data) => TriggerServerEvent(Events.Character.OnSave, JsonConvert.SerializeObject(data));

        public void SetPedBody()
        {
            var ped = PlayerPedId();
            var cultures = PedComponents.Where(x => x.Gender == (Gender)Data.SexType).ToList();
            var culture = cultures[Data.Culture];
            var head = culture.Heads[Data.Head];
            var body = culture.Body[Data.Body];
            var legs = culture.Legs[Data.Legs];
            var headTexture = culture.HeadTexture;

            Function.Call((Hash)0x704C908E9C405136, ped);
            Function.Call((Hash)0xD3A7B003ED343FD9, ped, FromHexToHash(head), true, true, false);

            Function.Call((Hash)0x704C908E9C405136, ped);
            Function.Call((Hash)0xD3A7B003ED343FD9, ped, FromHexToHash(body), true, true, false);

            Function.Call((Hash)0x704C908E9C405136, ped);
            Function.Call((Hash)0xD3A7B003ED343FD9, ped, FromHexToHash(legs), true, true, false);

            // Remove default pants
            RemovePedComponent(ClothCategories.Pants);
        }

        public async Task SetPedFaceFeatures()
        {
            foreach (var part in Data.FaceParts)
            {
                SetPedFaceFeature(part.Key, part.Value);
            }
        }

        public async Task SetPedClothes()
        {
            foreach (var cloth in Data.Clothes)
            {
                if (cloth.Value != 0)
                {
                    SetPedComponentEnabled(cloth.Value);
                    UpdatePedVariation();
                    await Delay(500);
                }
            }
        }

        public void SetPedBodyComponents()
        {
            SetPedBodyComponent((uint)Clothes.BodyTypes[Data.BodyType]);
            SetPedBodyComponent((uint)Clothes.WaistTypes[Data.WaistType]);
        }

        public async Task UpdateOverlay()
        {
            int ped = PlayerPedId();

            if (textureId != -1)
            {
                ResetPedTexture2(textureId);
                DeletePedTexture(textureId);
            }

            textureId = Function.Call<int>((Hash)0xC5E7204F322E49EB, Data.Texture["albedo"], Data.Texture["normal"], Data.Texture["material"]);

            foreach (var layer in Data.Overlays.Values)
            {
                if (layer.Visibility != 0)
                {
                    int overlayId = AddPedOverlay(textureId, layer.TxId, layer.TxNormal, layer.TxMaterial, layer.TxColorType, layer.TxOpacity, layer.TxUnk);

                    if (layer.TxColorType == 0)
                    {
                        SetPedOverlayPalette(textureId, overlayId, layer.Palette);
                        SetPedOverlayPaletteColour(textureId, overlayId, layer.PaletteColorPrimary, layer.PaletteColorSecondary, layer.PaletteColorTertiary);
                    }

                    SetPedOverlayVariation(textureId, overlayId, layer.Var);
                    SetPedOverlayOpacity(textureId, overlayId, layer.Opacity);
                }

                while (!IsPedTextureValid(textureId))
                {
                    await Delay(250);
                }

                OverrideTextureOnPed(ped, (uint)GetHashKey("heads"), textureId);
                UpdatePedTexture(textureId);
                UpdatePedVariation();
            }
        }
        public void UpdatePlayerPed() => Function.Call((Hash)0x704C908E9C405136, PlayerPedId());

        public void SetPedRandomComponentVariation(int variation) => Function.Call((Hash)0xC8A9481A01E63C28, PlayerPedId(), variation);
        public void SetRandomOutfitVariation() => Function.Call((Hash)0x283978A15512B2FE, PlayerPedId(), true);

        public async void InitComponents()
        {
            IsComponentsReady = false;

            while (Clothes == null)
            {
                await Delay(0);
            }

            RemoveEmptyComponents(Gender.Male, Clothes.HairMale);
            RemoveEmptyComponents(Gender.Female, Clothes.HairFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BeardMale);
            RemoveEmptyComponents(Gender.Male, Clothes.EyesMale);
            RemoveEmptyComponents(Gender.Female, Clothes.EyesFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.TeethMale);
            RemoveEmptyComponents(Gender.Female, Clothes.TeethFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BraceletsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.BraceletsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.RingsLeftHandMale);
            RemoveEmptyComponents(Gender.Female, Clothes.RingsLeftHandFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.HolstersMale);
            RemoveEmptyComponents(Gender.Female, Clothes.HolstersFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.EyewearMale);
            RemoveEmptyComponents(Gender.Female, Clothes.EyewearFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.HatsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.HatsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.ShirtsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.ShirtsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.VestMale);
            RemoveEmptyComponents(Gender.Female, Clothes.VestFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.PantsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.PantsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.SpursMale);
            RemoveEmptyComponents(Gender.Female, Clothes.SpursFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.ChapsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.ChapsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.CloakMale);
            RemoveEmptyComponents(Gender.Female, Clothes.CloakFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BadgesMale);
            RemoveEmptyComponents(Gender.Female, Clothes.BadgesFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.MasksMale);
            RemoveEmptyComponents(Gender.Female, Clothes.MasksFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.SpatsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.SpatsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.NeckwearMale);
            RemoveEmptyComponents(Gender.Female, Clothes.NeckwearFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BootsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.BootsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.AccessoriesMale);
            RemoveEmptyComponents(Gender.Female, Clothes.AccessoriesFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.GauntletsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.GauntletsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.NecktiesMale);
            RemoveEmptyComponents(Gender.Female, Clothes.NecktiesFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.SuspendersMale);
            RemoveEmptyComponents(Gender.Female, Clothes.SuspendersFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.GunbeltMale);
            RemoveEmptyComponents(Gender.Female, Clothes.GunbeltFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BeltMale);
            RemoveEmptyComponents(Gender.Female, Clothes.BeltFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.BuckleMale);
            RemoveEmptyComponents(Gender.Female, Clothes.BuckleFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.CoatsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.CoatsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.CoatsMpMale);
            RemoveEmptyComponents(Gender.Female, Clothes.CoatsMpFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.PonchosMale);
            RemoveEmptyComponents(Gender.Female, Clothes.PonchosFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.ArmorsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.ArmorsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.GlovesMale);
            RemoveEmptyComponents(Gender.Female, Clothes.GlovesFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.SatchelsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.SatchelsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.LegAttachmentsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.LegAttachmentsFemale);
            RemoveEmptyComponents(Gender.Male, Clothes.LoadoutsMale);
            RemoveEmptyComponents(Gender.Female, Clothes.LoadoutsFemale);
            RemoveEmptyComponents(Gender.Female, Clothes.SkirtsFemale);

            IsComponentsReady = true;
        }

        private void RemoveEmptyComponents(Gender gender, List<uint> components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                var cloth = components[i];
                var category = GetPedComponentCategory(cloth, (int)gender, true);

                // Find an index not equal to 0
                if (category == 0)
                {
                    components.RemoveAt(i);
                }
            }
        }

        private void RemoveEmptyComponents(Gender gender, List<string> components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                var cloth = components[i];
                var category = GetPedComponentCategory(uint.Parse(cloth, NumberStyles.AllowHexSpecifier), (int)gender, true);

                // Find an index not equal to 0
                if (category == 0)
                {
                    components.RemoveAt(i);
                }
            }
        }

        private bool IsDecimal(string value)
        {
            if (value.Contains("."))
            {
                var args = value.Split('.');
                var p1 = args[0];
                var p2 = args[1];

                return p1.All(char.IsNumber) && p2.All(char.IsNumber);
            }
            else
            {
                return value.All(char.IsNumber);
            }
        }

        public void SetMoney(decimal amount)
        {
            Data.Economy.Money = amount;
        }

        public void SetBank(decimal amount)
        {
            Data.Economy.Bank = amount;
        }

        public void AddMoney(decimal amount)
        {
            Data.Economy.Money += amount;
        }

        public void AddBank(decimal amount)
        {
            Data.Economy.Bank += amount;
        }

        public void RemoveMoney(decimal amount)
        {
            var newAmount = Data.Economy.Money - amount;

            if (newAmount >= 0)
            {
                Data.Economy.Money -= amount;
            }
        }

        public void RemoveBank(decimal amount)
        {
            var newAmount = Data.Economy.Bank - amount;

            if (newAmount >= 0)
            {
                Data.Economy.Bank -= amount;
            }
        }

        #region Commands

        [Command("character.setmoney")]
        private void SetMoneyCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            SetMoney(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: setmoney <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: setmoney <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnSetMoney, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: setmoney <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: setmoney <pid> <montant>");
                    }
                }
            }
        }

        [Command("character.setbank")]
        private void SetBankCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            SetBank(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: setbank <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: setbank <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnSetBank, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: setbank <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: setbank <pid> <montant>");
                    }
                }
            }
        }

        [Command("character.addmoney")]
        private void AddMoneyCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            AddMoney(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: addmoney <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: addmoney <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnAddMoney, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: addmoney <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: addmoney <pid> <montant>");
                    }
                }
            }
        }

        [Command("character.addbank")]
        private void AddBankCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            AddBank(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: addbank <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: addbank <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnAddBank, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: addbank <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: addbank <pid> <montant>");
                    }
                }
            }
        }

        [Command("character.removemoney")]
        private void RemoveMoneyCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            RemoveMoney(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: removemoney <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: removemoney <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnRemoveMoney, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: removemoney <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: removemoney <pid> <montant>");
                    }
                }
            }
        }

        [Command("character.removebank")]
        private void RemoveBankCommand(int source, List<object> args, string raw)
        {
            if (permission.HasPermission("admin"))
            {
                if (args.Count == 1)
                {
                    var amount = args[0].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            RemoveBank(decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: removebank <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: removebank <montant>");
                    }
                }
                else if (args.Count == 2)
                {
                    var player = args[0].ToString();
                    var amount = args[1].ToString();

                    if (!string.IsNullOrEmpty(amount))
                    {
                        if (IsDecimal(amount))
                        {
                            TriggerServerEvent(Events.Character.OnRemoveBank, GetPlayerServerId(int.Parse(player)), decimal.Parse(amount));
                        }
                        else
                        {
                            Error("Argument invalide, utilisation: removebank <pid> <montant>");
                        }
                    }
                    else
                    {
                        Error("Utilisation: removebank <pid> <montant>");
                    }
                }
            }
        }

        #endregion

        #region Events

        [EventHandler(Events.Character.OnSetMoney)]
        private void OnSetMoney(decimal amount) => SetMoney(amount);

        [EventHandler(Events.Character.OnSetBank)]
        private void OnSetBank(decimal amount) => SetBank(amount);

        [EventHandler(Events.Character.OnAddMoney)]
        private void OnAddCash(decimal amount) => AddMoney(amount);

        [EventHandler(Events.Character.OnAddBank)]
        private void OnAddBank(decimal amount) => AddBank(amount);

        [EventHandler(Events.Character.OnRemoveMoney)]
        private void OnRemoveMoney(decimal amount) => RemoveMoney(amount);

        [EventHandler(Events.Character.OnRemoveBank)]
        private void OnRemoveBank(decimal amount) => RemoveBank(amount);

        #endregion
    }
}
