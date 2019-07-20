// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using GeoAPI.Geometries;
using Microsoft.Xna.Framework;
using NetTopologySuite.Index.Strtree;
using SectorDirector.Core.FormatModels.Udmf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Math;

namespace SectorDirector.Engine
{
    [DebuggerDisplay("ID: {LineDefId}")]
    public sealed class Line
    {
        public readonly int LineDefId;
        public readonly int SectorId;
        public readonly int V1;
        public readonly Vector2 Vertex1;
        public readonly int V2;
        public readonly Vector2 Vertex2;
        public readonly int PortalToSectorId;

        public Line(int lineDefId, int sectorId, int v1, Vertex vertex1, int v2, Vertex vertex2, int portalToSectorId = -1)
        {
            LineDefId = lineDefId;
            SectorId = sectorId;
            V1 = v1;
            Vertex1 = vertex1.ToVector2();
            V2 = v2;
            Vertex2 = vertex2.ToVector2();
            PortalToSectorId = portalToSectorId;
        }

        public static bool HasCrossed(ref Vector2 v1, ref Vector2 v2, ref Vector2 point) =>
            (v2.X - v1.X) * (point.Y - v1.Y) - (v2.Y - v1.Y) * (point.X - v1.X) > 0;

        public static float CrossProduct(ref Vector2 v1, ref Vector2 v2) => v1.X * v2.Y - v2.X * v1.Y;

        public static Vector2 Intersection(
            ref Vector2 v1,
            ref Vector2 v2,
            ref Vector2 p1,
            ref Vector2 p2)
        {
            var v1_minus_v2 = v1 - v2;
            var p1_minus_p2 = p1 - p2;

            var v1_minus_v2_cross_p1_minus_p2 = CrossProduct(ref v1_minus_v2, ref p1_minus_p2);

            var v1_cross_v2 = CrossProduct(ref v1, ref v2);
            var p1_cross_p2 = CrossProduct(ref p1, ref p2);

            var x_temp1 = new Vector2(v1_cross_v2, v1_minus_v2.X);
            var x_temp2 = new Vector2(p1_cross_p2, p1_minus_p2.X);

            var y_temp1 = new Vector2(v1_cross_v2, v1_minus_v2.Y);
            var y_temp2 = new Vector2(p1_cross_p2, p1_minus_p2.Y);

            return new Vector2(
                x: CrossProduct(ref x_temp1, ref x_temp2) / v1_minus_v2_cross_p1_minus_p2,
                y: CrossProduct(ref y_temp1, ref y_temp2) / v1_minus_v2_cross_p1_minus_p2);
        }

        public static bool IsPointOnLineSegment(ref Vector2 v1, ref Vector2 v2, ref Vector2 p) =>
            p.X <= Max(v1.X, v2.X) && p.X >= Min(v1.X, v2.X) &&
            p.Y <= Max(v1.Y, v2.Y) && p.Y >= Min(v1.Y, v2.Y);
    }

    public sealed class SectorInfo
    {
        public readonly Sector Info;
        public readonly List<Line> Lines = new List<Line>();

        public SectorInfo(Sector info, IEnumerable<Line> lines)
        {
            Info = info;
            Lines.AddRange(lines);
        } 
    }

    public sealed class MapGeometry
    {
        public MapData Map { get; }

        public Vector2[] Vertices { get; }
        public Line[] Lines { get; }
        public SectorInfo[] Sectors { get; }
        public int[] ThingToSectorId { get; }

        public Vector2 BottomLeftCorner { get; }
        public Vector2 Area { get; }

        readonly STRtree<int> _sectorIdLookup = new STRtree<int>();

        public MapGeometry(MapData map)
        {
            Map = map;

            Vertices = map.Vertices.Select(v => v.ToVector2()).ToArray();

            var minX = Vertices.Min(p => p.X);
            var maxX = Vertices.Max(p => p.X);
            var minY = Vertices.Min(p => p.Y);
            var maxY = Vertices.Max(p => p.Y);

            BottomLeftCorner = new Vector2(minX, minY);
            Area = new Vector2(maxX - minX, maxY - minY);

            var lines = new List<Line>();
            for (int lineDefId = 0; lineDefId < Map.LineDefs.Count; lineDefId++)
            {
                var lineDef = Map.LineDefs[lineDefId];
                var frontSide = Map.SideDefs[lineDef.SideFront];

                if (!lineDef.TwoSided)
                {
                    lines.Add(new Line(lineDefId, frontSide.Sector, lineDef.V1, Map.Vertices[lineDef.V1], lineDef.V2, Map.Vertices[lineDef.V2]));
                }
                else
                {
                    var backSide = Map.SideDefs[lineDef.SideBack];
                    lines.Add(new Line(lineDefId, frontSide.Sector, lineDef.V1, Map.Vertices[lineDef.V1], lineDef.V2, Map.Vertices[lineDef.V2], portalToSectorId: backSide.Sector));
                    lines.Add(new Line(lineDefId, backSide.Sector, lineDef.V1, Map.Vertices[lineDef.V1], lineDef.V2, Map.Vertices[lineDef.V2], portalToSectorId: frontSide.Sector));
                }
            }
            Lines = lines.ToArray();

            Sectors = new SectorInfo[map.Sectors.Count];

            foreach (var sectorIndex in Enumerable.Range(0, Sectors.Length))
            {
                var linesForSector = Lines.Where(line => line.SectorId == sectorIndex);

                Sectors[sectorIndex] = new SectorInfo(map.Sectors[sectorIndex], linesForSector);

                _sectorIdLookup.Insert(GetSectorMinimumBoundingRectangle(sectorIndex), sectorIndex);
            }
            _sectorIdLookup.Build();

            ThingToSectorId = new int[map.Things.Count];

            foreach (var thingIndex in Enumerable.Range(0, map.Things.Count))
            {
                ThingToSectorId[thingIndex] = FindSurroundingSector(map.Things[thingIndex].GetPosition());
            }
        }

        public int FindSurroundingSector(Vector2 position)
        {
            var envelope = new Envelope(new Coordinate(position.X, position.Y));

            foreach (var sectorId in _sectorIdLookup.Query(envelope))
            {
                if (IsInsideSector(sectorId, ref position))
                {
                    return sectorId;
                }
            }

            return -1;
        }

        public Envelope GetSectorMinimumBoundingRectangle(int sectorId)
        {
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (Line line in Sectors[sectorId].Lines)
            {
                minX = Min(Min(minX, (float)line.Vertex1.X), (float)line.Vertex2.X);
                maxX = Max(Max(maxX, (float)line.Vertex1.X), (float)line.Vertex2.X);
                minY = Min(Min(minY, (float)line.Vertex1.Y), (float)line.Vertex2.Y);
                maxY = Max(Max(maxY, (float)line.Vertex1.Y), (float)line.Vertex2.Y);
            }

            return new Envelope(minX, maxX, minY, maxY);
        }

        public bool IsInsideSector(int sectorIndex, ref Vector2 point)
        {
            // https://stackoverflow.com/questions/11716268/point-in-polygon-algorithm
            bool insideSector = false;
            foreach (var line in Sectors[sectorIndex].Lines)
            {
                var vertex1 = line.Vertex1;
                var vertex2 = line.Vertex2;

                if (((vertex1.Y >= point.Y) != (vertex2.Y >= point.Y)) &&
                    (point.X <= (vertex2.X - vertex1.X) * (point.Y - vertex1.Y) / (vertex2.Y - vertex1.Y) + vertex1.X))
                {
                    insideSector = !insideSector;
                }
            }

            return insideSector;
        }
    }
}
