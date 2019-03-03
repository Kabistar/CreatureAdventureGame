using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCreatureGame
{
    public enum ProjectionDirection
    {
        WORLDTOSCREEN,
        SCREENTOWORLD
    }

    public class Camera
    {
        protected float zoom;
        protected Vector2 position;
        protected Vector2 origin;
        protected float rotation;

        public Camera()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            origin = Vector2.Zero;
            position = Vector2.Zero;
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public Matrix GetTransformation()
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0)) *
                   Matrix.CreateTranslation(new Vector3(origin, 0)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)); ;
        }

        public Vector2 ProjectCoordinates(Vector2 positionToConvert, ProjectionDirection direction)
        {
            if (direction == ProjectionDirection.SCREENTOWORLD)
                return Vector2.Transform(new Vector2(positionToConvert.X, positionToConvert.Y), Matrix.Invert(GetTransformation()));
            else if (direction == ProjectionDirection.WORLDTOSCREEN)
                return Vector2.Transform(new Vector2(positionToConvert.X, positionToConvert.Y), GetTransformation());
            else
                throw new ArgumentException("Projection Direction provided is not valid.");
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
