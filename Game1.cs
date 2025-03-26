using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.MediaFoundation;

namespace MG4G;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Player player1, player2;
    private Texture2D ballTexture;
    private Texture2D pixel;
    private Ball ball;
    private Texture2D hoopTextureRight, hoopTextureLeft;
    private Hoop hoopLeft, hoopRight;
    private SpriteFont font;
    private float lastShoot;
    private Score score;
    private Player lastToHaveBall;
    private Song lebron;
    private string what;
    private Vector2 where;
    private float howLong;
    private Rectangle threePLineLeft;
    private Rectangle threePLineRight;
    private bool fromThree;
    private bool[] shootHit;
    private float[] shootHitTime;
    private Rectangle[] gTending;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        //_graphics.IsFullScreen = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        ballTexture = Content.Load<Texture2D>("goat");
        playerTexture = Content.Load<Texture2D>("images");
        hoopTextureRight = Content.Load<Texture2D>("hoopRight");
        hoopTextureLeft = Content.Load<Texture2D>("hoopLeft");
        font = Content.Load<SpriteFont>("File");
        pixel = Content.Load<Texture2D>("pixel");

        ball = new Ball(ballTexture, new Vector2(1920/2-20, 1080-200-20));
        player1 = new Player(playerTexture, new Vector2((1920/2)/2-100, 1080-350), Keys.A, Keys.D, Keys.Space, Keys.E, Keys.Q, ball);
        player2 = new Player(playerTexture, new Vector2((1920/2)/2+1920/2-100, 1080-350), Keys.Left, Keys.Right, Keys.Up, Keys.PageDown, Keys.PageUp, ball);
        ball.Player1 = player1;
        ball.Player2 = player2; 
        hoopLeft = new Hoop(hoopTextureLeft, new Vector2(0, 1080 - 150/*hoop height/width*/ - 400/*the hoop height*/));
        hoopRight = new Hoop(hoopTextureRight, new Vector2(1920 - 150/*hoop height/width*/, 1080 - 150/*hoop height/width*/ - 400/*the hoop height*/));
        score = new Score(new Vector2(1920/2-50, 0), font);
        threePLineLeft = new Rectangle((int)hoopLeft.Position.X+700+hoopLeft.Hitbox.Width, 1080-100, 10, 100);
        threePLineRight = new Rectangle((int)hoopRight.Position.X-700, 1080-100, 10, 100);
        shootHit = new bool[2]{false, false};
        shootHitTime = new float[2];
        gTending = new Rectangle[2]{new Rectangle((int)hoopLeft.Position.X, (int)hoopLeft.Position.Y - hoopLeft.Hitbox.Height*2, 400, 400), new Rectangle((int)hoopRight.Position.X-250, (int)hoopRight.Position.Y - hoopRight.Hitbox.Height*2, 400, 400)};

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
        }



        // TODO: Add your update logic here

        player1.Update(gameTime);
        player2.Update(gameTime);
        ball.Update(gameTime);

        if (player1.HasBall){
            fromThree = player1.Position.X <= threePLineLeft.X ? true : false;
        }
        if (player2.HasBall){
            fromThree = player2.Position.X >= threePLineRight.X ? true : false;
        }

        if(ball.Hitbox.Intersects(player1.Hitbox) && !player1.ShootB && !player2.HasBall){
            lastShoot = gameTime.TotalGameTime.Seconds;
            player1.HasBall = true;
            player1.ShootB = true;
        } else if(ball.Hitbox.Intersects(player2.Hitbox) && !player2.ShootB && !player1.HasBall){
            lastShoot = gameTime.TotalGameTime.Seconds;
            player2.HasBall = true;
            player2.ShootB = true;
        }

        if(gameTime.TotalGameTime.Seconds >= lastShoot+3.5f){
            player2.ShootB = false;
            player1.ShootB = false;
        }

        if(player1.HasBall){
            lastToHaveBall = player1;
        } else{
            lastToHaveBall = player2;
        }

        Dunk(gameTime);

        if(gameTime.TotalGameTime.Seconds >= howLong + 3f){
            what = "";
        }

        for(int i = 0; i < 2; i++){
            if(gameTime.TotalGameTime.Seconds >= shootHitTime[i] + 3f){
                shootHit[i] = false;
            }
        }

        madeShoot(gameTime);
        GTending(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(150,200,175));

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        hoopLeft.Draw(_spriteBatch);
        hoopRight.Draw(_spriteBatch);
        _spriteBatch.Draw(pixel, threePLineLeft, Color.White);
        _spriteBatch.Draw(pixel, threePLineRight, Color.Black);
        player1.Draw(_spriteBatch);
        player2.Draw(_spriteBatch);
        ball.Draw(_spriteBatch);
        score.DrawScore(_spriteBatch);
        _spriteBatch.DrawString(font, what != null ? what : " ", where, Color.Black);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void Dunk(GameTime gameTime){

        Player player = ball.WhoHasTheBall(gameTime);

        //left dunk
        if(player != null && player.Hitbox.Intersects(hoopLeft.Hitbox) && ball.Position.Y - 20f <= hoopLeft.Position.Y){
            player.HasBall = false;
            score.UpdateScore(2, 0, gameTime);
            what = "Dunk!";
            where = hoopLeft.Position - new Vector2(-30, 100);
            howLong = gameTime.TotalGameTime.Seconds;
        }

        //right dunk
        if(player != null && player.Hitbox.Intersects(hoopRight.Hitbox) && ball.Position.Y - 20f <= hoopRight.Position.Y){
            player.HasBall = false;
            score.UpdateScore(0, 2, gameTime);
            what = "Dunk!";
            where = hoopRight.Position - new Vector2(30, 100);
            howLong = gameTime.TotalGameTime.Seconds;
        }
    }

    public void madeShoot(GameTime gameTime){

        //left hoop
        if(ball.Hitbox.Intersects(hoopLeft.Hitbox) && ball.Position.X >= 20 && ball.Position.X <= 130 && !shootHit[0] && ball.Position.Y > hoopLeft.Position.Y){
            shootHit[0] = true;
            shootHitTime[0] = gameTime.TotalGameTime.Seconds;
            where = hoopLeft.Position - new Vector2(-30, 100);
            what = fromThree ? "3 pointer" : "2 pointer";
            howLong = gameTime.TotalGameTime.Seconds;
            score.UpdateScore(fromThree ? 3 : 2, 0, gameTime);
            fromThree = false;
        }
        
        //right hoop
        if(ball.Hitbox.Intersects(hoopRight.Hitbox) && ball.Position.X <= 1980-20-50 && ball.Position.X >= 1980-130-50 && !shootHit[1] && ball.Position.Y > hoopRight.Position.Y){
            shootHit[1] = true;
            shootHitTime[1] = gameTime.TotalGameTime.Seconds;
            where = hoopRight.Position - new Vector2(30+100, 100);
            what = fromThree ? "3 pointer" : "2 pointer";
            howLong = gameTime.TotalGameTime.Seconds;
            score.UpdateScore(0, fromThree ? 3 : 2, gameTime);
            fromThree = false;
        }
    }

    public void GTending(GameTime gameTime){
        for(int i = 0; i < 2; i++){
            if(ball.Hitbox.Intersects(gTending[i]) && ball.Velocity.Y < 0){
                what = "GTending!";
                where.X = (float)gTending[0].X;
                where.Y = (float)gTending[0].Y;
                howLong = gameTime.TotalGameTime.Seconds; 
                if (player1.HasBall){
                    player1.HasBall = false;
                    player2.HasBall = true;
                }
            }
        }
    }
}
