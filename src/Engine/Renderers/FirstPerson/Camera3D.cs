// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Microsoft.Xna.Framework;

namespace SectorDirector.Engine.Renderers
{
    public sealed class Camera3D
    {
        private Matrix _worldToPerspectiveTransformation = Matrix.Identity;
        private Matrix _perspectiveToWorldTransformation = Matrix.Identity;
        private Vector3 _center = Vector3.Zero;
        private float _rotationInRadians = 0;
        private bool _matrixStale;

        public Matrix WorldToPerspectiveTransformation
        {
            get
            {
                UpdateMatricesIfStale();
                return _worldToPerspectiveTransformation;
            }
        }
        public Matrix PerspectiveToWorldTransformation
        {
            get
            {
                UpdateMatricesIfStale();
                return _perspectiveToWorldTransformation;
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

        public Vector3 Center
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

        public Vector3 PerspectiveToWorld(Vector3 screenCoordinate) => Vector3.Transform(screenCoordinate, PerspectiveToWorldTransformation);
        public Vector3 WorldToPerspective(Vector3 worldCoordinate) => Vector3.Transform(worldCoordinate, WorldToPerspectiveTransformation);

        private void UpdateMatricesIfStale()
        {
            if (_matrixStale)
            {
                var centerTranslationMatrix = Matrix.CreateTranslation(-_center.X, -_center.Y, -_center.Z);
                var rotationMatrix = Matrix.CreateRotationZ(-_rotationInRadians);

                _worldToPerspectiveTransformation =
                    centerTranslationMatrix *
                    rotationMatrix;

                _perspectiveToWorldTransformation = Matrix.Invert(_worldToPerspectiveTransformation);

                _matrixStale = false;
            }
        }
    }
}