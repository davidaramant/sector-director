// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.CollectionExtensions;
using SectorDirector.Engine.Input;
using static SectorDirector.Engine.CollidingThing;

namespace SectorDirector.Engine
{
    
    public sealed class PlayerInfo : CollidingThing
    {
        private const float MsToMoveSpeed = 80f / 1000f;
        private const float MsToRotateSpeed = 5f / 1000f;
        private const float PlayerRadius = 16;
        public float ViewHeight { get; } = 41;


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

            var position = new Vector2((float)playerThing.X, (float)playerThing.Y);
            var angle = MathHelper.ToRadians(playerThing.Angle);

            var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            var currentSectorId = map.ThingToSectorId[playerThingIndex];

            return new CollidingThingInitializer(map, currentSectorId, position, direction, PlayerRadius);
        }
    }
}