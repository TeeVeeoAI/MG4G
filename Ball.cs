using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;

namespace MG4G
{
    public class Ball
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        private Vector2 velocity;
        private float rotationAngle;
        private Player player1;
        private Player player2;

        public Player Player1{ set => player1 = value; }
        public Player Player2{ set => player2 = value; }
        public Rectangle Hitbox{ get => hitbox; }
        public Vector2 Velocity{ get => velocity; }
        public float VelocityX{ get => velocity.X; set => velocity.X = value; }
        public float VelocityY{ get => velocity.Y; set => velocity.Y = value; }
        public Vector2 Position{ get => position; }

        public Ball(Texture2D texture, Vector2 position){
            this.texture = texture;
            this.position = position;

            hitbox = new Rectangle((int)position.X, (int)position.Y, 50, 50);

            velocity.X = 4;
            velocity.Y = 4;
        }

        public Player WhoHasTheBall(GameTime gameTime){
            Player player = null;
            if (player1.HasBall){
                position.X = player1.Position.X+100f-25f;
                position.Y = player1.Position.Y+150f-10f;
                player = player1;
            }
            else if (player2.HasBall){
                position.X = player2.Position.X+100f-25f;
                position.Y = player2.Position.Y+150f-10f;
                player = player2;
            }

            return player;
        }

        public void SpinTheBall(GameTime gameTime){
            rotationAngle += (float)gameTime.ElapsedGameTime.TotalSeconds * 5f;
            float circle = MathHelper.Pi * 2;
            rotationAngle %= circle;
        }

        public void Move(GameTime gameTime){
            position.X += velocity.X;
            position.Y -= velocity.Y;

            if(position.Y <= 20 || position.Y >= 1080-20-50){
                velocity.Y = velocity.Y*-1;
            }

            if(position.X <= 20 || position.X >= 1920-20){
                velocity.X = velocity.X*-1;
            }
        }

        public void Update(GameTime gameTime){
            SpinTheBall(gameTime);
            if (WhoHasTheBall(gameTime) == null){
                Move(gameTime);
            }
            
            hitbox.Location = position.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, null, Color.White, rotationAngle, new Vector2(texture.Width/2, texture.Height/2), SpriteEffects.None, 0f);
        }
    }
}