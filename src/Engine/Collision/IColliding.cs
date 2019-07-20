using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Collision
{
    interface IColliding
    {
        void Move(ref Vector2 direction, float distance);
        void Rotate(float rotationRadians);

    }
}
