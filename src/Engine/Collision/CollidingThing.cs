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
                int currentSectorId,
                Vector2 position = new Vector2(),
                Vector2 direction = new Vector2(),
                float radius = 8)
            {
                Map = map;
                CurrentSectorId = currentSectorId;
                Position = position;
                Direction = direction;
                Radius = radius;
                VerticalPosition = map.Sectors[currentSectorId].Info.HeightFloor;

            }
            public MapGeometry Map;
            public Vector2 Position;
            public Vector2 Direction;
            public int CurrentSectorId;
            public float Radius;
            public float VerticalPosition;
        }

        private readonly MapGeometry _map;
        private readonly List<int> _possibleSectorsToEnter;
        private int HeightSourceSectorId;
        public int CurrentSectorId { get; private set; }
        public Vector2 Position;
        public Vector2 Direction;

        public float Height { get; } = 56;
        public float VerticalPosition { get; private set; } = 0;
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
            VerticalPosition = data.VerticalPosition;
            HeightSourceSectorId = CurrentSectorId;
        }

        public void Move(ref Vector2 direction, float desiredDistance)
        {
            direction.Normalize();

            _possibleSectorsToEnter.Clear();
            Vector2 nearestEdgeCollisionPoint = Vector2.One * float.MaxValue;

            bool collides = FindNearestEdgeCollision(CurrentSectorId, desiredDistance, ref direction, ref nearestEdgeCollisionPoint);

            if (collides)
            {
                //TODO find collision angle, leftover vector applied to slide
            }
            else
            {
                Position += direction * desiredDistance;

                CurrentSectorId = PickResultingSector();
                // Only reset our height when we have left the previous sector, or if the current sector is higher
                float newHeight = _map.Sectors[CurrentSectorId].Info.HeightFloor;
                if (HeightSourceSectorId != CurrentSectorId && 
                    (!_possibleSectorsToEnter.Contains(HeightSourceSectorId) || newHeight > VerticalPosition))
                {
                    HeightSourceSectorId = CurrentSectorId;
                    VerticalPosition = newHeight;
                }

            }
        }

        public bool FindNearestEdgeCollision(int sectorId, float distance, ref Vector2 direction, ref Vector2 nearestEdge)
        {
            ref SectorInfo currentSector = ref _map.Sectors[sectorId];
            bool foundCollision = false;
            float nearestEdgeDistance = Vector2.Distance(Position, nearestEdge);

            Vector2 potentialPosition = Position + (direction * distance);

            foreach (Line line in currentSector.Lines)
            {
                int portalId = line.PortalToSectorId;
                bool canCollideWithLine = true;
                if (portalId >= 0)
                {
                    var mapInfo = _map.Sectors[portalId].Info;
                    float heightDiff = mapInfo.HeightFloor - VerticalPosition;
                    if (heightDiff < ClimbableHeight && VerticalPosition + Height < mapInfo.HeightCeiling)
                    {
                        canCollideWithLine = false;
                        float unusedX = 0, unusedY = 0;
                        bool collidesWithSectorBoundary = CircleCollidesWithLine(ref unusedX, ref unusedY, line, Position, Radius);
                        if (!_possibleSectorsToEnter.Contains(portalId) && collidesWithSectorBoundary)
                        {
                            _possibleSectorsToEnter.Add(portalId);
                            Vector2 newEdgeCollision = Vector2.One * float.MaxValue;
                            bool result = FindNearestEdgeCollision(portalId, distance, ref direction, ref newEdgeCollision);
                            if (result)
                            {
                                UpdateNearestEdge(ref nearestEdge, ref nearestEdgeDistance, newEdgeCollision);
                                foundCollision = true;
                            }
                        }
                    }
                }

                if (canCollideWithLine)
                {
                    float firstX = 0;
                    float secondX = 0;
                    bool collidesWithLine = CircleCollidesWithLine(ref firstX, ref secondX, line, potentialPosition, Radius);
                    if (collidesWithLine)
                    {
                        float collisionPointX = 0;
                        collisionPointX = (firstX + secondX) / 2;
                        Vector2 lineDirection = line.Vertex2 - line.Vertex1;
                        Vector2 intersectionMidpoint = line.Vertex1 + (lineDirection * collisionPointX);

                        UpdateNearestEdge(ref nearestEdge, ref nearestEdgeDistance, intersectionMidpoint);
                        foundCollision = true;
                    }
                }
            }
            return foundCollision;
        }
        private bool CircleCollidesWithLine(ref float firstX, ref float secondX, Line line, Vector2 circleCenter, float radius)
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
                ((line.Vertex2.X - line.Vertex1.X) * (line.Vertex1.X - circleCenter.X)) +
                ((line.Vertex2.Y - line.Vertex1.Y) * (line.Vertex1.Y - circleCenter.Y)));
            float c = (float)
                (
                Math.Pow(circleCenter.X, 2) +
                Math.Pow(circleCenter.Y, 2) +
                Math.Pow(line.Vertex1.X, 2) +
                Math.Pow(line.Vertex1.Y, 2) +
                (-2 * (circleCenter.X * line.Vertex1.X +
                       circleCenter.Y * line.Vertex1.Y)) -
                Math.Pow(radius, 2));

            // If expression is less than zero, we don't intersect
            float expression = b * b - 4 * a * c;

            // We intersecet a line at two points. Finding the midpoint of these two points is where the tip of the closest circle can get
            firstX = (float)(-b + Math.Sqrt(expression)) / (2 * a);
            secondX = (float)(-b - Math.Sqrt(expression)) / (2 * a);

            return expression > 0 && ((firstX > 0 && firstX < 1) || (secondX > 0 && secondX < 1));

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