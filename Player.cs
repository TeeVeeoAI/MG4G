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
        private float velocityY = 0;
        private KeyboardState newState;
        private KeyboardState oldState;
        private Keys left;
        private Keys right;
        private Keys up;

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right, Keys up){
            this.texture = texture;
            this.position = position;
            this.left = left;
            this.right = right;
            this.up = up;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 50, 150);
        }

        public void Move(GameTime gameTime){
            if(newState.IsKeyDown(left) && position.X >= 0){
                position.X -= velocityX*1.1f;
            }
            if(newState.IsKeyDown(right) && position.X <= 1920-50){
                position.X += velocityX*1.1f;
            }
            if(newState.IsKeyDown(up) && oldState.IsKeyDown(up)){
                Jump(gameTime);
            }

            hitbox.Location = position.ToPoint();
        }

        public void Jump(GameTime gameTime){
            
        }

        public void Update(GameTime gameTime){
            newState = Keyboard.GetState();
            Move(gameTime);

            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Color.White);
        }
    }
}