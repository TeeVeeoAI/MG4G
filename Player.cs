using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MG4G
{
    public class Player
    {
        // Basic
        private Texture2D texture;
        private Vector2 position;
        private Rectangle hitbox;
        private Vector2 velocity;
        private SpriteEffects spriteEffects;

        // Keyboard Controls
        private KeyboardState newState;
        private KeyboardState oldState;
        private Keys left;
        private Keys right;
        private Keys up;
        private Keys shootL;
        private Keys shootR;
        private Keys steal;

        // Physics
        private float gravity = 0.6f;
        private float jumpStrength = -20f;
        private Vector2 shootStrength = new Vector2(6f, -25f);
        private bool isGrounded;
        private float groundY = 1080 - 350;

        // Ball
        private Ball ball;
        private bool hasBall;
        private bool shootB;
        private Vector2 startPos;

        public Rectangle Hitbox { get => hitbox; }
        public Vector2 Position { get => position; }
        public bool HasBall { get => hasBall; set => hasBall = value; }
        public bool ShootB { get => shootB; set => shootB = value; }

        public Player(Texture2D texture, Vector2 position, Keys left, Keys right, Keys up, Keys shootL, Keys shootR, Keys steal, Ball ball)
        {
            this.texture = texture;
            this.position = position;
            this.startPos = position;
            this.left = left;
            this.right = right;
            this.up = up;
            this.shootL = shootL;
            this.shootR = shootR;
            this.steal = steal;
            this.ball = ball;

            hitbox = new Rectangle((int)position.X, (int)position.Y, 100, 300);

            isGrounded = false;
            hasBall = false;
            shootB = false;
        }

        public void Move(GameTime gameTime){
            // Get keyboard state
            newState = Keyboard.GetState();

            // Move Left
            if (newState.IsKeyDown(left) && position.X >= 0 && isGrounded){
                velocity.X = -4;
            }

            // Move Right
            if (newState.IsKeyDown(right) && position.X <= 1920 - hitbox.Width && isGrounded){
                velocity.X = 4;
            }

            // Stop Movement if both keys are pressed or released
            if (newState.IsKeyDown(left) && newState.IsKeyDown(right) && isGrounded || 
                newState.IsKeyUp(left) && oldState.IsKeyDown(left) && isGrounded ||
                newState.IsKeyUp(right) && oldState.IsKeyDown(right) && isGrounded ||
                newState.IsKeyUp(left) && newState.IsKeyUp(right) && isGrounded)
            {
                velocity.X = 0;
            }

            // Jumping
            if (newState.IsKeyDown(up) && oldState.IsKeyUp(up) && isGrounded){
                velocity.Y = jumpStrength; // Apply jump force
                isGrounded = false;
            }

            // Shooting
            if (newState.IsKeyDown(shootL) && oldState.IsKeyUp(shootL) && hasBall){
                ball.VelocityX = shootStrength.X + velocity.X;
                ball.VelocityY = shootStrength.Y;
                Shoot(gameTime);
            }
            if (newState.IsKeyDown(shootR) && oldState.IsKeyUp(shootR) && hasBall){
                ball.VelocityX = -shootStrength.X + velocity.X;
                ball.VelocityY = shootStrength.Y;
                Shoot(gameTime);
            }

            oldState = newState;
        }

        public void ApplyPhysics(GameTime gameTime){
            if (!isGrounded)
            {
                velocity.Y += gravity;
                if (position.X >= 1920 - hitbox.Width ||
                    position.X <= 0 )
                {
                    velocity.X = 0;
                }
            }

            position += velocity;

            // Ground collision detection
            if (position.Y >= groundY)
            {
                position.Y = groundY;
                velocity.Y = 0;
                isGrounded = true;
            }

            hitbox.Location = position.ToPoint();
        }

        public void Shoot(GameTime gameTime){
            ball.VelocityX = ball.Velocity.X;
            ball.VelocityY = ball.Velocity.Y;
            hasBall = false;
        }

        public bool Steal(GameTime gameTime){
            if (newState.IsKeyDown(steal)){
                return true;
            } else {
                return false;
            }
        }

        public void Dunk(GameTime gameTime){
            hasBall = false;
        }

        public void Reset(GameTime gameTime){
            position = startPos;
        }

        public void Update(GameTime gameTime){
            newState = Keyboard.GetState();
            Move(gameTime);
            ApplyPhysics(gameTime);
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