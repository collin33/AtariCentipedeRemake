using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AtariCentipedeRemake
{
    public class Game1 : Game
    {
        // Player data
        Texture2D playerTexture;
        Vector2 playerPosition;
        float playerSpeed;

        // Player Pew --> Pew ---> PEWW!!! ----------> KABOOOOOOOOOOOOM!!!! (Laser)
        Texture2D playerLaser;
        Vector2 playerLaserPosition = Vector2.Zero;
        float playerLaserSpeed = 600f;
        bool playerLaserActive = false;

        // Alien data
        List<GameObject> aliens;
        GameObject alien;
        int numberOfAliens = 30;
        int numberOfAliensInHorizontalRow = 10;
        int alienLeftMargin = 30;
        int alienTopMargin = 10;
        int alienSpacing = 70;
        int alienHorizontalMoveCount = 0;
        double timeAlienLastHorizontalMove = 0;
        int alienVerticalStepInterval = 2500;
        double timeAlienLastVerticalMove = 0;
        int alienHorizontalStepInterval = 500;

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
            aliens = new List<GameObject>();
            alien = new GameObject();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("Centipede_player_placeholder");
            playerLaser = Content.Load<Texture2D>("laser1");
            alien.AddTexture(this.Content.Load<Texture2D>("Centipede_head_placeholder"));

            for (int i = 0; i < numberOfAliens; i++)
            {
                // TODO: Place aliens in a matix grid.
                int divider = numberOfAliens / numberOfAliensInHorizontalRow;
                int x = alienLeftMargin + (i / divider) * alienSpacing;
                int y = alienTopMargin + (i % divider) * alienSpacing;
                var nextAlien = new GameObject(alien);
                nextAlien.position.X = x;
                nextAlien.position.Y = y;
                aliens.Add(nextAlien);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            // up
            if (kstate.IsKeyDown(Keys.Up))
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // down
            if (kstate.IsKeyDown(Keys.Down))
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // left
            if (kstate.IsKeyDown(Keys.Left))
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // right
            if (kstate.IsKeyDown(Keys.Right))
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // player horizontal movement bounds
            if (playerPosition.X > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
                playerPosition.X = _graphics.PreferredBackBufferWidth - playerTexture.Width / 2;
            else if (playerPosition.X < playerTexture.Width / 2)
                playerPosition.X = playerTexture.Width / 2;
            
            //player vertical movement bounds
            if (playerPosition.Y > _graphics.PreferredBackBufferHeight - playerTexture.Height / 2)
                playerPosition.Y = _graphics.PreferredBackBufferHeight - playerTexture.Height / 2;
            else if (playerPosition.Y < (_graphics.PreferredBackBufferHeight / 4 * 3) + playerTexture.Height / 2)
                playerPosition.Y = (_graphics.PreferredBackBufferHeight / 4 * 3) + playerTexture.Height / 2;

            // space - laser command
            if (!playerLaserActive & kstate.IsKeyDown(Keys.Space))
            {
                playerLaserActive = true;
                playerLaserPosition.X = playerPosition.X - playerLaser.Width /2;
                playerLaserPosition.Y = playerPosition.Y - playerLaser.Height;
            }

            // activated laser
            if (playerLaserActive)
            {
                playerLaserPosition.Y -= playerLaserSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerLaserPosition.Y < playerLaser.Height)
                {
                    playerLaserActive = false;
                }
            }

            // alien movement script
            var now = gameTime.TotalGameTime.TotalMilliseconds;
            if (now - timeAlienLastVerticalMove > alienVerticalStepInterval)
            {
                foreach (GameObject alien in aliens)
                {
                    alien.position.Y += alien.speed;
                }
                timeAlienLastVerticalMove = now;
            }

            if (now - timeAlienLastHorizontalMove > alienHorizontalStepInterval)
            {
                foreach (GameObject alien in aliens)
                {
                    if (alienHorizontalMoveCount < 5) // TODO: Refactor magic number.
                    {
                        alien.position.X += alien.speed;
                    }
                    else
                    {
                        alien.position.X -= alien.speed;
                    }
                }
                alienHorizontalMoveCount++;
                alienHorizontalMoveCount %= 10; // TODO: Refactor magic number.
                timeAlienLastHorizontalMove = now;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Set background colour
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            if (playerLaserActive)
            {
                _spriteBatch.Draw(playerLaser, playerLaserPosition, Color.White);
            }

            foreach (GameObject alien in aliens)
            {
                alien.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
