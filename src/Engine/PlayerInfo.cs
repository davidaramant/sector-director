// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Engine.Collision;
using SectorDirector.Engine.Input;
using SectorDirector.Engine.Renderers.FirstPerson;

namespace SectorDirector.Engine
{
    
    public sealed class PlayerInfo : CollidingThing
    {
        private const float MsToMoveSpeed = 180f / 1000f;
        private const float MsToRotateSpeed = 5f / 1000f;
        private const float PlayerRadius = 16;
        public float ViewHeight { get; } = 41;

        public FirstPersonCameraSettings CameraSettings { get; } = new FirstPersonCameraSettings
        {
            FieldOfView = 60,
            MinClippingDistance = 1,
            MaxClippingDistance = 1000,
            EyeWidth = 100
        };

        public PlayerInfo(MapGeometry map) : base(GetInitialThingValues(map))
        {

        }

        public void Update(ContinuousInputs inputs, GameTime gameTime)
        {
            var distance = gameTime.ElapsedGameTime.Milliseconds * MsToMoveSpeed;
            var rotationAmount = gameTime.ElapsedGameTime.Milliseconds * MsToRotateSpeed;

            if (inputs.Forward)
            {
                Move(ref Direction, distance);
            }
            else if (inputs.Backward)
            {
                var direction = new Vector2 { X = -Direction.X, Y = -Direction.Y };

                Move(ref direction, distance);
            }
            if (inputs.StrafeLeft)
            {
                var direction = new Vector2 { X = -Direction.Y, Y = Direction.X };

                Move(ref direction, distance);
            }
            else if (inputs.StrafeRight)
            {
                var direction = new Vector2 { X = Direction.Y, Y = -Direction.X };

                Move(ref direction, distance);
            }

            if (inputs.TurnRight)
            {
                Rotate(-rotationAmount);
            }
            else if (inputs.TurnLeft)
            {
                Rotate(rotationAmount);
            }
        }

        private static CollidingThingInitializer GetInitialThingValues(MapGeometry map)
        { 
            var playerThing = map.Map.Things.First(t => t.Type == 1);
            var playerThingIndex = map.Map.Things.IndexOf(playerThing);

            var position = playerThing.GetPosition();
            var angle = MathHelper.ToRadians(playerThing.Angle);

            var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            var currentSectorId = map.ThingToSectorId[playerThingIndex];

            var verticalPosition = map.Sectors[currentSectorId].Info.HeightFloor;

            return new CollidingThingInitializer(map, currentSectorId, position, direction, PlayerRadius);
        }
    }
}