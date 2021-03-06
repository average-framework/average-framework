﻿namespace Client.Core.Enums
{
    public class PostEffect
    {
        public const string CameraTakePicture = "CameraTakePicture"; // Flash Noir (prise photo)
        public const string CameraTransitionBlink = "CameraTransitionBlink"; // Black Screen
        public const string CameraTransitionFlash = "CameraTransitionFlash"; // Flash Blanc (léger blur)
        public const string CameraTransitionWipe_L = "CameraTransitionWipe_L"; // Black Screen vers la droite
        public const string CameraTransitionWipe_R = "CameraTransitionWipe_R"; // Black Screen vers la gauche
        public const string CameraViewfinder = "CameraViewfinder"; // Effet vielle photo
        public const string CameraViewfinderForceOutro = "CameraViewfinderForceOutro"; //  à déterminer
        public const string CameraTransitionBlinkSlow = "CameraTransitionBlinkSlow"; //  Black screen + blur
        public const string CamTransition01 = "CamTransition01"; // Flash Noir
        public const string CamTransitionBlink = "CamTransitionBlink";
        public const string CamTransitionBlinkSick = "CamTransitionBlinkSick"; // Fade in Fade out + Blur
        public const string CamTransitionSlow = "CamTransitionSlow";
        public const string DeathFailMP01 = "DeathFailMP01"; // Effet black & white (mort)
        public const string Duel = "Duel"; // à déterminer
        public const string PedKill = "PedKill"; // Flash blanc (clair)
        public const string MP_ArrowDrain = "MP_ArrowDrain"; // Clignotant léger bleu/violet
        public const string MP_ArrowDisorient = "MP_ArrowDisorient"; // Clignotant léger bleu/violet + Nausée
        public const string MP_ArrowTracking = "MP_ArrowTracking"; // Clignotant léger bleu/violet + Nausée
        public const string MP_MoonshineToxic = "MP_MoonshineToxic"; // à déterminer
        public const string MP_SuddenDeath = "MP_SuddenDeath"; // Flash Rouge (léger)
        public const string MP_DM_Annesburg_ReduceDustDensity = "MP_DM_Annesburg_ReduceDustDensity"; // à determiner
        public const string MP_Rhodes_ReduceDustDensity = "MP_Rhodes_ReduceDustDensity"; // à déterminer
        public const string MP_OutofAreaDirectional = "MP_OutofAreaDirectional"; // Flash Noir > petits coins clignotant noir
        public const string MP_BountyLagrasSwamp = "MP_BountyLagrasSwamp"; // Rose/Bleu + Nausée
        public const string MP_Region = "MP_Region"; // Filtre rouge
        public const string MP_InRegion_Exit = "MP_InRegion_Exit"; // FadeOut Noir (léger)
        public const string MP_RiderFormation = "MP_RiderFormation"; // Bleu coin (léger) (style Glacial)
        public const string MP_LobbyBW01_Intro = "MP_LobbyBW01_Intro"; // Black & White avec Grain
        public const string MP_RaceBoostStrat = "MP_RaceBoostStrat"; // Clignotant Jaune (Fade out) (Style Puissance Consommée)
        public const string MP_BountySelection = "MP_BountySelection"; // éclaircit l'image (léger)
        public const string MP_Downed = "MP_Downed"; // Clignotant rouge/bleu + Blur (Style tunnelvision)
        public const string MP_PedKill = "MP_PedKill"; // Flash blanc (clair)
        public const string MP_DeathInk_Color00 = "MP_DeathInk_Color00"; // Taches colorées
        public const string MP_DeathInk_Color01 = "MP_DeathInk_Color01"; // Taches colorées
        public const string MP_DeathInk_Color02 = "MP_DeathInk_Color02"; // Taches colorées
        public const string MP_DeathInk_Color03 = "MP_DeathInk_Color03"; // Taches colorées
        public const string MP_DeathInk_Color04 = "MP_DeathInk_Color04"; // Taches colorées
        public const string MP_DeathInk_Color05 = "MP_DeathInk_Color05"; // Taches colorées
        public const string MP_DeathInk_Color06 = "MP_DeathInk_Color06"; // Taches colorées
        public const string MP_DeathInk_Color07 = "MP_DeathInk_Color07"; // Taches colorées
        public const string MP_DeathInk_Color08 = "MP_DeathInk_Color08"; // Taches colorées
        public const string MP_DeathInk_Color09 = "MP_DeathInk_Color09"; // Taches colorées
        public const string MP_DeathInk_Color10 = "MP_DeathInk_Color10"; // Taches colorées
        public const string MP_CampWipeL = "MP_CampWipeL"; // Swipe vers la droite (Style Transition)
        public const string MP_CampWipeR = "MP_CampWipeR"; // Swipe vers la gauche
        public const string MP_CampWipeUp = "MP_CampWipeUp"; // Swipe vers le bas
        public const string MP_CampWipeDown = "MP_CampWipeDown"; // Swipe vers le haut
        public const string MP_RewardsExposureLoop = "MP_RewardsExposureLoop"; // à déterminer
        public const string POSTFX_CONSUMABLE_STAMINA_FORT = "POSTFX_CONSUMABLE_STAMINA_FORT"; // à déterminer
        public const string POSTFX_CONSUMABLE_STAMINA = "POSTFX_CONSUMABLE_STAMINA"; // à déterminer
        public const string RespawnMissionCheckpoint = "RespawnMissionCheckpoint"; // Blur total(Fade out)
        public const string RespawnWithHonor = "RespawnWithHonor"; // Blur total (Fadeo out)
        public const string Title_GameIntro = "Title_GameIntro"; // Intro du jeu
        public const string MissionChoice = "MissionChoice"; // Flash Blanc + clignotant léger sombre
        public const string Mission_GNG0_Ride = "Mission_GNG0_Ride"; // Sombre
        public const string ChapterTitle_IntroCh01 = "ChapterTitle_IntroCh01";
        public const string ChapterTitle_IntroCh02 = "ChapterTitle_IntroCh02"; // Pause sur papier (chapitre II) + black screen (fade in)
        public const string ChapterTitle_IntroCh03 = "ChapterTitle_IntroCh03";
        public const string ChapterTitle_IntroCh04 = "ChapterTitle_IntroCh04";
        public const string ChapterTitle_IntroCh05 = "ChapterTitle_IntroCh05";
        public const string ChapterTitle_IntroCh06 = "ChapterTitle_IntroCh06";
        public const string ChapterTitle_IntroCh08Epi01 = "ChapterTitle_IntroCh08Epi01"; // Pause sur papier (style screenshot) + black screen (fade in)
        public const string ChapterTitle_IntroCh09Epi02 = "ChapterTitle_IntroCh08Epi01";
        public const string KillCamHonorChange = "KillCamHonorChange"; // Couleurs Brulées
        public const string ODR3_Injured01Loop = "ODR3_Injured01Loop"; // Rouge + Nausée
        public const string ODR3_Injured02Loop = "ODR3_Injured02Loop"; // Rouge(+) + Nausée
        public const string ODR3_Injured03Loop = "ODR3_Injured03Loop"; // Rouge(++) + Nausée
        public const string OJDominoBlur = "OJDominoBlur";
        public const string OJDominoValid = "OJDominoValid";
        public const string OJFiveFinger = "OJFiveFinger";
        public const string OJPokerPlayerTurn = "OJPokerPlayerTurn"; // Blur side + sombre
        public const string Title_Gen_FewDaysLater = "Title_Gen_FewDaysLater"; // Ecrit Few Days Later (.._onblack" ne marche pas)
        public const string Title_Gen_FewDaysLater_onblack = "Title_Gen_FewDaysLater_onblack";
        public const string Title_Gen_FewHoursLater = "Title_Gen_FewHoursLater";
        public const string Title_Gen_FewHoursLater_onblack = "Title_Gen_FewHoursLater_onblack";
        public const string Title_Gen_FewWeeksLater = "Title_Gen_FewWeeksLater";
        public const string Title_Gen_FewWeeksLater_onblack = "Title_Gen_FewWeeksLater_onblack";
        public const string Title_Gen_FewMonthsLater = "Title_Gen_FewMonthsLater";
        public const string Title_Gen_FewMonthsLater_onblack = "Title_Gen_FewMonthsLater_onblack";
        public const string Title_Gen_daylater = "Title_Gen_daylater";
        public const string Title_Gen_daylater_onblack = "Title_Gen_daylater_onblack";
        public const string Title_Gen_somedaysLater = "Title_Gen_somedaysLater";
        public const string Title_Gen_somedaysLater_onblack = "Title_Gen_somedaysLater_onblack";
        public const string Title_Gen_someyearsLater = "Title_Gen_someyearsLater";
        public const string Title_Gen_someyearsLater_onblack = "Title_Gen_someyearsLater_onblack";
        public const string Title_Gen_coupledayslater = "Title_Gen_coupledayslater";
        public const string Title_Gen_coupledayslater_onblack = "Title_Gen_coupledayslater_onblack";
        public const string Title_Gen_couplemonthslater = "Title_Gen_couplemonthslater";
        public const string Title_Gen_couplemonthslater_onblack = "Title_Gen_couplemonthslater_onblack";
        public const string Title_Gen_coupleweekslater = "Title_Gen_coupleweekslater";
        public const string Title_Gen_coupleweekslater_onblack = "Title_Gen_coupleweekslater_onblack";
        public const string RespawnEstablish01 = "RespawnEstablish01"; // Black & White + Couleurs (Fade out)
        public const string PlayerOverpower = "PlayerOverpower"; // Flash jaune
        public const string PlayerHealthLow = "PlayerHealthLow"; // Clignotant Rouge
        public const string PlayerDrunk01 = "PlayerDrunk01"; // Strong Drunk Vert/Violet (LSD Style)
        public const string PlayerRPGCore = "PlayerRPGCore"; // Violet + Nausée + Blur
        public const string PlayerRPGCoreDeadEye = "PlayerRPGCoreDeadEye"; //  Blur
        public const string PlayerRPGEmptyCoreHealth = "PlayerRPGEmptyCoreHealth"; //  clignotant Rouge
        public const string PlayerRPGEmptyCoreStamina = "PlayerRPGEmptyCoreStamina"; //  clignotant Bleu
        public const string PlayerRPGEmptyCoreDeadEye = "PlayerRPGEmptyCoreDeadEye"; //  clignotant Orange
        public const string PlayerHonorLevelGood = "PlayerHonorLevelGood"; // Sépia jaune(Fade out)
        public const string PlayerHonorLevelBad = "PlayerHonorLevelBad"; // Sépia gris/bleu (Fade out)
        public const string PlayerHonorChoiceGood = "PlayerHonorChoiceGood"; // Sépia jaune léger (Fade out)
        public const string PlayerHonorChoiceBad = "PlayerHonorChoiceBad"; // Sépia gris/bleu léger (Fade out)
        public const string PlayerDrunk01_PassOut = "PlayerDrunk01_PassOut"; // Blur Clignotant violent (Fade in) + black screen
        public const string PlayerWakeUpDrunk = "PlayerWakeUpDrunk"; // Black screen (Fade out) + Nausée
        public const string PlayerHealthPoorMOB3 = "PlayerHealthPoorMOB3"; // Nausée
        public const string PlayerHeathCrackpot = "PlayerHeathCrackpot"; // Flash Couleur (Fade out)
        public const string PlayerSickDoctorsOpinion = "PlayerSickDoctorsOpinion"; // Rouge + Nausée
        public const string PlayerSickDoctorsOpinionOutGood = "PlayerSickDoctorsOpinionOutGood"; // Eblouissement + clignotant léger
        public const string PlayerSickDoctorsOpinionOutBad = "PlayerSickDoctorsOpinionOutBad"; // Eblouissement + clignotant
        public const string PlayerImpaceFall = "PlayerImpaceFall"; // Flash Rouge (Fade out)
        public const string PlayerDrugsPoisonWell = "PlayerDrugsPoisonWell"; // Vert/Violet + Coins noir (Style Poison)
        public const string PlayerKnockout_SerialKiller = "PlayerKnockout_SerialKiller"; // Blur violent (fade in) (Style Assomée)
        public const string PlayerDrunkSaloon1 = "PlayerDrunkSaloon1"; // Nausée forte verte
        public const string PlayerKnockout_WeirdoPat = "PlayerKnockout_WeirdoPat"; // Nausée forte Verte
        public const string PlayerHealthPoorCS = "PlayerHealthPoorCS"; // Sépia grain
        public const string UI_PauseTransition = "UI_PauseTransition";
        public const string UI_PauseTransitionOut = "UI_PauseTransitionOut";
        public const string WheelHUDIn = "WheelHUDIn";
        public const string CamTransitionBlinkSlow = "CamTransitionBlinkSlow";
        public const string cutscene_mar6_train = "cutscene_mar6_train";
        public const string cutscene_rbch2rsc11_bink1 = "cutscene_rbch2rsc11_bink1";
        public const string cutscene_rbch2rsc11_bink2 = "cutscene_rbch2rsc11_bink2";
        public const string cutscene_rbch2rsc11_bink3 = "cutscene_rbch2rsc11_bink3";
        public const string cutscene_rbch2rsc11_bink4 = "cutscene_rbch2rsc11_bink4";
        public const string cutscene_rbch2rsc11_bink5 = "cutscene_rbch2rsc11_bink5";
        public const string cutscene_rbch2rsc11_bink6 = "cutscene_rbch2rsc11_bink6";
        public const string cutscene_rbch2rsc11_bink7 = "cutscene_rbch2rsc11_bink7";
        public const string cutscene_rbch2rsc11_bink8 = "cutscene_rbch2rsc11_bink8";
        public const string deadeye = "deadeye";
        public const string DeadEyeEmpty = "DeadEyeEmpty";
        public const string EagleEye = "EagleEye";
        public const string GunslingerFill = "GunslingerFill";
        public const string killCam = "killCam";
        public const string KingCastleBlue = "KingCastleBlue";
        public const string KingCastleRed = "KingCastleRed";
        public const string MissionFail01 = "MissionFail01";
        public const string Mission_EndCredits = "Mission_EndCredits";
        public const string Mission_FIN1_RideBad = "Mission_FIN1_RideBad";
        public const string Mission_FIN1_RideGood = "Mission_FIN1_RideGood";
        public const string MP_HealthDrop = "MP_HealthDrop";
        public const string MP_RaceBoostStart = "MP_RaceBoostStart";
        public const string PauseMenuIn = "PauseMenuIn";
        public const string PlayerDrunkAberdeen = "PlayerDrunkAberdeen";
        public const string PlayerHealthCrackpot = "PlayerHealthCrackpot";
        public const string PlayerHealthPoorGuarma = "PlayerHealthPoorGuarma";
        public const string PlayerImpact04 = "PlayerImpact04";
        public const string PlayerImpactFall = "PlayerImpactFall";
        public const string PlayerRPGWarnHealth = "PlayerRPGWarnHealth";
        public const string PlayerWakeUpAberdeen = "PlayerWakeUpAberdeen";
        public const string PlayerWakeUpInterrogation = "PlayerWakeUpInterrogation";
        public const string PlayerWakeUpKnockout = "PlayerWakeUpKnockout";
        public const string PoisonDartPassOut = "PoisonDartPassOut";
        public const string RespawnPulse01 = "RespawnPulse01";
        public const string RespawnSkyWithHonor = "RespawnSkyWithHonor";
        public const string SkyTimelapse_0600_01 = "SkyTimelapse_0600_01";
        public const string skytl_0000_01clear = "skytl_0000_01clear";
        public const string skytl_0600_01clear = "skytl_0600_01clear";
        public const string skytl_0900_01clear = "skytl_0900_01clear";
        public const string skytl_0900_04storm = "skytl_0900_04storm";
        public const string skytl_1200_01clear = "skytl_1200_01clear";
        public const string skytl_1200_03clouds = "skytl_1200_03clouds";
        public const string skytl_1500_03clouds = "skytl_1500_03clouds";
        public const string skytl_1500_04storm = "skytl_1500_04storm";
        public const string title_ch01_colter = "title_ch01_colter";
        public const string Title_Ch03_ClemensPoint = "Title_Ch03_ClemensPoint";
    }
}
