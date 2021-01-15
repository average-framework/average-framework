using System.Collections.Generic;

namespace Shared.Core.DataModels
{
    public class CharacterData
    {
        public string CharacterId { get; set; }
        public string RockstarId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nationality { get; set; }
        public string CityOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public int SexType { get; set; }
        public int Culture { get; set; }
        public int Head { get; set; }
        public int Body { get; set; }
        public int Legs { get; set; }
        public int BodyType { get; set; }
        public int WaistType { get; set; }
        public float Scale { get; set; }
        public string CreationDate { get; set; }
        public EconomyData Economy { get; set; } = new EconomyData();
        public PositionData Position { get; set; } = new PositionData();
        public PlayerCoreData Core { get; set; } = new PlayerCoreData();
        public JobData Job { get; set; } = new JobData();
        public Dictionary<string, dynamic> Texture { get; set; } = new Dictionary<string, dynamic>();
        public Dictionary<int, float> FaceParts { get; set; } = new Dictionary<int, float>();
        public Dictionary<string, uint> Clothes { get; set; } = new Dictionary<string, uint>();
        public Dictionary<string, OverlayData> Overlays { get; set; } = new Dictionary<string, OverlayData>();
        public Dictionary<string, int> Ammos { get; set; } = new Dictionary<string, int>();

        public CharacterData() { }

        public class JobData
        {
            public string Name { get; set; } = "unemployed";
            public int Level { get; set; } = 0;

            public JobData() { }
        }

        public class PlayerCoreData
        {
            public int Health { get; set; } = 100;
            public int Hunger { get; set; } = 100;
            public int Thirst { get; set; } = 100;

            public PlayerCoreData() { }
        }

        public class EconomyData
        {
            public decimal Money { get; set; }
            public decimal Bank { get; set; }

            public EconomyData() { }

            public EconomyData(decimal money, decimal bank)
            {
                Money = money;
                Bank = bank;
            }
        }

        public class PositionData
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float H { get; set; }

            public PositionData() { }

            public PositionData(float x, float y, float z, float h)
            {
                X = x;
                Y = y;
                Z = z;
                H = h;
            }
        }

        public class OverlayData
        {
            public string Name { get; set; }
            public int Visibility { get; set; } = 0;
            public int TxColorType { get; set; } = 0;
            public float TxOpacity { get; set; } = 1.0f;
            public int TxUnk { get; set; } = 0;
            public int PaletteColorPrimary { get; set; } = 0;
            public int PaletteColorSecondary { get; set; } = 0;
            public int PaletteColorTertiary { get; set; } = 0;
            public int Var { get; set; } = 0;
            public float Opacity { get; set; } = 1.0f;
            public uint TxId { get; set; } = 0;
            public uint TxNormal { get; set; } = 0;
            public uint TxMaterial { get; set; } = 0;
            public uint Palette { get; set; } = 1;

            public OverlayData(string name, int visibility, uint txId, uint txNormal, uint txMaterial, int txColorType, float txOpacity, int txUnk, uint palette, int paletteColorPrimary, int paletteColorSecondary, int paletteColorTertiary, int var, float opacity)
            {
                Name = name;
                Visibility = visibility;
                TxId = txId;
                TxNormal = txNormal;
                TxMaterial = txMaterial;
                TxColorType = txColorType;
                TxOpacity = txOpacity;
                TxUnk = txUnk;
                Palette = palette;
                PaletteColorPrimary = paletteColorPrimary;
                PaletteColorSecondary = paletteColorSecondary;
                PaletteColorTertiary = paletteColorTertiary;
                Var = var;
                Opacity = opacity;
            }

            public OverlayData() { }

            public OverlayData(string name)
            {
                Name = name;
            }

            public OverlayData(string name, int txColorType)
            {
                Name = name;
                TxColorType = txColorType;
            }
        }
    }
}
