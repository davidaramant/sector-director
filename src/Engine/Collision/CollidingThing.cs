// Copyright (c) 2019, Andrew Lonsway
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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
            public CollidingThingInitializer(
                MapGeometry map, 
                int currentSectorId = -1, 
                Vector2 position = new Vector2(), 
                Vector2 direction = new Vector2(),
                float radius = 8)
            {
                Map = map;
                CurrentSectorId = currentSectorId;
                Position = position;
                Direction = direction;
                Radius = radius;
            }
            public MapGeometry Map;
            public Vector2 Position;
            public Vector2 Direction;
            public int CurrentSectorId;
            public float Radius;
        }

        private readonly MapGeometry _map;
        private readonly List<int> _possibleSectorsToEnter;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;

        public float Height { get; } = 56;
        public float Width { get; } = 32;
        public float Radius { get; } = 8;
        public float ClimbableHeight { get; } = 24;
        public float Angle { get; private set; }
        public Matrix RotationTransform { get; private set; }


        public CollidingThing(CollidingThingInitializer data)
        {
            _map = data.Map;
            _possibleSectorsToEnter = new List<int>(_map.Sectors.Length);

            Position = data.Position;
            Direction = data.Direction;
            CurrentSectorId = data.CurrentSectorId;
            Radius = data.Radius;
            //Height = _map.Sectors[CurrentSectorId].Info.HeightFloor;
        }

        public void Move(ref Vector2 direction, float desiredDistance)
        {
            direction.Normalize();

            _possibleSectorsToEnter.Clear();
            Vector2 nearestEdgeCollisionPoint = Vector2.One * float.MaxValue;

            bool collides = FindNearestEdgeCollision(CurrentSectorId, desiredDistance, ref direction, ref nearestEdgeCollisionPoint);

            if (!collides)
            {
                //TODO find collisoin angle, leftover vector applied to slide
            }
            else
            {
                Position += direction * desiredDistance;

                CurrentSectorId = PickResultingSector();
            }

        }


        public bool FindNearestEdgeCollision(int sector, float distance, ref Vector2 direction, ref Vector2 nearestEdge)
        {
            ref SectorInfo currentSector = ref _map.Sectors[CurrentSectorId];

            float nearestEdgeDistance = float.MaxValue;

            Vector2 potentialPosition = Position + (direction * distance);

            foreach (Line line in currentSector.Lines)
            {
                int portalId = line.PortalToSectorId;
                if (portalId >= 0)
                {
                    if (!_possibleSectorsToEnter.Contains(portalId))
                    {
                        _possibleSectorsToEnter.Add(portalId);
                        Vector2 newEdgeCollision = Vector2.One * float.MaxValue;
                        bool result = FindNearestEdgeCollision(portalId, distance, ref direction, ref newEdgeCollision);
                        if (result)
                        {
                            UpdateNearestEdge(ref nearestEdge, ref nearestEdgeDistance, newEdgeCollision);
                        }
                    }
                }
                else
                {
                    /*
                    Portal id is -1, we are colliding into the void
                    This is an equation to model the intersection points of a line and a circle
                    a = (x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2
                    b = 2[ (x2 - x1) (x1 - x3) + (y2 - y1) (y1 - y3) + (z2 - z1) (z1 - z3) ]
                    c = x3^2 + y3^2 + z3^2 + x1^2 + y1^2 + z1^2 - 2[x3 x1 + y3 y1 + z3 z1] - r^2 
                    */

                    float a = (float)
                        (
                        Math.Pow(line.Vertex2.X - line.Vertex1.X, 2) + 
                        Math.Pow(line.Vertex2.Y - line.Vertex1.Y, 2));
                    float b = 2 * (float)
                        (
                        ((line.Vertex2.X - line.Vertex1.X) * (line.Vertex1.X - potentialPosition.X)) + 
                        ((line.Vertex2.Y - line.Vertex1.Y) * (line.Vertex1.Y - potentialPosition.Y)));
                    float c = (float)
                        (
                        Math.Pow(potentialPosition.X, 2) + 
                        Math.Pow(potentialPosition.Y, 2) + 
                        Math.Pow(line.Vertex1.X, 2) + 
                        Math.Pow(line.Vertex1.Y, 2) +
                        (-2 * (potentialPosition.X * line.Vertex1.X + 
                               potentialPosition.Y * line.Vertex1.Y)) - 
                        Math.Pow(Radius, 2));

                    // If expression is less than zero, we don't intersect
                    float expression = b * b - 4 * a * c;
                    if (expression > 0)
                    {
                        float collisionPointX = 0;
                        // We intersecet a line at two points. Finding the midpoint of these two points is where the tip of the closest circle can get
                        float firstX = (float)(-b + Math.Sqrt(expression)) / (2 * a);
                        float secondX = (float)(-b - Math.Sqrt(expression)) / (2 * a);
                        // If both of these points are less than 0 or greater than 1, the segment does not actually collide
                        if ((firstX > 0 && firstX < 1) || (secondX > 0 && secondX < 1))
                        {
                            collisionPointX = (firstX + secondX) / 2;
                            Vector2 lineDirection = line.Vertex2 - line.Vertex1;
                            Vector2 intersectionMidpoint = line.Vertex1 + (lineDirection * collisionPointX);

                            UpdateNearestEdge(ref nearestEdge, ref nearestEdgeDistance, intersectionMidpoint);
                        }
                    }
                }
                

                
            }
            return nearestEdgeDistance == float.MaxValue;
        }
        private void UpdateNearestEdge(ref Vector2 nearestEdge, ref float nearestEdgeDistance, Vector2 firstIntersection)
        {
            if (Vector2.Distance(Position, firstIntersection) < nearestEdgeDistance)
            {
                nearestEdge = firstIntersection;
                nearestEdgeDistance = Vector2.Distance(nearestEdge, Position);
            }
        }

        public void Rotate(float rotationRadians)
        {
            Angle += rotationRadians;
            RotationTransform = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, RotationTransform);
        }

        private int PickResultingSector()
        {
            return _possibleSectorsToEnter.FirstOr(sectorId => _map.IsInsideSector(sectorId, ref Position), CurrentSectorId);
        }
    }
}