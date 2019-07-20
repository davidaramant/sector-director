// Copyright (c) 2019, Andrew Lonsway
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SectorDirector.Core.CollectionExtensions;
using SectorDirector.Engine.Collision;
using SectorDirector.Engine.Input;

namespace SectorDirector.Engine
{
    public class CollidingThing : IColliding
    {
        public struct CollidingThingInitializer
        {
            public CollidingThingInitializer(MapGeometry map, int currentSectorId = -1, Vector2 position = new Vector2(), Vector2 direction = new Vector2())
            {
                Map = map;
                CurrentSectorId = currentSectorId;
                Position = position;
                Direction = direction;
            }
            public MapGeometry Map;
            public Vector2 Position;
            public Vector2 Direction;
            public int CurrentSectorId;
        }

        private readonly MapGeometry _map;
        private readonly List<int> _possibleSectorsToEnter;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;

        public float Height { get; } = 56;
        public float Width { get; } = 32;
        public float Radius { get; } = 16;
        public float ClimbableHeight { get; } = 24;


        public CollidingThing(CollidingThingInitializer data)
        {
            _map = data.Map;
            _possibleSectorsToEnter = new List<int>(_map.Sectors.Length);

            Position = data.Position;
            Direction = data.Direction;
            CurrentSectorId = data.CurrentSectorId;
        }

        public void Move(ref Vector2 direction, float distance)
        {
            // TODO
            // Check all sectors that you are overlapping with 
            // Project yourself to go as far as you can

            _possibleSectorsToEnter.Clear();

            var movement = direction * distance;
            var newPlayerEdge = Position + movement + direction * Radius;

            ref SectorInfo currentSector = ref _map.Sectors[CurrentSectorId];

            /*foreach (var lineId in currentSector.LineIds)
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
            }*/

            Position += movement;
            CurrentSectorId = PickResultingSector();
        }

        public   void Rotate(float rotationRadians)
        {
            var rotation = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, rotation);
        }

        private int PickResultingSector()
        {
            return _possibleSectorsToEnter.FirstOr(sectorId => _map.IsInsideSector(sectorId, ref Position), CurrentSectorId);
        }
    }
}