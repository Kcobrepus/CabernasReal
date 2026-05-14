using Microsoft.Xna.Framework;

namespace CabernasReal.Classes
{
    public class Spike
    {
        private Vector2 position;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Spike(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

        }

        public void Update(GameTime gameTime)
        {

        }
    }
}