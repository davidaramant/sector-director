using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorDirector.Engine.Renderers.FirstPerson
{
    public struct FirstPersonCameraSettings
    {
        public float FieldOfView;
        public float MinClippingDistance;
        public float MaxClippingDistance; // fog
        public float EyeWidth;
    }
}
