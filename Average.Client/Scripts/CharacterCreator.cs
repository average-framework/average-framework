using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Core;
using Client.Core.Controllers;
using Client.Core.Enums;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Client.Core.UI.Menu;
using Client.Models;
using Shared.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;
using static Client.Constant;
using static Client.Core.Internal.CAPI;
using static Shared.Core.DataModels.CharacterData;
using Hash = CitizenFX.Core.Native.Hash;

namespace Client.Scripts
{
    public class CharacterCreator : Script
    {
        #region Variables

        protected Menu menu;

        protected MenuContainer mainMenu;
        protected MenuContainer faceMenu;
        protected MenuContainer faceFeaturesMenu;
        protected MenuContainer bodyMenu;
        protected MenuContainer clothesMenu;
        protected MenuContainer infoMenu;
        protected MenuContainer faceOverlayMenu;

        // Info Menu
        protected MenuItemList genderItem;
        protected MenuTextboxItem firstNameItem;
        protected MenuTextboxItem lastnameItem;
        protected MenuTextboxItem nationalityItem;
        protected MenuTextboxItem cityOfBirthItem;
        protected MenuTextboxItem dateOfBirthItem;
        protected MenuItemSlider<float> pedScaleItem;
        protected MenuSliderSelectorItem<int> culturesItem = null;
        protected MenuSliderSelectorItem<int> headsItem = null;
        protected MenuSliderSelectorItem<int> bodyItem = null;
        protected MenuSliderSelectorItem<int> legsItem = null;

        // Cloth Menu
        protected MenuSliderSelectorItem<int> hatsItem;
        protected MenuSliderSelectorItem<int> eyewearItem;
        protected MenuSliderSelectorItem<int> neckwearItem;
        protected MenuSliderSelectorItem<int> necktiesItem;
        protected MenuSliderSelectorItem<int> shirtsItem;
        protected MenuSliderSelectorItem<int> suspendersItem;
        protected MenuSliderSelectorItem<int> vestItem;
        protected MenuSliderSelectorItem<int> coatsItem;
        protected MenuSliderSelectorItem<int> coatsMPItem;
        protected MenuSliderSelectorItem<int> ponchosItem;
        protected MenuSliderSelectorItem<int> cloakItem;
        protected MenuSliderSelectorItem<int> glovesItem;
        protected MenuSliderSelectorItem<int> ringsRightHandItem;
        protected MenuSliderSelectorItem<int> ringsLeftHandItem;
        protected MenuSliderSelectorItem<int> braceletsItem;
        protected MenuSliderSelectorItem<int> gunbeltItem;
        protected MenuSliderSelectorItem<int> beltItem;
        protected MenuSliderSelectorItem<int> buckleItem;
        protected MenuSliderSelectorItem<int> holstersItem;
        protected MenuSliderSelectorItem<int> pantsItem;
        protected MenuSliderSelectorItem<int> skirtsItem;
        protected MenuSliderSelectorItem<int> bootsItem;
        protected MenuSliderSelectorItem<int> chapsItem;
        protected MenuSliderSelectorItem<int> spursItem;
        protected MenuSliderSelectorItem<int> spatsItem;
        protected MenuSliderSelectorItem<int> satchelsItem;
        protected MenuSliderSelectorItem<int> masksItem;
        protected MenuSliderSelectorItem<int> loadoutsItem;
        protected MenuSliderSelectorItem<int> legAttachmentsItem;
        protected MenuSliderSelectorItem<int> gauntletsItem;
        //protected MenuSliderSelectorItem<int> badgesItem;
        //protected MenuSliderSelectorItem<int> armorsItem;
        protected MenuSliderSelectorItem<int> accessoriesItem;

        // Face Menu
        protected MenuSliderSelectorItem<int> mustacheItem;
        protected MenuSliderSelectorItem<int> beardItem;
        protected MenuSliderSelectorItem<int> teethItem;
        protected MenuSliderSelectorItem<int> hairItem;
        protected MenuSliderSelectorItem<int> eyesItem;
        protected MenuSliderSelectorItem<float> headWidthItem = null;
        protected MenuSliderSelectorItem<float> eyebrowHeightItem = null;
        protected MenuSliderSelectorItem<float> eyebrowWidthItem = null;
        protected MenuSliderSelectorItem<float> eyebrowDepthItem = null;
        protected MenuSliderSelectorItem<float> earsWidthItem = null;
        protected MenuSliderSelectorItem<float> earsAngleItem = null;
        protected MenuSliderSelectorItem<float> earsHeightItem = null;
        protected MenuSliderSelectorItem<float> earsLobeSizeItem = null;
        protected MenuSliderSelectorItem<float> cheeckBonesHeightItem = null;
        protected MenuSliderSelectorItem<float> cheeckBonesWidthItem = null;
        protected MenuSliderSelectorItem<float> cheeckBonesDepthItem = null;
        protected MenuSliderSelectorItem<float> jawHeightItem = null;
        protected MenuSliderSelectorItem<float> jawWidthItem = null;
        protected MenuSliderSelectorItem<float> jawDepthItem = null;
        protected MenuSliderSelectorItem<float> chinHeightItem = null;
        protected MenuSliderSelectorItem<float> chinWidthItem = null;
        protected MenuSliderSelectorItem<float> chinDepthItem = null;
        protected MenuSliderSelectorItem<float> eyeLidHeightItem = null;
        protected MenuSliderSelectorItem<float> eyeLidWidthItem = null;
        protected MenuSliderSelectorItem<float> eyesDepthItem = null;
        protected MenuSliderSelectorItem<float> eyesAngleItem = null;
        protected MenuSliderSelectorItem<float> eyesDistanceItem = null;
        protected MenuSliderSelectorItem<float> eyesHeightItem = null;
        protected MenuSliderSelectorItem<float> noseWidthItem = null;
        protected MenuSliderSelectorItem<float> noseSizeItem = null;
        protected MenuSliderSelectorItem<float> noseHeightItem = null;
        protected MenuSliderSelectorItem<float> noseAngleItem = null;
        protected MenuSliderSelectorItem<float> noseCurvatureItem = null;
        protected MenuSliderSelectorItem<float> noStrilsDistanceItem = null;
        protected MenuSliderSelectorItem<float> mouthWidthItem = null;
        protected MenuSliderSelectorItem<float> mouthDepthItem = null;
        protected MenuSliderSelectorItem<float> mouthXPosItem = null;
        protected MenuSliderSelectorItem<float> mouthYPosItem = null;
        protected MenuSliderSelectorItem<float> upperLipHeightItem = null;
        protected MenuSliderSelectorItem<float> upperLipWidthItem = null;
        protected MenuSliderSelectorItem<float> upperLipDepthItem = null;
        protected MenuSliderSelectorItem<float> lowerLipHeightItem = null;
        protected MenuSliderSelectorItem<float> lowerLipWidthItem = null;
        protected MenuSliderSelectorItem<float> lowerLipDepthItem = null;

        // Body Menu
        protected MenuSliderSelectorItem<int> bodyTypesItem;
        protected MenuSliderSelectorItem<int> waistTypesItem;

        // Face Overlay Menu
        protected MenuSliderSelectorItem<int> overlayTypeItem = null;
        protected MenuSliderSelectorItem<int> overlayItem = null;
        protected MenuSliderSelectorItem<int> overlayVarItem = null;
        protected MenuSliderSelectorItem<int> overlayPrimaryColorItem = null;
        protected MenuSliderSelectorItem<int> overlaySecondaryColorItem = null;
        protected MenuSliderSelectorItem<int> overlayTertiaryColorItem = null;
        protected MenuSliderSelectorItem<int> overlayPaletteItem = null;
        protected MenuSliderSelectorItem<float> overlayOpacityItem = null;
        protected MenuItemCheckbox overlayVisibilityItem = null;

        protected Gender gender = Gender.Male;

        protected PedComponent culture = null;
        protected List<PedComponent> cultures = null;
        protected Dictionary<string, dynamic> textureType = new Dictionary<string, dynamic>();
        protected Dictionary<int, float> characterFaceParts = new Dictionary<int, float>
        {
            { FaceParts.CheeckBonesDepth, 0f },
            { FaceParts.CheeckBonesHeight, 0f },
            { FaceParts.CheeckBonesWidth, 0f },
            { FaceParts.ChinDepth, 0f },
            { FaceParts.ChinHeight, 0f },
            { FaceParts.ChinWidth, 0f },
            { FaceParts.EarsAngle, 0f },
            { FaceParts.EarsHeight, 0f },
            { FaceParts.EarsLobeSize, 0f },
            { FaceParts.EarsWidth, 0f },
            { FaceParts.EyebrowDepth, 0f },
            { FaceParts.EyebrowHeight, 0f },
            { FaceParts.EyebrowWidth, 0f },
            { FaceParts.EyeLidHeight, 0f },
            { FaceParts.EyeLidWidth, 0f },
            { FaceParts.EyesAngle, 0f },
            { FaceParts.EyesDepth, 0f },
            { FaceParts.EyesDistance, 0f },
            { FaceParts.EyesHeight, 0f },
            { FaceParts.HeadWidth, 0f },
            { FaceParts.JawDepth, 0f },
            { FaceParts.JawHeight, 0f },
            { FaceParts.JawWidth, 0f },
            { FaceParts.LowerLipDepth, 0f },
            { FaceParts.LowerLipHeight, 0f },
            { FaceParts.LowerLipWidth, 0f },
            { FaceParts.MouthDepth, 0f },
            { FaceParts.MouthWidth, 0f },
            { FaceParts.MouthXPos, 0f },
            { FaceParts.MouthYPos, 0f },
            { FaceParts.NoseAngle, 0f },
            { FaceParts.NoseCurvature, 0f },
            { FaceParts.NoseHeight, 0f },
            { FaceParts.NoseSize, 0f },
            { FaceParts.NoseWidth, 0f },
            { FaceParts.NoStrilsDistance, 0f },
            { FaceParts.UpperLipDepth, 0f },
            { FaceParts.UpperLipHeight, 0f },
            { FaceParts.UpperLipWidth, 0f },
        };
        protected Dictionary<string, uint> characterClothes = new Dictionary<string, uint>()
        {
            { ClothCategories.Accessories, 0 },
            { ClothCategories.Armors, 0 },
            { ClothCategories.Badges, 0 },
            { ClothCategories.Beards, 0 },
            { ClothCategories.Beltbuckles, 0 },
            { ClothCategories.Belts, 0 },
            { ClothCategories.Boots, 0 },
            { ClothCategories.Bracelts, 0 },
            { ClothCategories.Chaps, 0 },
            { ClothCategories.Cloaks, 0 },
            { ClothCategories.Coats, 0 },
            { ClothCategories.CoatsMP, 0 },
            { ClothCategories.Eyes, 0 },
            { ClothCategories.Eyewear, 0 },
            { ClothCategories.Gauntlets, 0 },
            { ClothCategories.Gloves, 0 },
            { ClothCategories.Gunbelts, 0 },
            { ClothCategories.Hairs, 0 },
            { ClothCategories.Hats, 0 },
            { ClothCategories.Heads, 0 },
            { ClothCategories.LegAttachements, 0 },
            { ClothCategories.Legs, 0 },
            { ClothCategories.Loadouts, 0 },
            { ClothCategories.Masks, 0 },
            { ClothCategories.Neckties, 0 },
            { ClothCategories.Neckwear, 0 },
            { ClothCategories.OffhandHolsters, 0 },
            { ClothCategories.Pants, 0 },
            { ClothCategories.Ponchos, 0 },
            { ClothCategories.RingsLeftHand, 0 },
            { ClothCategories.RingsRightHand, 0 },
            { ClothCategories.Satchels, 0 },
            { ClothCategories.Shirts, 0 },
            { ClothCategories.Skirts, 0 },
            { ClothCategories.Spats, 0 },
            { ClothCategories.Spurs, 0 },
            { ClothCategories.Suspenders, 0 },
            { ClothCategories.Teeth, 0 },
            { ClothCategories.Torsos, 0 },
            { ClothCategories.Vests, 0 },
        };
        protected Dictionary<string, OverlayData> characterOverlays = new Dictionary<string, OverlayData>
        {
            { "eyebrows", new OverlayData("eyebrows") },
            { "scars", new OverlayData("scars", 1) },
            { "eyeliners", new OverlayData("eyeliners") },
            { "lipsticks", new OverlayData("lipsticks") },
            { "acne", new OverlayData("acne", 1) },
            { "shadows", new OverlayData("shadows") },
            { "beardstabble", new OverlayData("beardstabble") },
            { "paintedmasks", new OverlayData("paintedmasks") },
            { "ageing", new OverlayData("ageing", 1) },
            { "blush", new OverlayData("blush") },
            { "complex", new OverlayData("complex", 1) },
            { "disc", new OverlayData("disc", 1) },
            { "foundation", new OverlayData("foundation") },
            { "freckles", new OverlayData("freckles", 1) },
            { "grime", new OverlayData("grime") },
            { "hair", new OverlayData("hair") },
            { "moles", new OverlayData("moles", 1) },
            { "spots", new OverlayData("spots", 1) },
        };
        protected Dictionary<string, object> characterInfos = new Dictionary<string, object>
        {
            { "firstname", "" },
            { "lastname", "" },
            { "nationality", "" },
            { "cityOfBirth", "" },
            { "dateOfBirth", "" },
        };

        protected int textureId = -1;
        protected int defaultCamera = 0;
        protected int faceCamera = 0;
        protected int bodyCamera = 0;
        protected int footCamera = 0;
        protected int currentCamIndex = 0;
        private float scale = 1f;
        private Vector3 destinationPosition = new Vector3(1.5f, 0f, 50f);

        #endregion

        public bool IsOpen { get; private set; } = false;
        public bool IsCreated { get; set; }

        public CharacterCreator(Main main) : base(main, "character_creator")
        {
            NUI.RegisterNUICallback(Events.UI.OnKeyUp, OnKeyUp);

            menu = Main.GetScript<Menu>();
            menu.CanCloseMenu = false;
        }

        #region NUI Callbacks

        private CallbackDelegate OnKeyUp(IDictionary<string, object> data, CallbackDelegate result)
        {
            if (IsOpen)
            {
                var key = int.Parse(data["key"].ToString());

                if (key == 37)
                {
                    // LEFT

                    var heading = GetEntityHeading(PlayerPedId());
                    heading -= 90f;
                    TaskAchieveHeading(PlayerPedId(), heading, 1000);
                }
                else if (key == 39)
                {
                    // RIGHT

                    var heading = GetEntityHeading(PlayerPedId());
                    heading += 90f;
                    TaskAchieveHeading(PlayerPedId(), heading, 1000);
                }
                else if (key == 38)
                {
                    // TOP

                    if (!IsCamInterpolating(defaultCamera) && !IsCamInterpolating(faceCamera) && !IsCamInterpolating(bodyCamera) && !IsCamInterpolating(footCamera))
                    {
                        if (currentCamIndex < 3)
                        {
                            currentCamIndex += 1;

                            SwitchCamera(currentCamIndex);

                            switch (currentCamIndex)
                            {
                                case 0:
                                    break;
                                case 1:
                                    SetCamActiveWithInterp(faceCamera, defaultCamera, 1000, 1, 0);
                                    break;
                                case 2:
                                    SetCamActiveWithInterp(bodyCamera, faceCamera, 1000, 1, 0);
                                    break;
                                case 3:
                                    SetCamActiveWithInterp(footCamera, bodyCamera, 1000, 1, 0);
                                    break;
                            }
                        }
                    }
                }
                else if (key == 40)
                {
                    // BOTTOM

                    if (!IsCamInterpolating(defaultCamera) && !IsCamInterpolating(faceCamera) && !IsCamInterpolating(bodyCamera) && !IsCamInterpolating(footCamera))
                    {
                        if (currentCamIndex > 0)
                        {
                            currentCamIndex -= 1;

                            SwitchCamera(currentCamIndex);

                            switch (currentCamIndex)
                            {
                                case 0:
                                    SetCamActiveWithInterp(defaultCamera, faceCamera, 1000, 1, 0);
                                    break;
                                case 1:
                                    SetCamActiveWithInterp(faceCamera, bodyCamera, 1000, 1, 0);
                                    break;
                                case 2:
                                    SetCamActiveWithInterp(bodyCamera, footCamera, 1000, 1, 0);
                                    break;
                                case 3:
                                    break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        private async void Save()
        {
            if (firstNameItem.Value.ToString().Length < firstNameItem.MinLength ||
                firstNameItem.Value.ToString().Length > firstNameItem.MaxLength ||
                string.IsNullOrEmpty(firstNameItem.Value.ToString()) ||
                firstNameItem.Value.ToString().Contains("\""))
            {
                return;
            }

            if (lastnameItem.Value.ToString().Length < lastnameItem.MinLength ||
                lastnameItem.Value.ToString().Length > lastnameItem.MaxLength ||
                string.IsNullOrEmpty(lastnameItem.Value.ToString()) ||
                lastnameItem.Value.ToString().Contains("\""))
            {
                return;
            }

            if (nationalityItem.Value.ToString().Length < nationalityItem.MinLength ||
                nationalityItem.Value.ToString().Length > nationalityItem.MaxLength ||
                string.IsNullOrEmpty(nationalityItem.Value.ToString()) ||
                nationalityItem.Value.ToString().Contains("\""))
            {
                return;
            }

            if (cityOfBirthItem.Value.ToString().Length < cityOfBirthItem.MinLength ||
                cityOfBirthItem.Value.ToString().Length > cityOfBirthItem.MaxLength ||
                string.IsNullOrEmpty(cityOfBirthItem.Value.ToString()) ||
                cityOfBirthItem.Value.ToString().Contains("\""))
            {
                return;
            }

            if (dateOfBirthItem.Value.ToString().Length < dateOfBirthItem.MinLength ||
              dateOfBirthItem.Value.ToString().Length > dateOfBirthItem.MaxLength ||
              string.IsNullOrEmpty(dateOfBirthItem.Value.ToString()))
            {
                return;
            }

            if (!char.IsNumber(dateOfBirthItem.Value.ToString()[0]) &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[1]) &&
                dateOfBirthItem.Value.ToString()[2] != '/' &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[3]) &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[4]) &&
                dateOfBirthItem.Value.ToString()[5] != '/' &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[6]) &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[7]) &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[8]) &&
                !char.IsNumber(dateOfBirthItem.Value.ToString()[9]))
            {
                return;
            }

            var dateSplit = dateOfBirthItem.Value.ToString().Split('/');

            if (int.Parse(dateSplit[0]) < 1 || int.Parse(dateSplit[0]) > 31)
            {
                return;
            }

            if (int.Parse(dateSplit[1]) < 1 || int.Parse(dateSplit[1]) > 12)
            {
                return;
            }

            if (int.Parse(dateSplit[2]) < 1770 || int.Parse(dateSplit[2]) > 1832)
            {
                return;
            }

            if (characterClothes.ContainsKey("0"))
            {
                characterClothes.Remove("0");
            }

            var user = Main.GetScript<User>();

            var characterData = new CharacterData
            {
                CharacterId = RandomString(),
                RockstarId = user.Data.RockstarId,
                Firstname = firstNameItem.Value.ToString(),
                Lastname = lastnameItem.Value.ToString(),
                Nationality = nationalityItem.Value.ToString(),
                CityOfBirth = cityOfBirthItem.Value.ToString(),
                DateOfBirth = dateOfBirthItem.Value.ToString(),
                CreationDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
                SexType = (int)gender,
                Culture = culturesItem.Value,
                Head = headsItem.Value,
                Body = bodyItem.Value,
                Legs = legsItem.Value,
                Scale = pedScaleItem.Value,
                BodyType = bodyTypesItem.Value,
                WaistType = waistTypesItem.Value,
                Texture = new Dictionary<string, dynamic>
                {
                    { "albedo", textureType["albedo"] },
                    { "normal", textureType["normal"] },
                    { "material", textureType["material"] }
                },
                FaceParts = characterFaceParts,
                Clothes = characterClothes,
                Overlays = characterOverlays,
                Economy = new EconomyData((decimal)Config["DefaultMoney"], (decimal)Config["DefaultBank"]),
                Position = new PositionData(destinationPosition.X, destinationPosition.Y, destinationPosition.Z, 0f),
            };

            menu.CloseMenu();
            NUI.Focus(false, false);

            var character = Main.GetScript<CharacterManager>();
            character.CreateCharacter(characterData);

            await NUI.FadeOut();
            await Delay(2000);

            character.Load();
            await character.IsReady();

            await Delay(0);
            RenderScriptCams(false, false, 0, true, true, 0);

            await NUI.FadeIn();

            NetworkEndTutorialSession();

            if (Main.ScriptIsStarted<PlayerController>())
            {
                Main.GetScript<PlayerController>().CoreSystemEnabled = true;
            }

            NUI.Visibility("hit-menu", 0, true);

            PauseClock(false, 0);
            SetWeatherTypeFrozen(false);
            FreezeEntityPosition(PlayerPedId(), false);

            IsOpen = false;
            IsCreated = true;

            Dispose();
        }

        private void SwitchCamera(int type)
        {
            switch (type)
            {
                case 0:
                    SetCamActive(defaultCamera, true);
                    SetCamActive(faceCamera, false);
                    SetCamActive(bodyCamera, false);
                    SetCamActive(footCamera, false);
                    break;
                case 1:
                    SetCamActive(defaultCamera, false);
                    SetCamActive(faceCamera, true);
                    SetCamActive(bodyCamera, false);
                    SetCamActive(footCamera, false);
                    break;
                case 2:
                    SetCamActive(defaultCamera, false);
                    SetCamActive(faceCamera, false);
                    SetCamActive(bodyCamera, true);
                    SetCamActive(footCamera, false);
                    break;
                case 3:
                    SetCamActive(defaultCamera, false);
                    SetCamActive(faceCamera, false);
                    SetCamActive(bodyCamera, false);
                    SetCamActive(footCamera, true);
                    break;
            }
        }

        private void InitCamera(Vector3 spawnPosition, float heading)
        {
            var ped = PlayerPedId();

            var defaultCamCoordOffset = Config["DefaultCamera"]["CamCoordOffset"];
            var defaultPointCamOffset = Config["DefaultCamera"]["PointCamOffset"];
            var defaultCamFov = (float)Config["DefaultCamera"]["Fov"];

            var faceCamCoordOffset = Config["FaceCamera"]["CamCoordOffset"];
            var facePointCamOffset = Config["FaceCamera"]["PointCamOffset"];
            var faceCamFov = (float)Config["FaceCamera"]["Fov"];

            var bodyCamCoordOffset = Config["BodyCamera"]["CamCoordOffset"];
            var bodyPointCamOffset = Config["BodyCamera"]["PointCamOffset"];
            var bodyCamFov = (float)Config["BodyCamera"]["Fov"];

            var footCamCoordOffset = Config["FootCamera"]["CamCoordOffset"];
            var footPointCamOffset = Config["FootCamera"]["PointCamOffset"];
            var footCamFov = (float)Config["FootCamera"]["Fov"];

            var headHeight = GetPedBoneCoords(ped, 168, 0f, 0f, 0f).Z + pedScaleItem.Value - 0.4f;
            var bodyHeight = GetPedBoneCoords(ped, 420, 0f, 0f, 0f).Z + pedScaleItem.Value - 0.8f;

            defaultCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", spawnPosition.X + (float)defaultCamCoordOffset[0], spawnPosition.Y + (float)defaultCamCoordOffset[1], spawnPosition.Z + (float)defaultCamCoordOffset[2], 0.0f, 0.0f, heading, defaultCamFov, false, 0);
            faceCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", spawnPosition.X + (float)faceCamCoordOffset[0], spawnPosition.Y + (float)faceCamCoordOffset[1], spawnPosition.Z + (float)faceCamCoordOffset[2], 0.0f, 0.0f, heading, faceCamFov, false, 0);
            bodyCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", spawnPosition.X + (float)bodyCamCoordOffset[0], spawnPosition.Y + (float)bodyCamCoordOffset[1], spawnPosition.Z + (float)bodyCamCoordOffset[2], 0.0f, 0.0f, heading, bodyCamFov, false, 0);
            footCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", spawnPosition.X + (float)footCamCoordOffset[0], spawnPosition.Y + (float)footCamCoordOffset[1], spawnPosition.Z + (float)footCamCoordOffset[2], 0.0f, 0.0f, heading, footCamFov, false, 0);

            PointCamAtCoord(defaultCamera, spawnPosition.X + (float)defaultPointCamOffset[0], spawnPosition.Y + (float)defaultPointCamOffset[1], spawnPosition.Z + (float)defaultPointCamOffset[2]);

            PointCamAtCoord(faceCamera, spawnPosition.X + (float)facePointCamOffset[0], spawnPosition.Y + (float)facePointCamOffset[1], headHeight + (float)facePointCamOffset[2]);
            SetCamCoord(faceCamera, spawnPosition.X + (float)faceCamCoordOffset[0], spawnPosition.Y + (float)faceCamCoordOffset[1], headHeight + (float)faceCamCoordOffset[2]);

            PointCamAtCoord(bodyCamera, spawnPosition.X + (float)bodyPointCamOffset[0], spawnPosition.Y + (float)bodyPointCamOffset[1], bodyHeight + (float)bodyPointCamOffset[2]);
            SetCamCoord(bodyCamera, spawnPosition.X + (float)bodyCamCoordOffset[0], spawnPosition.Y + (float)bodyCamCoordOffset[1], bodyHeight + (float)bodyCamCoordOffset[2]);

            PointCamAtCoord(footCamera, spawnPosition.X + (float)footPointCamOffset[0], spawnPosition.Y + (float)footPointCamOffset[1], bodyHeight + (float)footPointCamOffset[2]);
            SetCamCoord(footCamera, spawnPosition.X + (float)footCamCoordOffset[0], spawnPosition.Y + (float)footCamCoordOffset[1], bodyHeight + (float)footCamCoordOffset[2]);

            SwitchCamera(0);
            RenderScriptCams(true, true, 1000, true, true, 0);
        }

        public void Init(Vector3 spawnPosition, float heading)
        {
            IsOpen = true;
            Main.GetScript<Menu>().CanCloseMenu = false;
            mainMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.CreatingCharacter"]);

            SetWeatherType((uint)WeatherType.Sunny, true, true, true, 0f, false);
            SetWeatherTypeFrozen(true);
            NetworkClockTimeOverride(12, 0, 0, 1, true);
            PauseClock(true, 0);

            FreezeEntityPosition(PlayerPedId(), false);

            InitClothMenu();
            InitFaceMenu();
            InitBodyMenu();
            InitFaceFeaturesMenu();
            InitFaceOverlayMenu();
            InitInfoMenu(spawnPosition);

            mainMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.Info"], infoMenu));
            mainMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.Head"], faceMenu));
            mainMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.Body"], bodyMenu));
            mainMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.Clothes"], clothesMenu, () => { }));
            mainMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.CreateCharacter"], () => Save()));

            menu.CreateSubMenu(mainMenu);
            menu.OpenMenu(mainMenu);

            NUI.Focus(true, true);

            InitCamera(spawnPosition, heading);
            InitDefaultPed();
        }

        private void InitDefaultPed()
        {
            culture = gender == Gender.Male ? PedComponents[0] : PedComponents[6];
            cultures = PedComponents.Where(x => x.Gender == gender).ToList();

            culturesItem.Text = $"{Lang.Current["Client.CharacterCreator.Origin"]}: " + culturesItem.Value;

            headsItem.MinValue = 0;
            headsItem.MaxValue = culture.Heads.Count - 1;
            headsItem.Value = 0;

            bodyItem.MinValue = 0;
            bodyItem.MaxValue = culture.Body.Count - 1;
            bodyItem.Value = 0;

            legsItem.MinValue = 0;
            legsItem.MaxValue = culture.Legs.Count - 1;
            legsItem.Value = 0;

            UpdatePedBodyComponent(culture.Heads, headsItem, Lang.Current["Client.CharacterCreator.Heads"]);
            UpdatePedBodyComponent(culture.Body, bodyItem, Lang.Current["Client.CharacterCreator.Body"]);
            UpdatePedBodyComponent(culture.Legs, legsItem, Lang.Current["Client.CharacterCreator.Legs"]);

            RandomizeFace();

            InitPedOverlay();
            InitDefaultPedComponents();
        }

        private void InitDefaultPedComponents()
        {
            // Define max cloth value by gender
            beardItem.MaxValue = Clothes.BeardMale.Count - 1;
            skirtsItem.MaxValue = Clothes.LoadoutsFemale.Count - 1;
            hairItem.MaxValue = (gender == Gender.Male ? Clothes.HairMale.Count : Clothes.HairFemale.Count) - 1;
            eyesItem.MaxValue = (gender == Gender.Male ? Clothes.EyesMale.Count : Clothes.EyesFemale.Count) - 1;
            teethItem.MaxValue = (gender == Gender.Male ? Clothes.TeethMale.Count : Clothes.TeethFemale.Count) - 1;
            braceletsItem.MaxValue = (gender == Gender.Male ? Clothes.BraceletsMale.Count : Clothes.BraceletsFemale.Count) - 1;
            ringsLeftHandItem.MaxValue = (gender == Gender.Male ? Clothes.RingsLeftHandMale.Count : Clothes.RingsLeftHandFemale.Count) - 1;
            ringsRightHandItem.MaxValue = (gender == Gender.Male ? Clothes.RingsRightHandMale.Count : Clothes.RingsRightHandFemale.Count) - 1;
            holstersItem.MaxValue = (gender == Gender.Male ? Clothes.HolstersMale.Count : Clothes.HolstersFemale.Count) - 1;
            eyewearItem.MaxValue = (gender == Gender.Male ? Clothes.EyewearMale.Count : Clothes.EyewearFemale.Count) - 1;
            hatsItem.MaxValue = (gender == Gender.Male ? Clothes.HatsMale.Count : Clothes.HatsFemale.Count) - 1;
            shirtsItem.MaxValue = (gender == Gender.Male ? Clothes.ShirtsMale.Count : Clothes.ShirtsFemale.Count) - 1;
            vestItem.MaxValue = (gender == Gender.Male ? Clothes.VestMale.Count : Clothes.VestFemale.Count) - 1;
            pantsItem.MaxValue = (gender == Gender.Male ? Clothes.PantsMale.Count : Clothes.PantsFemale.Count) - 1;
            spursItem.MaxValue = (gender == Gender.Male ? Clothes.SpursMale.Count : Clothes.SpursFemale.Count) - 1;
            chapsItem.MaxValue = (gender == Gender.Male ? Clothes.ChapsMale.Count : Clothes.ChapsFemale.Count) - 1;
            cloakItem.MaxValue = (gender == Gender.Male ? Clothes.CloakMale.Count : Clothes.CloakFemale.Count) - 1;
            //badgesItem.MaxValue = (gender == Gender.Male ? Clothes.BadgesMale.Count : Clothes.BadgesFemale.Count) - 1;
            masksItem.MaxValue = (gender == Gender.Male ? Clothes.MasksMale.Count : Clothes.MasksFemale.Count) - 1;
            spatsItem.MaxValue = (gender == Gender.Male ? Clothes.SpatsMale.Count : Clothes.SpatsFemale.Count) - 1;
            neckwearItem.MaxValue = (gender == Gender.Male ? Clothes.NeckwearMale.Count : Clothes.NeckwearFemale.Count) - 1;
            bootsItem.MaxValue = (gender == Gender.Male ? Clothes.BootsMale.Count : Clothes.BootsFemale.Count) - 1;
            accessoriesItem.MaxValue = (gender == Gender.Male ? Clothes.AccessoriesMale.Count : Clothes.AccessoriesFemale.Count) - 1;
            gauntletsItem.MaxValue = (gender == Gender.Male ? Clothes.GauntletsMale.Count : Clothes.GauntletsFemale.Count) - 1;
            necktiesItem.MaxValue = (gender == Gender.Male ? Clothes.NecktiesMale.Count : Clothes.NecktiesFemale.Count) - 1;
            suspendersItem.MaxValue = (gender == Gender.Male ? Clothes.SuspendersMale.Count : Clothes.SuspendersFemale.Count) - 1;
            gunbeltItem.MaxValue = (gender == Gender.Male ? Clothes.GunbeltMale.Count : Clothes.GunbeltFemale.Count) - 1;
            beltItem.MaxValue = (gender == Gender.Male ? Clothes.BeltMale.Count : Clothes.BeltFemale.Count) - 1;
            buckleItem.MaxValue = (gender == Gender.Male ? Clothes.BuckleMale.Count : Clothes.BuckleFemale.Count) - 1;
            coatsItem.MaxValue = (gender == Gender.Male ? Clothes.CoatsMale.Count : Clothes.CoatsFemale.Count) - 1;
            coatsMPItem.MaxValue = (gender == Gender.Male ? Clothes.CoatsMpMale.Count : Clothes.CoatsMpFemale.Count) - 1;
            ponchosItem.MaxValue = (gender == Gender.Male ? Clothes.PonchosMale.Count : Clothes.PonchosFemale.Count) - 1;
            //armorsItem.MaxValue = (gender == Gender.Male ? Clothes.ArmorsMale.Count : Clothes.ArmorsFemale.Count) - 1;
            glovesItem.MaxValue = (gender == Gender.Male ? Clothes.GlovesMale.Count : Clothes.GlovesFemale.Count) - 1;
            satchelsItem.MaxValue = (gender == Gender.Male ? Clothes.SatchelsMale.Count : Clothes.SatchelsFemale.Count) - 1;
            legAttachmentsItem.MaxValue = (gender == Gender.Male ? Clothes.LegAttachmentsMale.Count : Clothes.LegAttachmentsFemale.Count) - 1;
            loadoutsItem.MaxValue = (gender == Gender.Male ? Clothes.LoadoutsMale.Count : Clothes.LoadoutsFemale.Count) - 1;

            if (gender == Gender.Male)
            {
                beardItem.Value = new Random(Environment.TickCount + 3).Next(0, Clothes.BeardMale.Count - 1);
            }
            else
            {
                beardItem.Visible = false;
                mustacheItem.Visible = false;
            }

            hairItem.Value = new Random(Environment.TickCount + 2).Next(0, (gender == Gender.Male ? Clothes.HairMale.Count : Clothes.HairFemale.Count) - 1);
            eyesItem.Value = new Random(Environment.TickCount + 4).Next(0, (gender == Gender.Male ? Clothes.EyesMale.Count : Clothes.EyesFemale.Count) - 1);
            teethItem.Value = new Random(Environment.TickCount + 1).Next(0, (gender == Gender.Male ? Clothes.TeethMale.Count : Clothes.TeethFemale.Count) - 1);

            if (gender == Gender.Male)
            {
                SetPedComponent(beardItem, Lang.Current["Client.CharacterCreator.Beard"], Clothes.BeardMale, beardItem.Value);
            }

            SetPedComponent(hairItem, Lang.Current["Client.CharacterCreator.Hair"], gender == Gender.Male ? Clothes.HairMale : Clothes.HairFemale, hairItem.Value);
            SetPedComponent(eyesItem, Lang.Current["Client.CharacterCreator.Eyes"], gender == Gender.Male ? Clothes.EyesMale : Clothes.EyesFemale, eyesItem.Value);
            SetPedComponent(teethItem, Lang.Current["Client.CharacterCreator.Teeth"], gender == Gender.Male ? Clothes.TeethMale : Clothes.TeethFemale, teethItem.Value);

            RemovePedComponent(ClothCategories.Pants);

            bodyTypesItem.Value = new Random(Environment.TickCount + 5).Next(0, Clothes.BodyTypes.Count - 1);
            waistTypesItem.Value = new Random(Environment.TickCount + 6).Next(0, Clothes.WaistTypes.Count - 1);

            SetPedBodyComponent(Clothes.BodyTypes, bodyTypesItem.Value);
            SetPedBodyComponent(Clothes.WaistTypes, waistTypesItem.Value);
        }

        private void InitInfoMenu(Vector3 spawnPosition)
        {
            infoMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.Info"].ToString().ToUpper());
            menu.CreateSubMenu(infoMenu);

            genderItem = new MenuItemList($"{Lang.Current["Client.CharacterCreator.Sex"]}: ", 0, new List<MenuItemList.KeyValue<object>>
            {
                new MenuItemList.KeyValue<object>(Lang.Current["Client.CharacterCreator.Male"], 0),
                new MenuItemList.KeyValue<object>(Lang.Current["Client.CharacterCreator.Female"], 1)
            },
            new Action<int, MenuItemList.KeyValue<object>>(async (index, value) =>
            {
                gender = (Gender)(int)value.Value;
                var model = gender == Gender.Male ? (uint)GetHashKey("mp_male") : (uint)GetHashKey("mp_female");

                SetPlayerModel(model);

                await Delay(0);

                InitDefaultPed();

                if (gender == Gender.Female)
                {
                    clothesMenu.AddItem(skirtsItem);
                }
                else
                {
                    clothesMenu.RemoveItem(skirtsItem);
                }
            }));

            firstNameItem = new MenuTextboxItem($"{Lang.Current["Client.CharacterCreator.Firstname"]}:", "", "Robert", "", 2, 20, null);
            lastnameItem = new MenuTextboxItem($"{Lang.Current["Client.CharacterCreator.Lastname"]}:", "", "Defontenais", "", 2, 20, null);
            nationalityItem = new MenuTextboxItem($"{Lang.Current["Client.CharacterCreator.Nationality"]}:", "", "Américaine", "", 2, 20, null);
            cityOfBirthItem = new MenuTextboxItem($"{Lang.Current["Client.CharacterCreator.PlaceOfBirth"]}:", "", "Valentine", "", 2, 20, null);
            dateOfBirthItem = new MenuTextboxItem($"{Lang.Current["Client.CharacterCreator.DateOfBirth"]}:", "", "01/01/1832", "", 10, 10, null);
            pedScaleItem = new MenuItemSlider<float>($"{Lang.Current["Client.CharacterCreator.Height"]}: ", 0.8f, 1.2f, 1f, 0.01f, async (value) =>
            {
                var ped = PlayerPedId();
                var size = (int)((value - 0.2f) * 100f);
                pedScaleItem.Text = $"{Lang.Current["Client.CharacterCreator.Height"]}: { (value < pedScaleItem.MaxValue ? "1m" : "2m") }{ (size == 100 ? "00" : size.ToString()) }";
                scale = value;

                CAPI.SetPedScale(PlayerPedId(), value);

                await Delay(100);

                var headHeight = GetPedBoneCoords(ped, 168, 0f, 0f, 0f).Z + value - 0.4f;
                PointCamAtCoord(faceCamera, spawnPosition.X + 0.1f, spawnPosition.Y, headHeight);
                SetCamCoord(faceCamera, spawnPosition.X + 1f, spawnPosition.Y + 1f, headHeight);

                var bodyHeight = GetPedBoneCoords(ped, 420, 0f, 0f, 0f).Z + pedScaleItem.Value - 0.8f;
                PointCamAtCoord(bodyCamera, spawnPosition.X + 0.1f, spawnPosition.Y, bodyHeight);
                SetCamCoord(bodyCamera, spawnPosition.X + 1f, spawnPosition.Y + 2f, bodyHeight);
            });

            // Default ped culture
            culture = PedComponents[0];

            // Default ped head texture
            textureType["albedo"] = GetHashKey(culture.HeadTexture);

            // Select culture by gender
            cultures = PedComponents.Where(x => x.Gender == gender).ToList();

            culturesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Origin"], 0, cultures.Count - 1, 0, 1, (value) =>
            {
                culturesItem.Text = $"{Lang.Current["Client.CharacterCreator.Origin"]}: " + culturesItem.Value;
                culture = cultures[culturesItem.Value];
                textureType["albedo"] = GetHashKey(culture.HeadTexture);

                headsItem.MinValue = 0;
                headsItem.MaxValue = culture.Heads.Count - 1;
                headsItem.Value = 0;

                bodyItem.MinValue = 0;
                bodyItem.MaxValue = culture.Body.Count - 1;
                bodyItem.Value = 0;

                legsItem.MinValue = 0;
                legsItem.MaxValue = culture.Legs.Count - 1;
                legsItem.Value = 0;

                UpdatePedBodyComponent(culture.Heads, headsItem, Lang.Current["Client.CharacterCreator.Heads"]);
                UpdatePedBodyComponent(culture.Body, bodyItem, Lang.Current["Client.CharacterCreator.Body"]);
                UpdatePedBodyComponent(culture.Legs, legsItem, Lang.Current["Client.CharacterCreator.Legs"]);
            });

            headsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Heads"], 0, culture.Heads.Count - 1, 0, 1, (value) =>
            {
                UpdatePedBodyComponent(culture.Heads, headsItem, Lang.Current["Client.CharacterCreator.Heads"]);
            });

            bodyItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Body"], 0, culture.Body.Count - 1, 0, 1, (value) =>
            {
                UpdatePedBodyComponent(culture.Body, bodyItem, Lang.Current["Client.CharacterCreator.Body"]);
            });

            legsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Legs"], 0, culture.Legs.Count - 1, 0, 1, (value) =>
            {
                UpdatePedBodyComponent(culture.Legs, legsItem, Lang.Current["Client.CharacterCreator.Legs"]);
            });

            infoMenu.AddItem(genderItem);
            infoMenu.AddItem(firstNameItem);
            infoMenu.AddItem(lastnameItem);
            infoMenu.AddItem(nationalityItem);
            infoMenu.AddItem(cityOfBirthItem);
            infoMenu.AddItem(dateOfBirthItem);
            infoMenu.AddItem(pedScaleItem);
            infoMenu.AddItem(culturesItem);
            infoMenu.AddItem(headsItem);
            infoMenu.AddItem(bodyItem);
            infoMenu.AddItem(legsItem);
        }

        private void InitFaceMenu()
        {
            faceMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.Faces"].ToString().ToUpper());
            menu.CreateSubMenu(faceMenu);

            eyesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Eyes"], 0, (gender == Gender.Male ? Clothes.EyesMale.Count : Clothes.EyesFemale.Count) - 1, 0, 1, (value) =>
            {
                SetPedComponent(eyesItem, Lang.Current["Client.CharacterCreator.Eyes"], gender == Gender.Male ? Clothes.EyesMale : Clothes.EyesFemale, value);
            });

            hairItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Hair"], -1, (gender == Gender.Male ? Clothes.HairMale.Count : Clothes.HairFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(hairItem, Lang.Current["Client.CharacterCreator.Hair"], gender == Gender.Male ? Clothes.HairMale : Clothes.HairFemale, value);
            });

            mustacheItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Moustache"], -1, Clothes.MustacheMale.Count - 1, -1, 1, (value) =>
            {
                SetPedComponent(mustacheItem, Lang.Current["Client.CharacterCreator.Moustache"], Clothes.MustacheMale, value);
            }, gender == Gender.Male ? true : false);

            beardItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Beard"], -1, Clothes.BeardMale.Count - 1, -1, 1, (value) =>
            {
                SetPedComponent(beardItem, Lang.Current["Client.CharacterCreator.Beard"], Clothes.BeardMale, value);
            }, gender == Gender.Male ? true : false);

            teethItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Teeth"], 0, (gender == Gender.Male ? Clothes.TeethMale.Count : Clothes.TeethFemale.Count) - 1, 0, 1, (value) =>
            {
                SetPedComponent(teethItem, Lang.Current["Client.CharacterCreator.Teeth"], gender == Gender.Male ? Clothes.TeethMale : Clothes.TeethFemale, value);
            });

            faceMenu.AddItem(eyesItem);
            faceMenu.AddItem(hairItem);
            faceMenu.AddItem(mustacheItem);
            faceMenu.AddItem(beardItem);
            faceMenu.AddItem(teethItem);
        }

        private void InitFaceFeaturesMenu()
        {
            faceFeaturesMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.FaceTraits"].ToString().ToUpper());
            faceMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.FaceTraits"], faceFeaturesMenu));

            menu.CreateSubMenu(faceFeaturesMenu);

            faceFeaturesMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.RandomFace"], () =>
            {
                RandomizeFace();
            }));

            headWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.HeadWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.HeadWidth, value);
                characterFaceParts[FaceParts.HeadWidth] = value;
            });
            faceFeaturesMenu.AddItem(headWidthItem);

            eyebrowHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyebrowsHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyebrowHeight, value);
                characterFaceParts[FaceParts.EyebrowHeight] = value;
            });
            faceFeaturesMenu.AddItem(eyebrowHeightItem);

            eyebrowWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyebrowsWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyebrowWidth, value);
                characterFaceParts[FaceParts.EyebrowWidth] = value;
            });
            faceFeaturesMenu.AddItem(eyebrowWidthItem);

            eyebrowDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyebrowsDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyebrowDepth, value);
                characterFaceParts[FaceParts.EyebrowDepth] = value;
            });
            faceFeaturesMenu.AddItem(eyebrowDepthItem);

            earsWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EarsWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EarsWidth, value);
                characterFaceParts[FaceParts.EarsWidth] = value;
            });
            faceFeaturesMenu.AddItem(earsWidthItem);

            earsAngleItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EarsCurvature"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EarsAngle, value);
                characterFaceParts[FaceParts.EarsAngle] = value;
            });
            faceFeaturesMenu.AddItem(earsAngleItem);

            earsHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EarsSize"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EarsHeight, value);
                characterFaceParts[FaceParts.EarsHeight] = value;
            });
            faceFeaturesMenu.AddItem(earsHeightItem);

            earsLobeSizeItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LobeSize"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EarsLobeSize, value);
                characterFaceParts[FaceParts.EarsLobeSize] = value;
            });
            faceFeaturesMenu.AddItem(earsLobeSizeItem);

            cheeckBonesHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.CheekbonesHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.CheeckBonesHeight, value);
                characterFaceParts[FaceParts.CheeckBonesHeight] = value;
            });
            faceFeaturesMenu.AddItem(cheeckBonesHeightItem);

            cheeckBonesWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.CheekbonesWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.CheeckBonesWidth, value);
                characterFaceParts[FaceParts.CheeckBonesWidth] = value;
            });
            faceFeaturesMenu.AddItem(cheeckBonesWidthItem);

            cheeckBonesDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.CheekbonesDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.CheeckBonesDepth, value);
                characterFaceParts[FaceParts.CheeckBonesDepth] = value;
            });
            faceFeaturesMenu.AddItem(cheeckBonesDepthItem);

            jawHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.JawHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.JawHeight, value);
                characterFaceParts[FaceParts.JawHeight] = value;
            });
            faceFeaturesMenu.AddItem(jawHeightItem);

            jawWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.JawWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.JawWidth, value);
                characterFaceParts[FaceParts.JawWidth] = value;
            });
            faceFeaturesMenu.AddItem(jawWidthItem);

            jawDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.JawDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.JawDepth, value);
                characterFaceParts[FaceParts.JawDepth] = value;
            });
            faceFeaturesMenu.AddItem(jawDepthItem);

            chinHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.ChinHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.ChinHeight, value);
                characterFaceParts[FaceParts.ChinHeight] = value;
            });
            faceFeaturesMenu.AddItem(chinHeightItem);

            chinWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.ChinWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.ChinWidth, value);
                characterFaceParts[FaceParts.ChinWidth] = value;
            });
            faceFeaturesMenu.AddItem(chinWidthItem);

            chinDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.ChinDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.ChinDepth, value);
                characterFaceParts[FaceParts.ChinDepth] = value;
            });
            faceFeaturesMenu.AddItem(chinDepthItem);

            eyeLidHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyelidHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyeLidHeight, value);
                characterFaceParts[FaceParts.EyeLidHeight] = value;
            });
            faceFeaturesMenu.AddItem(eyeLidHeightItem);

            eyeLidWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyelidWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyeLidWidth, value);
                characterFaceParts[FaceParts.EyeLidWidth] = value;
            });
            faceFeaturesMenu.AddItem(eyeLidWidthItem);

            eyesDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyesDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyesDepth, value);
                characterFaceParts[FaceParts.EyesDepth] = value;
            });
            faceFeaturesMenu.AddItem(eyesDepthItem);

            eyesAngleItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyesAngle"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyesAngle, value);
                characterFaceParts[FaceParts.EyesAngle] = value;
            });
            faceFeaturesMenu.AddItem(eyesAngleItem);

            eyesDistanceItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyesDistance"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyesDistance, value);
                characterFaceParts[FaceParts.EyesDistance] = value;
            });
            faceFeaturesMenu.AddItem(eyesDistanceItem);

            eyesHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.EyesHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.EyesHeight, value);
                characterFaceParts[FaceParts.EyesHeight] = value;
            });
            faceFeaturesMenu.AddItem(eyesHeightItem);

            noseWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NoseWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoseWidth, value);
                characterFaceParts[FaceParts.NoseWidth] = value;
            });
            faceFeaturesMenu.AddItem(noseWidthItem);

            noseSizeItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NoseSize"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoseSize, value);
                characterFaceParts[FaceParts.NoseSize] = value;
            });
            faceFeaturesMenu.AddItem(noseSizeItem);

            noseHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NoseHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoseHeight, value);
                characterFaceParts[FaceParts.NoseHeight] = value;
            });
            faceFeaturesMenu.AddItem(noseHeightItem);

            noseAngleItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NoseAngle"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoseAngle, value);
                characterFaceParts[FaceParts.NoseAngle] = value;
            });
            faceFeaturesMenu.AddItem(noseAngleItem);

            noseCurvatureItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NoseCurvature"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoseCurvature, value);
                characterFaceParts[FaceParts.NoseCurvature] = value;
            });
            faceFeaturesMenu.AddItem(noseCurvatureItem);

            noStrilsDistanceItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.NostrilsDistance"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.NoStrilsDistance, value);
                characterFaceParts[FaceParts.NoStrilsDistance] = value;
            });
            faceFeaturesMenu.AddItem(noStrilsDistanceItem);

            mouthWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.MouthWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.MouthWidth, value);
                characterFaceParts[FaceParts.MouthWidth] = value;
            });
            faceFeaturesMenu.AddItem(mouthWidthItem);

            mouthDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.MouthDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.MouthDepth, value);
                characterFaceParts[FaceParts.MouthDepth] = value;
            });
            faceFeaturesMenu.AddItem(mouthDepthItem);

            mouthXPosItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.MouthHorzPos"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.MouthXPos, value);
                characterFaceParts[FaceParts.MouthXPos] = value;
            });
            faceFeaturesMenu.AddItem(mouthXPosItem);

            mouthYPosItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.MouthVertPos"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.MouthYPos, value);
                characterFaceParts[FaceParts.MouthYPos] = value;
            });
            faceFeaturesMenu.AddItem(mouthYPosItem);

            upperLipHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsSupHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.UpperLipHeight, value);
                characterFaceParts[FaceParts.UpperLipHeight] = value;
            });
            faceFeaturesMenu.AddItem(upperLipHeightItem);

            upperLipWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsSupWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.UpperLipWidth, value);
                characterFaceParts[FaceParts.UpperLipWidth] = value;
            });
            faceFeaturesMenu.AddItem(upperLipWidthItem);

            upperLipDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsSupDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.UpperLipDepth, value);
                characterFaceParts[FaceParts.UpperLipDepth] = value;
            });
            faceFeaturesMenu.AddItem(upperLipDepthItem);

            lowerLipHeightItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsInfHeight"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.LowerLipHeight, value);
                characterFaceParts[FaceParts.LowerLipHeight] = value;
            });
            faceFeaturesMenu.AddItem(lowerLipHeightItem);

            lowerLipWidthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsInfWidth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.LowerLipWidth, value);
                characterFaceParts[FaceParts.LowerLipWidth] = value;
            });
            faceFeaturesMenu.AddItem(lowerLipWidthItem);

            lowerLipDepthItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.LipsInfDepth"], -1f, 1f, 0f, 0.1f, (value) =>
            {
                SetPedFaceFeature(FaceParts.LowerLipDepth, value);
                characterFaceParts[FaceParts.LowerLipDepth] = value;
            });
            faceFeaturesMenu.AddItem(lowerLipDepthItem);
        }

        private void InitPedOverlay()
        {
            if (gender == Gender.Male)
            {
                textureType["albedo"] = GetHashKey("mp_head_mr1_sc08_c0_000_ab");
                textureType["normal"] = GetHashKey("mp_head_mr1_000_nm");
                textureType["material"] = 0x7FC5B1E1;
                textureType["color_type"] = 1;
                textureType["texture_opacity"] = 1.0f;
                textureType["unk_arg"] = 0;
            }
            else
            {
                textureType["albedo"] = GetHashKey("mp_head_fr1_sc08_c0_000_ab");
                textureType["normal"] = GetHashKey("head_fr1_mp_002_nm");
                textureType["material"] = 0x7FC5B1E1;
                textureType["color_type"] = 1;
                textureType["texture_opacity"] = 1.0f;
                textureType["unk_arg"] = 0;
            }
        }

        private void InitFaceOverlayMenu()
        {
            faceOverlayMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.Facies"].ToString().ToUpper());
            faceMenu.AddItem(new MenuItem(Lang.Current["Client.CharacterCreator.Facies"], faceOverlayMenu));

            menu.CreateSubMenu(faceOverlayMenu);

            var overlays = OverlaysInfo.Where(x => x.Name == "eyebrows").ToList();
            var overlayInfo = overlays[0];

            characterOverlays[overlayInfo.Name].TxId = overlayInfo.Id;
            characterOverlays[overlayInfo.Name].TxNormal = overlayInfo.Normal;
            characterOverlays[overlayInfo.Name].TxMaterial = overlayInfo.Ma;

            overlayTypeItem = new MenuSliderSelectorItem<int>("", 0, OverlayLayers.Count - 1, 0, 1, async (value) =>
            {
                var overlay = characterOverlays.ElementAt(value).Value;

                overlays = OverlaysInfo.Where(x => x.Name == overlay.Name).ToList();

                if (overlays.Exists(x => x.Id == overlay.TxId))
                {
                    overlayInfo = overlays.Find(x => x.Id == overlay.TxId);
                }
                else
                {
                    overlayInfo = overlays[0];
                }

                await Delay(0);

                overlayTypeItem.Text = overlay.Name;

                overlayItem.MaxValue = overlays.Count - 1;
                overlayItem.Value = overlays.IndexOf(overlayInfo);
                overlayItem.Text = $"{Lang.Current["Client.CharacterCreator.Type"]}: " + overlayItem.Value;

                overlayInfo = overlays[overlayItem.Value];

                characterOverlays[overlayInfo.Name].TxId = overlayInfo.Id;
                characterOverlays[overlayInfo.Name].TxNormal = overlayInfo.Normal;
                characterOverlays[overlayInfo.Name].TxMaterial = overlayInfo.Ma;

                overlayVisibilityItem.Checked = characterOverlays[overlayInfo.Name].Visibility == 1 ? true : false;

                switch (overlayInfo.Name)
                {
                    case "eyeliners":
                        overlayItem.Visible = false;
                        overlayVarItem.Visible = true;
                        overlayVarItem.MaxValue = 15;
                        break;
                    case "shadows":
                        overlayItem.Visible = false;
                        overlayVarItem.Visible = true;
                        overlayVarItem.MaxValue = 5;
                        break;
                    case "lipsticks":
                        overlayItem.Visible = false;
                        overlayVarItem.Visible = true;
                        overlayVarItem.MaxValue = 7;
                        break;
                    default:
                        overlayItem.Visible = true;
                        overlayVarItem.Visible = false;
                        break;
                }

                switch (overlay.TxColorType)
                {
                    case 0:
                        overlayPaletteItem.Visible = true;
                        overlayPrimaryColorItem.Visible = true;
                        overlaySecondaryColorItem.Visible = true;
                        overlayTertiaryColorItem.Visible = true;
                        break;
                    case 1:
                        overlayPaletteItem.Visible = false;
                        overlayPrimaryColorItem.Visible = false;
                        overlaySecondaryColorItem.Visible = false;
                        overlayTertiaryColorItem.Visible = false;
                        break;
                    case 2:
                        overlayPaletteItem.Visible = true;
                        overlayPrimaryColorItem.Visible = true;
                        overlaySecondaryColorItem.Visible = true;
                        overlayTertiaryColorItem.Visible = true;
                        break;
                }

                overlayVarItem.Value = characterOverlays[overlayInfo.Name].Var;
                overlayVarItem.Text = $"{Lang.Current["Client.CharacterCreator.Variant"]}: " + overlayVarItem.Value;

                overlayPrimaryColorItem.Value = characterOverlays[overlayInfo.Name].PaletteColorPrimary;
                overlayPrimaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.PrimaryColor"]}: " + overlayPrimaryColorItem.Value;

                overlaySecondaryColorItem.Value = characterOverlays[overlayInfo.Name].PaletteColorSecondary;
                overlaySecondaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.SecondaryColor"]}: " + overlaySecondaryColorItem.Value;

                overlayTertiaryColorItem.Value = characterOverlays[overlayInfo.Name].PaletteColorTertiary;
                overlayTertiaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.TertiaryColor"]}: " + overlayTertiaryColorItem.Value;

                overlayPaletteItem.MinValue = 0;
                overlayPaletteItem.MaxValue = Clothes.ColorPalettes.Count - 1;
                overlayPaletteItem.Value = Clothes.ColorPalettes.IndexOf(characterOverlays[overlayInfo.Name].Palette.ToString("X"));
                overlayPaletteItem.Text = $"{Lang.Current["Client.CharacterCreator.Palette"]}: " + overlayPaletteItem.Value;

                overlayOpacityItem.Value = characterOverlays[overlayInfo.Name].Opacity;
                overlayOpacityItem.Text = $"{Lang.Current["Client.CharacterCreator.Opacity"]}: " + overlayOpacityItem.Value;

                await Delay(0);

                UpdateOverlay();
            });

            overlayItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Type"], 0, overlays.Count - 1, 0, 1, async (value) =>
            {
                overlayItem.Text = $"{Lang.Current["Client.CharacterCreator.Type"]}: " + overlayItem.Value;

                overlayInfo = overlays[overlayItem.Value];

                characterOverlays[overlayInfo.Name].TxId = overlayInfo.Id;
                characterOverlays[overlayInfo.Name].TxNormal = overlayInfo.Normal;
                characterOverlays[overlayInfo.Name].TxMaterial = overlayInfo.Ma;

                await Delay(0);

                UpdateOverlay();
            });

            overlayVisibilityItem = new MenuItemCheckbox(Lang.Current["Client.CharacterCreator.Visible"], false, async (isChecked) =>
            {
                characterOverlays[overlayInfo.Name].Visibility = isChecked ? 1 : 0;

                await Delay(0);

                UpdateOverlay();
            });

            overlayVarItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Variant"], 0, 254, 0, 1, async (value) =>
            {
                overlayVarItem.Text = $"{Lang.Current["Client.CharacterCreator.Variant"]}: " + overlayVarItem.Value;

                characterOverlays[overlayInfo.Name].Var = overlayVarItem.Value;

                await Delay(0);

                UpdateOverlay();
            }, false);

            overlayPrimaryColorItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.PrimaryColor"], 0, 254, 0, 1, async (value) =>
            {
                overlayPrimaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.PrimaryColor"]}: " + overlayPrimaryColorItem.Value;

                characterOverlays[overlayInfo.Name].PaletteColorPrimary = overlayPrimaryColorItem.Value;

                await Delay(0);

                UpdateOverlay();
            });

            overlaySecondaryColorItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.SecondaryColor"], 0, 254, 0, 1, async (value) =>
            {
                overlaySecondaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.SecondaryColor"]}: " + overlaySecondaryColorItem.Value;

                characterOverlays[overlayInfo.Name].PaletteColorSecondary = overlaySecondaryColorItem.Value;

                await Delay(0);

                UpdateOverlay();
            });

            overlayTertiaryColorItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.TertiaryColor"], 0, 254, 0, 1, async (value) =>
            {
                overlayTertiaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.TertiaryColor"]}: " + overlayTertiaryColorItem.Value;

                characterOverlays[overlayInfo.Name].PaletteColorTertiary = overlayTertiaryColorItem.Value;

                await Delay(0);

                UpdateOverlay();
            });

            overlayPaletteItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Palette"], 0, Clothes.ColorPalettes.Count - 1, 0, 1, async (value) =>
            {
                overlayPaletteItem.Text = $"{Lang.Current["Client.CharacterCreator.Palette"]}: " + overlayPaletteItem.Value;

                characterOverlays[overlayInfo.Name].Palette = uint.Parse(Clothes.ColorPalettes[overlayPaletteItem.Value], System.Globalization.NumberStyles.AllowHexSpecifier);

                await Delay(0);

                UpdateOverlay();
            });

            overlayOpacityItem = new MenuSliderSelectorItem<float>(Lang.Current["Client.CharacterCreator.Opacity"], 0f, 1f, 1f, 0.1f, async (value) =>
            {
                overlayOpacityItem.Text = $"{Lang.Current["Client.CharacterCreator.Opacity"]}: " + overlayOpacityItem.Value;

                characterOverlays[overlayInfo.Name].Opacity = overlayOpacityItem.Value;

                await Delay(0);

                UpdateOverlay();
            });

            faceOverlayMenu.AddItem(overlayTypeItem);
            faceOverlayMenu.AddItem(overlayItem);
            faceOverlayMenu.AddItem(overlayVisibilityItem);
            faceOverlayMenu.AddItem(overlayVarItem);
            faceOverlayMenu.AddItem(overlayPrimaryColorItem);
            faceOverlayMenu.AddItem(overlaySecondaryColorItem);
            faceOverlayMenu.AddItem(overlayTertiaryColorItem);
            faceOverlayMenu.AddItem(overlayPaletteItem);
            faceOverlayMenu.AddItem(overlayOpacityItem);

            overlayTypeItem.Text = OverlayLayers[overlayTypeItem.Value].Name;

            overlayItem.Text = $"{Lang.Current["Client.CharacterCreator.Type"]}: " + overlayItem.Value;
            overlayVarItem.Text = $"{Lang.Current["Client.CharacterCreator.Variant"]}: " + overlayVarItem.Value;
            overlayPrimaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.PrimaryColor"]}: " + overlayPrimaryColorItem.Value;
            overlaySecondaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.SecondaryColor"]}: " + overlaySecondaryColorItem.Value;
            overlayTertiaryColorItem.Text = $"{Lang.Current["Client.CharacterCreator.TertiaryColor"]}: " + overlayTertiaryColorItem.Value;
            overlayPaletteItem.Text = $"{Lang.Current["Client.CharacterCreator.Palette"]}: " + overlayPaletteItem.Value;
            overlayOpacityItem.Text = $"{Lang.Current["Client.CharacterCreator.Opacity"]}: " + overlayOpacityItem.Value;
        }

        private void InitBodyMenu()
        {
            bodyMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.Body"].ToString().ToUpper());
            menu.CreateSubMenu(bodyMenu);

            bodyTypesItem = new MenuSliderSelectorItem<int>("Morphologie", 0, Clothes.BodyTypes.Count - 1, 0, 1, (value) =>
            {
                bodyTypesItem.Text = $"{Lang.Current["Client.CharacterCreator.Morphology"]}: " + bodyTypesItem.Value;

                SetPedBodyComponent((uint)Clothes.BodyTypes[bodyTypesItem.Value]);
            });

            waistTypesItem = new MenuSliderSelectorItem<int>("Poids", 0, Clothes.WaistTypes.Count - 1, 0, 1, (value) =>
            {
                waistTypesItem.Text = $"{Lang.Current["Client.CharacterCreator.Weight"]}: " + waistTypesItem.Value;

                SetPedBodyComponent((uint)Clothes.WaistTypes[waistTypesItem.Value]);
            });

            bodyMenu.AddItem(bodyTypesItem);
            bodyMenu.AddItem(waistTypesItem);
        }

        private void InitClothMenu()
        {

            clothesMenu = new MenuContainer(Lang.Current["Client.CharacterCreator.Clothes"].ToString().ToUpper());
            menu.CreateSubMenu(clothesMenu);

            hatsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Hat"], -1, (gender == Gender.Male ? Clothes.HatsMale.Count : Clothes.HatsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(hatsItem, Lang.Current["Client.CharacterCreator.Hat"], gender == Gender.Male ? Clothes.HatsMale : Clothes.HatsFemale, value);
            });

            eyewearItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Glasses"], -1, (gender == Gender.Male ? Clothes.EyewearMale.Count : Clothes.EyewearFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(eyewearItem, Lang.Current["Client.CharacterCreator.Glasses"], gender == Gender.Male ? Clothes.EyewearMale : Clothes.EyewearFemale, value);
            });

            neckwearItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Bandana"], -1, (gender == Gender.Male ? Clothes.NeckwearMale.Count : Clothes.NeckwearFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(neckwearItem, Lang.Current["Client.CharacterCreator.Bandana"], gender == Gender.Male ? Clothes.NeckwearMale : Clothes.NeckwearFemale, value);
            });

            necktiesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Tie"], -1, (gender == Gender.Male ? Clothes.NecktiesMale.Count : Clothes.NecktiesFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(necktiesItem, Lang.Current["Client.CharacterCreator.Tie"], gender == Gender.Male ? Clothes.NecktiesMale : Clothes.NecktiesFemale, value); ;
            });
            shirtsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Shirt"], -1, (gender == Gender.Male ? Clothes.ShirtsMale.Count : Clothes.ShirtsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(shirtsItem, Lang.Current["Client.CharacterCreator.Shirt"], gender == Gender.Male ? Clothes.ShirtsMale : Clothes.ShirtsFemale, value);
            });

            suspendersItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Suspenders"], -1, (gender == Gender.Male ? Clothes.SuspendersMale.Count : Clothes.SuspendersFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(suspendersItem, Lang.Current["Client.CharacterCreator.Suspenders"], gender == Gender.Male ? Clothes.SuspendersMale : Clothes.SuspendersFemale, value);
            });

            vestItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Vest"], -1, (gender == Gender.Male ? Clothes.VestMale.Count : Clothes.VestFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(vestItem, Lang.Current["Client.CharacterCreator.Vest"], gender == Gender.Male ? Clothes.VestMale : Clothes.VestFemale, value);
            });

            coatsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Coat"], -1, (gender == Gender.Male ? Clothes.CoatsMale.Count : Clothes.CoatsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(coatsItem, Lang.Current["Client.CharacterCreator.Coat"], gender == Gender.Male ? Clothes.CoatsMale : Clothes.CoatsFemale, value);
            });

            coatsMPItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Jacket"], -1, (gender == Gender.Male ? Clothes.CoatsMpMale.Count : Clothes.CoatsMpFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(coatsMPItem, Lang.Current["Client.CharacterCreator.Jacket"], gender == Gender.Male ? Clothes.CoatsMpMale : Clothes.CoatsMpFemale, value);
            });

            ponchosItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Poncho"], -1, (gender == Gender.Male ? Clothes.PonchosMale.Count : Clothes.PonchosFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(ponchosItem, Lang.Current["Client.CharacterCreator.Poncho"], gender == Gender.Male ? Clothes.PonchosMale : Clothes.PonchosFemale, value);
            });

            cloakItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Cape"], -1, (gender == Gender.Male ? Clothes.CloakMale.Count : Clothes.CloakFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(cloakItem, Lang.Current["Client.CharacterCreator.Cape"], gender == Gender.Male ? Clothes.CloakMale : Clothes.CloakFemale, value);
            });

            glovesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Gloves"], -1, (gender == Gender.Male ? Clothes.GlovesMale.Count : Clothes.GlovesFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(glovesItem, Lang.Current["Client.CharacterCreator.Gloves"], gender == Gender.Male ? Clothes.GlovesMale : Clothes.GlovesFemale, value);
            });

            ringsRightHandItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.RingRightHand"], -1, (gender == Gender.Male ? Clothes.RingsRightHandMale.Count : Clothes.RingsRightHandFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(ringsRightHandItem, Lang.Current["Client.CharacterCreator.RingRightHand"], gender == Gender.Male ? Clothes.RingsRightHandMale : Clothes.RingsRightHandFemale, value);
            });

            ringsLeftHandItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.RingLeftHand"], -1, (gender == Gender.Male ? Clothes.RingsLeftHandMale.Count : Clothes.RingsLeftHandFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(ringsLeftHandItem, Lang.Current["Client.CharacterCreator.RingLeftHand"], gender == Gender.Male ? Clothes.RingsLeftHandMale : Clothes.RingsLeftHandFemale, value);
            });

            braceletsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Bracelet"], -1, (gender == Gender.Male ? Clothes.BraceletsMale.Count : Clothes.BraceletsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(braceletsItem, Lang.Current["Client.CharacterCreator.Bracelet"], gender == Gender.Male ? Clothes.BraceletsMale : Clothes.BraceletsFemale, value);
            });

            gunbeltItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Gunbelt"], -1, (gender == Gender.Male ? Clothes.GunbeltMale.Count : Clothes.GunbeltFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(gunbeltItem, Lang.Current["Client.CharacterCreator.Gunbelt"], gender == Gender.Male ? Clothes.GunbeltMale : Clothes.GunbeltFemale, value);
            });

            beltItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Belt"], -1, (gender == Gender.Male ? Clothes.BeltMale.Count : Clothes.BeltFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(beltItem, Lang.Current["Client.CharacterCreator.Belt"], gender == Gender.Male ? Clothes.BeltMale : Clothes.BeltFemale, value);
            });

            buckleItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Buckle"], -1, (gender == Gender.Male ? Clothes.BuckleMale.Count : Clothes.BuckleFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(buckleItem, Lang.Current["Client.CharacterCreator.Buckle"], gender == Gender.Male ? Clothes.BuckleMale : Clothes.BuckleFemale, value);
            });

            holstersItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Cases"], -1, (gender == Gender.Male ? Clothes.HolstersMale.Count : Clothes.HolstersFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(holstersItem, Lang.Current["Client.CharacterCreator.Cases"], gender == Gender.Male ? Clothes.HolstersMale : Clothes.HolstersFemale, value);
            });

            pantsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Pants"], -1, (gender == Gender.Male ? Clothes.PantsMale.Count : Clothes.PantsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(pantsItem, Lang.Current["Client.CharacterCreator.Pants"], gender == Gender.Male ? Clothes.PantsMale : Clothes.PantsFemale, value);
            });

            skirtsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Skirt"], -1, Clothes.SkirtsFemale.Count - 1, -1, 1, (value) =>
            {
                SetPedComponent(skirtsItem, Lang.Current["Client.CharacterCreator.Skirt"], Clothes.SkirtsFemale, value);
            });

            bootsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Boots"], -1, (gender == Gender.Male ? Clothes.BootsMale.Count : Clothes.BootsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(bootsItem, Lang.Current["Client.CharacterCreator.Boots"], gender == Gender.Male ? Clothes.BootsMale : Clothes.BootsFemale, value);
            });

            chapsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Chaps"], -1, (gender == Gender.Male ? Clothes.ChapsMale.Count : Clothes.ChapsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(chapsItem, Lang.Current["Client.CharacterCreator.Chaps"], gender == Gender.Male ? Clothes.ChapsMale : Clothes.ChapsFemale, value);
            });

            spursItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Spurs"], -1, (gender == Gender.Male ? Clothes.SpursMale.Count : Clothes.SpursFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(spursItem, Lang.Current["Client.CharacterCreator.Spurs"], gender == Gender.Male ? Clothes.SpursMale : Clothes.SpursFemale, value);
            });

            spatsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Spats"], -1, (gender == Gender.Male ? Clothes.SpatsMale.Count : Clothes.SpatsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(spatsItem, Lang.Current["Client.CharacterCreator.Spats"], gender == Gender.Male ? Clothes.SpatsMale : Clothes.SpatsFemale, value);
            });

            satchelsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Satchel"], -1, (gender == Gender.Male ? Clothes.SatchelsMale.Count : Clothes.SatchelsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(satchelsItem, Lang.Current["Client.CharacterCreator.Satchel"], gender == Gender.Male ? Clothes.SatchelsMale : Clothes.SatchelsFemale, value);
            });

            masksItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Mask"], -1, (gender == Gender.Male ? Clothes.MasksMale.Count : Clothes.MasksFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(masksItem, Lang.Current["Client.CharacterCreator.Mask"], gender == Gender.Male ? Clothes.MasksMale : Clothes.MasksFemale, value);
            });

            loadoutsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Strap"], -1, (gender == Gender.Male ? Clothes.LoadoutsMale.Count : Clothes.LoadoutsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(loadoutsItem, Lang.Current["Client.CharacterCreator.Strap"], gender == Gender.Male ? Clothes.LoadoutsMale : Clothes.LoadoutsFemale, value);
            });

            legAttachmentsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.LegAttachment"], -1, (gender == Gender.Male ? Clothes.LegAttachmentsMale.Count : Clothes.LegAttachmentsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(legAttachmentsItem, Lang.Current["Client.CharacterCreator.LegAttachment"], gender == Gender.Male ? Clothes.LegAttachmentsMale : Clothes.LegAttachmentsFemale, value);
            });

            gauntletsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Gauntlets"], -1, (gender == Gender.Male ? Clothes.GauntletsMale.Count : Clothes.GauntletsFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(gauntletsItem, Lang.Current["Client.CharacterCreator.Gauntlets"], gender == Gender.Male ? Clothes.GauntletsMale : Clothes.GauntletsFemale, value);
            });

            //badgesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Badges"], -1, (gender == Gender.Male ? Clothes.BadgesMale.Count : Clothes.BadgesFemale.Count) - 1, -1, 1, (value) =>
            //{
            //    SetPedComponent(badgesItem, Lang.Current["Client.CharacterCreator.Badges"], gender == Gender.Male ? Clothes.BadgesMale : Clothes.BadgesFemale, value);
            //});

            //armorsItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Armor"], -1, (gender == Gender.Male ? Clothes.ArmorsMale.Count : Clothes.ArmorsFemale.Count) - 1, -1, 1, (value) =>
            //{
            //    SetPedComponent(armorsItem, Lang.Current["Client.CharacterCreator.Armor"], gender == Gender.Male ? Clothes.ArmorsMale : Clothes.ArmorsFemale, value);
            //});

            accessoriesItem = new MenuSliderSelectorItem<int>(Lang.Current["Client.CharacterCreator.Accessories"], -1, (gender == Gender.Male ? Clothes.AccessoriesMale.Count : Clothes.AccessoriesFemale.Count) - 1, -1, 1, (value) =>
            {
                SetPedComponent(accessoriesItem, Lang.Current["Client.CharacterCreator.Accessories"], gender == Gender.Male ? Clothes.AccessoriesMale : Clothes.AccessoriesFemale, value);
            });

            clothesMenu.AddItem(hatsItem);
            clothesMenu.AddItem(eyewearItem);
            clothesMenu.AddItem(neckwearItem);
            clothesMenu.AddItem(necktiesItem);
            clothesMenu.AddItem(shirtsItem);
            clothesMenu.AddItem(vestItem);
            clothesMenu.AddItem(suspendersItem);
            clothesMenu.AddItem(coatsItem);
            clothesMenu.AddItem(coatsMPItem);
            clothesMenu.AddItem(ponchosItem);
            clothesMenu.AddItem(cloakItem);
            clothesMenu.AddItem(glovesItem);
            clothesMenu.AddItem(ringsRightHandItem);
            clothesMenu.AddItem(ringsLeftHandItem);
            clothesMenu.AddItem(braceletsItem);
            clothesMenu.AddItem(gunbeltItem);
            clothesMenu.AddItem(beltItem);
            clothesMenu.AddItem(buckleItem);
            clothesMenu.AddItem(holstersItem);
            clothesMenu.AddItem(pantsItem);
            clothesMenu.AddItem(bootsItem);
            clothesMenu.AddItem(chapsItem);
            clothesMenu.AddItem(spursItem);
            clothesMenu.AddItem(spatsItem);
            clothesMenu.AddItem(satchelsItem);
            clothesMenu.AddItem(masksItem);
            clothesMenu.AddItem(loadoutsItem);
            clothesMenu.AddItem(legAttachmentsItem);
            clothesMenu.AddItem(gauntletsItem);
            //clothesMenu.AddItem(badgesItem);
            //clothesMenu.AddItem(armorsItem);
            clothesMenu.AddItem(accessoriesItem);
        }

        private void RandomizeFace()
        {
            var values = new Dictionary<int, float>();

            for (int i = 0; i < Clothes.FaceParts.Count; i++)
            {
                var part = int.Parse(Clothes.FaceParts[i], System.Globalization.NumberStyles.AllowHexSpecifier);
                var rand = new Random(Environment.TickCount * i).Next(-10, 10) / 10f;
                SetPedFaceFeature(part, rand);
                values.Add(part, rand);

                characterFaceParts[part] = rand;
            }

            // Set item face part value
            headWidthItem.Value = values[FaceParts.HeadWidth];
            eyebrowHeightItem.Value = values[FaceParts.EyebrowHeight];
            eyebrowWidthItem.Value = values[FaceParts.EyebrowWidth];
            eyebrowDepthItem.Value = values[FaceParts.EyebrowDepth];
            earsWidthItem.Value = values[FaceParts.EarsWidth];
            earsHeightItem.Value = values[FaceParts.EarsHeight];
            earsAngleItem.Value = values[FaceParts.EarsAngle];
            earsLobeSizeItem.Value = values[FaceParts.EarsLobeSize];
            cheeckBonesHeightItem.Value = values[FaceParts.CheeckBonesHeight];
            cheeckBonesDepthItem.Value = values[FaceParts.CheeckBonesDepth];
            cheeckBonesWidthItem.Value = values[FaceParts.CheeckBonesWidth];
            jawHeightItem.Value = values[FaceParts.JawHeight];
            jawDepthItem.Value = values[FaceParts.JawDepth];
            jawWidthItem.Value = values[FaceParts.JawWidth];
            chinHeightItem.Value = values[FaceParts.ChinHeight];
            chinWidthItem.Value = values[FaceParts.ChinWidth];
            chinDepthItem.Value = values[FaceParts.ChinDepth];
            eyeLidHeightItem.Value = values[FaceParts.EyeLidHeight];
            eyeLidWidthItem.Value = values[FaceParts.EyeLidWidth];
            eyesDepthItem.Value = values[FaceParts.EyesDepth];
            eyesAngleItem.Value = values[FaceParts.EyesAngle];
            eyesDistanceItem.Value = values[FaceParts.EyesDistance];
            eyesHeightItem.Value = values[FaceParts.EyesHeight];
            noseWidthItem.Value = values[FaceParts.NoseWidth];
            noseSizeItem.Value = values[FaceParts.NoseSize];
            noseHeightItem.Value = values[FaceParts.NoseHeight];
            noseCurvatureItem.Value = values[FaceParts.NoseCurvature];
            noseAngleItem.Value = values[FaceParts.NoseAngle];
            noStrilsDistanceItem.Value = values[FaceParts.NoStrilsDistance];
            mouthDepthItem.Value = values[FaceParts.MouthDepth];
            mouthWidthItem.Value = values[FaceParts.MouthWidth];
            mouthXPosItem.Value = values[FaceParts.MouthXPos];
            mouthYPosItem.Value = values[FaceParts.MouthYPos];
            upperLipDepthItem.Value = values[FaceParts.UpperLipDepth];
            upperLipHeightItem.Value = values[FaceParts.UpperLipHeight];
            upperLipWidthItem.Value = values[FaceParts.UpperLipWidth];
            lowerLipDepthItem.Value = values[FaceParts.LowerLipDepth];
            lowerLipHeightItem.Value = values[FaceParts.LowerLipHeight];
            lowerLipWidthItem.Value = values[FaceParts.LowerLipWidth];
        }

        private async void UpdateOverlay()
        {
            var ped = PlayerPedId();

            if (textureId != -1)
            {
                ResetPedTexture2(textureId);
                DeletePedTexture(textureId);
            }

            textureId = Function.Call<int>((Hash)0xC5E7204F322E49EB, textureType["albedo"], textureType["normal"], textureType["material"]);

            foreach (var layer in characterOverlays.Values)
            {
                if (layer.Visibility != 0)
                {
                    var overlayId = AddPedOverlay(textureId, layer.TxId, layer.TxNormal, layer.TxMaterial, layer.TxColorType, layer.TxOpacity, layer.TxUnk);

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
                    await Delay(0);
                }

                OverrideTextureOnPed(ped, (uint)GetHashKey("heads"), textureId);
                UpdatePedTexture(textureId);
                UpdatePedVariation();
            }
        }

        private void UpdatePedBodyComponent(List<string> components, MenuSliderSelectorItem<int> item, string text)
        {
            var component = components[item.Value];

            item.Text = text + ": " + item.Value;

            //Function.Call((Hash)0x704C908E9C405136, PlayerPedId());
            UpdatePedVariation(PlayerPedId());
            SetPedComponentEnabled(PlayerPedId(), (uint)FromHexToHash(component), true, true, false);
        }

        private async void SetPedComponent(MenuSliderSelectorItem<int> item, string text, List<uint> components, int index)
        {
            item.Text = text + ": " + index;

            if (index != -1)
            {
                uint category = GetPedComponentCategory(components[index], (int)gender, true);

                characterClothes[category.ToString("X")] = components[index];

                SetPedComponentEnabled(components, index);
                UpdatePedVariation();

                // Find an index not equal to 0
                int cIndex = 0;

                while (category == 0)
                {
                    cIndex++;
                    category = GetPedComponentCategory(components[cIndex], (int)gender, true);

                    if (category != 0)
                    {
                        RemovePedComponent(category);
                    }

                    await Delay(0);
                }
            }
            else
            {
                var category = GetPedComponentCategory(components[0], (int)gender, true);

                characterClothes[category.ToString("X")] = 0;

                // Find an index not equal to 0
                var cIndex = 0;

                while (category == 0)
                {
                    cIndex++;
                    category = GetPedComponentCategory(components[cIndex], (int)gender, true);

                    await Delay(0);
                }

                RemovePedComponent(category);
            }

            await Delay(0);

            UpdatePedVariation();
        }

        private async void SetPedComponent(MenuSliderSelectorItem<int> item, string text, List<string> components, int index)
        {
            item.Text = text + ": " + index;

            if (index != -1)
            {
                var comp = uint.Parse(components[index], System.Globalization.NumberStyles.AllowHexSpecifier);
                uint category = GetPedComponentCategory(comp, (int)gender, true);

                characterClothes[category.ToString("X")] = comp;

                SetPedComponentEnabled(components, index);
                UpdatePedVariation();

                // Find an index not equal to 0
                int cIndex = 0;
                var comp2 = uint.Parse(components[cIndex], System.Globalization.NumberStyles.AllowHexSpecifier);

                while (category == 0)
                {
                    cIndex++;
                    category = GetPedComponentCategory(comp2, (int)gender, true);

                    if (category != 0)
                    {
                        RemovePedComponent(category);
                    }

                    await Delay(0);
                }
            }
            else
            {
                var category = GetPedComponentCategory(uint.Parse(components[0], System.Globalization.NumberStyles.AllowHexSpecifier), (int)gender, true);

                characterClothes[category.ToString("X")] = 0;

                // Find an index not equal to 0
                var cIndex = 0;
                var comp2 = uint.Parse(components[cIndex], System.Globalization.NumberStyles.AllowHexSpecifier);

                while (category == 0)
                {
                    cIndex++;
                    category = GetPedComponentCategory(comp2, (int)gender, true);

                    await Delay(0);
                }

                RemovePedComponent(category);
            }

            await Delay(0);

            UpdatePedVariation();
        }

        [EventHandler(Events.CFX.OnResourceStop)]
        private void OnResourceStop(string resourceName)
        {
            if (resourceName == ResourceName)
            {
                SetCamActive(defaultCamera, false);
                SetCamActive(faceCamera, false);
                SetCamActive(bodyCamera, false);
                SetCamActive(footCamera, false);

                RenderScriptCams(false, true, 1000, true, true, 0);

                DestroyCam(defaultCamera, true);
                DestroyCam(faceCamera, true);
                DestroyCam(bodyCamera, true);
                DestroyCam(footCamera, true);
            }
        }
    }
}
