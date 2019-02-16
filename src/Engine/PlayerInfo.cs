// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Microsoft.Xna.Framework;

namespace SectorDirector.Engine
{
    public sealed class PlayerInfo
    {
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public int Radius { get; } = 10;

        public PlayerInfo(Point position, float rotationAngleRadians)
        {
            Position = new Vector2(position.X, position.Y);
            Direction = new Vector2(1, 0);
            Rotate(rotationAngleRadians);
        }

        //public void Update(MapData mapData, MovementInputs inputs, GameTime gameTime)
        //{
        //    var moveSpeed = 5.0f * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        //    var rotSpeed = 3.0f * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

        //    if (inputs.HasFlag(MovementInputs.Forward))
        //    {
        //        Move(mapData, Direction, moveSpeed);
        //    }
        //    else if (inputs.HasFlag(MovementInputs.Backward))
        //    {
        //        var direction = new Vector2 { X = -Direction.X, Y = -Direction.Y };

        //        Move(mapData, direction, moveSpeed);
        //    }
        //    if (inputs.HasFlag(MovementInputs.StrafeLeft))
        //    {
        //        var direction = new Vector2 { X = Direction.Y, Y = -Direction.X };

        //        Move(mapData, direction, moveSpeed);
        //    }
        //    else if (inputs.HasFlag(MovementInputs.StrafeRight))
        //    {
        //        var direction = new Vector2 { X = -Direction.Y, Y = Direction.X };

        //        Move(mapData, direction, moveSpeed);
        //    }

        //    if (inputs.HasFlag(MovementInputs.TurnRight))
        //    {
        //        Rotate(rotSpeed);
        //    }
        //    else if (inputs.HasFlag(MovementInputs.TurnLeft))
        //    {
        //        Rotate(-rotSpeed);
        //    }
        //}

        //public void Move(MapData mapData, Vector2 direction, float speed)
        //{
        //    // Should MapData be passed in here?  Feels odd...
        //    var movement = direction * speed;
        //    var newPosition = Position + movement;
        //    var newBoundingEdge = newPosition + direction * _radius;

        //    if (mapData.IsPassable((int)newBoundingEdge.X, (int)Position.Y))
        //    {
        //        Position.X = newPosition.X;
        //    }
        //    if (mapData.IsPassable((int)Position.X, (int)newBoundingEdge.Y))
        //    {
        //        Position.Y = newPosition.Y;
        //    }
        //}

        public void Rotate(float rotationRadians)
        {
            var rotation = Matrix.CreateRotationZ(rotationRadians);

            Direction = Vector2.Transform(Direction, rotation);
        }
    }
}