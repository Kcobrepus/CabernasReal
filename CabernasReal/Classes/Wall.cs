using Microsoft.Xna.Framework;

namespace CabernasReal.Classes
{
    public class Wall
    {
        public Vector2 Position;

        public Rectangle Bounds =>
            new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                64,
                64
            );

        public Wall(int x, int y)
        {
            Position = new Vector2(x * 64, y * 64);
        }
    }
}