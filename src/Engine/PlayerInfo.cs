// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class PlayerInfo
    {
        private readonly MapGeometry _map;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;
        public int Radius { get; } = 10;

        private const float _msToMoveSpeed = 80f / 1000f;
        private const float _msToRotateSpeed = 5f / 1000f;

        public PlayerInfo(MapGeometry map)
        {
            _map = map;
            var playerThing = map.Map.Things.First(t => t.Type == 1);

            Position = new Vector2((float)playerThing.X, (float)playerThing.Y);
            Direction = new Vector2(1, 0);
            Rotate(MathHelper.ToRadians(playerThing.Angle));

            CurrentSectorId = Enumerable.Range(0, _map.SectorCount).First(IsInsideSector);
        }

        private bool IsInsideSector(int sectorId)
        {
            foreach (var lineDefId in _map.GetSector(sectorId).LineIds)
            {
                ref Line line = ref _map.GetLine(lineDefId);

                ref Vector2 v1 = ref _map.GetVertex(line.V1);
                ref Vector2 v2 = ref _map.GetVertex(line.V2);

                var d = (Position.X - v1.X) * (v2.Y - v1.Y) - (Position.Y - v1.Y) * (v2.X - v1.X);

                if (d < 0)
                    return false;
            }

            return true;
        }

        public void Update(MovementInputs inputs, GameTime gameTime)
        {
            var moveSpeed = gameTime.ElapsedGameTime.Milliseconds * _msToMoveSpeed;
            var rotSpeed = gameTime.ElapsedGameTime.Milliseconds * _msToRotateSpeed;

            if (inputs.HasFlag(MovementInputs.Forward))
            {
                Move(ref Direction, moveSpeed);
            }
            else if (inputs.HasFlag(MovementInputs.Backward))
            {
                var direction = new Vector2 { X = -Direction.X, Y = -Direction.Y };

                Move(ref direction, moveSpeed);
            }
            if (inputs.HasFlag(MovementInputs.StrafeLeft))
            {
                var direction = new Vector2 { X = -Direction.Y, Y = Direction.X };

                Move(ref direction, moveSpeed);
            }
            else if (inputs.HasFlag(MovementInputs.StrafeRight))
            {
                var direction = new Vector2 { X = Direction.Y, Y = -Direction.X };

                Move(ref direction, moveSpeed);
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

        public void Move(ref Vector2 direction, float speed)
        {
            var movement = direction * speed;
            var newPosition = Position + movement;
            var newPlayerEdge = newPosition + direction * Radius;

            ref SectorInfo currentSector = ref _map.GetSector(CurrentSectorId);
            foreach (var lineId in currentSector.LineIds)
            {
                ref Line line = ref _map.GetLine(lineId);
                ref Vector2 v1 = ref _map.GetVertex(line.V1);
                ref Vector2 v2 = ref _map.GetVertex(line.V2);

                if (Line.HasCrossed(ref v1, ref v2, ref newPlayerEdge))
                {
                    if (line.PortalToSectorId != -1)
                    {
                        CurrentSectorId = line.PortalToSectorId;
                    }
                    else
                    {
                        // TODO: vector shearing
                        movement = new Vector2();
                    }
                }
            }
            Position += movement;
        }

        public void Rotate(float rotationRadians)
        {
            var rotation = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, rotation);
        }
    }
}