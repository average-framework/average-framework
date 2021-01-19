using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Client.Core.Controllers;
using Client.Core.Enums;
using Client.Core.Extensions;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Client.Core.UI.Menu;
using Client.Models;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using static Client.Core.Internal.Log;

namespace Client.Scripts
{
    public class MapEditor : Script
    {
        private Menu menu;
        private MenuContainer mainMenu;
        private MenuContainer environmentMenu;
        private MenuContainer settingsMenu;
        private MenuContainer sceneMenu;
        private MenuContainer myScenesMenu;
        private MenuItem coordsItem;
        private MenuItem rotationItem;
        private MenuItem hashItem;
        private MenuItem currentSceneItem;
        private MenuItemList interpolationItem;
        private MenuItemCheckbox visibleItem;
        private MenuItemCheckbox gravityItem;
        private MenuItemCheckbox collisionItem;
        private MenuItemCheckbox superpositionItem;
        private MenuItemCheckbox onGroundItem;
        private MenuItemCheckbox drawAxesItem;
        private MenuItemCheckbox drawBoxItem;
        private MenuItemCheckbox lockObjItem;
        private MenuSliderSelectorItem<int> selectorItem;
        private MenuSliderSelectorItem<int> alphaItem;
        private MenuSliderSelectorItem<float> offsetPrecisionItem;
        private MenuSliderSelectorItem<float> xOffsetItem;
        private MenuSliderSelectorItem<float> yOffsetItem;
        private MenuSliderSelectorItem<float> zOffsetItem;
        private bool isNoclip;
        private bool isFocus;
        private float noclipSpeed = 1f;
        private float rotationSpeed = 1f;
        private readonly float minOffset = -50f;
        private readonly float maxOffset = 50f;
        private int lastObjEntity = -1;
        private int currentObjEntity = -1;
        private int currentObjType;
        private uint currentObjModel;
        private string currentSceneName = "";
        private string currentObjModelString = "";
        private Vector3 lastPointCoords = Vector3.Zero;
        private Vector3 currentPointCoords = Vector3.Zero;
        private Vector3 currentObjRotation = Vector3.Zero;
        private readonly List<MapScene> scenes = new List<MapScene>();

        private readonly TaskManager task;
        private readonly TaskManager.CAction tUpdate;
        private readonly TaskManager.CAction tUpdate1;
        private readonly TaskManager.CAction tUpdate2;

        private readonly CharacterManager character;
        
        public bool IsOpen { get; private set; }

        public MapEditor(Main main) : base(main)
        {
            task = Main.GetScript<TaskManager>();
            character = Main.GetScript<CharacterManager>();
            menu = Main.GetScript<Menu>();

            Constant.Objects = Configuration<Objects>.Parse("utils/objects");

            Task.Factory.StartNew(async () =>
            {
                await character.IsReady();

                InitMenus();
            });

            RegisterNuiEvents();

            SetEntityCollision(PlayerPedId(), true, true);
            SetEntityVisible(PlayerPedId(), true);
            SetEntityInvincible(PlayerPedId(), false);

            Event(Events.MapEditor.OnGetAllScenesName).On(message =>
            {
                var scenes = message.Payloads[0].Convert<Dictionary<string, List<MapObject>>>();

                if (scenes.Count == 0)
                    return;

                foreach (var scene in scenes)
                {
                    var objects = new List<MapObject>();

                    foreach (var obj in scene.Value)
                    {
                        var entity = -1;

                        if (currentObjType == 4)
                        {
                            //entity = StartParticleFxLoopedAtCoord(currentObjModelString, obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, 1f, true, true, true, false);
                        }
                        else if (currentObjType == 8)
                        {
                            //entity = CreateVehicle(obj.Model, obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Rotation.Z, true, false, false, false);
                        }
                        else
                        {
                            entity = CreateObject(obj.Model, obj.Position.X, obj.Position.Y, obj.Position.Z, false,
                                false, true, false, false);
                        }

                        obj.Entity = entity;

                        SetEntityCoordsNoOffset(entity, obj.Position.X, obj.Position.Y, obj.Position.Z, true, true, true);
                        SetEntityRotation(entity, obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, 2, true);
                        SetEntityAlpha(entity, obj.Alpha, false);
                        SetEntityCollision(entity, obj.Collision, true);
                        SetEntityHasGravity(entity, obj.Gravity);
                        SetEntityVisible(entity, obj.Visible);

                        if (obj.PlaceOnGround)
                            PlaceObjectOnGroundProperly(obj.Entity, obj.Superposition ? 1 : 0);

                        objects.Add(obj);
                    }

                    this.scenes.Add(new MapScene(scene.Key, new MenuContainer(scene.Key), objects));
                }
            }).Emit();

            tUpdate = new TaskManager.CAction(-1, 0, false, () =>
            {
                Update();
                UpdateScenesObjects();
                FocusControl();
                UpdateNoclip();
            });

            tUpdate1 = new TaskManager.CAction(1000, 0, false, () =>
            {
                UpdateScenesObjects();

                if (currentObjEntity != -1)
                {
                    var objPosition = GetEntityCoords(currentObjEntity, true, true);

                    coordsItem.Text = $"{Lang.Current["Client.MapEditor.Coords"]}: " + objPosition;
                    rotationItem.Text = $"{Lang.Current["Client.MapEditor.Rotate"]}: " + currentObjRotation;
                    hashItem.Text = "Hash: " + currentObjModel;
                }
            });

            tUpdate2 = new TaskManager.CAction(60000, 0, false, () =>
            {
                if (scenes.Exists(x => x.Name == currentSceneName))
                    SaveScene(scenes.Find(x => x.Name == currentSceneName));
            });
        }

        private void UpdateScenesObjects()
        {
            foreach (var scene in scenes)
            foreach (var obj in scene.Objects)
            {
                SetEntityCoordsNoOffset(obj.Entity, obj.Position.X, obj.Position.Y, obj.Position.Z, true, true, true);
                SetEntityRotation(obj.Entity, obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, 2, true);

                if (obj.PlaceOnGround) PlaceObjectOnGroundProperly(obj.Entity, obj.Superposition ? 1 : 0);
            }
        }

        private void RegisterNuiEvents()
        {
            NUI.RegisterNUICallback(Events.UI.OnKeyUp, OnKeyUp);
        }

        #region Menu Inits

        private void InitMenus()
        {
            mainMenu = new MenuContainer("MAP EDITOR");

            menu.CreateSubMenu(mainMenu);

            InitEnvironmentMenu();
            InitSceneMenu();
            InitSettingsMenu();

            menu.OnMenuChangeHandler += (oldMenu, newMenu) =>
            {
                if (newMenu == environmentMenu)
                {
                    if (currentObjEntity == -1)
                    {
                        if (currentObjType == 4)
                        {
                            //_currentObjEntity = StartParticleFxLoopedAtCoord(currentObjModelString, currentPointCoords.X, currentPointCoords.Y, currentPointCoords.Z, currentObjRotation.X, currentObjRotation.Y, currentObjRotation.Z, 1f, true, true, true, false);
                        }
                        else if (currentObjType == 8)
                        {
                            //_currentObjEntity = CreateVehicle(currentObjModel, currentPointCoords.X, currentPointCoords.Y, currentPointCoords.Z, currentPointCoords.Z, false, false, false, false);
                        }
                        else
                        {
                            currentObjEntity = CreateObject(currentObjModel, currentPointCoords.X,
                                currentPointCoords.Y, currentPointCoords.Z, false, false, true, false, false);
                        }
                    }
                }
                else if (newMenu == myScenesMenu)
                {
                    if (currentObjEntity != -1)
                        if (DoesEntityExist(currentObjEntity))
                        {
                            if (currentObjType == 4)
                                StopParticleFxLooped(currentObjEntity, false);
                            else if (currentObjType == 8)
                                DeleteVehicle(ref currentObjEntity);
                            else
                                DeleteEntity(ref currentObjEntity);

                            currentObjEntity = -1;
                        }
                }
            };
        }

        private void InitSceneMenu()
        {
            sceneMenu = new MenuContainer(Lang.Current["Client.MapEditor.Scenes"].ToUpper());
            menu.CreateSubMenu(sceneMenu);

            currentSceneItem = new MenuItem(Lang.Current["Client.MapEditor.CurrentScene"] + currentSceneName, environmentMenu, false);
            sceneMenu.AddItem(currentSceneItem);

            var sceneItem = new MenuItem(Lang.Current["Client.MapEditor.Scene"], sceneMenu);
            mainMenu.AddItem(sceneItem);

            InitCreateSceneMenu();
            InitAllScenesMenu();
        }

        private void InitAllScenesMenu()
        {
            myScenesMenu = new MenuContainer(Lang.Current["Client.MapEditor.MyScenes"].ToUpper());
            menu.CreateSubMenu(myScenesMenu);

            var mySceneItem = new MenuItem(Lang.Current["Client.MapEditor.MyScenes"], myScenesMenu, () =>
            {
                myScenesMenu.Items.Clear();

                for (var i = 0; i < scenes.Count; i++)
                {
                    var scene = scenes[i];

                    menu.CreateSubMenu(scene.Container);

                    scene.Container.Items.Clear();

                    MapObject currentEntity = null;

                    var settingsMenu = new MenuContainer(Lang.Current["Client.MapEditor.Settings"].ToUpper());

                    menu.CreateSubMenu(settingsMenu);

                    var teleport = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Teleport"], false, value => { });

                    MenuSliderSelectorItem<int> objectSelector = null;
                    objectSelector = new MenuSliderSelectorItem<int>(Lang.Current["Client.MapEditor.Object"], 0,
                        scene.Objects.Count - 1 <= 0 ? 0 : scene.Objects.Count - 1, 0, 1, index =>
                        {
                            // Temporary (prevent negative index problem)
                            if (index < 0)
                                index = 0;
                            else if (index > scene.Objects.Count - 1)
                                // Update MaxValue for prevent negative index on add new MapObject in collection
                                index = scene.Objects.Count - 1;
                            // ------------------------------------------

                            objectSelector.MaxValue = scene.Objects.Count - 1;

                            if (scene.Objects.Count > 0)
                            {
                                currentEntity = scene.Objects[index];

                                SetEntityAlpha(currentEntity.Entity, 150, false);

                                if (teleport.Checked)
                                    SetEntityCoordsNoOffset(PlayerPedId(), currentEntity.Position.X,
                                        currentEntity.Position.Y, currentEntity.Position.Z, true, true, true);

                                foreach (var obj in scene.Objects)
                                    if (obj.Entity != currentEntity.Entity)
                                        SetEntityAlpha(obj.Entity, 255, false);

                                objectSelector.Text = $"{Lang.Current["Client.MapEditor.Object"]}: " + currentEntity.Model;
                            }
                        });

                    var editing = new MenuItem(Lang.Current["Client.MapEditor.Modify"], () =>
                    {
                        currentSceneName = scene.Name;
                        currentSceneItem.Text = Lang.Current["Client.MapEditor.CurrentScene"] + currentSceneName;
                        currentSceneItem.Visible = true;

                        menu.OpenMenu(environmentMenu);
                    });

                    var save = new MenuItem(Lang.Current["Client.MapEditor.Save"], () => { SaveScene(scene); });

                    var deleteScene = new MenuItem(Lang.Current["Client.MapEditor.DeleteScene"], () =>
                    {
                        foreach (var obj in scene.Objects)
                            if (DoesEntityExist(obj.Entity))
                            {
                                var ent = obj.Entity;
                                DeleteEntity(ref ent);
                            }

                        var item = myScenesMenu.Items.Find(x => x.Text == scene.Name);
                        myScenesMenu.Items.Remove(item);

                        menu.OpenMenu(myScenesMenu);

                        scenes.Remove(scene);

                        currentSceneName = "";

                        currentSceneItem.Visible = false;

                        TriggerServerEvent(Events.MapEditor.OnDeleteScene, scene.Name);
                    });

                    var deleteObject = new MenuItem(Lang.Current["Client.MapEditor.DeleteObject"], () =>
                    {
                        if (currentEntity.Entity != -1)
                            if (DoesEntityExist(currentEntity.Entity))
                            {
                                var currentMapObject = scene.Objects.Find(x => x.Entity == currentEntity.Entity);

                                scene.Objects.Remove(currentMapObject);

                                var tempEnt = currentEntity.Entity;
                                DeleteEntity(ref tempEnt);

                                if (scene.Objects.Count == 1)
                                    currentEntity = scene.Objects[0];
                                else if (scene.Objects.Count == 0) objectSelector.Text = Lang.Current["Client.MapEditor.NoObject"];

                                objectSelector.Value = 0;

                                if (scene.Objects.Count <= 0)
                                    objectSelector.MaxValue = 0;
                                else
                                    objectSelector.MaxValue = scene.Objects.Count - 1;
                            }
                    });

                    scene.Container.AddItem(objectSelector);
                    scene.Container.AddItem(teleport);
                    scene.Container.AddItem(editing);
                    scene.Container.AddItem(save);
                    scene.Container.AddItem(deleteScene);
                    scene.Container.AddItem(deleteObject);

                    var sceneItem = new MenuItem(scene.Name, scene.Container);
                    myScenesMenu.AddItem(sceneItem);
                }

                menu.OpenMenu(myScenesMenu);
            });
            sceneMenu.AddItem(mySceneItem);
        }

        private void SaveScene(MapScene scene)
        {
            var dict = new Dictionary<string, List<MapObject>>();

            dict.Add(scene.Name, new List<MapObject>());

            foreach (var obj in scene.Objects)
                dict[scene.Name].Add(obj);

            TriggerServerEvent(Events.MapEditor.OnSaveScene, scene.Name, JsonConvert.SerializeObject(dict));
        }

        private void InitCreateSceneMenu()
        {
            var createSceneMenu = new MenuContainer(Lang.Current["Client.MapEditor.CreateScene"].ToUpper());
            menu.CreateSubMenu(createSceneMenu);

            var createSceneItem = new MenuItem(Lang.Current["Client.MapEditor.CreateScene"], createSceneMenu);
            sceneMenu.AddItem(createSceneItem);

            var sceneNameItem = new MenuTextboxItem(Lang.Current["Client.MapEditor.SceneName"], string.Empty, "Test", "", 1, 20,
                value => { currentSceneName = value.ToString(); });

            var createItem = new MenuItem(Lang.Current["Client.MapEditor.Create"], () =>
            {
                if (!string.IsNullOrEmpty(currentSceneName))
                {
                    if (!scenes.Exists(x => x.Name == currentSceneName))
                    {
                        scenes.Add(new MapScene(currentSceneName, new MenuContainer(currentSceneName.ToUpper()),
                            new List<MapObject>()));

                        currentSceneItem.Text = Lang.Current["Client.MapEditor.CurrentScene"] + currentSceneName;
                        currentSceneItem.Visible = true;

                        menu.CanCloseMenu = false;
                        // menu.MainMenu = environmentMenu;
                        menu.OpenMenu(environmentMenu);
                    }
                }
            });

            createSceneMenu.AddItem(sceneNameItem);
            createSceneMenu.AddItem(createItem);
        }

        private void InitEnvironmentMenu()
        {
            environmentMenu = new MenuContainer("ENVIRONNEMENT");
            menu.CreateSubMenu(environmentMenu);

            var categories = new List<MenuItemList.KeyValue<object>>
            {
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Generic"], Constant.Objects.Generic),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Structure"], Constant.Objects.Structure),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Interior"], Constant.Objects.Interior),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Area"], Constant.Objects.Area),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.VFX"], Constant.Objects.VFX),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Vegetation"], Constant.Objects.Vegetation),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Various"], Constant.Objects.LevDes),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.WeaponModels"], Constant.Objects.WeaponModels),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Transport"], Constant.Objects.Transport),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.VehicleProps"], Constant.Objects.VehicleProps),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.VehicleWheels"], Constant.Objects.VehicleWheels),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.MapEditor.Unknow"], Constant.Objects.Unknow)
            };
            
            var allObjects = new List<string>();
            allObjects.AddRange(Constant.Objects.Urban);
            allObjects.AddRange(Constant.Objects.Layout);
            allObjects.AddRange(Constant.Objects.LevDes);
            //            allObjects.AddRange(Object.MODELS);
            allObjects.AddRange(Constant.Objects.Summer);
            allObjects.AddRange(Constant.Objects.Unknow);
            allObjects.AddRange(Constant.Objects.Interior);
            allObjects.AddRange(Constant.Objects.Structure);
            //            allObjects.AddRange(Object.TRANSPORT);
            allObjects.AddRange(Constant.Objects.Vegetation);
            allObjects.AddRange(Constant.Objects.VehicleProps);
            allObjects.AddRange(Constant.Objects.VehicleWheels);
            //            allObjects.AddRange(Object.PEDS);
            allObjects.AddRange(Constant.Objects.Area);

            var currentArrayName = categories[0].Key;
            var currentArray = Constant.Objects.Generic;

            var categoryItem = new MenuItemList(Lang.Current["Client.MapEditor.Category"], 0, categories,
                (index, value) =>
                {
                    currentObjType = index;

                    currentArrayName = value.Key;
                    currentArray = (List<string>) value.Value;

                    selectorItem.Value = 0;
                    selectorItem.MinValue = 0;
                    selectorItem.MaxValue = currentArray.Count - 1;
                    selectorItem.Text = $"{value.Key}: [{selectorItem.Value}/{currentArray.Count - 1}]";

                    SetEntityVisible(currentObjEntity, visibleItem.Checked);
                    SetEntityHasGravity(currentObjEntity, gravityItem.Checked);
                    SetEntityCollision(currentObjEntity, collisionItem.Checked, true);
                    SetEntityAlpha(currentObjEntity, alphaItem.Value, false);

                    xOffsetItem.Value = 0f;
                    yOffsetItem.Value = 0f;
                    zOffsetItem.Value = 0f;
                });

            var searchItem = new MenuTextboxItem(Lang.Current["Client.MapEditor.Search"], "", Lang.Current["Client.MapEditor.ObjectName"], "", 0, 100, value =>
            {
                var objects = allObjects.Where(x =>
                    x.Contains(value.ToString()) && (x.StartsWith("p_") || x.StartsWith("s_") || x.StartsWith("w_") ||
                                                     x.StartsWith("mp_") || x.StartsWith("mp005") ||
                                                     x.StartsWith("mp006") || x.StartsWith("prop"))).ToList();

                if (objects.Count > 0)
                {
                    currentArray = objects;

                    selectorItem.Value = 0;
                    selectorItem.MaxValue = objects.Count - 1;
                    selectorItem.Text = $"[{0}/{objects.Count}]: {objects[selectorItem.Value]}";
                }
                else
                {
                    selectorItem.Value = 0;
                    selectorItem.MaxValue = 0;
                    selectorItem.Text = Lang.Current["Client.MapEditor.NoObjectFinded"];
                }
            });

            selectorItem = new MenuSliderSelectorItem<int>($"{currentArrayName}: ", 0, currentArray.Count - 1, 0, 1,
                index =>
                {
                    currentObjModel = (uint) GetHashKey(currentArray[index]);
                    currentObjModelString = currentArray[index];

                    if (lastObjEntity != currentObjEntity)
                    {
                        lastObjEntity = currentObjEntity;
                        DeleteEntity(ref lastObjEntity);
                    }

                    var position = new Vector3(currentPointCoords.X + xOffsetItem.Value,
                        currentPointCoords.Y + yOffsetItem.Value, currentPointCoords.Z + zOffsetItem.Value);

                    if (IsModelAVehicle(currentObjModel))
                        currentObjEntity = CreateVehicle(currentObjModel, position.X, position.Y, position.Z,
                            currentObjRotation.Z, false, false, false, false);
                    else
                        currentObjEntity = CreateObject(currentObjModel, position.X, position.Y, position.Z, false,
                            false, true, false, false);

                    selectorItem.Text = $"[{index}/{currentArray.Count - 1}]: {currentObjModelString}";

                    SetEntityVisible(currentObjEntity, visibleItem.Checked);
                    SetEntityHasGravity(currentObjEntity, gravityItem.Checked);
                    SetEntityCollision(currentObjEntity, collisionItem.Checked, true);
                    SetEntityAlpha(currentObjEntity, alphaItem.Value, false);

                    xOffsetItem.Value = 0f;
                    yOffsetItem.Value = 0f;
                    zOffsetItem.Value = 0f;
                });

            visibleItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Visible"], true, value =>
            {
                if (currentObjEntity != -1)
                {
                    visibleItem.Checked = value;
                    SetEntityVisible(currentObjEntity, value);
                }
            });

            gravityItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Gravity"], false, value =>
            {
                if (currentObjEntity != -1)
                {
                    gravityItem.Checked = value;
                    SetEntityHasGravity(currentObjEntity, value);
                }
            });

            collisionItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Collision"], true, value =>
            {
                if (currentObjEntity != -1) SetEntityCollision(currentObjEntity, value, true);
            });

            superpositionItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Layering"], true, value => { });

            onGroundItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.AlignToSurface"], true, value => { });

            drawAxesItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.ShowAxes"], true, value => { });
            drawBoxItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.ShowBox"], true, value => { });
            lockObjItem = new MenuItemCheckbox(Lang.Current["Client.MapEditor.Lock"], false, value => { });

            interpolationItem = new MenuItemList(Lang.Current["Client.MapEditor.InterpolationType"], 0, new List<MenuItemList.KeyValue<object>>
                {
                    new MenuItemList.KeyValue<object>("Map", 1),
                    new MenuItemList.KeyValue<object>("Object", 16),
                    new MenuItemList.KeyValue<object>("Water", 32)
                },
                (index, value) => { });

            alphaItem = new MenuSliderSelectorItem<int>($"{Lang.Current["Client.MapEditor.Opacity"]}: ", 0, 254, 254, 1, value =>
            {
                if (currentObjEntity != -1)
                    SetEntityAlpha(currentObjEntity, value, false);

                alphaItem.Text = $"Opacité: {(int) ((float) value / (float) alphaItem.MaxValue * 100f)}%";
            });

            offsetPrecisionItem = new MenuSliderSelectorItem<float>("Précision de l'offset", 0.01f, 1f, 0.01f, 0.01f,
                value =>
                {
                    offsetPrecisionItem.Text = $"Précision de l'offset: {value}";

                    xOffsetItem.Step = value;
                    yOffsetItem.Step = value;
                    zOffsetItem.Step = value;
                });

            xOffsetItem = new MenuSliderSelectorItem<float>("Offset X", minOffset, maxOffset, 0f, 0.01f,
                value => { xOffsetItem.Text = $"Offset X: {value}"; });

            yOffsetItem = new MenuSliderSelectorItem<float>("Offset Y", minOffset, maxOffset, 0f, 0.01f,
                value => { yOffsetItem.Text = $"Offset Y: {value}"; });

            zOffsetItem = new MenuSliderSelectorItem<float>("Offset Z", minOffset, maxOffset, 0f, 0.01f,
                value => { zOffsetItem.Text = $"Offset Z: {value}"; });

            coordsItem = new MenuItem(Lang.Current["Client.MapEditor.Coords"]);
            rotationItem = new MenuItem(Lang.Current["Client.MapEditor.Rotate"]);
            hashItem = new MenuItem("Hash");

            environmentMenu.AddItem(categoryItem);
            environmentMenu.AddItem(searchItem);
            environmentMenu.AddItem(selectorItem);
            environmentMenu.AddItem(visibleItem);
            environmentMenu.AddItem(gravityItem);
            environmentMenu.AddItem(collisionItem);
            environmentMenu.AddItem(superpositionItem);
            environmentMenu.AddItem(onGroundItem);
            environmentMenu.AddItem(interpolationItem);
            environmentMenu.AddItem(drawAxesItem);
            environmentMenu.AddItem(drawBoxItem);
            environmentMenu.AddItem(alphaItem);
            environmentMenu.AddItem(lockObjItem);
            environmentMenu.AddItem(offsetPrecisionItem);
            environmentMenu.AddItem(xOffsetItem);
            environmentMenu.AddItem(yOffsetItem);
            environmentMenu.AddItem(zOffsetItem);
            environmentMenu.AddItem(coordsItem);
            environmentMenu.AddItem(rotationItem);
            environmentMenu.AddItem(hashItem);

            selectorItem.Text = $"{currentArrayName}: [{selectorItem.Value}/{currentArray.Count - 1}]";
            alphaItem.Text = $"{Lang.Current["Client.MapEditor.Opacity"]}: {(int) ((float) alphaItem.Value / (float) alphaItem.MaxValue * 100f)}%";
        }

        private void InitSettingsMenu()
        {
            settingsMenu = new MenuContainer(Lang.Current["Client.MapEditor.Settings"].ToUpper());
            menu.CreateSubMenu(settingsMenu);

            var settingsItem = new MenuItem(Lang.Current["Client.MapEditor.Settings"], settingsMenu);
            mainMenu.AddItem(settingsItem);

            MenuSliderSelectorItem<float> speedItem = null;
            speedItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.MapEditor.MoveSpeed"], 0.1f, 10f, 1f, 0.1f, value =>
            {
                noclipSpeed = value;

                speedItem.Text =Lang.Current["Client.MapEditor.MoveSpeed"] + value.ToString("0.0");
            });

            MenuSliderSelectorItem<float> rotationSpeedItem = null;
            rotationSpeedItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.MapEditor.RotateSpeed"], 1f, 10f, 1f, 0.1f, value =>
            {
                rotationSpeed = rotationSpeedItem.Value;

                rotationSpeedItem.Text = Lang.Current["Client.MapEditor.RotateSpeed"] + value;
            });

            settingsMenu.AddItem(speedItem);
            settingsMenu.AddItem(rotationSpeedItem);

            speedItem.Text = Lang.Current["Client.MapEditor.MoveSpeed"] + speedItem.Value;
            rotationSpeedItem.Text = Lang.Current["Client.MapEditor.RotateSpeed"] + rotationSpeedItem.Value;
        }

        #endregion

        private void ToggleNoclip(bool enable)
        {
            var ped = PlayerPedId();

            isNoclip = enable;

            if (enable)
            {
                SetEntityInvincible(ped, true);
                SetEntityVisible(ped, false);
                SetEntityCollision(ped, false, false);
            }
            else
            {
                SetEntityInvincible(ped, false);
                SetEntityVisible(ped, true);
                SetEntityCollision(ped, true, true);
            }
        }

        private void UpdateNoclip()
        {
            if (isNoclip)
            {
                var ped = PlayerPedId();
                var position = GetEntityCoords(ped, true, true);
                var camDirection = CAPI.GetCamDirection();

                SetEntityVelocity(ped, 0.0001f, 0.0001f, 0.0001f);

                if (IsControlPressed(0, (uint) Keys.Z))
                {
                    position.X += noclipSpeed * camDirection.X;
                    position.Y += noclipSpeed * camDirection.Y;
                    position.Z += noclipSpeed * camDirection.Z;
                }

                if (IsControlPressed(0, (uint) Keys.S))
                {
                    position.X -= noclipSpeed * camDirection.X;
                    position.Y -= noclipSpeed * camDirection.Y;
                    position.Z -= noclipSpeed * camDirection.Z;
                }

                // Empêche un effet de rollback durant un changement de direction brutal
                SetPedMaxMoveBlendRatio(ped, 0f);
                SetPedMinMoveBlendRatio(ped, 0f);
                SetPedMoveAnimsBlendOut(ped);
                SetPedDesiredMoveBlendRatio(ped, 0f);

                SetEntityCoordsNoOffset(ped, position.X, position.Y, position.Z, true, true, true);

                RotateObjectControl();
            }
        }

        private void RotateObjectControl()
        {
            if (IsControlPressed(0, (uint) Keys.Q))
                currentObjRotation.Z += rotationSpeed;
            else if (IsControlPressed(0, (uint) Keys.E))
                currentObjRotation.Z -= rotationSpeed;

            if (IsControlPressed(0, (uint) Keys.DOWN))
                currentObjRotation.X += rotationSpeed;
            else if (IsControlPressed(0, (uint) Keys.UP))
                currentObjRotation.X -= rotationSpeed;

            if (IsControlPressed(0, (uint) Keys.RIGHT))
                currentObjRotation.Y += rotationSpeed;
            else if (IsControlPressed(0, (uint) Keys.LEFT))
                currentObjRotation.Y -= rotationSpeed;
        }

        private void FocusControl()
        {
            if (IsControlJustReleased(0, (uint) Keys.F))
            {
                isFocus = !isFocus;
                NUI.Focus(isFocus, isFocus);
            }
        }

        private void PlaceObject()
        {
            if (!string.IsNullOrEmpty(currentSceneName))
            {
                var entity = 0;
                var position = new Vector3(lastPointCoords.X + xOffsetItem.Value,
                    lastPointCoords.Y + yOffsetItem.Value, lastPointCoords.Z + zOffsetItem.Value);

                if (currentObjType == 4)
                {
                    // VFX
                    //entity = StartParticleFxLoopedAtCoord(currentObjModelString, position.X, position.Y, position.Z, currentObjRotation.X, currentObjRotation.Y, currentObjRotation.Y, 1f, true, true, true, false);
                }
                else if (currentObjType == 8)
                {
                    // VEHICLES
                    //entity = CreateVehicle(currentObjModel, position.X, position.Y, position.Z, currentObjRotation.Z, false, false, false, false);
                }
                else
                {
                    // OBJECTS
                    entity = CreateObject(currentObjModel, position.X, position.Y, position.Z, false, false, true,
                        false, false);
                }

                SetEntityCoordsNoOffset(entity, position.X, position.Y, position.Z, true, true, true);
                SetEntityRotation(entity, currentObjRotation.X, currentObjRotation.Y, currentObjRotation.Z, 2, true);

                if (onGroundItem.Checked)
                    PlaceObjectOnGroundProperly(entity, superpositionItem.Checked ? 1 : 0);

                if (scenes.Exists(x => x.Name == currentSceneName))
                    scenes.Find(x => x.Name == currentSceneName).Objects.Add(new MapObject(entity, currentObjModel,
                        position, currentObjRotation, alphaItem.Value, gravityItem.Checked, visibleItem.Checked,
                        superpositionItem.Checked, collisionItem.Checked, onGroundItem.Checked));
            }
        }

        private void PickupObject()
        {
            var raycast = CAPI.RaycastGameplayCamera(6f, 16);
            var hit = (bool) raycast[0];

            if (hit)
            {
                var entity = (int) raycast[3];

                if (lastObjEntity != currentObjEntity)
                {
                    lastObjEntity = currentObjEntity;
                    DeleteEntity(ref lastObjEntity);
                }

                currentObjModel = (uint) GetEntityModel(entity);
                currentObjEntity = CreateObject(currentObjModel, 0f, 0f, 0f, false, false, true, false, false);
            }
        }

        #region Ticks

        private void Update()
        {
            if (isNoclip)
            {
                if (IsControlJustReleased(0, (uint) Keys.MOUSE1))
                {
                    PlaceObject();
                }
                else if (IsControlJustReleased(0, (uint) Keys.MOUSE3))
                {
                    PickupObject();
                }
                else
                {
                    var raycast = CAPI.RaycastGameplayCamera(60f,
                        (int) interpolationItem.Values[interpolationItem.Index].Value);
                    currentPointCoords = (Vector3) raycast[1];

                    if (currentObjEntity != -1)
                    {
                        var entityCoords = GetEntityCoords(currentObjEntity, true, true);

                        if (!lockObjItem.Checked)
                        {
                            if (lastPointCoords != currentPointCoords)
                                lastPointCoords = currentPointCoords;

                            SetEntityCoordsNoOffset(currentObjEntity, currentPointCoords.X + xOffsetItem.Value,
                                currentPointCoords.Y + yOffsetItem.Value, currentPointCoords.Z + zOffsetItem.Value,
                                true, true, true);
                        }
                        else
                        {
                            SetEntityCoordsNoOffset(currentObjEntity, lastPointCoords.X + xOffsetItem.Value,
                                lastPointCoords.Y + yOffsetItem.Value, lastPointCoords.Z + zOffsetItem.Value, true,
                                true, true);
                        }

                        SetEntityRotation(currentObjEntity, currentObjRotation.X, currentObjRotation.Y,
                            currentObjRotation.Z, 2, true);

                        if (onGroundItem.Checked)
                            PlaceObjectOnGroundProperly(currentObjEntity, superpositionItem.Checked ? 1 : 0);

                        var lineWidth = 4f;
                        var xAxeX = 0f;
                        var xAxeY = 0f;
                        var yAxeX = 0f;
                        var yAxeY = 0f;
                        var zAxeX = 0f;
                        var zAxeY = 0f;

                        GetScreenCoordFromWorldCoord(entityCoords.X + lineWidth, entityCoords.Y, entityCoords.Z,
                            ref xAxeX, ref xAxeY);
                        GetScreenCoordFromWorldCoord(entityCoords.X, entityCoords.Y + lineWidth, entityCoords.Z,
                            ref yAxeX, ref yAxeY);
                        GetScreenCoordFromWorldCoord(entityCoords.X, entityCoords.Y, entityCoords.Z + lineWidth,
                            ref zAxeX, ref zAxeY);

                        if (drawAxesItem.Checked)
                        {
                            // Z Line
                            Function.Call((Hash) (uint) GetHashKey("DRAW_LINE"), entityCoords.X, entityCoords.Y,
                                entityCoords.Z, entityCoords.X, entityCoords.Y, entityCoords.Z + lineWidth, 224, 83, 83,
                                254);
                            // X Line
                            Function.Call((Hash) (uint) GetHashKey("DRAW_LINE"), entityCoords.X, entityCoords.Y,
                                entityCoords.Z, entityCoords.X + lineWidth, entityCoords.Y, entityCoords.Z, 3, 252, 173,
                                254);
                            // Y Line
                            Function.Call((Hash) (uint) GetHashKey("DRAW_LINE"), entityCoords.X, entityCoords.Y,
                                entityCoords.Z, entityCoords.X, entityCoords.Y + lineWidth, entityCoords.Z, 3, 182, 252,
                                254);

                            CAPI.DrawText(xAxeX, xAxeY - 0.025f, 0.25f, "X");
                            CAPI.DrawText(yAxeX, yAxeY - 0.025f, 0.25f, "Y");
                            CAPI.DrawText(zAxeX, zAxeY - 0.025f, 0.25f, "Z");
                        }

                        if (drawBoxItem.Checked)
                            CAPI.DrawBoxOnEntityModel(currentObjEntity);
                    }
                }
            }
        }

        #endregion

        #region Nui Events

        private CallbackDelegate OnKeyUp(IDictionary<string, object> data, CallbackDelegate result)
        {
            var key = int.Parse(data["key"].ToString());

            if (IsOpen)
                // F
                if (key == 70)
                {
                    isFocus = !isFocus;
                    NUI.Focus(isFocus, isFocus);
                }

            return result;
        }

        #endregion

        #region Commands

        [Command("editor.edit")]
        private async void MapEditorCommand(int source, List<object> args, string raw)
        {
            if (args.Count == 1)
            {
                var cmd = args[0].ToString();

                switch (cmd)
                {
                    case "open":
                        menu.CanCloseMenu = false;
                        menu.OpenMenu(mainMenu);

                        Main.GetScript<PlayerController>().CoreSystemEnabled = false;
                        Main.GetScript<PlayerController>().FullCore();

                        ToggleNoclip(true);

                        task.Add(tUpdate);
                        task.Add(tUpdate1);
                        task.Add(tUpdate2);

                        NUI.Focus(true, true);

                        IsOpen = true;

                        Warn(Lang.Current["Client.MapEditor.Open"]);
                        break;
                    case "close":
                        menu.CanCloseMenu = true;
                        menu.CloseMenu();

                        Main.GetScript<PlayerController>().CoreSystemEnabled = true;
                        Main.GetScript<PlayerController>().FullCore();                        
                        
                        NUI.Focus(false, false);

                        ToggleNoclip(false);

                        task.DeleteAction(tUpdate);
                        task.DeleteAction(tUpdate1);
                        task.DeleteAction(tUpdate2);

                        IsOpen = false;

                        if (currentObjEntity != -1)
                            if (DoesEntityExist(currentObjEntity))
                                DeleteEntity(ref currentObjEntity);

                        Warn(Lang.Current["Client.MapEditor.Close"]);
                        break;
                    case "help":
                        Warn(Lang.Current["Client.MapEditor.AvailableCommands"]);
                        WriteLog("- open");
                        WriteLog("- close");
                        break;
                }
            }
        }

        #endregion

        #region Events

        [EventHandler(Events.CFX.OnResourceStop)]
        private void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName)
            {
                if (DoesEntityExist(currentObjEntity)) DeleteEntity(ref currentObjEntity);

                foreach (var scene in scenes)
                foreach (var obj in scene.Objects)
                    if (DoesEntityExist(obj.Entity))
                    {
                        var ent = obj.Entity;
                        DeleteEntity(ref ent);
                    }
            }
        }

        #endregion
    }
}