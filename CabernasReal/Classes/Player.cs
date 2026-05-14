using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CabernasReal.Classes
{
    internal class Player
    {
        private Game1 game; //reference from Game1 to Player

        private float moveSpeed = 500f; // pixels per second
        private Vector2 velocity;
        private float gravity = 1100f;
        private float jumpForce = -450f;
        private bool isOnGround = false;

        private bool powerUp = false;
        private bool doubleJump = false;
        private bool hasJumped = true;

        private Rectangle Bounds =>
    new Rectangle(
        (int)position.X,
        (int)position.Y,
        64,
        64
    );

        // Current player position in the matrix (multiply by tileSize prior to drawing)

        private Vector2 position;
        public Vector2 Position => position;



        public Player(Game1 game, float x, float y) // constructor que dada a as posições guarda a sua posição
        {
            this.game = game;   
            position = new Vector2(x * 64, y * 64);
        }

        private bool CollidesWithWall(float x, float y)
        {
            Rectangle future = new Rectangle((int)x, (int)y, 64, 64);

            foreach (Wall wall in game.walls)
            {
                if (future.Intersects(wall.Bounds))
                    return true;
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 direction = Vector2.Zero;

            if (kState.IsKeyDown(Keys.A) || (kState.IsKeyDown(Keys.Left))) direction.X = -1;
            if (kState.IsKeyDown(Keys.D) || (kState.IsKeyDown(Keys.Right))) direction.X = 1;

            // Horizontal velocity
            velocity.X = direction.X * moveSpeed;

            // Jump
            if (kState.IsKeyDown(Keys.Space) && isOnGround)
            {
                game.SF_jump.Play();
                velocity.Y = jumpForce;
                isOnGround = false;
                hasJumped = true;
            }

            if (kState.IsKeyUp(Keys.Space) && hasJumped)
            {
                doubleJump = false;
            }
                //Double Jump

                if (powerUp)
            {
                if (kState.IsKeyDown(Keys.Space) && doubleJump == false && isOnGround == false)
                {
                    game.SF_doublejump.Play();
                    velocity.Y = jumpForce;
                    doubleJump = true;
                    hasJumped = false;
                }
            }

            // Gravity
            velocity.Y += gravity * dt;

            // ===== X AXIS COLLISION =====
            float newX = position.X + velocity.X * dt;

            if (!CollidesWithWall(newX, position.Y))
                position.X = newX;

            // ===== Y AXIS COLLISION =====
            float newY = position.Y + velocity.Y * dt;

            if (!CollidesWithWall(position.X, newY))
            {
                position.Y = newY;
                isOnGround = false;
            }
            else
            {
                // If moving downward and hit wall → on ground
                if (velocity.Y > 0)
                    isOnGround = true;
                    doubleJump = true;

                velocity.Y = 0;
            }

            foreach (Enemy enemy in game.enemies)
            {
                if (Bounds.Intersects(enemy.Bounds))
                {
                    game.RestartLevel();
                    return;
                }
            }

            foreach (Spike spike in game.spikes)
            {
                if (Bounds.Intersects(spike.Bounds))
                {
                    game.RestartLevel();
                    return;
                }
            }

            foreach (Power power in game.powers)
            {
                if (Bounds.Intersects(power.Bounds))
                {
                    game.SF_powerup.Play();
                    game.powers.Remove(power);
                    powerUp = true;
                    isOnGround = true;
                    return;
                }
            }

            foreach (Exit exit in game.exits)
            {
                if (Bounds.Intersects(exit.Bounds))
                {
                    game.SF_powerup.Play();
                    MediaPlayer.Stop();
                    MediaPlayer.Play(game.Mus_final);
                    game.currentState = Game1.GameState.Win;
                    return;
                }
            }

        }
    }
}
