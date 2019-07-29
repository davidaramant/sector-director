// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Renderers
{
    public sealed class Camera2D
    {
        private Matrix _worldToScreenTransformation = Matrix.Identity;
        private Matrix _screenToWorldTransformation = Matrix.Identity;
        private Point _screenBounds = Point.Zero;
        private Vector2 _center = Vector2.Zero;
        private Vector2 _viewOffset = Vector2.Zero;
        private float _rotationInRadians = 0;
        private float _zoom = 1;
        private bool _matrixStale;

        public Matrix WorldToScreenTransformation
        {
            get
            {
                UpdateMatricesIfStale();
                return _worldToScreenTransformation;
            }
        }
        public Matrix ScreenToWorldTransformation
        {
            get
            {
                UpdateMatricesIfStale();
                return _screenToWorldTransformation;
            }
        }

        public Point ScreenBounds
        {
            get => _screenBounds;
            set
            {
                if (_screenBounds != value)
                {
                    _screenBounds = value;
                    _matrixStale = true;
                }
            }
        }

        public float RotationInRadians
        {
            get => _rotationInRadians;
            set
            {
                if (_rotationInRadians != value)
                {
                    _rotationInRadians = value;
                    _matrixStale = true;
                }
            }
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    _matrixStale = true;
                }
            }
        }

        /// <summary>
        /// What the camera is pointed to (in world coordinates) relative to the center
        /// </summary>
        public Vector2 ViewOffset
        {
            get => _viewOffset;
            set
            {
                if (_viewOffset != value)
                {
                    _viewOffset = value;
                    _matrixStale = true;
                }
            }
        }

        public Vector2 Center
        {
            get => _center;
            set
            {
                if (_center != value)
                {
                    _center = value;
                    _matrixStale = true;
                }
            }
        }

        public Vector2 ScreenToWorld(Vector2 screenCoordinate) => Vector2.Transform(screenCoordinate, ScreenToWorldTransformation);
        public Vector2 WorldToScreen(Vector2 worldCoordinate) => Vector2.Transform(worldCoordinate, WorldToScreenTransformation);

        private void UpdateMatricesIfStale()
        {
            if (_matrixStale)
            {
                var yAxisFlipMatrix = Matrix.CreateScale(1, -1, 0);
                var originTranslationMatrix = Matrix.CreateTranslation(_screenBounds.X / 2, _screenBounds.Y / 2, 0);
                var centerTranslationMatrix = Matrix.CreateTranslation(-_center.X + 0.5f, _center.Y - 0.5f, 0);
                var centerOffsetTranslationMatrix = Matrix.CreateTranslation(-_viewOffset.X, _viewOffset.Y, 0);
                var zoomMatrix = Matrix.CreateScale(_zoom, _zoom, 0);
                var rotationMatrix = Matrix.CreateRotationZ(_rotationInRadians);

                _worldToScreenTransformation =
                    yAxisFlipMatrix *
                    centerTranslationMatrix * 
                    rotationMatrix *
                    centerOffsetTranslationMatrix *
                    zoomMatrix *
                    originTranslationMatrix;

                _screenToWorldTransformation = Matrix.Invert(_worldToScreenTransformation);

                _matrixStale = false;
            }
        }
    }
}