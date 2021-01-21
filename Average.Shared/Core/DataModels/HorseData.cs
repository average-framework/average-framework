﻿using System.Collections.Generic;

namespace Shared.Core.DataModels
{
    public class HorseData
    {
        public string HorseId { get; set; }
        public uint Model { get; set; }
        public Dictionary<string, uint> Components { get; set; }
        public int Entity { get; set; }
        public int OwnerServerId { get; set; }
        public string Name { get; set; }
        public string RockstarId { get; set; }
        public int Gender { get; set; }
        public int IsStored { get; set; }
        public int IsDead { get; set; }
        public CharacterData.PositionData Position { get; set; } = new CharacterData.PositionData();

        public HorseData(string rockstarId, string name, int gender, uint model, Dictionary<string, uint> components, int isStored, int isDead)
        {
            RockstarId = rockstarId;
            Name = name;
            Gender = gender;
            Model = model;
            Components = components;
            IsStored = isStored;
            IsDead = isDead;
        }
    }
}