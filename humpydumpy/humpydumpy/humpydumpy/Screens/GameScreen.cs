using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{
    public class GameScreen : Screen
    {
        public float scoreTime = 0;
        public float elapsedTime = 0;
        public float levelingUpTime = 0;
        public float highScore = 0;

        /// <summary>
        /// This is the random force applied by "wind", or other factors.
        /// </summary>
        public const float randomWindForceFactor = 15f;

        /// <summary>
        /// This value affects the magnitude of the effect of the random force applied to Humpty Dumpty.
        /// TODO 0.01f seems to make the movement smoother, but the effect of the randomness is reduced.
        /// </summary>
        public const float rotationalForceFactor = 0.01f;

        /// <summary>
        /// This value affects the magnitude of the effect of the accelerometer's force applied to Humpty Dumpty.
        /// </summary>
        public const float accelerometerFactor = 0.07f;

        /// <summary>
        /// This value is the threshold for falling or tipping over, or losing the game. For the maximum value, refer to maxThreshold.
        /// </summary>
        public const float tippingThreshold = 0.8f;

        /// <summary>
        /// This value allows the user to determine whether he is about to fall over or not.
        /// </summary>
        public const float warningThreshold = tippingThreshold - 0.4f;

        /// <summary>
        /// This value affects the reduction or drag of the movement of Humpty Dumpty from left to right.
        /// </summary>
        public const float movementDrag = 0.9f;

        private HumptyDumpty humptyDumpty;
        private King king;
        private Vector3 acceleration;

        public GameScreen(Game1 game, TextureHandler textureHandler) {
            this.textureHandler = textureHandler;
            this.game = game;
        }

        public void setTextureHandler(TextureHandler textureHandler){ this.textureHandler = textureHandler;  }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {

            // Player initializations
            humptyDumpty = new HumptyDumpty(textureHandler);
            humptyDumpty.Initialize();


            king = new King(textureHandler, humptyDumpty);
            king.Initialize();

            // Game initializations
        }

        public void ResetGame()
        {
            humptyDumpty.Reset();
            king.Reset();
            scoreTime = 0;
        }


        public int getScore()
        {
            return (int)(scoreTime / 100);
        }

        public void Update(GameTime gameTime, TouchCollection tc, Vector3 acceleration) {
            this.acceleration = acceleration;

            if (tc.Count == 0)
                humptyDumpty.setBalance(false);

            foreach (TouchLocation touch in tc)
            {
                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
                {
                    if (humptyDumpty.Status == HumptyDumpty.DEAD)
                    {

                        
                        humptyDumpty.Reset();
                        scoreTime = 0;
                    }
                    else
                    {
                        humptyDumpty.setBalance(true);
                    }
                }
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            levelingUpTime += gameTime.ElapsedGameTime.Milliseconds;

            if (humptyDumpty.Status != HumptyDumpty.DEAD)
                scoreTime += gameTime.ElapsedGameTime.Milliseconds;

            if (humptyDumpty.Status == HumptyDumpty.TIPPING)
            {
                scoreTime += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (highScore < scoreTime)
                highScore = scoreTime;


            if (levelingUpTime >= 5000)
            {
                levelingUpTime = 0;
                humptyDumpty.Level = humptyDumpty.Level + 1;
            }


            king.Update(gameTime);
            humptyDumpty.Update(gameTime, acceleration);
        }

        public new void Draw(SpriteBatch spriteBatch) {

            //spriteBatch.Draw(textureHandler.getBackground(), Vector2.Zero, Color.White);
            SpriteFont font = textureHandler.getFont();
            if (Game1.DEBUG)
            {
                spriteBatch.DrawString(font, "Rotation: " + humptyDumpty.Rotation.ToString(), new Vector2(10, 50), Color.White);
                /*
                spriteBatch.DrawString(font, "forceApplied: " + humptyDumpty.Force.ToString(), new Vector2(10, 50), Color.White);
                spriteBatch.DrawString(font, "acceleration: " + MathHelper.ToDegrees(acceleration.X).ToString(), new Vector2(10, 70), Color.White);
                spriteBatch.DrawString(font, "texturePosition: " + humptyDumpty.Position.X.ToString() + ":" + humptyDumpty.Position.Y.ToString(), new Vector2(10, 90), Color.White);
                 */

            }

            //spriteBatch.Draw(textureProjectile, Color.White);
            spriteBatch.DrawString(font, "Score: " + ((int)(scoreTime / 100)).ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, "High Score: " + ((int)(highScore / 100)).ToString(), new Vector2(10, 30), Color.White);

            humptyDumpty.Draw(spriteBatch);
            king.Draw(spriteBatch);
        }


        public static int positiveOrNegative()
        {
            Random random = new Random();
            return ((random.Next(2) == 1) ? 1 : -1);
        }

        public HumptyDumpty getHumptyDumpty()
        {
            return humptyDumpty;
        }


    }
}
