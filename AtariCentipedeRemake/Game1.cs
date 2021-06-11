using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtariCentipedeRemake
{
    public class Game1 : Game
    {
        Texture2D playerTexture;
        Vector2 playerPosition;
        float playerSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set window title
            Window.Title = "Atari Centipede Remake door Collin Meinders 19GD";

            // Set game resolution
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            _graphics.ToggleFullScreen();

            // TODO: Add your initialization logic here
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - 32);
            playerSpeed = 500f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("Centipede_player_placeholder");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerPosition.X > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
                playerPosition.X = _graphics.PreferredBackBufferWidth - playerTexture.Width / 2;
            else if (playerPosition.X < playerTexture.Width / 2)
                playerPosition.X = playerTexture.Width / 2;

            if (playerPosition.Y > _graphics.PreferredBackBufferHeight - playerTexture.Height / 2)
                playerPosition.Y = _graphics.PreferredBackBufferHeight - playerTexture.Height / 2;
            else if (playerPosition.Y < (_graphics.PreferredBackBufferHeight / 4 * 3 ) + playerTexture.Height / 2)
                playerPosition.Y = (_graphics.PreferredBackBufferHeight / 4 * 3 ) + playerTexture.Height / 2;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Set background colour
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
