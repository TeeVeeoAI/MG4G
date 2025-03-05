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
        private Keys shootL;
        private Keys shootR;


        //Jump
        private bool jump;
        private Vector2 prevPos;
        private bool hitTop;
        private float jumpStartTime;
        int jumpHeight;


        //Ball
        private Ball ball;
        private bool hasBall;
        private bool shootB;


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

        public bool ShootB{
            get{ return shootB; }
            set{ shootB = value; }
        }

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right, Keys up, Keys shootL, Keys shootR, Ball ball){
            this.texture = texture;
            this.position = position;
            this.left = left;
            this.right = right;
            this.up = up;
            this.shootL = shootL;
            this.shootR = shootR;
            this.ball = ball;

            hitbox = new Rectangle((int)position.X, (int)position.Y, 100, 300);

            hitTop = false;
            hasBall = false;
            jump = false;
            shootB = false;
            jumpHeight = 400;
        }

        public void Move(GameTime gameTime){

            //Move.Left
            if(newState.IsKeyDown(left) && position.X >= 0 && !jump){
                velocityX = -4;
            }
            
            //Move.Right
            if(newState.IsKeyDown(right) && position.X <= 1920-hitbox.Width && !jump){
                velocityX = 4;
            }

            if (newState.IsKeyDown(left) && newState.IsKeyDown(right)){
                velocityX = 0;
            }
            
            //Move.Stop
            if(((newState.IsKeyUp(left) && oldState.IsKeyDown(left) && position.X >= 0) || (newState.IsKeyUp(right) && oldState.IsKeyDown(right) && position.X <= 1920-hitbox.Width)) && !jump){
                velocityX = 0;
            }

            //Move.Up
            if(newState.IsKeyDown(up) && oldState.IsKeyUp(up) && !jump){
                jump = true;
                prevPos = position;
                jumpStartTime = gameTime.TotalGameTime.Seconds;
            }

            //Shoot
            if(newState.IsKeyDown(shootL) && oldState.IsKeyUp(shootL) && hasBall){
                ball.VelocityX = 4;
                Shoot(gameTime);
            }
            if(newState.IsKeyDown(shootR) && oldState.IsKeyUp(shootR) && hasBall){
                ball.VelocityX = -4;
                Shoot(gameTime);
            }

            position.X += velocityX*1.1f;

            hitbox.Location = position.ToPoint();
        }

        public void Jump(GameTime gameTime){
            if(jump){
                float velocityXB = velocityX;
                float bVelocityY = 4;

                velocityY = jumpHeight*3/100-1 - (gameTime.TotalGameTime.Seconds - jumpStartTime);
                velocityX = velocityXB * 0.99f;

                if (hasBall)
                    ball.VelocityY = bVelocityY + velocityY/2;

                if (position.X >= 1920-hitbox.Width || position.X <= 0){
                    velocityX = 0;
                }
                
                if(position.Y <= prevPos.Y - jumpHeight){
                    hitTop = true;
                }

                if(hitTop){
                    velocityY = velocityY*-1;
                    if (hasBall)
                        ball.VelocityY = bVelocityY - velocityY/2;
                }

                position.Y -= velocityY*1.1f;
            }
            
            if (position.Y > 1080-hitbox.Height-50 && hitTop){
                velocityY = 0;
                jump = false;
                hitTop = false;
                velocityX = 0;
                ball.VelocityY = 4;
            }
        }

        public void Shoot(GameTime gameTime){
            ball.VelocityX = ball.VelocityX;
            ball.VelocityY = ball.VelocityY;
            hasBall = false;
        }

        public void Dunk(GameTime gameTime){
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