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
        private KeyboardState newState;
        private KeyboardState oldState;
        private Keys left;
        private Keys right;

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right){
            this.texture = texture;
            this.position = position;
            this.left = left;
            this.right = right;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 50, 150);
        }

        public void Move(){
            if(newState.IsKeyDown(left) && position.X >= 0){
                position.X -= 2;
            }
            if(newState.IsKeyDown(right) && position.X <= 1920){
                position.X += 2;
            }

            hitbox.Location = position.ToPoint();
        }

        public void Update(GameTime gameTime){
            newState = Keyboard.GetState();
            Move();

            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Color.White);
        }
    }
}