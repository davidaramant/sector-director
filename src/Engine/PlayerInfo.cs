// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.CollectionExtensions;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine
{
    public sealed class PlayerInfo
    {
        private readonly MapGeometry _map;
        private readonly List<int> _possibleSectorsToEnter;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;
        public float Angle { get; private set; }
        public Matrix RotationTransform { get; private set; }

        public float Height { get; } = 56;
        public float Width { get; } = 32;
        public float Radius { get; } = 16;
        public float ClimbableHeight { get; } = 24;

        private const float MsToMoveSpeed = 80f / 1000f;
        private const float MsToRotateSpeed = 5f / 1000f;

        public PlayerInfo(MapGeometry map)
        {
            _map = map;
            _possibleSectorsToEnter = new List<int>(map.Sectors.Length);
            var playerThing = map.Map.Things.First(t => t.Type == 1);
            var playerThingIndex = map.Map.Things.IndexOf(playerThing);

            Position = new Vector2((float)playerThing.X, (float)playerThing.Y);
            Direction = new Vector2(0, 1);
            Rotate(MathHelper.ToRadians(playerThing.Angle) - MathHelper.PiOver2);

            CurrentSectorId = _map.ThingToSectorId[playerThingIndex];
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
            return _possibleSectorsToEnter.FirstOr(sectorId => _map.IsInsideSector(sectorId, ref Position), CurrentSectorId);
        }

        public void Rotate(float rotationRadians)
        {
            Angle += rotationRadians;
            RotationTransform = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, RotationTransform);
        }
    }
}