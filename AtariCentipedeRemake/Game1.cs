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

        // Centipede data
        List<GameObject> fullCentipede;
        GameObject centipede;
        GameObject centipedeHead;
        int centipedeLenght = 20;
        int centipedeLenghtInHorizontalRow = 20;
        int centipedeLeftMargin = 30;
        int centipedeTopMargin = 64;
        int centipedeSpacing = 32;
        double timeCentipedeLastHorizontalMove = 0;
        int centipedeHorizontalStepInterval = 125;

        int aniX;

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
            fullCentipede = new List<GameObject>();
            centipede = new GameObject();
            centipedeHead = new GameObject();
            aniX = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("Player");
            playerLaser = Content.Load<Texture2D>("laser1");
            centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyA"));
            centipedeHead.AddTexture(this.Content.Load<Texture2D>("CentipedeHead")); 

            for (int i = 0; i < centipedeLenght; i++)
            {
                int divider = centipedeLenght / centipedeLenghtInHorizontalRow;
                int x = centipedeLeftMargin + (i / divider) * centipedeSpacing;
                int y = centipedeTopMargin + (i % divider) * centipedeSpacing;
                if (i == 0)
                {
                    var nextCentipedeBodyPart = new GameObject(centipedeHead);
                    nextCentipedeBodyPart.position.X = x;
                    nextCentipedeBodyPart.position.Y = y;
                    nextCentipedeBodyPart.centiHead = true;
                    fullCentipede.Add(nextCentipedeBodyPart);
                }
                else
                {
                    var nextCentipedeBodyPart = new GameObject(centipede);
                    nextCentipedeBodyPart.position.X = x;
                    nextCentipedeBodyPart.position.Y = y;
                    nextCentipedeBodyPart.centiHead = false;
                    nextCentipedeBodyPart.animCount = aniX;
                    fullCentipede.Add(nextCentipedeBodyPart);
                }
                if (aniX == 0)
                {
                    aniX = 2;
                }
                else
                {
                    aniX = aniX - 1;
                }
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

            // centipede movement script
            var now = gameTime.TotalGameTime.TotalMilliseconds;

            if (now - timeCentipedeLastHorizontalMove > centipedeHorizontalStepInterval)
            {
                foreach (GameObject centipede in fullCentipede)
                {
                    if (centipede.horizontalDirection == true) // TODO: Refactor magic number.
                    { 
                        if (centipede.position.X > _graphics.PreferredBackBufferWidth - playerTexture.Width * 2)
                        {
                            centipede.position.Y += centipede.speed;
                            centipede.horizontalDirection = !centipede.horizontalDirection;
                        }
                        else
                        {
                            centipede.position.X += centipede.speed;
                            if (centipede.centiHead == false)
                            {
                                if (centipede.animCount == 0)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyA"));
                                }
                                else if (centipede.animCount == 1)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyB"));
                                }
                                else if (centipede.animCount == 2)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyC"));
                                    centipede.animCount = 0;
                                }
                            }
                        }

                    }
                    else if (centipede.horizontalDirection == false)
                    {
                        if (centipede.position.X < playerTexture.Width / 2)
                        {
                            centipede.position.Y += centipede.speed;
                            centipede.horizontalDirection = !centipede.horizontalDirection;
                        }
                        else
                        {
                            centipede.position.X -= centipede.speed;
                            if (centipede.centiHead == false)
                            {
                                if (centipede.animCount == 0)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyC"));
                                }
                                else if (centipede.animCount == 1)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyB"));
                                }
                                else if (centipede.animCount == 2)
                                {
                                    centipede.AddTexture(this.Content.Load<Texture2D>("CentipedeBodyA"));
                                    centipede.animCount = 0;
                                }
                            }
                        }
                    }
                }
                timeCentipedeLastHorizontalMove = now;
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

            foreach (GameObject centipede in fullCentipede)
            {
                centipede.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
