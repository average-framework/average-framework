using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core.Enums;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Shared.Core.DataModels;
using static CitizenFX.Core.Native.API;
using static Client.Core.Managers.TaskManager;

namespace Client.Core.Controllers
{
    public class PlayerController : Script
    {
        private TaskManager task;
        private CharacterManager character;
        private PermissionManager permission;
        private int deathCamera = -1;
        private bool isDead;
        private bool isSprinting;
        private bool canRegen;
        private bool useScenario;
        private CAction tUpdate3;
        private TimeSpan timeToWaitSpan = TimeSpan.FromMilliseconds(timeToWait);
        private const int timeToWait = 5000;

        protected List<string> weapons = new List<string>
        {
            Weapon.WEAPON_BOW,
            Weapon.WEAPON_MELEE_ANCIENT_HATCHET,
            Weapon.WEAPON_MELEE_BROKEN_SWORD,
            Weapon.WEAPON_MELEE_CLEAVER,
            Weapon.WEAPON_MELEE_HATCHET,
            Weapon.WEAPON_MELEE_HATCHET_DOUBLE_BIT,
            Weapon.WEAPON_MELEE_HATCHET_DOUBLE_BIT_RUSTED,
            Weapon.WEAPON_MELEE_HATCHET_HEWING,
            Weapon.WEAPON_MELEE_HATCHET_HUNTER,
            Weapon.WEAPON_MELEE_HATCHET_HUNTER_RUSTED,
            Weapon.WEAPON_MELEE_HATCHET_MELEEONLY,
            Weapon.WEAPON_MELEE_HATCHET_VIKING,
            Weapon.WEAPON_MELEE_KNIFE,
            Weapon.WEAPON_MELEE_MACHETE,
            Weapon.WEAPON_MELEE_MACHETE_COLLECTOR,
            Weapon.WEAPON_MELEE_MACHETE_HORROR,
            Weapon.WEAPON_PISTOL_M1899,
            Weapon.WEAPON_PISTOL_MAUSER,
            Weapon.WEAPON_PISTOL_MAUSER_DRUNK,
            Weapon.WEAPON_PISTOL_SEMIAUTO,
            Weapon.WEAPON_PISTOL_VOLCANIC,
            Weapon.WEAPON_REPEATER_CARBINE,
            Weapon.WEAPON_REPEATER_EVANS,
            Weapon.WEAPON_REPEATER_HENRY,
            Weapon.WEAPON_REPEATER_WINCHESTER,
            Weapon.WEAPON_REVOLVER_CATTLEMAN,
            Weapon.WEAPON_REVOLVER_DOUBLEACTION,
            Weapon.WEAPON_REVOLVER_LEMAT,
            Weapon.WEAPON_REVOLVER_NAVY,
            Weapon.WEAPON_REVOLVER_SCHOFIELD,
            Weapon.WEAPON_RIFLE_BOLTACTION,
            Weapon.WEAPON_RIFLE_ELEPHANT,
            Weapon.WEAPON_RIFLE_SPRINGFIELD,
            Weapon.WEAPON_RIFLE_VARMINT,
            Weapon.WEAPON_SHOTGUN_DOUBLEBARREL,
            Weapon.WEAPON_SHOTGUN_PUMP,
            Weapon.WEAPON_SHOTGUN_REPEATING,
            Weapon.WEAPON_SHOTGUN_SAWEDOFF,
            Weapon.WEAPON_SHOTGUN_SEMIAUTO,
            Weapon.WEAPON_SNIPERRIFLE_CARCANO,
            Weapon.WEAPON_SNIPERRIFLE_ROLLINGBLOCK,
            Weapon.WEAPON_THROWN_THROWING_KNIVES,
            Weapon.WEAPON_THROWN_TOMAHAWK,
            Weapon.WEAPON_THROWN_TOMAHAWK_ANCIENT
        };

        public int Health
        {
            get => GetAttributeCoreValue(PlayerPedId(), 0);
            set
            {
                var ped = PlayerPedId();
                SetEntityMaxHealth(ped, 100);
                SetEntityHealth(ped, value, 0);
                Function.Call((Hash) 0xC6258F41D86676E0, ped, 0, value);
            }
        }

        public int Hunger { get; set; } = 100;
        public int Thirst { get; set; } = 100;
        public int Stamina { get; set; } = 100;
        public int HungerDecreaseValue { get; set; } = 1;
        public int ThirstDecreaseValue { get; set; } = 2;
        public int StaminaDecreaseValue { get; set; } = 1;
        public bool CoreSystemEnabled { get; set; }

        public PlayerController(Main main) : base(main)
        {
            task = Main.GetScript<TaskManager>();
            character = Main.GetScript<CharacterManager>();
            permission = Main.GetScript<PermissionManager>();

            Hud.SetVisibility(false);
            Hud.SetHelpTextVisible(false);
            Hud.SetPlayerVisible(true);
            Hud.SetHorseVisible(false);
            Hud.SetDeathScreenVisible(false);

            deathCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 0f, 0f, 0f, 0f, 0.0f, 0f, 40.0f, false, 0);

            Task.Factory.StartNew(async () =>
            {
                await character.IsReady();

                Health = character.Data.Core.Health;
                Hunger = character.Data.Core.Hunger;
                Thirst = character.Data.Core.Thirst;

                tUpdate3 = new CAction(3000, 0, true, Update3);

                task.Add(new CAction(60000, 0, false, Update));
                task.Add(new CAction(-1, 0, false, Update5));
                task.Add(new CAction(1000, 0, false, Update5));
            });

            var ped = PlayerPedId();

            ClearPedBloodDamage(ped);
            ClearPedLastDamageBone(ped);
        }

        #region Tasks

        private void Update()
        {
            if (CoreSystemEnabled)
            {
                UpdateHunger();
                UpdateThirst();
                UpdateHealth();
            }

            UpdatePosition();
        }

        [Tick]
        private async Task Update2()
        {
            var ped = PlayerPedId();

            if (IsPedDeadOrDying(ped, true))
            {
                isDead = true;

                if (!AnimpostfxIsRunning(PostEffect.PauseMenuIn)) AnimpostfxPlay(PostEffect.PauseMenuIn);
            }

            if (isDead)
            {
                if (!DoesCamExist(deathCamera))
                    deathCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 0f, 0f, 0f, 0f, 0.0f, 0f, 40.0f, false,
                        0);

                if (DoesCamExist(deathCamera))
                {
                    var pos = GetEntityCoords(ped, true, true);
                    SetCamCoord(deathCamera, pos.X - 3f, pos.Y - 3f, pos.Z + 2f);
                    SetCamFov(deathCamera, 40f);
                    PointCamAtEntity(deathCamera, ped, 0f, +1.5f, 0f, true);
                    SetCamActive(deathCamera, true);
                    RenderScriptCams(true, true, 1000, true, true, 0);
                }

                if (timeToWaitSpan != TimeSpan.Zero)
                {
                    Hud.SetMessage("Appuyer sur", "E", "pour réapparaître");
                    Hud.SetVisibility(true);
                    Hud.SetCooldownVisible(true);
                    Hud.SetDeathScreenVisible(true);
                    Hud.SetPlayerVisible(false);
                    Hud.SetHorseVisible(false);
                    Hud.SetContainerVisible(true);

                    await Delay(1000);

                    timeToWaitSpan -= TimeSpan.FromSeconds(1);
                    Hud.SetTime(timeToWaitSpan);

                    if (timeToWaitSpan == TimeSpan.Zero)
                    {
                        Hud.SetDeathScreenVisible(false);
                        Hud.SetHelpTextVisible(true);
                        Hud.SetCooldownVisible(false);
                    }
                }
                else
                {
                    if (IsControlJustReleased(0, (uint) Keys.E)) await Revive(25, 50, 50);
                }
            }
            else
            {
                if (IsControlJustReleased(0, (uint) Keys.N2))
                {
                    Hud.SetVisibility(true);
                    Hud.SetContainerVisible(true);
                    Hud.SetPlayerVisible(true);
                    Hud.SetCooldownVisible(false);
                    Hud.SetDeathScreenVisible(false);
                    Hud.SetHelpTextVisible(false);
                    Hud.SetHorseVisible(IsPedOnMount(ped));

                    task.Add(tUpdate3);
                }
            }
        }

        private void Update3()
        {
            Hud.SetVisibility(false);
            Hud.SetContainerVisible(false);

            task.DeleteAction(tUpdate3);
        }

        [Tick]
        private async Task Update4()
        {
            var boneIndex = 0;
            var ped = PlayerPedId();

            foreach (var weapon in weapons)
                if (Function.Call<bool>((Hash) 0xDCF06D0CDFF68424, ped, (uint) GetHashKey(weapon), 0))
                    if (GetPedLastDamageBone(ped, ref boneIndex))
                    {
                        if (boneIndex == 33646 || boneIndex == 45454 || boneIndex == 6884 || boneIndex == 65478 ||
                            boneIndex == 56200 || boneIndex == 55120 || boneIndex == 43312)
                        {
                            var rnd = new Random(Environment.TickCount).Next(5000, 10000);
                            SetPedToRagdoll(ped, rnd, rnd, 0, true, true, false);
                            await Delay(rnd);
                            ReviveInjuredPed(ped);
                            await Delay(2000);

                            var pos = GetEntityCoords(ped, true, true);
                            var heading = GetEntityHeading(ped);

                            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, pos.X, pos.Y, pos.Z, heading, false,
                                false, false);
                        }

                        if (boneIndex == 46065 ||
                            boneIndex == 53675 ||
                            boneIndex == 43312 ||
                            boneIndex == 55120 ||
                            boneIndex == 33646 ||
                            boneIndex == 45454 ||
                            boneIndex == 54187 ||
                            boneIndex == 53675 ||
                            boneIndex == 22798 ||
                            boneIndex == 34606 ||
                            boneIndex == 54802 ||
                            boneIndex == 30226)
                            Function.Call((Hash) 0xFFD54D9FE71B966A, ped, 2, boneIndex, 0.0f, 0.1f, 0.0f, 0f, 0f, -1f,
                                0.01f);
                    }

            await Delay(250);
        }

        private void Update5()
        {
            var ped = PlayerPedId();

            if (IsControlJustReleased(0, (uint) Keys.N1))
                SetPedToRagdoll(PlayerPedId(), 10000, 10000, 0, true, true, true);

            if (IsControlJustReleased(0, (uint) Keys.J))
            {
                if (!useScenario)
                {
                    useScenario = true;

                    var pos = GetEntityCoords(ped, true, true);
                    Function.Call((Hash) 0x9FDA1B3D7E7028B3, ped, pos.X, pos.Y, pos.Z, 2f, 1, 1, 1, 1);
                }
                else
                {
                    useScenario = false;
                    ClearPedTasksImmediately(ped, 0, 0);
                }
            }

            if (IsControlJustPressed(0, (uint) Keys.L))
            {
                if (character.Data.SexType == 0)
                    CAPI.PlayClipset2("mech_loco_m@generic@reaction@handsup@unarmed@tough", "loop", 31, -1, 1.0f, 1.0f);
                else
                    CAPI.PlayClipset2("mech_loco_f@generic@reaction@handsup@unarmed@tough", "loop", 31, -1, 1.0f, 1.0f);
            }

            if (IsControlJustReleased(0, (uint) Keys.LALT)) isSprinting = !isSprinting;

            if (IsControlPressed(0, (uint) Keys.SHIFT))
            {
                canRegen = false;

                if (!IsPlayerFreeAiming(PlayerId()))
                {
                    canRegen = false;

                    if (isSprinting)
                    {
                        if (Stamina <= 25)
                        {
                            StaminaDecreaseValue = 2;
                            SetPedMaxMoveBlendRatio(ped, 1.7f);
                        }
                        else
                        {
                            StaminaDecreaseValue = 5;
                            SetPedMaxMoveBlendRatio(ped, 1.95f);
                        }
                    }
                    else
                    {
                        StaminaDecreaseValue = 2;
                        SetPedMaxMoveBlendRatio(ped, 1.7f);
                    }
                }
                else
                {
                    canRegen = true;
                    SetPedMaxMoveBlendRatio(ped, 0.6f);
                }
            }
            else
            {
                canRegen = true;
                SetPedMaxMoveBlendRatio(ped, 0.6f);
            }

            SetPedMinMoveBlendRatio(ped, 0f);
            SetPedMoveAnimsBlendOut(ped);
            SetPedDesiredMoveBlendRatio(ped, 10f);
        }

        private void Update6()
        {
            if (canRegen)
            {
                Stamina += 4;
            }
            else
            {
                if (Stamina <= 0)
                    Stamina = 0;
                else
                    Stamina -= StaminaDecreaseValue;
            }

            if (Stamina >= 100) Stamina = 100;
        }

        #endregion

        #region Methods

        public void FullCore()
        {
            Health = 100;
            Hunger = 100;
            Thirst = 100;
            
            ShowCores();

            Hud.SetHealth(Health);
            Hud.SetHunger(Hunger);
            Hud.SetThirst(Thirst);
        }
        
        public async Task Revive(int health = 100, int hunger = 100, int thirst = 100)
        {
            isDead = false;

            timeToWaitSpan = TimeSpan.FromMilliseconds(timeToWait);
            Hud.SetVisibility(false);
            Hud.SetHelpTextVisible(false);
            Hud.SetPlayerVisible(true);
            Hud.SetDeathScreenVisible(false);
            Hud.SetContainerVisible(false);
            Hud.SetTime(timeToWaitSpan);

            var ped = PlayerPedId();
            var pos = GetEntityCoords(ped, true, true);
            var heading = GetEntityHeading(ped);

            ReviveInjuredPed(ped);

            if (AnimpostfxIsRunning(PostEffect.PauseMenuIn)) AnimpostfxStop(PostEffect.PauseMenuIn);

            while (IsPedInjured(ped)) await Delay(100);

            await DeleteCamera();
            await Delay(750);

            Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, pos.X, pos.Y, pos.Z, heading, false, false, false);

            Health = 25;
            Hunger = 50;
            Thirst = 50;

            ShowCores();

            Hud.SetHealth(Health);
            Hud.SetHunger(Hunger);
            Hud.SetThirst(Thirst);

            await Delay(500);
        }

        private async Task DeleteCamera()
        {
            SetCamActive(deathCamera, false);
            RenderScriptCams(false, true, 1000, true, true, 0);

            while (DoesCamExist(deathCamera))
            {
                DestroyCam(deathCamera, false);
                await Delay(1000);
            }
        }

        private void UpdateHunger()
        {
            if (!isDead)
            {
                if (Hunger <= 0)
                    Hunger = 0;
                else
                    Hunger -= HungerDecreaseValue;

                Hud.SetHunger(Hunger);
            }
        }

        private void UpdateThirst()
        {
            if (!isDead)
            {
                if (Thirst <= 0)
                    Thirst = 0;
                else
                    Thirst -= ThirstDecreaseValue;

                Hud.SetThirst(Thirst);
            }
        }

        private void UpdateHealth()
        {
            if (!isDead)
            {
                if (Health <= 0)
                {
                    Health = 0;
                }
                else if (Health > 100)
                {
                    Health = 100;
                }
                else
                {
                    if (Hunger <= 0) Health -= 10;

                    if (Thirst <= 0) Health -= 10;
                }

                character.Data.Core.Health = Health;
                character.Data.Core.Hunger = Hunger;
                character.Data.Core.Thirst = Thirst;

                Hud.SetHealth(Health);
            }
        }

        private void UpdatePosition()
        {
            var ped = PlayerPedId();
            var coords = GetEntityCoords(ped, true, true);
            var heading = GetEntityHeading(ped);
            character.Data.Position = new CharacterData.PositionData(coords.X, coords.Y, coords.Z, heading);
        }

        public void ShowCores()
        {
            var ped = PlayerPedId();

            Hud.SetVisibility(true);
            Hud.SetContainerVisible(true);
            Hud.SetPlayerVisible(true);
            Hud.SetHelpTextVisible(false);

            if (IsPedOnMount(ped))
                Hud.SetHorseVisible(true);
            else
                Hud.SetHorseVisible(false);

            task.Add(tUpdate3);
        }

        #endregion

        #region Commands

#if  DEBUG
        [Command("kill")]
        private void Kill()
        {
            if (permission.HasPermission("admin"))
            {
                Health = 0;
            }
        }
#endif

        #endregion
        
        #region Events

        [EventHandler(Events.CFX.OnResourceStop)]
        private async void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName) await DeleteCamera();
        }

        #endregion
    }
}