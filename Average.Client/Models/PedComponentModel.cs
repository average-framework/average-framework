﻿using Client.Core.Enums;
using System.Collections.Generic;

namespace Client.Models
{
    public class PedComponentModel
    {
        public Gender Gender { get; set; }
        public List<string> Heads { get; set; }
        public List<string> Body { get; set; }
        public List<string> Legs { get; set; }
        public string HeadTexture { get; set; }

        public PedComponentModel() { }

        public PedComponentModel(Gender gender, List<string> heads, List<string> body, List<string> legs, string headTexture)
        {
            Gender = gender;
            Heads = heads;
            Body = body;
            Legs = legs;
            HeadTexture = headTexture;
        }
    }
}
