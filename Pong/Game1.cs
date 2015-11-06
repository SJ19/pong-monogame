using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake_Monogame;
using System;

namespace Snake_2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Player playerOne;
        private Player playerTwo;
        private Ball ball;

        private int ballSpeedX = 10;
        private int ballSpeedY = 10;

        private GamePadState gamePadStatePlayerOne;
        private GamePadState gamePadStatePlayerTwo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            int middleHeight = (Window.ClientBounds.Height / 2) - (GameConstants.PLAYER_HEIGHT / 2);
            playerOne = new Player(Content.Load<Texture2D>("player"), new Vector2(0, middleHeight));
            playerTwo = new Player(Content.Load<Texture2D>("player"), new Vector2(GraphicsDevice.Viewport.Width - GameConstants.PLAYER_WIDTH, middleHeight));
            ball = new Ball(Content.Load<Texture2D>("ball"), new Vector2((GraphicsDevice.Viewport.Width / 2) - (GameConstants.BALL_WIDTH / 2), (GraphicsDevice.Viewport.Height / 2) - (GameConstants.BALL_HEIGHT / 2)));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gamePadStatePlayerOne = GamePad.GetState(PlayerIndex.One);
            gamePadStatePlayerTwo = GamePad.GetState(PlayerIndex.Two);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            handleStick(playerOne, gamePadStatePlayerOne);
            handleStick(playerTwo, gamePadStatePlayerTwo);

            ballMove(ball, ballSpeedX, ballSpeedY);

            if (ballAndPlayerCollide(playerOne, ball) || ballAndPlayerCollide(playerTwo, ball))
            {
                ballSpeedX = -ballSpeedX;
            }

            if (ballHitsBottomOrTop(ball))
            {
                ballSpeedY = -ballSpeedY;
            }

            if (ballHitsWall(ball))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        private void ballMove(Ball ball, int changeX, int changeY)
        {
            ball.position.X += changeX;
            ball.position.Y += changeY;
        }

        private bool ballHitsWall(Ball ball)
        {
            return ball.position.X >= GraphicsDevice.Viewport.Width - ball.image.Width || ball.position.X <= 0;
        }

        private bool ballHitsBottomOrTop(Ball ball)
        {
            return ball.position.Y >= GraphicsDevice.Viewport.Height - ball.image.Height || ball.position.Y <= 0;
        }

        private void handleStick(Player player, GamePadState gamePadState)
        {
            bool reachedTopOfScreen = player.position.Y <= 0;
            bool reachedBottomOfScreen = player.position.Y > GraphicsDevice.Viewport.Height - GameConstants.PLAYER_HEIGHT;

            if (gamePadState.ThumbSticks.Left.Y > 0 && !reachedTopOfScreen)
            {
                player.position.Y -= 10;
            }
            if (gamePadState.ThumbSticks.Left.Y < 0 && !reachedBottomOfScreen)
            {
                player.position.Y += 10;
            }
        }

        private bool ballAndPlayerCollide(Player player, Ball ball)
        {
            Rectangle playerRectangle = new Rectangle((int)player.position.X, (int)player.position.Y, player.image.Width, player.image.Height);
            Rectangle ballRectangle = new Rectangle((int)ball.position.X, (int)ball.position.Y, ball.image.Width, ball.image.Height);

            return playerRectangle.Intersects(ballRectangle);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameConstants.BACKGROUND_COLOR);

            spriteBatch.Begin();
            spriteBatch.Draw(playerOne.image, playerOne.position, GameConstants.PLAYER_COLOR);
            spriteBatch.Draw(playerTwo.image, playerTwo.position, GameConstants.PLAYER_COLOR);
            spriteBatch.Draw(ball.image, ball.position, GameConstants.BALL_COLOR);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
