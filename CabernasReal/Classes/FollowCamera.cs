using Microsoft.Xna.Framework;

namespace CabernasReal.Classes
{
    public class FollowCamera
    {
        public Vector2 Position;

        public FollowCamera(Vector2 position)
        {
            Position = position;
        }

        public void Follow(Vector2 targetPosition, Vector2 screenSize)
        {
            Position = new Vector2(
                -targetPosition.X + (screenSize.X / 2),
                Position.Y = 0
            );
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(
                new Vector3(Position, 0)
            );
        }
    }
}
