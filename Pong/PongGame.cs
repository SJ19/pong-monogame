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

        private int ballSpeedX;
        private int ballSpeedY;

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
            rollDirectionX();
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

            if (ballHitsBottomOrTopOfScreen(ball))
            {
                ballSpeedY = -ballSpeedY;
            }

            if (ballHitsLeftOrRightOfScreen(ball))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Randomly generates the direction of the ball.
        /// </summary>
        private void rollDirectionX()
        {
            Random random = new Random();
            switch (random.Next(1) + 1)
            {
                case 1:
                    ballSpeedX = 10;
                    break;
                case 2:
                    ballSpeedX = 10;
                    break;
            }
            switch (random.Next(1) + 1)
            {
                case 1:
                    ballSpeedY = 10;
                    break;
                case 2:
                    ballSpeedY = 10;
                    break;
            }
        }

        /// <summary>
        /// Move the ball according to changeX and changeY.
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="changeX"></param>
        /// <param name="changeY"></param>
        private void ballMove(Ball ball, int changeX, int changeY)
        {
            ball.position.X += changeX;
            ball.position.Y += changeY;
        }

        /// <summary>
        /// Checks whether the ball hits the left or right side of the screen.
        /// </summary>
        /// <param name="ball"></param>
        /// <returns></returns>
        private bool ballHitsLeftOrRightOfScreen(Ball ball)
        {
            return ball.position.X >= GraphicsDevice.Viewport.Width - ball.image.Width || ball.position.X <= 0;
        }

        /// <summary>
        /// Checks whether the ball hits the top or bottom of the screen.
        /// </summary>
        /// <param name="ball"></param>
        /// <returns></returns>
        private bool ballHitsBottomOrTopOfScreen(Ball ball)
        {
            return ball.position.Y >= GraphicsDevice.Viewport.Height - ball.image.Height || ball.position.Y <= 0;
        }

        /// <summary>
        /// Checks for thumbstick input and does actions.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gamePadState"></param>
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

        /// <summary>
        /// Checks whether the ball and a player collide.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ball"></param>
        /// <returns></returns>
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
