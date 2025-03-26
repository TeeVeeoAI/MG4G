using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG4G
{
    public class Hoop
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;

        public Texture2D Texture{
            get{ return texture; }
            set{ texture = value; }
        }

        public Vector2 Position{
            get{ return position; }
            set{ position = value; }
        }

        public Rectangle Hitbox{ get => hitbox; set => hitbox = value; }

        public Hoop(Texture2D texture, Vector2 position){
            this.texture = texture;
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 150, 150);
        }

        public void Update(GameTime gameTime){
            
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, Color.White);
        }

    }
}