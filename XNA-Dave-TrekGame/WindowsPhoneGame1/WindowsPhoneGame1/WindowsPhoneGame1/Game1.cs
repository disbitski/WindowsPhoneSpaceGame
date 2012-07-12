using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace WindowsPhoneGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // declare
        Texture2D shipTexture;
        Texture2D backgroundTexture;

        Vector2 shipPosition;

        SoundEffect redAlert;
        SoundEffectInstance redAlertInstance;
         

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);


            //Backbuffer - Dave
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;  
        
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

            // TODO: use this.Content to load your game content here

            // load the sprite's texture
            shipTexture = Content.Load<Texture2D>("ship");

            // load the background texture
            backgroundTexture = Content.Load<Texture2D>("space");

            // center the sprite on screen
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            shipPosition = new Vector2(
                (viewport.Width - shipTexture.Width) / 2,
                (viewport.Height - shipTexture.Height) / 2);

            redAlert = Content.Load<SoundEffect>("RedAlert");
            redAlertInstance = redAlert.CreateInstance();  
        
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                TouchLocation t1 = touchCollection[0];

                double x = t1.Position.X - (shipPosition.X + (shipTexture.Width / 2));
                double y = t1.Position.Y - (shipPosition.Y + (shipTexture.Height / 2));
                double speed = Math.Sqrt(x * x + y * y) / 20;

                double angle = (float)Math.Atan2(y, x);

                shipPosition.X += (float)(speed * Math.Cos(angle));
                shipPosition.Y += (float)(speed * Math.Sin(angle));

                PlayRedAlert();

            }   



            base.Update(gameTime);
        }


        private void PlayRedAlert()
        {
            if (redAlertInstance.State != SoundState.Playing)
            {
                redAlertInstance.Play();
                redAlertInstance.Pan = 0f;
                redAlertInstance.Volume = 1f;
                //redAlertInstance.Pitch=0.5f;

            }

        }
   

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            spriteBatch.Draw(shipTexture, shipPosition, Color.White);

            spriteBatch.End();  
        }
    }
}
