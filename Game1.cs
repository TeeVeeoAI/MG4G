using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;

namespace MG4G;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Player player1, player2;
    private Texture2D ballTexture;
    private Ball ball;
    private Texture2D hoopTextureRight, hoopTextureLeft;
    private Hoop hoopLeft, hoopRight;
    
    private float lastShoot;

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

        ball = new Ball(ballTexture, new Vector2(1920/2-20, 1080-200-20));
        player1 = new Player(playerTexture, new Vector2((1920/2)/2-100, 1080-350), Keys.A, Keys.D, Keys.Space, Keys.R, ball);
        player2 = new Player(playerTexture, new Vector2((1920/2)/2+1920/2-100, 1080-350), Keys.Left, Keys.Right, Keys.Up, Keys.Down, ball);
        ball.Player1 = player1;
        ball.Player2 = player2; 
        hoopLeft = new Hoop(hoopTextureLeft, new Vector2(0, 1080 - 150/*hoop height/width*/ - 400/*the hoop height*/));
        hoopRight = new Hoop(hoopTextureRight, new Vector2(1920 - 150/*hoop height/width*/, 1080 - 150/*hoop height/width*/ - 400/*the hoop height*/));

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
        if(ball.Hitbox.Intersects(player1.Hitbox) && !player1.ShootB){
            lastShoot = gameTime.TotalGameTime.Seconds;
            player1.HasBall = true;
            player1.ShootB = true;
        } else if(ball.Hitbox.Intersects(player2.Hitbox) && !player2.ShootB){
            lastShoot = gameTime.TotalGameTime.Seconds;
            player2.HasBall = true;
            player2.ShootB = true;
        }
        if(gameTime.TotalGameTime.Seconds >= lastShoot+3.5f){
            player2.ShootB = false;
            player1.ShootB = false;
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(150,200,175));

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        hoopLeft.Draw(_spriteBatch);
        hoopRight.Draw(_spriteBatch);
        player1.Draw(_spriteBatch);
        player2.Draw(_spriteBatch);
        ball.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public Player Dunk(GameTime gameTime){

        Player player = ball.WhoHasTheBall(gameTime);

        if(player.Hitbox.Intersects(hoopLeft.Hitbox)){

        }




        return player;
    }
}