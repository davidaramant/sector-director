// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using SectorDirector.Core.FormatModels.Udmf;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class PlayerInfo
    {
        public Vector2 Position;
        public Vector2 Direction;
        public int Radius { get; } = 10;

        private const float _msToMoveSpeed = 80f / 1000f;
        private const float _msToRotateSpeed = 5f / 1000f;

        public PlayerInfo(Thing playerThing)
        {
            Position = new Vector2((float)playerThing.X, (float)playerThing.Y);
            Direction = new Vector2(1, 0);
            Rotate(MathHelper.ToRadians(playerThing.Angle));
        }

        public void Update(MapGeometry mapData, MovementInputs inputs, GameTime gameTime)
        {
            var moveSpeed = gameTime.ElapsedGameTime.Milliseconds * _msToMoveSpeed;
            var rotSpeed = gameTime.ElapsedGameTime.Milliseconds * _msToRotateSpeed;

            if (inputs.HasFlag(MovementInputs.Forward))
            {
                Move(mapData, ref Direction, moveSpeed);
            }
            else if (inputs.HasFlag(MovementInputs.Backward))
            {
                var direction = new Vector2 { X = -Direction.X, Y = -Direction.Y };

                Move(mapData, ref direction, moveSpeed);
            }
            if (inputs.HasFlag(MovementInputs.StrafeLeft))
            {
                var direction = new Vector2 { X = -Direction.Y, Y = Direction.X };

                Move(mapData, ref direction, moveSpeed);
            }
            else if (inputs.HasFlag(MovementInputs.StrafeRight))
            {
                var direction = new Vector2 { X = Direction.Y, Y = -Direction.X };

                Move(mapData, ref direction, moveSpeed);
            }

            if (inputs.HasFlag(MovementInputs.TurnRight))
            {
                Rotate(-rotSpeed);
            }
            else if (inputs.HasFlag(MovementInputs.TurnLeft))
            {
                Rotate(rotSpeed);
            }
        }

        public void Move(MapGeometry mapData, ref Vector2 direction, float speed)
        {
            // TODO: Collisions
            var movement = direction * speed;
            var newPosition = Position + movement;
            //var newBoundingEdge = newPosition + direction * Radius;

            //if (mapData.IsPassable((int)newBoundingEdge.X, (int)Position.Y))
            {
                Position.X = newPosition.X;
            }
            //if (mapData.IsPassable((int)Position.X, (int)newBoundingEdge.Y))
            {
                Position.Y = newPosition.Y;
            }
        }

        public void Rotate(float rotationRadians)
        {
            var rotation = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, rotation);
        }
    }
}