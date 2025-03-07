using System.Security;
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
        private Vector2 velocity;
        private SpriteEffects spriteEffects;


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


        public Rectangle Hitbox{ get => hitbox; }
        public Vector2 Position{ get => position; }
        public bool HasBall{ get => hasBall; set => hasBall = value; }
        public bool ShootB{ get =>shootB; set => shootB = value; }

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
                velocity.X = -4;
            }
            
            //Move.Right
            if(newState.IsKeyDown(right) && position.X <= 1920-hitbox.Width && !jump){
                velocity.X = 4;
            }

            if (newState.IsKeyDown(left) && newState.IsKeyDown(right)){
                velocity.X = 0;
            }
            
            //Move.Stop
            if(((newState.IsKeyUp(left) && oldState.IsKeyDown(left) && position.X >= 0)
                || (newState.IsKeyUp(right) && oldState.IsKeyDown(right) && position.X <= 1920-hitbox.Width)) && !jump){
                velocity.X = 0;
            }

            if(position.X <= 0 || position.X >= 1920-hitbox.Width){
                position.X += velocity.X*2;
                velocity.X = 0;
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

            position.X += velocity.X*1.1f;

            hitbox.Location = position.ToPoint();
        }

        public void Jump(GameTime gameTime){
            if(jump){
                float velocityXB = velocity.X;
                float bVelocityY = 4;

                velocity.Y = jumpHeight*3/100-1 - (gameTime.TotalGameTime.Seconds - jumpStartTime);
                velocity.X = velocityXB * 0.99f;

                if (hasBall)
                    ball.VelocityY = bVelocityY + velocity.Y/2;

                if (position.X >= 1920-hitbox.Width || position.X <= 0){
                    velocity.X = 0;
                }
                
                if(position.Y <= prevPos.Y - jumpHeight){
                    hitTop = true;
                }

                if(hitTop){
                    velocity.Y = velocity.Y*-1;
                    if (hasBall)
                        ball.VelocityY = bVelocityY - velocity.Y/2;
                }

                position.Y -= velocity.Y*1.1f;
            }
            
            if (position.Y > 1080-hitbox.Height-50 && hitTop){
                velocity.Y = 0;
                jump = false;
                hitTop = false;
                velocity.X = 0;
                if (hasBall)
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
            if (velocity.X > 0){
                spriteEffects = SpriteEffects.None;
            } else if (velocity.X < 0){
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, hitbox, null, Color.White, 0, new Vector2(0, 0), spriteEffects, 0);
        }
    }
}