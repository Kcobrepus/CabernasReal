using Microsoft.Xna.Framework;

namespace CabernasReal.Classes
{
    public class Enemy
    {
        private Vector2 position;

        private float speed = 100f;

        private float startY;
        private float range = 150f;

        private int direction = 1;

        public Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        public Vector2 Position => position;

        public Enemy(float x, float y)
        {
            position = new Vector2(x * 64, y * 64);

            startY = position.Y;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.Y += speed * direction * dt;

            if (position.Y > startY + range)
                direction = -1;

            if (position.Y < startY - range)
                direction = 1;
        }
    }
}