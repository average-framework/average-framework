using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Client.Core;
using Client.Core.Enums;
using Client.Core.Internal;
using Client.Core.Managers;
using Client.Core.UI;
using Client.Core.UI.Menu;
using Client.Models;
using Newtonsoft.Json;
using Shared.Core.DataModels;
using static CitizenFX.Core.Native.API;

namespace Client.Scripts
{
    public class HorseSeller : Script
    {
        private MenuContainer mainMenu;
        private MenuContainer myHorseMenu;
        private MenuContainer customizationHorseMenu;
        private MenuItem paidItem;
        private MenuItem paidAccessoriesItem;
        private MenuTextboxItem horseNameItem;
        private MenuItemList genderItem;
        private HorseInfo currentHorse;
        private Models.HorseSeller currentSeller;
        private HorseData currentCustomizationHorseData;

        private int camera = -1;
        private int buyCamera = -1;
        private int customizationCamera = -1;
        private int currentPreviewHorseEntity = -1;
        private decimal horseCost;

        private readonly CharacterManager character;
        private readonly Stable stable;
        private readonly Menu menu;
        
        private Dictionary<string, Tuple<HorseComponentInfo, int>> horseComponentsCopy;
        private Dictionary<string, Tuple<HorseComponentInfo, int>> horseComponentsHistory = new Dictionary<string, Tuple<HorseComponentInfo, int>>
        {
                {HorseComponentsCategories.Bedrolls, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Blankets, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Horns, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Lantern, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Manes, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Mask, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Saddles, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Stirrups, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Tails, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.SaddleBags, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)}
        };
        private readonly List<MenuItemList.KeyValue<object>> horseCategories = new List<MenuItemList.KeyValue<object>>
        {
            new MenuItemList.KeyValue<object>("nokota", "Nokota"),
            new MenuItemList.KeyValue<object>("thoroughbred", "Pur-sang"),
            new MenuItemList.KeyValue<object>("americanstandardbred", "Trotteur américain"),
            new MenuItemList.KeyValue<object>("andalousian", "Andalou"),
            new MenuItemList.KeyValue<object>("ardennes", "Ardennais"),
            new MenuItemList.KeyValue<object>("hungarianhalfbred", "Demi-sang hongrois"),
            new MenuItemList.KeyValue<object>("americanpaint", "Peinture américaine"),
            new MenuItemList.KeyValue<object>("appaloosa", "Appaloosa"),
            new MenuItemList.KeyValue<object>("dutchwarmblood", "Hollandais à sang chaud"),
            new MenuItemList.KeyValue<object>("breton", "Breton"),
            new MenuItemList.KeyValue<object>("cobgypsy", "Cob gitan"),
            new MenuItemList.KeyValue<object>("criollo", "Criollo"),
            new MenuItemList.KeyValue<object>("kladruber", "Kladruber"),
            new MenuItemList.KeyValue<object>("missourifoxtrotter", "Missouri fox trotter"),
            new MenuItemList.KeyValue<object>("mustang", "Mustang"),
            new MenuItemList.KeyValue<object>("norfolkroadster", "Roadster de norfolk"),
            new MenuItemList.KeyValue<object>("turkoman", "Turc"),
            new MenuItemList.KeyValue<object>("arabian", "Pur-sang arabe"),
            new MenuItemList.KeyValue<object>("kentucky", "Kentucky"),
            new MenuItemList.KeyValue<object>("morgan", "Morgan"),
            new MenuItemList.KeyValue<object>("tennesseewalker", "Marcheur du tennessee"),
            new MenuItemList.KeyValue<object>("belgian", "Belge"),
            new MenuItemList.KeyValue<object>("shire", "Comté"),
            new MenuItemList.KeyValue<object>("suffolkpunch", "Suffolk punch"),
            new MenuItemList.KeyValue<object>("misc", "Divers")
        };
        private readonly List<HorseInfo> horseInfos = new List<HorseInfo>
        {
            new HorseInfo(PedHash.A_C_HORSE_NOKOTA_REVERSEDAPPLEROAN, "nokota", "Rouan pommelé inversé", "Course",
                new HorseStats(3, 3, 7, 5), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_NOKOTA_WHITEROAN, "nokota", "Rouan blanc", "Course",
                new HorseStats(3, 3, 4, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_NOKOTA_BLUEROAN, "nokota", "Rouan Bleu", "Course",
                new HorseStats(3, 3, 4, 3), 130.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_THOROUGHBRED_BRINDLE, "thoroughbred", "Bringée", "Course",
                new HorseStats(3, 3, 7, 5), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_THOROUGHBRED_BLOODBAY, "thoroughbred", "Baie acajou", "Course",
                new HorseStats(3, 3, 4, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_THOROUGHBRED_DAPPLEGREY, "thoroughbred", "Gris pommelé", "Course",
                new HorseStats(3, 3, 4, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_THOROUGHBRED_BLACKCHESTNUT, "thoroughbred", "Châtaigne noire",
                "Course", new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_THOROUGHBRED_REVERSEDAPPLEBLACK, "thoroughbred",
                "Noire pommelé inversé", "Course", new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_AMERICANSTANDARDBRED_PALOMINODAPPLE, "americanstandardbred",
                "Palomino pommelé", "Course", new HorseStats(3, 3, 5, 4), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANSTANDARDBRED_SILVERTAILBUCKSKIN, "americanstandardbred",
                "Peau de daim queue d'argent", "Course", new HorseStats(4, 4, 5, 4), 400.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANSTANDARDBRED_BUCKSKIN, "americanstandardbred",
                "Peau de daim légère", "Course", new HorseStats(3, 3, 4, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANSTANDARDBRED_LIGHTBUCKSKIN, "americanstandardbred", "Noire",
                "Course", new HorseStats(3, 3, 4, 3), 130.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_ANDALUSIAN_DARKBAY, "andalousian", "Baie sombre", "Guerre",
                new HorseStats(5, 4, 3, 3), 140.0m),
            new HorseInfo(PedHash.A_C_HORSE_ANDALUSIAN_ROSEGRAY, "andalousian", "Gris rose", "Guerre",
                new HorseStats(7, 5, 3, 3), 440.0m),
            new HorseInfo(PedHash.A_C_HORSE_ANDALUSIAN_PERLINO, "andalousian", "Perlino", "Guerre",
                new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_ARDENNES_BAYROAN, "ardennes", "Baie rouanné", "Guerre",
                new HorseStats(5, 4, 3, 3), 140.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARDENNES_STRAWBERRYROAN, "ardennes", "Rouan fraise", "Guerre",
                new HorseStats(7, 5, 3, 3), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARDENNES_IRONGREYROAN, "ardennes", "Grise", "Guerre",
                new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_HUNGARIANHALFBRED_LIVERCHESTNUT, "hungarianhalfbred",
                "Châtaigne de foie", "Guerre", new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_HUNGARIANHALFBRED_DARKDAPPLEGREY, "hungarianhalfbred",
                "Gris pommelé foncé", "Guerre", new HorseStats(5, 4, 3, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_HUNGARIANHALFBRED_FLAXENCHESTNUT, "hungarianhalfbred",
                "Châtaigne de lin", "Guerre", new HorseStats(4, 3, 3, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_HUNGARIANHALFBRED_PIEBALDTOBIANO, "hungarianhalfbred", "Pie tobiano",
                "Guerre", new HorseStats(4, 3, 3, 3), 130.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_AMERICANPAINT_OVERO, "americanpaint", "Overo", "Travail",
                new HorseStats(3, 4, 3, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANPAINT_GREYOVERO, "americanpaint", "Overo Gris", "Travail",
                new HorseStats(5, 5, 4, 4), 425.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANPAINT_TOBIANO, "americanpaint", "Tobiano", "Travail",
                new HorseStats(3, 4, 3, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_AMERICANPAINT_SPLASHEDWHITE, "americanpaint", "Eclaboussé de blanc",
                "Travail", new HorseStats(3, 5, 3, 3), 140.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_BLANKET, "appaloosa", "Capée", "Travail",
                new HorseStats(3, 4, 3, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_LEOPARD, "appaloosa", "Léopard", "Travail",
                new HorseStats(5, 6, 4, 3), 430.0m),
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_BROWNLEOPARD, "appaloosa", "Léopard brun", "Travail",
                new HorseStats(5, 6, 4, 3), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_LEOPARDBLANKET, "appaloosa", "Capé léopard", "Travail",
                new HorseStats(3, 4, 4, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_FEWSPOTTED_PC, "appaloosa", "Peu de tâches", "Travail",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_APPALOOSA_BLACKSNOWFLAKE, "appaloosa", "Flocon de neige noire",
                "Travail", new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_DUTCHWARMBLOOD_CHOCOLATEROAN, "dutchwarmblood", "Rouan chocolat",
                "Travail", new HorseStats(5, 6, 4, 3), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_DUTCHWARMBLOOD_SOOTYBUCKSKIN, "dutchwarmblood", "Peau de suie",
                "Travail", new HorseStats(4, 5, 3, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_DUTCHWARMBLOOD_SEALBROWN, "dutchwarmblood", "Joint marron", "Travail",
                new HorseStats(4, 5, 3, 3), 150.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_BRETON_STEELGREY, "breton", "Gris fer", "Inconnu",
                new HorseStats(4, 5, 4, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_BRETON_GRULLODUN, "breton", "Grullo", "Inconnu",
                new HorseStats(5, 5, 5, 3), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_BRETON_SORREL, "breton", "Oseille", "Inconnu",
                new HorseStats(4, 5, 4, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_BRETON_REDROAN, "breton", "Rouan rouge", "Inconnu",
                new HorseStats(4, 5, 4, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_BRETON_MEALYDAPPLEBAY, "breton", "Baie pommelé pangaré", "Inconnu",
                new HorseStats(6, 7, 6, 4), 1000.0m),
            new HorseInfo(PedHash.A_C_HORSE_BRETON_SEALBROWN, "breton", "Noir pangaré", "Inconnu",
                new HorseStats(5, 6, 5, 3), 550.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_PIEBALD, "cobgypsy", "Pie", "Inconnu",
                new HorseStats(6, 5, 2, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_SKEWBALD, "cobgypsy", "Skewbald", "Inconnu",
                new HorseStats(7, 6, 3, 3), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_WHITEBLAGDON, "cobgypsy", "Blagdon blanc", "Inconnu",
                new HorseStats(6, 5, 2, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_PALOMINOBLAGDON, "cobgypsy", "Blagdon palomino", "Inconnu",
                new HorseStats(7, 6, 3, 3), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_SPLASHEDBAY, "cobgypsy", "Baie balzan", "Inconnu",
                new HorseStats(6, 6, 5, 4), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_GYPSYCOB_SPLASHEDPIEBALD, "cobgypsy", "Pie balzan", "Inconnu",
                new HorseStats(6, 5, 5, 4), 950.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_DUN, "criollo", "Dun", "Inconnu",
                new HorseStats(3, 4, 5, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_BAYBRINDLE, "criollo", "Baie bringé", "Inconnu",
                new HorseStats(4, 5, 6, 4), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_BAYFRAMEOVERO, "criollo", "Baie frame overo", "Inconnu",
                new HorseStats(5, 6, 7, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_BLUEROANOVERO, "criollo", "Overo noir rouanné", "Inconnu",
                new HorseStats(3, 4, 5, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_MARBLESABINO, "criollo", "Sabino marmoré", "Inconnu",
                new HorseStats(5, 6, 7, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_CRIOLLO_SORRELOVERO, "criollo", "Overo oseille", "Inconnu",
                new HorseStats(4, 5, 6, 4), 550.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_BLACK, "kladruber", "Noire", "Inconnu",
                new HorseStats(5, 5, 2, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_WHITE, "kladruber", "Blanc", "Inconnu",
                new HorseStats(5, 5, 2, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_GREY, "kladruber", "Grise", "Inconnu",
                new HorseStats(6, 6, 3, 4), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_SILVER, "kladruber", "Argentée", "Inconnu",
                new HorseStats(7, 7, 4, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_CREMELLO, "kladruber", "Cremello", "Inconnu",
                new HorseStats(6, 6, 3, 4), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_KLADRUBER_DAPPLEROSEGREY, "kladruber", "Alezan grisonnant pommelé",
                "Inconnu", new HorseStats(7, 7, 4, 5), 950.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_MISSOURIFOXTROTTER_AMBERCHAMPAGNE, "missourifoxtrotter",
                "Champagne ambre", "Course, Travail", new HorseStats(5, 6, 7, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_MISSOURIFOXTROTTER_SILVERDAPPLEPINTO, "missourifoxtrotter",
                "Pinto pommelé argentée", "Course, Travail", new HorseStats(5, 6, 7, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_MISSOURIFOXTROTTER_SABLECHAMPAGNE, "missourifoxtrotter",
                "Champagne de sable", "Course, Travail", new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_MUSTANG_WILDBAY, "mustang", "Baie sauvage", "Guerre, Travail",
                new HorseStats(4, 4, 3, 2), 55.0m),
            new HorseInfo(PedHash.A_C_HORSE_MUSTANG_TIGERSTRIPEDBAY, "mustang", "Baie trigré", "Guerre, Travail",
                new HorseStats(5, 5, 4, 3), 450.0m),
            new HorseInfo(PedHash.A_C_HORSE_MUSTANG_GRULLODUN, "mustang", "Grullo dun", "Guerre, Travail",
                new HorseStats(4, 4, 3, 3), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_MUSTANG_GOLDENDUN, "mustang", "dun dorée", "Inconnu",
                new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_BLACK, "norfolkroadster", "Noire", "Inconnu",
                new HorseStats(2, 4, 5, 4), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_ROSEGREY, "norfolkroadster", "Gris rose", "Inconnu",
                new HorseStats(2, 4, 5, 4), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_PIEBALDROAN, "norfolkroadster", "Pie rouanné",
                "Inconnu", new HorseStats(3, 5, 6, 5), 550.0m),
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_SPECKLEDGREY, "norfolkroadster", "Gris tachetée",
                "Inconnu", new HorseStats(2, 4, 5, 4), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_DAPPLEDBUCKSKIN, "norfolkroadster", "Isabelle pommelé",
                "Inconnu", new HorseStats(4, 6, 7, 6), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_NORFOLKROADSTER_SPOTTEDTRICOLOR, "norfolkroadster",
                "Capé tâché tricolore", "Inconnu", new HorseStats(4, 6, 7, 6), 950.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_TURKOMAN_GOLD, "turkoman", "Dorée", "Course, Guerre",
                new HorseStats(7, 5, 6, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_TURKOMAN_SILVER, "turkoman", "Argentée", "Course, Guerre",
                new HorseStats(7, 5, 6, 5), 950.0m),
            new HorseInfo(PedHash.A_C_HORSE_TURKOMAN_DARKBAY, "turkoman", "Baie brun", "Course, Guerre",
                new HorseStats(7, 5, 6, 5), 950.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_BLACK, "arabian", "Noire", "Superior",
                new HorseStats(6, 6, 6, 6), 1050.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_ROSEGREYBAY, "arabian", "Baie Gris rose", "Superior",
                new HorseStats(7, 7, 6, 6), 1250.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_WHITE, "arabian", "Blanc", "Superior",
                new HorseStats(5, 5, 6, 6), 1200.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_REDCHESTNUT, "arabian", "Châtaigne rouge", "Superior",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_GREY, "arabian", "Grise", "Superior",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_REDCHESTNUT_PC, "arabian", "Châtaigne rouge PC", "Superior",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_ARABIAN_WARPEDBRINDLE_PC, "arabian", "Brindle déformé", "Superior",
                new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_KENTUCKYSADDLE_GREY, "kentucky", "Grise", "Equitation",
                new HorseStats(3, 3, 3, 2), 50.0m),
            new HorseInfo(PedHash.A_C_HORSE_KENTUCKYSADDLE_BLACK, "kentucky", "Noire", "Equitation",
                new HorseStats(3, 3, 3, 2), 50.0m),
            new HorseInfo(PedHash.A_C_HORSE_KENTUCKYSADDLE_SILVERBAY, "kentucky", "Baie argentée", "Equitation",
                new HorseStats(3, 3, 3, 2), 50.0m),
            new HorseInfo(PedHash.A_C_HORSE_KENTUCKYSADDLE_CHESTNUTPINTO, "kentucky", "Azelan pinto", "Equitation",
                new HorseStats(3, 3, 3, 2), 50.0m),
            new HorseInfo(PedHash.A_C_HORSE_KENTUCKYSADDLE_BUTTERMILKBUCKSKIN_PC, "kentucky", "Lait au beurre",
                "Equitation", new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_MORGAN_PALOMINO, "morgan", "Palomino", "Equitation",
                new HorseStats(2, 3, 3, 2), 15.0m),
            new HorseInfo(PedHash.A_C_HORSE_MORGAN_BAYROAN, "morgan", "Baie rouanné", "Equitation",
                new HorseStats(2, 3, 3, 2), 55.0m),
            new HorseInfo(PedHash.A_C_HORSE_MORGAN_BAY, "morgan", "Baie", "Equitation",
                new HorseStats(2, 3, 3, 2), 55.0m),
            new HorseInfo(PedHash.A_C_HORSE_MORGAN_FLAXENCHESTNUT, "morgan", "Azelan crins lavés", "Equitation",
                new HorseStats(2, 3, 3, 2), 55.0m),
            new HorseInfo(PedHash.A_C_HORSE_MORGAN_LIVERCHESTNUT_PC, "morgan", "Châtaigne de foie", "Equitation",
                new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_DAPPLEBAY, "tennesseewalker", "Baie pommelé",
                "Equitation", new HorseStats(3, 4, 2, 2), 60.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_REDROAN, "tennesseewalker", "Rouan rouge",
                "Equitation", new HorseStats(3, 3, 2, 2), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_CHESTNUT, "tennesseewalker", "Châtaigne", "Equitation",
                new HorseStats(3, 3, 2, 2), 60.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_BLACKRABICANO, "tennesseewalker", "Rabicano noire",
                "Equitation", new HorseStats(3, 3, 2, 2), 60.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_FLAXENROAN, "tennesseewalker", "Roan de lin",
                "Equitation", new HorseStats(4, 5, 3, 3), 150.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_MAHOGANYBAY, "tennesseewalker", "Baie d'acajou",
                "Equitation", new HorseStats(3, 4, 2, 2), 60.0m),
            new HorseInfo(PedHash.A_C_HORSE_TENNESSEEWALKER_GOLDPALOMINO_PC, "tennesseewalker", "Palomino dorée",
                "Equitation", new HorseStats(0, 0, 0, 0), 0.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_BELGIAN_BLONDCHESTNUT, "belgian", "Châtaigne clair", "Brouillon",
                new HorseStats(3, 3, 3, 3), 120.0m),
            new HorseInfo(PedHash.A_C_HORSE_BELGIAN_MEALYCHESTNUT, "belgian", "Châtaigne repas", "Brouillon",
                new HorseStats(3, 3, 3, 3), 120.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_SHIRE_RAVENBLACK, "shire", "Raven noire", "Brouillon",
                new HorseStats(4, 4, 3, 2), 130.0m),
            new HorseInfo(PedHash.A_C_HORSE_SHIRE_DARKBAY, "shire", "Baie sombre", "Brouillon",
                new HorseStats(4, 3, 3, 2), 120.0m),
            new HorseInfo(PedHash.A_C_HORSE_SHIRE_LIGHTGREY, "shire", "Gris clair", "Brouillon",
                new HorseStats(4, 3, 3, 2), 120.0m),
        
            new HorseInfo(PedHash.A_C_HORSE_SUFFOLKPUNCH_SORREL, "suffolkpunch", "Oseille", "Brouillon",
                new HorseStats(3, 4, 3, 2), 120.0m),
            new HorseInfo(PedHash.A_C_HORSE_SUFFOLKPUNCH_REDCHESTNUT, "suffolkpunch", "Châtaigne rouge",
                "Brouillon", new HorseStats(3, 4, 3, 2), 120.0m),
        
            new HorseInfo(PedHash.A_C_DONKEY_01, "misc", "Ane", "Divers", new HorseStats(0, 0, 0, 0),
                0.0m),
            new HorseInfo(PedHash.A_C_HORSEMULE_01, "misc", "Mule", "Divers",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSEMULEPAINTED_01, "misc", "Paint", "Divers",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_WINTER02_01, "misc", "Hiver 1", "Divers",
                new HorseStats(0, 0, 0, 0), 0.0m),
            new HorseInfo(PedHash.A_C_HORSE_WINTER02_01, "misc", "Hiver 2", "Divers",
                new HorseStats(0, 0, 0, 0), 0.0m)
        };
        private readonly List<HorseComponentInfo> horseComponents;
        private readonly List<int> horseCache = new List<int>();

        public bool IsOpen { get; private set; }
        
        public HorseSeller(Main main) : base(main)
        {
            Constant.HorseSellers = Configuration<List<Models.HorseSeller>>.Parse("config/horse_sellers");
            Constant.HorseComponents = Configuration<HorseComponent>.Parse("utils/horse_components");
            
            horseComponents = new List<HorseComponentInfo>
            {
                new HorseComponentInfo(HorseComponentsCategories.Bedrolls, Constant.HorseComponents.Bedrolls, 50),
                new HorseComponentInfo(HorseComponentsCategories.Blankets, Constant.HorseComponents.Blankets, 50),
                new HorseComponentInfo(HorseComponentsCategories.Horns, Constant.HorseComponents.Horns, 50),
                new HorseComponentInfo(HorseComponentsCategories.Lantern, Constant.HorseComponents.Lantern, 50),
                new HorseComponentInfo(HorseComponentsCategories.Manes, Constant.HorseComponents.Manes, 50),
                new HorseComponentInfo(HorseComponentsCategories.Mask, Constant.HorseComponents.Mask, 50),
                new HorseComponentInfo(HorseComponentsCategories.Saddles, Constant.HorseComponents.Saddles, 50),
                new HorseComponentInfo(HorseComponentsCategories.Stirrups, Constant.HorseComponents.Stirrups, 50),
                new HorseComponentInfo(HorseComponentsCategories.Tails, Constant.HorseComponents.Tails, 50),
                new HorseComponentInfo(HorseComponentsCategories.SaddleBags, Constant.HorseComponents.SaddleBags, 50)
            };
            
            character = Main.GetScript<CharacterManager>();
            stable = Main.GetScript<Stable>();
            menu = Main.GetScript<Menu>();

            NUI.Focus(false, false);
            RenderScriptCams(false, false, 0, true, true, 0);
            RegisterSellers();
            
            Task.Factory.StartNew(async () =>
            {
                await character.IsReady();
                await stable.IsReady();
                
                InitMainMenu();
            });
        }

        private async void RegisterSellers()
        {
            var npc = Main.GetScript<NpcManager>();
            var blip = Main.GetScript<BlipManager>();
        
            foreach (var seller in Constant.HorseSellers)
            {
                var npcPosition = new Vector3(seller.Npc.Position.X, seller.Npc.Position.Y, seller.Npc.Position.Z);
                var npcHeading = seller.Npc.Position.W;
                var blipPosition = new Vector3(seller.Blip.Position.X, seller.Blip.Position.Y, seller.Blip.Position.Z);
        
                seller.NPCsInstance = await npc.Create((uint) GetHashKey(seller.Npc.Model), seller.Npc.ModelVariation, npcPosition, npcHeading);
                blip.Create(seller.Blip.Sprite, seller.Blip.Label, seller.Blip.Scale, blipPosition);
            }
        }
        
        #region Ticks
        
        [Tick]
        private async Task Update()
        {
            var pos = GetEntityCoords(PlayerPedId(), true, true);
        
            foreach (var seller in Constant.HorseSellers)
            {
                var interactPosition = new Vector3(seller.Interact.Position.X, seller.Interact.Position.Y, seller.Interact.Position.Z);
        
                if (GetDistanceBetweenCoords(pos.X, pos.Y, pos.Z, interactPosition.X, interactPosition.Y, interactPosition.Z, true) <= seller.Interact.Radius)
                {
                    if (!seller.IsNear)
                    {
                        seller.IsNear = true;
        
                        Hud.SetMessage(Lang.Current["Client.HorseSeller.Enter.MessageLeft"], Lang.Current["Client.Keys.ALT"], Lang.Current["Client.HorseSeller.Enter.MessageRight"]);
                        Hud.SetVisibility(true);
                        Hud.SetContainerVisible(true);
                        Hud.SetHelpTextVisible(true);
                        Hud.SetPlayerVisible(false, 0);
                        Hud.SetHorseVisible(false, 0);
                        Hud.SetDeathScreenVisible(false, 0);
                        Hud.SetCooldownVisible(false, 0);
                    }

                    if (IsControlJustReleased(0, (uint) Keys.LALT))
                    {
                        IsOpen = true;
                        
                        currentSeller = seller;
                        menu.OpenMenu(mainMenu);

                        await DeleteHorse(currentPreviewHorseEntity);
                        await CreatePreviewHorse((uint) currentHorse.Hash);
        
                        var cameraPosition = new Vector3(seller.Buy.CameraPosition.X, seller.Buy.CameraPosition.Y, seller.Buy.CameraPosition.Z);
                        camera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0.0f, 0.0f, 0f, 40f, false, 0);
        
                        PointCamAtEntity(camera, currentSeller.NPCsInstance.Handle, 1f, 0.5f, 0.25f, true);
        
                        SwitchCamera(0);
                        RenderScriptCams(true, true, 1000, true, true, 0);
        
                        NUI.Focus(true, true);
                        Hud.SetVisibility(false);
                    }
                }
                else
                {
                    if (seller.IsNear)
                    {
                        seller.IsNear = false;
        
                        Hud.SetVisibility(false);
                        Hud.SetContainerVisible(false);
                        Hud.SetHelpTextVisible(false);
                    }
        
                    await Delay(250);
                }
            }
        }
        
        #endregion
        
        #region Methods
        
        private decimal CalculateHorseTotalCost()
        {
            paidItem.Text = Lang.Current["Client.HorseSeller.Pay"]
                .Replace("{0}", CAPI.ConvertDecimalToString(horseCost));
            return horseCost;
        }
        
        private decimal CalculateHorseComponentsCost()
        {
            var total = 0m;
        
            if (horseComponentsCopy != null)
                // If the horse has already been changed
                foreach (var comp in horseComponentsHistory)
                {
                    var compCopy = horseComponentsCopy[comp.Key];
        
                    if (comp.Value.Item2 != compCopy.Item2)
                        // The component is not the same, calculate..
        
                        if (comp.Value.Item2 != -1)
                            total += comp.Value.Item1.Cost;
                }
            else
                // The horse as never been changed
                foreach (var comp in horseComponentsHistory)
                    if (comp.Value.Item2 != -1)
                        total += comp.Value.Item1.Cost;
                    else
                        total += 0;
        
            paidAccessoriesItem.Text = Lang.Current["Client.HorseSeller.Pay"]
                .Replace("{0}", CAPI.ConvertDecimalToString(total));
            return total;
        }
        
        private void ApplyComponents()
        {
            foreach (var component in horseComponentsHistory)
                if (component.Value.Item1.Hashes.Count > 0)
                    CAPI.SetPedComponentEnabled(currentPreviewHorseEntity,
                        uint.Parse(component.Value.Item1.Hashes[component.Value.Item2], NumberStyles.AllowHexSpecifier),
                        true, true, true);
        }
        
        private void SwitchCamera(int index1, int index2 = 0)
        {
            switch (index1)
            {
                case 0:
                    SetCamActive(camera, true);
                    SetCamActive(buyCamera, false);
                    SetCamActive(customizationCamera, false);
        
                    switch (index2)
                    {
                        case 0:
                            SetCamActiveWithInterp(camera, buyCamera, 1000, 1, 1);
                            break;
                        case 1:
                            SetCamActiveWithInterp(camera, customizationCamera, 1000, 1, 1);
                            break;
                    }
        
                    break;
                case 1:
                    SetCamActive(camera, false);
                    SetCamActive(buyCamera, true);
                    SetCamActive(customizationCamera, false);
                    SetCamActiveWithInterp(buyCamera, camera, 1000, 1, 1);
                    break;
                case 2:
                    SetCamActive(camera, false);
                    SetCamActive(buyCamera, false);
                    SetCamActive(customizationCamera, true);
                    SetCamActiveWithInterp(customizationCamera, camera, 1000, 1, 1);
                    break;
            }
        }
        
        private async Task DeleteHorse(int entity)
        {
            NetworkRequestControlOfEntity(entity);
            SetEntityAsMissionEntity(entity, true, true);
        
            while (DoesEntityExist(entity))
            {
                DeletePed(ref entity);
                DeleteEntity(ref entity);
                await Delay(0);
            }
        }
        
        private async Task CreatePreviewHorse(uint model)
        {
            for (var i = 0; i < horseCache.Count; i++)
            {
                var ent = horseCache[i];
        
                if (DoesEntityExist(ent))
                {
                    horseCache.Remove(ent);
        
                    NetworkRequestControlOfEntity(ent);
                    SetEntityAsMissionEntity(ent, true, true);
        
                    while (DoesEntityExist(ent))
                    {
                        DeletePed(ref ent);
                        DeleteEntity(ref ent);
                        await Delay(100);
                    }
                }
            }
        
            if (DoesEntityExist(currentPreviewHorseEntity))
            {
                NetworkRequestControlOfEntity(currentPreviewHorseEntity);
                SetEntityAsMissionEntity(currentPreviewHorseEntity, true, true);
        
                while (DoesEntityExist(currentPreviewHorseEntity))
                {
                    DeletePed(ref currentPreviewHorseEntity);
                    DeleteEntity(ref currentPreviewHorseEntity);
                    await Delay(0);
                }
            }
        
            if (!DoesEntityExist(currentPreviewHorseEntity))
            {
                await CAPI.LoadModel(model);
        
                var handle = CreatePed(model, currentSeller.Buy.Position.X, currentSeller.Buy.Position.Y,
                    currentSeller.Buy.Position.Z, currentSeller.Buy.Heading, false, false, false, false);
        
                SetBlockingOfNonTemporaryEvents(handle, true);
                SetEntityVisible(handle, true);
                SetEntityInvincible(handle, true);
                SetPedCanBeTargetted(handle, false);
                SetPedCanPlayGestureAnims(handle, 1, 1);
                SetPedCanRagdoll(handle, false);
                CAPI.SetPedOutfitPreset(handle, 0);
                FreezeEntityPosition(handle, true);
                SetEntityCompletelyDisableCollision(handle, true, true);
        
                switch (genderItem.Index)
                {
                    case 0:
                        CAPI.SetPedFaceFeature(currentPreviewHorseEntity, 0xA28B, 0.0f);
                        break;
                    case 1:
                        CAPI.SetPedFaceFeature(currentPreviewHorseEntity, 0xA28B, 1.0f);
                        break;
                }
        
                CAPI.UpdatePedVariation(currentPreviewHorseEntity);
        
                horseCache.Add(handle);
        
                currentPreviewHorseEntity = handle;
            }
        }
        
        private async Task CreatePreviewHorseWithComponents(HorseData horse)
        {
            if (horseComponentsCopy != null) horseComponentsCopy.Clear();
        
            for (var i = 0; i < horseCache.Count; i++)
            {
                var ent = horseCache[i];
        
                if (DoesEntityExist(ent))
                {
                    horseCache.Remove(ent);
        
                    NetworkRequestControlOfEntity(ent);
                    SetEntityAsMissionEntity(ent, true, true);
        
                    while (DoesEntityExist(ent))
                    {
                        DeletePed(ref ent);
                        DeleteEntity(ref ent);
                        await Delay(100);
                    }
                }
            }
        
            if (DoesEntityExist(currentPreviewHorseEntity))
            {
                NetworkRequestControlOfEntity(currentPreviewHorseEntity);
                SetEntityAsMissionEntity(currentPreviewHorseEntity, true, true);
        
                while (DoesEntityExist(currentPreviewHorseEntity))
                {
                    DeletePed(ref currentPreviewHorseEntity);
                    DeleteEntity(ref currentPreviewHorseEntity);
                    await Delay(0);
                }
            }
        
            if (!DoesEntityExist(currentPreviewHorseEntity))
            {
                await CAPI.LoadModel(horse.Model);
        
                var handle = CreatePed(horse.Model, currentSeller.Customization.Position.X,
                    currentSeller.Customization.Position.Y, currentSeller.Customization.Position.Z,
                    currentSeller.Customization.Heading, false, false, false, false);
        
                SetBlockingOfNonTemporaryEvents(handle, true);
                SetEntityVisible(handle, true);
                SetEntityInvincible(handle, true);
                SetPedCanBeTargetted(handle, false);
                SetPedCanPlayGestureAnims(handle, 1, 1);
                SetPedCanRagdoll(handle, false);
                CAPI.SetPedOutfitPreset(handle, 0);
                FreezeEntityPosition(handle, true);
                SetEntityCompletelyDisableCollision(handle, true, true);
        
                switch (horse.Gender)
                {
                    case 0:
                        CAPI.SetPedFaceFeature(handle, 0xA28B, 0.0f);
                        break;
                    case 1:
                        CAPI.SetPedFaceFeature(handle, 0xA28B, 1.0f);
                        break;
                }
        
                CAPI.UpdatePedVariation(handle);
        
                if (horse.Components.Count > 0)
                {
                    foreach (var component in horse.Components)
                    {
                        // Set player horse components in horseComponentHistory
                        var hcs = horseComponents.Find(x => x.Category == component.Key);
                        var comp = uint.Parse(component.Value.ToString("X"), NumberStyles.AllowHexSpecifier);
                        var compIndex = hcs.Hashes.IndexOf(component.Value.ToString("X"));
                        horseComponentsHistory[component.Key] = new Tuple<HorseComponentInfo, int>(hcs, compIndex);
        
                        CAPI.SetPedComponentEnabled(handle, comp, true, true, true);
                        CAPI.UpdatePedVariation(handle);
                    }
        
                    // Update render
                    menu.UpdateRender(customizationHorseMenu);
                }
        
                horseComponentsCopy = new Dictionary<string, Tuple<HorseComponentInfo, int>>(horseComponentsHistory);
                horseCache.Add(handle);
                currentPreviewHorseEntity = handle;
            }
        }
        
        #endregion
        
        #region Menus
        
        private void InitMainMenu()
        {
            mainMenu = new MenuContainer(Lang.Current["Client.HorseSeller.MainMenu.Title"]);
            menu.CanCloseMenu = true;
            menu.OnMenuCloseHandler += async menu =>
            {
                if (menu == mainMenu)
                {
                    IsOpen = false;
                    
                    SetCamActive(camera, false);
                    SetCamActive(buyCamera, false);
                    SetCamActive(customizationCamera, false);
        
                    RenderScriptCams(false, true, 1000, true, true, 0);
        
                    DestroyCam(camera, true);
                    DestroyCam(buyCamera, true);
                    DestroyCam(customizationCamera, true);
        
                    await DeleteHorse(currentPreviewHorseEntity);
                }
            };
            menu.OnMenuChangeHandler += (oldMenu, newMenu) =>
            {
                if (oldMenu != null)
                {
                    if (newMenu == customizationHorseMenu)
                    {
                        
                    }
                    else if (oldMenu == myHorseMenu)
                    {
                        SwitchCamera(0, 1);
                    }
                    else if (newMenu == mainMenu)
                    {
                        SwitchCamera(0);
                    }
                }
                else
                {
                    if (newMenu == mainMenu) SwitchCamera(0);
                }
            };
            menu.CreateSubMenu(mainMenu);
        
            InitHorseMenu();
            InitMyHorseMenu();
        }
        
        private void InitHorseMenu()
        {
            var horses = horseInfos.Where(x => x.Category == "nokota").ToList();
            currentHorse = horses[0];
        
            var buyMenu = new MenuContainer(Lang.Current["Client.HorseSeller.BuyMenu.Title"]);
            menu.CreateSubMenu(buyMenu);
        
            var buyMenuItem = new MenuItem(Lang.Current["Client.HorseSeller.BuyMenuItem.Title"], buyMenu, () =>
            {
                // Remove all components (from my horses menu)
                horseComponentsHistory.Clear();
                // ------------------------------------------
        
                buyCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", currentSeller.Buy.CameraPosition.X,
                    currentSeller.Buy.CameraPosition.Y, currentSeller.Buy.CameraPosition.Z, 0.0f, 0.0f, 0f,
                    currentSeller.Buy.CameraFov, false, 0);
                PointCamAtCoord(buyCamera, currentSeller.Buy.Position.X + currentSeller.Buy.CameraLookAtOffset.X,
                    currentSeller.Buy.Position.Y + currentSeller.Buy.CameraLookAtOffset.Y,
                    currentSeller.Buy.Position.Z + currentSeller.Buy.CameraLookAtOffset.Z);
        
                SwitchCamera(1);
        
                horseCost = currentHorse.Cost;
                CalculateHorseTotalCost();
            });
            mainMenu.AddItem(buyMenuItem);
        
            MenuItemList raceMenuItem = null;
            MenuSliderSelectorItem<int> horseSelectorMenuItem = null;
            MenuItem typeMenuItem = null;
            MenuStatsItem hpStatsItem = null;
            MenuStatsItem staminaStatsItem = null;
            MenuStatsItem speedStatsItem = null;
            MenuStatsItem accelerationStatsItem = null;
        
            raceMenuItem = new MenuItemList(Lang.Current["Client.HorseSeller.RaceMenuItem.Title"], 0, horseCategories, async (index, value) =>
            {
                horses = horseInfos.Where(x => x.Category == value.Key).ToList();
                currentHorse = horses[0];
        
                raceMenuItem.Text = $"{Lang.Current["Client.HorseSeller.RaceMenuItem.Title"]}: " + horseCategories.Find(x => x.Key == currentHorse.Category).Value;
                horseSelectorMenuItem.Value = 0;
                horseSelectorMenuItem.MaxValue = horses.Count - 1;
                horseSelectorMenuItem.Text = $"{Lang.Current["Client.HorseSeller.HorseSelectorMenuItem.Title"]}: " + currentHorse.Coat;
                typeMenuItem.Text = $"{Lang.Current["Client.HorseSeller.TypeMenuItem.Title"]}: " + currentHorse.Type;
                hpStatsItem.Value = currentHorse.Stats.HP;
                staminaStatsItem.Value = currentHorse.Stats.Stamina;
                speedStatsItem.Value = currentHorse.Stats.Speed;
                accelerationStatsItem.Value = currentHorse.Stats.Acceleration;
        
                horseCost = currentHorse.Cost;
                CalculateHorseTotalCost();
        
                await DeleteHorse(currentPreviewHorseEntity);
                await CreatePreviewHorse((uint) currentHorse.Hash);
            });
        
            horseNameItem = new MenuTextboxItem(Lang.Current["Client.HorseSeller.HorseNameItem.Title"], "", Lang.Current["Client.HorseSeller.HorseNameItem.PlaceHolder"], "", 0, 25, value => { });
        
            horseSelectorMenuItem = new MenuSliderSelectorItem<int>("Robe", 0, horses.Count - 1, 0, 1, async index =>
            {
                currentHorse = horses[index];
        
                horseSelectorMenuItem.Value = index;
                horseSelectorMenuItem.Text = $"{Lang.Current["Client.HorseSeller.HorseSelectorMenuItem.Title"]}: " + currentHorse.Coat;
                typeMenuItem.Text = $"{Lang.Current["Client.HorseSeller.TypeMenuItem.Title"]}: " + currentHorse.Type;
                hpStatsItem.Value = currentHorse.Stats.HP;
                staminaStatsItem.Value = currentHorse.Stats.Stamina;
                speedStatsItem.Value = currentHorse.Stats.Speed;
                accelerationStatsItem.Value = currentHorse.Stats.Acceleration;
        
                horseCost = currentHorse.Cost;
                CalculateHorseTotalCost();
        
                await DeleteHorse(currentPreviewHorseEntity);
                await CreatePreviewHorse((uint) currentHorse.Hash);
        
                ApplyComponents();
            });
        
            genderItem = new MenuItemList("Genre", 0, new List<MenuItemList.KeyValue<object>>
            {
                new MenuItemList.KeyValue<object>("male", Lang.Current["Client.HorseSeller.Male"]),
                new MenuItemList.KeyValue<object>("female", Lang.Current["Client.HorseSeller.Female"])
            }, (index, value) =>
            {
                switch (index)
                {
                    case 0:
                        CAPI.SetPedFaceFeature(currentPreviewHorseEntity, 0xA28B, 0.0f);
                        break;
                    case 1:
                        CAPI.SetPedFaceFeature(currentPreviewHorseEntity, 0xA28B, 1.0f);
                        break;
                }
        
                CAPI.UpdatePedVariation(currentPreviewHorseEntity);
            });
            hpStatsItem = new MenuStatsItem(Lang.Current["Client.HorseSeller.HpStatsItem"], 10, currentHorse.Stats.HP);
            staminaStatsItem = new MenuStatsItem(Lang.Current["Client.HorseSeller.StaminaStatsItem"], 10, currentHorse.Stats.Stamina);
            speedStatsItem = new MenuStatsItem(Lang.Current["Client.HorseSeller.SpeedStatsItem"], 10, currentHorse.Stats.Speed);
            accelerationStatsItem = new MenuStatsItem(Lang.Current["Client.HorseSeller.AccelerationStatsItem"], 10, currentHorse.Stats.Acceleration);
            typeMenuItem = new MenuItem(Lang.Current["Client.HorseSeller.TypeMenuItem.Title"]);
            horseSelectorMenuItem.Text = $"{Lang.Current["Client.HorseSeller.HorseSelectorMenuItem.Title"]}: " + currentHorse.Coat;
            typeMenuItem.Text = $"{Lang.Current["Client.HorseSeller.TypeMenuItem.Title"]}: " + currentHorse.Type;
            paidItem = new MenuItem($"{Lang.Current["Client.HorseSeller.Pay"].Replace("{0}", CAPI.ConvertDecimalToString(0))} $", async () =>
            {
                if (CalculateHorseTotalCost() <= character.Data.Economy.Money)
                {
                    FreezeEntityPosition(currentPreviewHorseEntity, true);
                    SetEntityCompletelyDisableCollision(currentPreviewHorseEntity, true, true);
        
                    if (!string.IsNullOrEmpty(horseNameItem.Value.ToString()))
                        // Empêche la saisie de charactère spéciaux, nombres, etc
                        if (horseNameItem.Value.ToString().All(char.IsLetter))
                        {
                            // Si le joueur à assez pour payer
                            character.RemoveMoney(CalculateHorseTotalCost());
        
                            var horseData = new HorseData(character.Data.RockstarId, horseNameItem.Value.ToString(),
                                genderItem.Index, (uint) GetEntityModel(currentPreviewHorseEntity),
                                new Dictionary<string, uint>(), 1, 0);
        
                            horseData.HorseId = CAPI.RandomString();
                            stable.Data.Add(horseData);
        
                            var json = JsonConvert.SerializeObject(horseData);
        
                            Log.Warn("json: " + json);
                            
                            await DeleteHorse(currentPreviewHorseEntity);
        
                            // Save horse in database
                            TriggerServerEvent(Events.Stable.OnSaveMyHorse, json);
        
                            menu.CloseMenu();
                            NUI.Focus(false, false);
        
                            RenderScriptCams(false, true, 1000, true, true, 1);
                        }
                }
            });
        
            horseCost = horseInfos.Where(x => x.Category == currentHorse.Category).FirstOrDefault().Cost;
            CalculateHorseTotalCost();
        
            buyMenu.AddItem(raceMenuItem);
            buyMenu.AddItem(horseNameItem);
            buyMenu.AddItem(horseSelectorMenuItem);
            buyMenu.AddItem(genderItem);
            buyMenu.AddItem(typeMenuItem);
            buyMenu.AddItem(hpStatsItem);
            buyMenu.AddItem(staminaStatsItem);
            buyMenu.AddItem(speedStatsItem);
            buyMenu.AddItem(accelerationStatsItem);
            buyMenu.AddItem(paidItem);
        }
        
        private void InitMyHorseMenu()
        {
            myHorseMenu = new MenuContainer(Lang.Current["Client.HorseSeller.MyHorsesMenuItem"].ToUpper());
            menu.CreateSubMenu(myHorseMenu);
        
            MenuItem myHorseItem = null;
        
            var myHorseMenuItem = new MenuItem(Lang.Current["Client.HorseSeller.MyHorsesMenuItem"], myHorseMenu, () =>
            {
                myHorseMenu.Items.Clear();
        
                for (var i = 0; i < stable.Data.Count; i++)
                {
                    var horse = stable.Data[i];
        
                    customizationHorseMenu = new MenuContainer(horse.Name.ToUpper());
                    menu.CreateSubMenu(customizationHorseMenu);
        
                    myHorseItem = new MenuItem(horse.Name, async () =>
                    {
                        if (horse.IsStored == 1)
                        {
                            currentCustomizationHorseData = horse;
                            customizationHorseMenu.Items.Clear();
        
                            InitAccessoriesMenu(customizationHorseMenu);
                            menu.UpdateRender(customizationHorseMenu);
        
                            await DeleteHorse(currentPreviewHorseEntity);
                            await CreatePreviewHorseWithComponents(currentCustomizationHorseData);
        
                            customizationCamera = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA",
                                currentSeller.Customization.CameraPosition.X,
                                currentSeller.Customization.CameraPosition.Y,
                                currentSeller.Customization.CameraPosition.Z, 0.0f, 0.0f, 0f, 40.0f, false, 0);
                            PointCamAtEntity(customizationCamera, currentPreviewHorseEntity,
                                currentSeller.Customization.CameraLookAtOffset.X,
                                currentSeller.Customization.CameraLookAtOffset.Y,
                                currentSeller.Customization.CameraLookAtOffset.Z, true);
        
                            SwitchCamera(2);
        
                            menu.OpenMenu(customizationHorseMenu);
                        }
                    });
        
                    myHorseMenu.AddItem(myHorseItem);
                }
        
                menu.UpdateRender(myHorseMenu);
            });
            mainMenu.AddItem(myHorseMenuItem);
        }
        
        private void InitAccessoriesMenu(MenuContainer container)
        {
            horseComponentsHistory = new Dictionary<string, Tuple<HorseComponentInfo, int>>
            {
                {HorseComponentsCategories.Bedrolls, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Blankets, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Horns, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Lantern, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Manes, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Mask, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Saddles, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Stirrups, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.Tails, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)},
                {HorseComponentsCategories.SaddleBags, new Tuple<HorseComponentInfo, int>(new HorseComponentInfo(), 0)}
            };
        
            MenuItemList categoryItemList;
            MenuSliderSelectorItem<int> componentItem = null;
        
            var currentComponentInfo = horseComponents[0];
        
            categoryItemList = new MenuItemList("Catégorie", 0, new List<MenuItemList.KeyValue<object>>
                {
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Bedrolls, "Sac de couchage"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Blankets, "Couvertures"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Horns, "Cornes"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Lantern, "Lanterne"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Manes, "Mânes"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Mask, "Masques"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Saddles, "Selles"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Stirrups, "Etriers"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.Tails, "Queues"),
                    new MenuItemList.KeyValue<object>(HorseComponentsCategories.SaddleBags, "Sâchoches de selle")
                },
                (index, value) =>
                {
                    currentComponentInfo = horseComponents[index];
        
                    componentItem.Value = horseComponentsHistory[currentComponentInfo.Category].Item2;
                    componentItem.MaxValue = currentComponentInfo.Hashes.Count - 1;
                    componentItem.Text = $"[{componentItem.Value}/{componentItem.MaxValue + 1}]";
        
                    CalculateHorseComponentsCost();
                });
        
            componentItem = new MenuSliderSelectorItem<int>("Sac de couchage", -1,
                currentComponentInfo.Hashes.Count - 1, -1, 1,
                index =>
                {
                    if (index != -1)
                    {
                        CAPI.SetPedComponentEnabled(currentPreviewHorseEntity,
                            uint.Parse(currentComponentInfo.Hashes[index], NumberStyles.AllowHexSpecifier), true, true,
                            true);
                        componentItem.Text = $"[{index}/{componentItem.MaxValue + 1}]";
        
                        horseComponentsHistory[currentComponentInfo.Category] =
                            new Tuple<HorseComponentInfo, int>(currentComponentInfo, index);
                    }
                    else
                    {
                        CAPI.SetPedComponentDisabled(currentPreviewHorseEntity,
                            (uint) CAPI.FromHexToHash(currentComponentInfo.Category), 0);
                        componentItem.Text = $"[{0}/{componentItem.MaxValue + 1}]";
        
                        horseComponentsHistory[currentComponentInfo.Category] =
                            new Tuple<HorseComponentInfo, int>(currentComponentInfo, -1);
                    }
        
                    CAPI.UpdatePedVariation(currentPreviewHorseEntity);
        
                    CalculateHorseComponentsCost();
                });
            paidAccessoriesItem = new MenuItem($"{Lang.Current["Client.HorseSeller.Pay"].Replace("{0}", CAPI.ConvertDecimalToString(0))} $", async () =>
            {
                if (CalculateHorseComponentsCost() <= character.Data.Economy.Money)
                {
                    character.RemoveMoney(CalculateHorseComponentsCost());
        
                    FreezeEntityPosition(currentPreviewHorseEntity, true);
                    SetEntityCompletelyDisableCollision(currentPreviewHorseEntity, true, true);
        
                    currentCustomizationHorseData.Components = new Dictionary<string, uint>();
        
                    foreach (var comp in horseComponentsHistory)
                    {
                        if (comp.Value.Item2 != -1)
                            if (comp.Value.Item1.Hashes.Count > 0 && comp.Value.Item2 > -1)
                                currentCustomizationHorseData.Components.Add(comp.Key,
                                    uint.Parse(comp.Value.Item1.Hashes[comp.Value.Item2],
                                        NumberStyles.AllowHexSpecifier));
                    }
        
                    var json = JsonConvert.SerializeObject(currentCustomizationHorseData);
        
                    menu.CloseMenu();
                    NUI.Focus(false, false);
        
                    RenderScriptCams(false, true, 1000, true, true, 1);
        
                    await DeleteHorse(currentPreviewHorseEntity);
        
                    // Save horse in database
                    TriggerServerEvent(Events.Stable.OnSaveMyHorse, json);
                }
            });
        
            componentItem.Text = $"[{1}/{componentItem.MaxValue + 1}]";
        
            container.AddItem(categoryItemList);
            container.AddItem(componentItem);
            container.AddItem(paidAccessoriesItem);
        }
        
        #endregion
        
        #region Events
        
        [EventHandler(Events.CFX.OnResourceStop)]
        private async void OnResourceStop(string resourceName)
        {
            if (resourceName == Constant.ResourceName)
            {
                await DeleteHorse(currentPreviewHorseEntity);
        
                SetCamActive(camera, false);
                SetCamActive(buyCamera, false);
                SetCamActive(customizationCamera, false);
        
                RenderScriptCams(false, true, 1000, true, true, 0);
        
                DestroyCam(camera, true);
                DestroyCam(buyCamera, true);
                DestroyCam(customizationCamera, true);
            }
        }
        
        #endregion
    }
}