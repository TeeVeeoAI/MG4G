using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG4G
{
    public class Player
    {


        //basic
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        private float velocityX;
        private float velocityY;


        //keyS and keyboardS
        private KeyboardState newState;
        private KeyboardState oldState;
        private Keys left;
        private Keys right;
        private Keys up;
        private Keys shoot;


        //Jump
        private bool jump;
        private Vector2 prevPos;
        private bool hitTop;


        //Ball
        private Ball ball;
        private bool hasBall;


        public Rectangle Hitbox{
            get{ return hitbox; }
        }
        public Vector2 Position{
            get{ return position; }
        }
        public bool HasBall{
            get{ return hasBall; }
            set{ hasBall = value; }
        }

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right, Keys up, Keys shoot, Ball ball){
            this.texture = texture;
            this.position = position;
            this.left = left;
            this.right = right;
            this.up = up;
            this.shoot = shoot;
            this.ball = ball;

            hitbox = new Rectangle((int)position.X, (int)position.Y, 100, 300);

            hitTop = false;
            hasBall = false;
            jump = false;
            velocityX = 4;
            velocityY = 4;
        }

        public void Move(GameTime gameTime){
            if(newState.IsKeyDown(left) && position.X >= 0){
                position.X -= velocityX*1.1f;
            }
            if(newState.IsKeyDown(right) && position.X <= 1920-hitbox.Width){
                position.X += velocityX*1.1f;
            }
            if(newState.IsKeyDown(up) && oldState.IsKeyUp(up) && !jump){
                jump = true;
                prevPos = position;
            }
            if(newState.IsKeyDown(shoot) && oldState.IsKeyUp(shoot) && hasBall){
                Shoot(gameTime);
            }

            hitbox.Location = position.ToPoint();
        }

        public void Jump(GameTime gameTime){
            if(jump){
                if(position.Y <= prevPos.Y - 300){
                    velocityY = velocityY*-1;
                    hitTop = true;
                }

                position.Y -= velocityY*1.1f;
            }
            if (position.Y > 1080-hitbox.Height-50 && hitTop){
                velocityY = velocityY*-1;
                jump = false;
                hitTop = false;
            }
        }

        public void Shoot(GameTime gameTime){
            ball.VelocityX = ball.VelocityX*-1;
            hasBall = false;
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