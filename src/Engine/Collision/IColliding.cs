// Copyright (c) 2019, Andrew Lonsway, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Collision
{
    interface IColliding
    {
        void Move(ref Vector2 direction, float distance);
        void Rotate(float rotationRadians);
    }
}
