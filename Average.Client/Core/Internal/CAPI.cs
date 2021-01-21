using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Core.Internal
{
    public static class CAPI
    {
        public class RaycastHit
        {
            public bool Hit { get; private set; }
            public Vector3 EndCoords { get; private set; }
            public Vector3 SurfaceNormal { get; private set; }
            public int EntityHit { get; private set; }
            public int EntityType { get => GetEntityType(EntityHit); }

            public RaycastHit(bool hit, Vector3 endCoords, Vector3 surfaceNormal, int entityHit)
            {
                Hit = hit;
                EndCoords = endCoords;
                SurfaceNormal = surfaceNormal;
                EntityHit = entityHit;
            }
        }

        public static async Task<bool> LoadModel(uint hash)
        {
            if (IsModelValid(hash))
            {
                Function.Call(Hash.REQUEST_MODEL, hash);

                while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
                {
                    await BaseScript.Delay(100);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsModelValid(uint hash) => Function.Call<bool>(Hash.IS_MODEL_VALID, hash);
        public static bool IsPedComponentEquipped(string category) => Function.Call<bool>((Hash)0xFB4891BD7578CDC1, PlayerPedId(), category);
        public static void ApplyWalkStyle(int ped, string walkStyle) => Function.Call((Hash)0xCB9401F918CB0F75, ped, walkStyle, true, -1);
        public static void RemoveWalkStyle(int ped, string walkStyle) => Function.Call((Hash)0xCB9401F918CB0F75, ped, walkStyle, false, -1);
        //public static void ClearHudPreset()
        //{
        //    foreach (var preset in Constant.HudPresets.Presets)
        //    {
        //        RemoveHudPreset(preset);
        //    }
        //}
        public static void SetHudPreset(string preset) => Function.Call((Hash)0x4CC5F2FC1332577F, GetHashKey(preset));
        public static void RemoveHudPreset(string preset) => Function.Call((Hash)0x8BC7C1F929D07BF3, GetHashKey(preset));
        public static Vector3 GetCoordsFromCam(float distance)
        {
            var rot = GetGameplayCamRot(2);
            var coord = GetGameplayCamCoord();

            var tz = rot.Z * 0.0174532924f;
            var tx = rot.X * 0.0174532924f;
            var num = Math.Abs(Math.Cos(tx));

            var newCoordX = coord.X + (-Math.Sin(tz)) * (num + distance);
            var newCoordY = coord.Y + (Math.Cos(tz)) * (num + distance);
            var newCoordZ = coord.Z + (Math.Sin(tx) * 8.0f);

            return new Vector3((float)newCoordX, (float)newCoordY, (float)newCoordZ);
        }
        public static RaycastHit GetTarget(int ped, float distance)
        {
            var camCoords = GetGameplayCamCoord();
            var farCoords = GetCoordsFromCam(distance);
            var rayHandle = StartShapeTestRay(camCoords.X, camCoords.Y, camCoords.Z, farCoords.X, farCoords.Y, farCoords.Z, -1, ped, 0);

            var hit = false;
            var endCoords = Vector3.Zero;
            var surfaceNormal = Vector3.Zero;
            var entityHit = 0;

            GetShapeTestResult(rayHandle, ref hit, ref endCoords, ref surfaceNormal, ref entityHit);

            return new RaycastHit(hit, endCoords, surfaceNormal, entityHit);
        }
        public static int GetEntityFrontOfPlayer(int ped, float distance)
        {
            var coordA = GetEntityCoords(ped, true, true);
            var coordB = GetOffsetFromEntityInWorldCoords(ped, 0.0f, distance, 0.0f);
            var rayHandle = StartShapeTestRay(coordA.X, coordA.Y, coordA.Z, coordB.X, coordB.Y, coordB.Z, 10, ped, 0);

            var hit = false;
            var endCoords = Vector3.Zero;
            var surfaceNormal = Vector3.Zero;
            var entityHit = 0;

            GetShapeTestResult(rayHandle, ref hit, ref endCoords, ref surfaceNormal, ref entityHit);

            return entityHit;
        }
        public static Vector3 GetCamDirection()
        {
            var heading = GetGameplayCamRelativeHeading() + GetEntityHeading(PlayerPedId());
            var pitch = GetGameplayCamRelativePitch();

            var x = (float)-Math.Sin(heading * Math.PI / 180.0f);
            var y = (float)Math.Cos(heading * Math.PI / 180.0f);
            var z = (float)Math.Sin(pitch * Math.PI / 180.0f);

            var len = (float)Math.Sqrt(x * x + y * y + z * z);

            if (len != 0)
            {
                x /= len;
                y /= len;
                z /= len;
            }

            return new Vector3(x, y, z);
        }
        public static Vector3 RotationToDirection(Vector3 rotation)
        {
            var adjustedRotation = new Vector3();
            adjustedRotation.X = (float)(Math.PI / 180f) * rotation.X;
            adjustedRotation.Y = (float)(Math.PI / 180f) * rotation.Y;
            adjustedRotation.Z = (float)(Math.PI / 180f) * rotation.Z;

            var direction = new Vector3();
            direction.X = (float)(-Math.Sin(adjustedRotation.Z) * Math.Abs(Math.Cos(adjustedRotation.X)));
            direction.Y = (float)(Math.Cos(adjustedRotation.Z) * Math.Abs(Math.Cos(adjustedRotation.X)));
            direction.Z = (float)(Math.Sin(adjustedRotation.X));

            return direction;
        }
        public static object[] RaycastGameplayCamera(float distance, int flag)
        {
            var cameraRotation = GetGameplayCamRot(2);
            var cameraCoord = GetGameplayCamCoord();
            var direction = RotationToDirection(cameraRotation);
            var destination = new Vector3();

            destination.X = cameraCoord.X + direction.X * distance;
            destination.Y = cameraCoord.Y + direction.Y * distance;
            destination.Z = cameraCoord.Z + direction.Z * distance;

            var ray = StartShapeTestRay(cameraCoord.X, cameraCoord.Y, cameraCoord.Z, destination.X, destination.Y, destination.Z, flag, -1, 1);
            var hit = false;
            var endCoords = Vector3.Zero;
            var normalCoords = Vector3.Zero;
            var entHit = -1;

            GetShapeTestResult(ray, ref hit, ref endCoords, ref normalCoords, ref entHit);
            return new object[] { hit, endCoords, normalCoords, entHit };
        }
        public static void DrawText(float x, float y, float scale, string text)
        {
            var str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            SetTextScale(scale, scale);
            SetTextColor(255, 255, 255, 255);
            SetTextCentre(true);
            SetTextDropshadow(15, 0, 0, 0, 255);
            DisplayText(str, x, y);
        }
        public static void DrawBoxOnEntityModel(int entity)
        {
            var model = (uint)GetEntityModel(entity);

            var minimum = Vector3.Zero;
            var maximum = Vector3.Zero;

            GetModelDimensions(model, ref minimum, ref maximum);

            var size = maximum - minimum;

            var objCoords = GetEntityCoords(entity, true, true);
            var objRot = GetEntityRotation(entity, 2);

            int alpha = 150;
            int red = 254;
            int green = 254;
            int blue = 254;

            var s = Math.Sin(objRot.Z / 180 * Math.PI);
            var c = Math.Cos(objRot.Z / 180 * Math.PI);

            // Front Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z,
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Front Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Back Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Back Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Top Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z, red, green, blue, alpha);

            // Top Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z, red, green, blue, alpha);

            // Bottom Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z,
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Bottom Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Left Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Left Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + (-(size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + (-(size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + (-(size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Right Bottom Left
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z,
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);

            // Right Top Right
            Function.Call((Hash)(uint)GetHashKey("DRAW_POLY"),
                objCoords.X + ((size.X / 2)) * c - ((size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + ((size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z + size.Z,
                objCoords.X + ((size.X / 2)) * c - (-(size.Y / 2)) * s, objCoords.Y + ((size.X / 2)) * s + (-(size.Y / 2)) * c, objCoords.Z, red, green, blue, alpha);
        }
        public static string RandomString()
        {
            Guid g = Guid.NewGuid();
            string guid = Convert.ToBase64String(g.ToByteArray());
            guid = guid.Replace("=", "");
            guid = guid.Replace("+", "");
            guid = guid.Replace("/", "");

            return guid;
        }
        public static string ConvertDecimalToString(decimal value) => value.ToString("C2").Remove(0, 1);
        public static List<int> GetAllPeds()
        {
            var results = new List<int>();
            var outEntity1 = -1;
            var firstPed = FindFirstPed(ref outEntity1);

            if (outEntity1 > 0)
            {
                results.Add(outEntity1);
            }
            for (var outEntity2 = -1; FindNextPed(firstPed, ref outEntity2); outEntity2 = -1)
            {
                if (outEntity2 > 0)
                {
                    results.Add(outEntity2);
                }
            }
            EndFindPed(firstPed);
            return results;
        }
        public static List<int> GetAllVehicles()
        {
            var results = new List<int>();
            var outEntity1 = -1;
            var firstPed = FindFirstVehicle(ref outEntity1);

            if (outEntity1 > 0)
            {
                results.Add(outEntity1);
            }
            for (var outEntity2 = -1; FindNextVehicle(firstPed, ref outEntity2); outEntity2 = -1)
            {
                if (outEntity2 > 0)
                {
                    results.Add(outEntity2);
                }
            }
            EndFindVehicle(firstPed);
            return results;
        }
        public static List<int> GetAllProps()
        {
            var results = new List<int>();
            var outEntity1 = -1;
            var firstPed = FindFirstObject(ref outEntity1);

            if (outEntity1 > 0)
            {
                results.Add(outEntity1);
            }
            for (var outEntity2 = -1; FindNextObject(firstPed, ref outEntity2); outEntity2 = -1)
            {
                if (outEntity2 > 0)
                {
                    results.Add(outEntity2);
                }
            }
            EndFindObject(firstPed);
            return results;
        }
        public static List<int> GetAllPickups()
        {
            var results = new List<int>();
            var outEntity1 = -1;
            var firstPed = FindFirstPickup(ref outEntity1);

            if (outEntity1 > 0)
            {
                results.Add(outEntity1);
            }
            for (var outEntity2 = -1; FindNextPickup(firstPed, ref outEntity2); outEntity2 = -1)
            {
                if (outEntity2 > 0)
                {
                    results.Add(outEntity2);
                }
            }
            EndFindPickup(firstPed);
            return results;
        }
        public static int CreateBlip(int sprite, string text, float scale, Vector3 position)
        {
            var blip = Function.Call<int>((Hash)0x554D9D53F696D002, 1664425300, position.X, position.Y, position.Z);
            SetBlipSprite(blip, sprite, 1);
            SetBlipScale(blip, scale);
            SetBlipNameFromPlayerString(blip, text);

            return blip;
        }
        public static void SetBlipNameFromPlayerString(int blip, string playerString) => Function.Call((Hash)0x9CB1A1623062F402, blip, playerString);
        public static void RequestModel(uint hash) => Function.Call(Hash.REQUEST_MODEL, hash);
        public static void SetRandomOutfitVariation(int handle, bool randomVariation) =>
            Function.Call((Hash)0x283978a15512b2fe, handle, randomVariation);
        public static void SetPedComponentEnabled(int ped, uint component, bool immediately, bool isMp, bool p4) => Function.Call((Hash)0xD3A7B003ED343FD9, ped, component, immediately, isMp, p4);
        public static void SetPedComponentDisabled(int ped, uint component, int p2) => Function.Call((Hash)0xD710A5007C2AC539, ped, component, p2);
        public static void DisplayHudComponent(string hash) =>
            Function.Call((Hash)0x8BC7C1F929D07BF3, GetHashKey(hash));
        public static void HideHudComponent(string hash) =>
            Function.Call((Hash)0x4CC5F2FC1332577F, GetHashKey(hash));
        public static void SetPedOutfitPreset(int ped, int presetId) => Function.Call((Hash)0x77FF8D35EEC6BBC4, ped, presetId, 0);
        public static bool OutfitFullyLoaded(int ped) => Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, ped);
        public static int GetPedNumOutfitPresets(int ped) => Function.Call<int>((Hash)0x10C70A515BC03707, ped);
        public static int FromHexToHash(string hex) => Convert.ToInt32("0x" + hex, 16);
        public static long FromHexStringToUInt(string hex)
        {
            var obj = uint.Parse(hex, NumberStyles.AllowHexSpecifier);
            return Convert.ToInt64(obj);
        }
        public static bool IsMetapedUsingComponent(int ped, uint component) => Function.Call<bool>((Hash)0xFB4891BD7578CDC1, ped, component);
        public static uint GetPedComponentCategory(uint componentHash, int metapedType, bool isMp) => Function.Call<uint>((Hash)0x5FF9A878C3D115B8, componentHash, metapedType, isMp);
        public static void SetPedScale(int ped, float scale) => Function.Call((Hash)0x25ACFC650B65C538, ped, scale);
        public static void SetPedScale(float scale) => Function.Call((Hash)0x25ACFC650B65C538, scale);
        public static void UpdatePlayerPed() => Function.Call((Hash)0x704C908E9C405136, PlayerPedId());
        public static void SetPedBodyComponent(uint component)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, PlayerPedId(), component);
            UpdatePedVariation();
        }
        public static void SetPedBodyComponent(List<int> components, int index)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, PlayerPedId(), components[index]);
            UpdatePedVariation();
        }
        public static void UpdatePedVariation()
        {
            Function.Call((Hash)0x704C908E9C405136, PlayerPedId());
            Function.Call((Hash)0xAAB86462966168CE, PlayerPedId(), 1);
            Function.Call((Hash)0xCC8CA3E88256E58F, PlayerPedId(), 0, 1, 1, 1, 0);
        }
        public static void UpdatePedVariation(int ped)
        {
            Function.Call((Hash)0x704C908E9C405136, ped);
            Function.Call((Hash)0xAAB86462966168CE, ped, 1);
            Function.Call((Hash)0xCC8CA3E88256E58F, ped, 0, 1, 1, 1, 0);
        }
        public static void SetPedFaceFeature(int index, float value)
        {
            Function.Call((Hash)0x704C908E9C405136, PlayerPedId());
            Function.Call((Hash)0x5653AB26C82938CF, PlayerPedId(), index, value);
        }
        public static void SetPedFaceFeature(int ped, int index, float value)
        {
            Function.Call((Hash)0x704C908E9C405136, ped);
            Function.Call((Hash)0x5653AB26C82938CF, ped, index, value);
        }
        public static bool HasPedComponentLoaded() => Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, PlayerPedId());
        public static void RemovePedComponent(uint category)
        {
            Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), category, 0);
            UpdatePedVariation();
        }
        public static void RemovePedComponent(string category)
        {
            var obj = uint.Parse(category, NumberStyles.AllowHexSpecifier);
            var hash = Convert.ToInt64(obj);

            Function.Call((Hash)0xD710A5007C2AC539, PlayerPedId(), hash, 0);
            UpdatePedVariation();
        }
        public static void RemovePedComponent(int ped, uint category)
        {
            Function.Call((Hash)0xD710A5007C2AC539, ped, category, 0);
            UpdatePedVariation();
        }
        public static void RemovePedComponent(int ped, string category)
        {
            var obj = uint.Parse(category, NumberStyles.AllowHexSpecifier);
            var hash = Convert.ToInt64(obj);

            Function.Call((Hash)0xD710A5007C2AC539, ped, hash, 0);
            UpdatePedVariation();
        }
        public static void PlayScenarioAtPosition(int ped, string scenario, Vector3 position, float heading) =>
            Function.Call(Hash.TASK_START_SCENARIO_AT_POSITION, ped, GetHashKey(scenario), position.X, position.Y, position.Z, heading, 0, 0, 0, 0, -1082130432, 0);
        public static void PlayScenarioInPlace(int ped, string scenario, bool playEnterAnim = true, int p4 = -1082130432) =>
            Function.Call(Hash._TASK_START_SCENARIO_IN_PLACE, ped, GetHashKey(scenario), -1, playEnterAnim, false, p4, false);
        public static void SetPedRelationshipGroupDefaultHash(int ped, uint hash) => Function.Call((Hash)0xADB3F206518799E8, ped, hash);
        public static void SetAnimalMood(int ped, int mood) =>
            Function.Call((Hash)0xCC97B29285B1DC3B, ped, mood);
        public static void SetPedConfigFlag(int ped, int flagId, bool value) =>
            Function.Call((Hash)0x1913FE4CBF41C463, ped, flagId, value);
        public static void SetAnimalTuningBoolParam(int ped, int p1, bool p2) =>
            Function.Call((Hash)0x9FF1E042FA597187, ped, p1, p2);
        public static void TaskAnimalInteraction(int ped, int targetPed, int p2, int p3, int p4) =>
            Function.Call((Hash)0xCD181A959CFDD7F4, ped, targetPed, p2, p3, p4);
        public static bool GetGroundZFor3DCoord(float x, float y, float z, ref float groundZ, bool p4) =>
            Function.Call<bool>((Hash)0x24FA4267BB8D2431, x, y, z, groundZ, p4);
        public static bool TaskEmote(int ped, int category, int p2, uint emoteType, bool p4, bool p5, bool p6, bool p7, bool p8) =>
            Function.Call<bool>((Hash)0xB31A277C1AC7B7FF, ped, category, p2, emoteType, p4, p5, p6, p7, p8);
        public static bool TaskEmote(int ped, int category, int p2, string emoteType, bool p4, bool p5, bool p6, bool p7, bool p8) =>
            Function.Call<bool>((Hash)0xB31A277C1AC7B7FF, ped, category, p2, (uint)GetHashKey(emoteType), p4, p5, p6, p7, p8);
        public static bool TaskFullBodyEmote(int ped, uint emoteType) =>
            Function.Call<bool>((Hash)0xB31A277C1AC7B7FF, ped, 1, 2, emoteType, false, false, false, false, false);
        public static bool TaskUpperBodyEmote(int ped, uint emoteType) =>
            Function.Call<bool>((Hash)0xB31A277C1AC7B7FF, ped, 0, 0, emoteType, true, true, false, false, false);
        public static bool TaskEmote(int ped, uint emoteType, bool p3, bool p4, bool p5, bool p6, bool p7) =>
            Function.Call<bool>((Hash)0xB31A277C1AC7B7FF, ped, 0, 0, emoteType, p3, p4, p5, p6, p7);
        public static async void PlayClipset(string dict, string anim, int flag, int duration)
        {
            RequestAnimDict(dict);

            while (!HasAnimDictLoaded(dict))
            {
                await BaseScript.Delay(100);
            }

            if (IsEntityPlayingAnim(PlayerPedId(), dict, anim, 3))
            {
                ClearPedSecondaryTask(PlayerPedId());
            }
            else
            {
                Function.Call(Hash.TASK_PLAY_ANIM, PlayerPedId(), dict, anim, 1.0f, 8.0f, duration, flag, 0, true, 0, false, 0, false);
            }
        }
        public static async void PlayClipset2(string dict, string anim, int flag, int duration, float blendIn, float blendOut)
        {
            RequestAnimDict(dict);

            while (!HasAnimDictLoaded(dict))
            {
                await BaseScript.Delay(100);
            }

            if (IsEntityPlayingAnim(PlayerPedId(), dict, anim, 3))
            {
                ClearPedSecondaryTask(PlayerPedId());
            }
            else
            {
                Function.Call(Hash.TASK_PLAY_ANIM, PlayerPedId(), dict, anim, blendIn, blendOut, duration, flag, 0.0f, true, 0, false, 0, false);
            }
        }
        public static void RemovePedWoundEffect(int ped, float p2) => Function.Call((Hash)0x66B1CB778D911F49, ped, p2);
        public static void SetStance(int ped, string stance) => Function.Call((Hash)0x923583741DC87BCE, ped, stance);
        public static void SetWalking(int ped, string walking) => Function.Call((Hash)0x89F5E7ADECCCB49C, ped, walking);
        public static int CreatePickup(uint model, Vector3 position) => Function.Call<int>((Hash)0xFBA08C503DD5FA58, model, position.X, position.Y, position.Z, false, 0, 0, 0, 0, 0, 0, 0);
        public static void BlockPickupPlacementLight(int pickup, int block) => Function.Call((Hash)0x0552AA3FFC5B87AA, pickup, block);
        public static void DropCurrentPedWeapon() => Function.Call((Hash)0xC6A6789BB405D11C, PlayerPedId(), 1);
        public static void DoorSystemSetDoorState(uint hash, bool isLocked) => Function.Call((Hash)0x6BAB9442830C7F53, (dynamic)hash, isLocked ? 1 : 0);
        public static void DisableFirstPersonCamThisFrame() => Function.Call((Hash)0x9C473089A934C930);
        public static void NetworkSetFriendlyFireOption(bool toggle) => Function.Call((Hash)0xF808475FA571D823, toggle);
        public static void SetRelationShipBetweenGroups(int relationship, uint group1, uint group2) => Function.Call((Hash)0xBF25EB89375A37AD, relationship, group1, group2);
        public static void SetPedComponentEnabled(List<string> components, int index) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), uint.Parse(components[index], System.Globalization.NumberStyles.AllowHexSpecifier), true, true, false);
        public static void SetPedComponentEnabled(List<uint> components, int index) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components[index], true, true, false);
        public static void SetPedComponentEnabled(List<int> components, int index) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components[index], true, true, false);
        public static void SetPedComponentEnabled(uint components) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components, true, true, false);
        public static void SetPedComponentEnabled(uint components, bool p2) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components, true, true, p2);
        public static void SetPedComponentEnabled(int components) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components, true, true, false);
        public static void SetPedComponentEnabled(int components, bool p2) => Function.Call((Hash)0xD3A7B003ED343FD9, PlayerPedId(), components, true, true, p2);
        public static void ResetPedTexture(int textureId) => Function.Call((Hash)0x8472A1789478F82F, textureId);
        public static void ResetPedTexture2(int textureId) => Function.Call((Hash)0xB63B9178D0F58D82, textureId);
        public static void DeletePedTexture(int textureId) => Function.Call((Hash)0x6BEFAA907B076859, textureId);
        public static int CreatePedTexture(uint albedoHash, uint normalHash, uint unkHash) => Function.Call<int>((Hash)0xC5E7204F322E49EB, albedoHash, normalHash, unkHash);
        public static int AddPedOverlay(int textureId, uint albedoHash, uint normalHash, uint unkHash, int colorType, float opacity, int p6) => Function.Call<int>((Hash)0x86BB5FF45F193A02, textureId, albedoHash, normalHash, unkHash, colorType, opacity, p6);
        public static void SetPedOverlayPalette(int textureId, int overlayId, uint paletteHash) => Function.Call((Hash)0x1ED8588524AC9BE1, textureId, overlayId, paletteHash);
        public static void SetPedOverlayPaletteColour(int textureId, int overlayId, int primaryColor, int secondaryColor, int tertiaryColor) => Function.Call((Hash)0x2DF59FFE6FFD6044, textureId, overlayId, primaryColor, secondaryColor, tertiaryColor);
        public static void SetPedOverlayVariation(int textureId, int overlayId, int variation) => Function.Call((Hash)0x3329AAE2882FC8E4, textureId, overlayId, variation);
        public static void SetPedOverlayOpacity(int textureId, int overlayId, float opacity) => Function.Call((Hash)0x6C76BC24F8BB709A, textureId, overlayId, opacity);
        public static bool IsPedTextureValid(int textureId) => Function.Call<bool>((Hash)0x31DC8D3F216D8509, textureId);
        public static void OverrideTextureOnPed(int ped, uint componentHash, int textureId) => Function.Call((Hash)0x0B46E25761519058, ped, componentHash, textureId);
        public static void UpdatePedTexture(int textureId) => Function.Call((Hash)0x92DAABA2C1C10B0E, textureId);
        public static async void SetPlayerModel(uint model)
        {
            await LoadModel(model);
            Function.Call((Hash)0xED40380076A31506, PlayerId(), model, true);
        }
        public static void SetWeatherType(uint weatherType, bool p1, bool p2, bool p3, float p4, bool p5) => Function.Call((Hash)0x59174F1AFE095B5A, weatherType, p1, p2, p3, p4, p5);
        public static void SetWeatherTypeFrozen(bool toggle) => Function.Call((Hash)0xD74ACDF7DB8114AF, toggle);
        public static int GetVariationCount(string hash) => Constant.Peds.Find(x => x.HashString == hash).Variation;
    }
}
