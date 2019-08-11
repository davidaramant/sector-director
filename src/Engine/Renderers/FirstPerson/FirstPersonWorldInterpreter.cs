using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;


namespace SectorDirector.Engine.Renderers.FirstPerson
{
    public sealed class FirstPersonWorldInterpreter
    {
        private FirstPersonCameraSettings _settings;
        public FirstPersonCameraSettings Settings
        {
            set
            {
                _settings = value;
            }
        }
        
        public FirstPersonWorldInterpreter()
        {
            _settings = new FirstPersonCameraSettings
            {
                FieldOfView = 90,
                MinClippingDistance = 5,
                MaxClippingDistance = 500
            };
        }
        public FirstPersonWorldInterpreter(FirstPersonCameraSettings settings)
        {
            _settings = settings;
        }
        
        public (bool isInView, bool isFacing, float horizontalDegrees, float verticalDegrees) ConvertWorldPointToScreenDegrees(
            Vector3 point)
        {
            Vector3 normalizedPoint = Vector3.Normalize(point);
            float distance = point.ToVector2().Length();
            // HORIZONTAL CALCULATIONS 

            Vector2 targetedVectorDirection = Vector2.Normalize(point.ToVector2());

            float centerAngleDiff = MathShortcuts.AngleBetween(new Vector2(0, 1), targetedVectorDirection) * MathShortcuts.RadToDeg;
            // Since AngleBetween doesn't give a sign, this calculates it ourselves
            // We go from left to right, so clockwise in our angle
            if (point.X < 0)
            {
                centerAngleDiff *= -1;
            }

            bool isInHorizontalFieldOfView = Math.Abs(centerAngleDiff) < _settings.FieldOfView / 2;
            bool isFacing = normalizedPoint.Y > 0;

            // VERTICAL CALCULATIONS
            float vertDistance = point.Z;
            
            float centerDistance = (float)(Math.Cos(centerAngleDiff * MathShortcuts.DegToRad) * distance);
            float verticalViewAngle = (float)(Math.Atan(vertDistance / centerDistance) * MathShortcuts.RadToDeg);
            if (point.Z > 0)
            {
                verticalViewAngle = Math.Abs(verticalViewAngle);
            }
            else
            {
                verticalViewAngle = -1 * Math.Abs(verticalViewAngle);
            }

            bool isInVerticalView = Math.Abs(verticalViewAngle) < _settings.FieldOfView / 2;

            return (isInHorizontalFieldOfView && isInVerticalView, isFacing, centerAngleDiff, verticalViewAngle);

        }
        public (bool shouldDraw, Point p1) ConvertWorldPointToScreenPoint(
            ScreenBuffer buffer,
            Vector3 cameraPosition,
            Vector2 cameraDirection,
            Vector3 point)
        {
            var (isInView, isFacing, horizontalDegrees, verticalDegrees) = ConvertWorldPointToScreenDegrees(point);
            return (isInView, new Point(
                (int)(((horizontalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Width),
                (int)(((-verticalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Height)));
        }

        public (bool shouldDraw, Point p1, Point p2) ConvertWorldLineToScreenPoints(
            IScreenBuffer buffer, 
            Vector3 vector1, 
            Vector3 vector2)
        {
            var point1Result = ConvertWorldPointToScreenDegrees(vector1);
            var point2Result = ConvertWorldPointToScreenDegrees(vector2);

            var point1HorizontalDegrees = point1Result.horizontalDegrees;
            var point1VerticalDegrees = point1Result.verticalDegrees;
            var point2HorizontalDegrees = point2Result.horizontalDegrees;
            var point2VerticalDegrees = point2Result.verticalDegrees;

            if (point1Result.isFacing && point1Result.isInView && !point2Result.isInView && !point2Result.isFacing && vector2.X < 0)
            {
                point2HorizontalDegrees += 360;
            }
            if (point2Result.isFacing && point2Result.isInView && !point1Result.isInView && !point1Result.isFacing && vector1.X > 0)
            {
                point1HorizontalDegrees -= 360;
            }
            

            var point1 = new Point(
                (int)(((point1HorizontalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Width), 
                (int)(((-point1VerticalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Height));
            var point2 = new Point(
                (int)(((point2HorizontalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Width),
                (int)(((-point2VerticalDegrees / _settings.FieldOfView) + 0.5f) * buffer.Height));


            float cameraDistanceTo1 = vector1.Length();
            float cameraDistanceTo2 = vector2.Length();

            bool isCloseEnough = Math.Min(cameraDistanceTo1, cameraDistanceTo2) < _settings.MaxClippingDistance;
            bool isFarEnough = Math.Min(cameraDistanceTo1, cameraDistanceTo2) > _settings.MinClippingDistance;

            var showBoth = isCloseEnough && isFarEnough && (
                point1Result.isInView ||
                point2Result.isInView || (point1Result.isFacing && point2Result.isFacing));

            return (showBoth, point1, point2);
        }
    }
}
