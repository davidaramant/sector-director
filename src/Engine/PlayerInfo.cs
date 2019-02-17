// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.FormatModels.Udmf;

namespace SectorDirector.Engine
{
    public sealed class PlayerInfo
    {
        private readonly MapGeometry _map;
        private readonly List<int> _possibleSectorsToEnter;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;
        public int Radius { get; } = 10;

        private const float MsToMoveSpeed = 80f / 1000f;
        private const float MsToRotateSpeed = 5f / 1000f;

        public PlayerInfo(MapGeometry map)
        {
            _map = map;
            _possibleSectorsToEnter = new List<int>(map.Sectors.Length);
            var playerThing = map.Map.Things.First(t => t.Type == 1);

            Position = new Vector2((float)playerThing.X, (float)playerThing.Y);
            Direction = new Vector2(1, 0);
            Rotate(MathHelper.ToRadians(playerThing.Angle));

            CurrentSectorId = Enumerable.Range(0, _map.Sectors.Length).First(sectorId => IsInsideSector(ref Position, sectorId));
        }

        private bool IsInsideSector(ref Vector2 position, int sectorId)
        {
            ref SectorInfo sector = ref _map.Sectors[sectorId];
            foreach (var lineDefId in sector.LineIds)
            {
                ref Line line = ref _map.Lines[lineDefId];

                ref Vector2 v1 = ref _map.Vertices[line.V1];
                ref Vector2 v2 = ref _map.Vertices[line.V2];

                var d = (position.X - v1.X) * (v2.Y - v1.Y) - (position.Y - v1.Y) * (v2.X - v1.X);

                if (d < 0)
                    return false;
            }

            return true;
        }

        public void Update(MovementInputs inputs, GameTime gameTime)
        {
            var distance = gameTime.ElapsedGameTime.Milliseconds * MsToMoveSpeed;
            var rotationAmount = gameTime.ElapsedGameTime.Milliseconds * MsToRotateSpeed;

            if (inputs.HasFlag(MovementInputs.Forward))
            {
                Move(ref Direction, distance);
            }
            else if (inputs.HasFlag(MovementInputs.Backward))
            {
                var direction = new Vector2 { X = -Direction.X, Y = -Direction.Y };

                Move(ref direction, distance);
            }
            if (inputs.HasFlag(MovementInputs.StrafeLeft))
            {
                var direction = new Vector2 { X = -Direction.Y, Y = Direction.X };

                Move(ref direction, distance);
            }
            else if (inputs.HasFlag(MovementInputs.StrafeRight))
            {
                var direction = new Vector2 { X = Direction.Y, Y = -Direction.X };

                Move(ref direction, distance);
            }

            if (inputs.HasFlag(MovementInputs.TurnRight))
            {
                Rotate(-rotationAmount);
            }
            else if (inputs.HasFlag(MovementInputs.TurnLeft))
            {
                Rotate(rotationAmount);
            }
        }

        public void Move(ref Vector2 direction, float distance)
        {
            _possibleSectorsToEnter.Clear();

            var movement = direction * distance;
            var newPlayerEdge = Position + movement + direction * Radius;

            ref SectorInfo currentSector = ref _map.Sectors[CurrentSectorId];

            foreach (var lineId in currentSector.LineIds)
            {
                ref Line line = ref _map.Lines[lineId];
                ref Vector2 v1 = ref _map.Vertices[line.V1];
                ref Vector2 v2 = ref _map.Vertices[line.V2];

                if (Line.HasCrossed(ref v1, ref v2, ref newPlayerEdge))
                {
                    var intersection = Line.Intersection(ref v1, ref v2, ref Position, ref newPlayerEdge);
                    if (Line.IsPointOnLineSegment(ref v1, ref v2, ref intersection))
                    {
                        if (line.PortalToSectorId != -1)
                        {
                            _possibleSectorsToEnter.Add(line.PortalToSectorId);
                        }
                        else
                        {
                            // TODO: vector shearing
                            movement = new Vector2();
                        }
                    }
                }
            }

            Position += movement;
            CurrentSectorId = PickResultingSector();
        }

        private int PickResultingSector()
        {
            foreach (var sectorId in _possibleSectorsToEnter)
            {
                if (IsInsideSector(ref Position, sectorId))
                {
                    return sectorId;
                }
            }

            return CurrentSectorId;
        }

        public void Rotate(float rotationRadians)
        {
            var rotation = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, rotation);
        }
    }
}