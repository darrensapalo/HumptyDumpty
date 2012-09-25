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
using Microsoft.Devices.Sensors;
using humpydumpy.Screens;

namespace humpydumpy
{

    public enum ScreenType
    {
        AboutScreen = 0,
        TutorialScreen,
        GameScreen,
        LoadScreen,
        MainMenuScreen,
        PauseScreen,
        GameOverScreen,
    }
    /// <summary>
    /// Humpty Dumpty Game
    /// </summary>
    /// TODO Add "King's Rage" which affects the forceApplied at random moments of the game.
    /// 
    /// Storyline concept
    /// (1) As the King's Jester, Humpty Dumpty is put commanded to balance himself on a wall.
    /// (2) To amuse the King, he throws random objects { 'apple', 'banana', 'oranges' } at 
    /// Humpty Dumpty. At this part, the King is laughing so happy.
    /// (3) When Humpty Dumpty is almost falling or tipping over, the King is further happy. (more score)


    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public TextureHandler textureHandler;

        /// <summary>
        /// This is the Accelerometer object which handles the input from a Windows Phone.
        /// </summary>
        Accelerometer accelSensor;
        public Vector3 acceleration;

        public const Boolean DEBUG = false;

        Screen currentScreen;
        GameScreen gameScreen;
        PauseScreen pauseScreen;
        LoadScreen loadScreen;
        GameOverScreen gameOverScreen;
        MainMenuScreen mainMenuScreen;
        AboutScreen aboutScreen;
        TutorialScreen tutorialScreen;

        public HumptyDumpty getHumptyDumpty()
        {
            return gameScreen.getHumptyDumpty();
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        protected override void Initialize()
        {
            /*
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
            */

            textureHandler = new TextureHandler(GraphicsDevice);
            gameScreen = new GameScreen(this, textureHandler);
            pauseScreen = new PauseScreen(this, textureHandler);
            loadScreen = new LoadScreen(this, textureHandler);
            gameOverScreen = new GameOverScreen(this, textureHandler);
            mainMenuScreen = new MainMenuScreen(this, textureHandler);
            aboutScreen = new AboutScreen(this, textureHandler);
            tutorialScreen = new TutorialScreen(this, textureHandler);
            currentScreen = loadScreen;

            initializeAccelerometer();
            base.Initialize();
        }

        public int getScore()
        {
            return gameScreen.getScore();
        }

        private void initializeContent()
        {
            gameScreen.Initialize(GraphicsDevice);
            pauseScreen.Initialize(GraphicsDevice);
            loadScreen.Initialize(GraphicsDevice);
            gameOverScreen.Initialize(GraphicsDevice);
        }

        public void changeScreen(ScreenType screen)
        {
            switch (screen)
            {
                case ScreenType.AboutScreen: currentScreen = aboutScreen; break;
                case ScreenType.MainMenuScreen: currentScreen = mainMenuScreen; break;
                case ScreenType.PauseScreen: currentScreen = pauseScreen; break;
                case ScreenType.TutorialScreen: currentScreen = tutorialScreen; break;
                case ScreenType.GameScreen: currentScreen = gameScreen; break;
                case ScreenType.GameOverScreen: currentScreen = gameOverScreen; break;
            }
        }

        private void initializeAccelerometer()
        {
            // D! Port to accelerometer. Fix the gyro + acce next time.
            accelSensor = new Accelerometer();
            accelSensor.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(AccelerometerReadingChanged);
            accelSensor.Start();
        }

        private void AccelerometerReadingChanged(object sender, SensorReadingEventArgs<AccelerometerReading> readings)
        {
            Vector3 v = readings.SensorReading.Acceleration;
            
            // D! Since landscape left is the orientation, interchange the Y axis and the X axis.
            acceleration.X = -v.Y; 
            acceleration.Y = -v.X;
            acceleration.Z = v.Z;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont font = Content.Load<SpriteFont>("SpriteFont1");
            Texture2D genericProjectileTexture, genericKingTexture;
            Texture2D defaultTexture;
            Texture2D background;
            Texture2D projectile;


            genericProjectileTexture = new Texture2D(GraphicsDevice, 1, 1);
            genericProjectileTexture.SetData<Color>(new Color[] { Color.Red });
            genericProjectileTexture.Name = "generic projectile";

            background = Content.Load<Texture2D>("background");

            genericKingTexture = new Texture2D(GraphicsDevice, 1, 1);
            genericKingTexture.SetData<Color>(new Color[] { Color.Green });
            genericKingTexture.Name = "king";

            projectile = Content.Load<Texture2D>("apple");     textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("banana");    textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("lemon");     textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("mango");     textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("orange");    textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("slipper");   textureHandler.addProjectile(projectile);
            projectile = Content.Load<Texture2D>("tomato");    textureHandler.addProjectile(projectile);
            defaultTexture = Content.Load<Texture2D>("dgdl"); textureHandler.setLoadingImage(defaultTexture);

            textureHandler.HowToScreen = Content.Load<Texture2D>("samplehowto");
            textureHandler.MainMenuScreen = Content.Load<Texture2D>("samplesplashscreen");
            textureHandler.AboutScreen = Content.Load<Texture2D>("sampleaboutscreen");

            textureHandler.addTexture(genericProjectileTexture);
            textureHandler.addTexture(genericKingTexture);
            textureHandler.setBackground(background);
            textureHandler.setFont(font);

            initializeContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void playAgain()
        {
            currentScreen = gameScreen;
            gameScreen.ResetGame();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (gameScreen.getHumptyDumpty().Status == HumptyDumpty.DEAD)
                currentScreen = gameOverScreen;

            TouchCollection tc = TouchPanel.GetState();
            if (currentScreen is GameScreen)
                ((GameScreen)currentScreen).Update(gameTime, tc, acceleration);
            else if (currentScreen is LoadScreen && ((LoadScreen)currentScreen).isFinishedLoading())
                currentScreen = mainMenuScreen;
            else
                currentScreen.Update(gameTime, tc);

            
            base.Update(gameTime);
        }
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            if (currentScreen is GameScreen)
                ((GameScreen)currentScreen).Draw(spriteBatch);
            else
                currentScreen.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
