using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG4G
{
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        private float velocityX = 4;
        private float velocityY = 4;
        private KeyboardState newState;
        private KeyboardState oldState;
        private Keys left;
        private Keys right;
        private Keys up;
        private bool jump = false;
        private Vector2 prevPos;

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right, Keys up){
            this.texture = texture;
            this.position = position;
            this.left = left;
            this.right = right;
            this.up = up;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 200, 500);
        }

        public void Move(GameTime gameTime){
            if(newState.IsKeyDown(left) && position.X >= 0){
                position.X -= velocityX*1.1f;
            }
            if(newState.IsKeyDown(right) && position.X <= 1920-200){
                position.X += velocityX*1.1f;
            }
            if(newState.IsKeyDown(up) && oldState.IsKeyDown(up) && !jump){
                jump = true;
                prevPos = position;
            }

            hitbox.Location = position.ToPoint();
        }

        public void Jump(GameTime gameTime){
            if(jump){
                if(position.Y <= prevPos.Y - 300){
                    velocityY = velocityY*-1;
                }

                position.Y -= velocityY*1.1f;
            }
            if (position.Y >= prevPos.Y){
                velocityY = velocityY*-1;
                jump = false;
            }
        }

        public void Update(GameTime gameTime){
            newState = Keyboard.GetState();
            Move(gameTime);
            Jump(gameTime);

            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Color.White);
        }
    }
}