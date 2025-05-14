using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG4G
{
    public class Ball
    {
        private Texture2D texture;
        private Vector2 position, velocity;
        private Rectangle hitbox;
        private float rotationAngle;
        private Player player1, player2;
        // Gravity Variables
        private float gravity = 0.6f;
        private float bounceFactor = 0.7f; // Reduces velocity on bounce
        private float groundY = 1080 - 70; // Approximate ground level

        public Player Player1 { set => player1 = value; }
        public Player Player2 { set => player2 = value; }
        public Rectangle Hitbox { get => hitbox; }
        public Vector2 Velocity { get => velocity; }
        public float VelocityX { set => velocity.X = value; }
        public float VelocityY { set => velocity.Y = value; }
        public Vector2 Position { get => position; }

        public Ball(Texture2D texture, Vector2 position){
            this.texture = texture;
            this.position = position;

            hitbox = new Rectangle((int)position.X, (int)position.Y, 50, 50);

            velocity.X = 4;
            velocity.Y = 0;
        }

        public Player WhoHasTheBall(GameTime gameTime){
            Player player = null;
            if (player1.HasBall){
                position.X = player1.Position.X + 75f;
                position.Y = player1.Position.Y + 140f;
                player = player1;
            }
            else if (player2.HasBall){
                position.X = player2.Position.X + 75f;
                position.Y = player2.Position.Y + 140f;
                player = player2;
            }
            return player;
        }

        public void ApplyPhysics(GameTime gameTime){
            // Apply gravity if not held by a player
            if (WhoHasTheBall(gameTime) == null){
                velocity.Y += gravity;

                // Apply movement
                position += velocity;

                // Collision with ground
                if (position.Y >= groundY){
                    position.Y = groundY;
                    velocity.Y *= -bounceFactor; // Bounces back with reduced speed

                    // Stop bouncing if velocity is too low
                    if (velocity.Y < 0.5f && velocity.Y > -0.5f){
                        velocity.Y = 0;
                        velocity.X = 0;
                    }
                }
            }

            // Update hitbox
            hitbox.Location = position.ToPoint();
        }

        public void SpinTheBall(GameTime gameTime){
            rotationAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * 5f;
            float circle = MathHelper.Pi * 2;
            rotationAngle %= circle;
        }

        public void Move(GameTime gameTime){
            if (WhoHasTheBall(gameTime) == null){
                position.X += velocity.X;

                // Bounce off walls
                if (position.X <= 20 || position.X >= 1920 - 30){
                    velocity.X *= -1f;
                }
            }
        }

        public void Update(GameTime gameTime){
            Move(gameTime);
            ApplyPhysics(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, null, Color.White, rotationAngle, new Vector2(texture.Width/2, texture.Height/2), SpriteEffects.None, 0f);
        }
    }
}
