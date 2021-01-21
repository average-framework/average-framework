using System.Collections.Generic;
using CitizenFX.Core;

namespace Client.Models
{
    public class CameraInfo
    {
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraLookAtOffset { get; set; }
        public Vector3 Position { get; set; }
        public float CameraFov { get; set; }
        public float Heading { get; set; }
    }
}