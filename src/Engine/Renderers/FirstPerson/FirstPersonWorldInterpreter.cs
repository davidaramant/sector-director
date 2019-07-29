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
        private FirstPersonCameraSettings Settings;
        
        public FirstPersonWorldInterpreter()
        {
            Settings = new FirstPersonCameraSettings
            {
                FieldOfView = 90,
                MinClippingDistance = 5,
                MaxClippingDistance = 500
            };
        }
        public FirstPersonWorldInterpreter(FirstPersonCameraSettings settings)
        {
            Settings = settings;
        }

        public void SetFieldOfView(float newFieldOfView)
        {
            Settings.FieldOfView = newFieldOfView;
        }
        
        public (bool isInView, bool isFacing, float horizontalDegrees, float verticalDegrees) ConvertWorldPointToScreenDegrees(
            Vector3 cameraPosition,
            Vector2 cameraDirection,
            Vector3 point)
        {

            // HORIZONTAL CALCULATIONS 
            Vector2 rightBoundDirection = cameraDirection.Rotate(-0.5f * Settings.FieldOfView * MathShortcuts.DegToRad);
            Vector2 leftBoundDirection = cameraDirection.Rotate(0.5f * Settings.FieldOfView * MathShortcuts.DegToRad);

            Vector2 targetedVectorDirection = Vector2.Normalize(point.ToVector2() - cameraPosition.ToVector2());

            float leftAngleDiff = (MathShortcuts.AngleBetween(leftBoundDirection, targetedVectorDirection) * MathShortcuts.RadToDeg);
            float rightAngleDiff = (MathShortcuts.AngleBetween(rightBoundDirection, targetedVectorDirection) * MathShortcuts.RadToDeg);

            // Since AngleBetween doesn't give a sign, this calculates it ourselves
            if (rightAngleDiff > Settings.FieldOfView)
            {
                leftAngleDiff *= -1;
            }
            if (leftAngleDiff > Settings.FieldOfView)
            {
                rightAngleDiff *= -1;
            }

            // We only want to not show if we have our back to this object
            bool isInHorizontalView = rightAngleDiff < Settings.FieldOfView && leftAngleDiff < Settings.FieldOfView;
            bool isFacing = Math.Abs(rightAngleDiff) < Settings.FieldOfView || Math.Abs(leftAngleDiff) < Settings.FieldOfView;

            // VERTICAL CALCULATIONS 

            // Very simplified bc im lazy. We can redo this later if someone wants to figure out the headache of Vector3 rotation
            float vertDistance = point.Z - cameraPosition.Z;
            
            float horizDistance = Vector2.Distance(point.ToVector2(), cameraPosition.ToVector2());
            float planeDistance = (float)Math.Sqrt(Math.Pow(horizDistance, 2) + Math.Pow(Settings.EyeWidth, 2));

            float viewAngle = (float)(Math.Atan(vertDistance / planeDistance) * MathShortcuts.RadToDeg);
            
            bool isInVerticalView = Math.Abs(viewAngle) < Settings.FieldOfView;

            return (isInHorizontalView && isInVerticalView, isFacing, leftAngleDiff, (Settings.FieldOfView / 2) - viewAngle);

        }
        public (bool shouldDraw, Point p1) ConvertWorldPointToScreenPoint(
            ScreenBuffer buffer,
            Vector3 cameraPosition,
            Vector2 cameraDirection,
            Vector3 point)
        {
            var (isInView, isFacing, horizontalDegrees, verticalDegrees) = ConvertWorldPointToScreenDegrees(cameraPosition, cameraDirection, point);
            return (isInView, new Point(
                (int)((horizontalDegrees / Settings.FieldOfView) * buffer.Width), 
                (int)((verticalDegrees / Settings.FieldOfView) * buffer.Height)));
        }

        public (bool shouldDraw, Point p1, Point p2) ConvertWorldLineToScreenPoints(
            ScreenBuffer buffer, 
            Vector3 cameraPosition,
            Vector2 cameraDirection,
            Vector3 vector1, 
            Vector3 vector2)
        {
            var point1Result = ConvertWorldPointToScreenDegrees(cameraPosition, cameraDirection, vector1);
            var point2Result = ConvertWorldPointToScreenDegrees(cameraPosition, cameraDirection, vector2);

            var point1HorizontalDegrees = point1Result.horizontalDegrees;
            var point1VerticalDegrees = point1Result.verticalDegrees;
            var point2HorizontalDegrees = point2Result.horizontalDegrees;
            var point2VerticalDegrees = point2Result.verticalDegrees;

            // This is some bullshit where we can be not facing a point offscreen, 
            // Causing the angle to go around our backs and connect the opposite direction.
            bool leftToRightHorizontal = ((vector2.X - vector1.X) * (cameraPosition.Y - vector1.Y) - (vector2.Y - vector1.Y) * (cameraPosition.X - vector1.X) < 0);
            if (leftToRightHorizontal && point1Result.isInView && point1Result.isFacing && !point2Result.isFacing && !point2Result.isInView)
            {
                point2HorizontalDegrees *= -1;
            }
            if (!leftToRightHorizontal && point2Result.isInView && point2Result.isFacing && !point1Result.isFacing && !point1Result.isInView)
            {
                point1HorizontalDegrees *= -1;
            }

            var point1 = new Point(
                (int)((point1HorizontalDegrees / Settings.FieldOfView) * buffer.Width), 
                (int)((point1VerticalDegrees / Settings.FieldOfView) * buffer.Height));
            var point2 = new Point(
                (int)((point2HorizontalDegrees / Settings.FieldOfView) * buffer.Width), 
                (int)((point2VerticalDegrees / Settings.FieldOfView) * buffer.Height));


            float cameraDistanceTo1 = Vector3.Distance(cameraPosition, vector1);
            float cameraDistanceTo2 = Vector3.Distance(cameraPosition, vector2);

            bool isCloseEnough = Math.Min(cameraDistanceTo1, cameraDistanceTo2) < Settings.MaxClippingDistance;
            bool isFarEnough = Math.Min(cameraDistanceTo1, cameraDistanceTo2) > Settings.MinClippingDistance;

            var showBoth = isCloseEnough && isFarEnough && (
                point1Result.isInView ||
                point2Result.isInView ||
                MathShortcuts.Intersects(cameraPosition.ToVector2(), cameraPosition.ToVector2() + (cameraDirection * Settings.MaxClippingDistance), vector1.ToVector2(), vector2.ToVector2()));

            return (showBoth, point1, point2);
        }
    }
}
