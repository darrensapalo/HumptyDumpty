using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using humpydumpy.Screens;
using Microsoft.Xna.Framework.GamerServices;

namespace humpydumpy
{
    public class HumptyDumpty
    {
        
/// <summary>
        /// This is a generic Random object used for generating random results.
        /// </summary>
        public Random random = new Random();

        /// <summary>
        /// This is the current number of milliseconds that have passed since the game started.
        /// Wind starts after 10 seconds.
        /// </summary>
        float windTime = -10000;

        /// <summary>
        /// This vector holds the direction of the movement of Humpty Dumpty.
        /// </summary>
        Vector2 mDirection = Vector2.Zero;

        /// <summary>
        /// This value holds the magnitude or speed by which Humpty Dumpty moves.
        /// </summary>
        Vector2 mForce = Vector2.Zero;

        /// <summary>
        /// This is the rotational value of Humpty Dumpty. Its range is from -1 to 1.
        /// </summary>
        private float rotationalForce = 0.2f;

        /// <summary>
        /// This value is the regular instantaneous speed of Humpty Dumpty.
        /// </summary>
        private float instantaneousSpeed = 0.93F;

        /// <summary>
        /// This value is the maximum speed that Humpty Dumpty can achieve. Note that this may or 
        /// may not be attainable because of the movementDrag attribute which decreases the speed.
        /// </summary>
        float mSpeedCap = 9.8F;

        /// <summary>
        /// This holds Humpty Dumpty's level.
        /// </summary>
        private int mLevel = 1;

        public Rectangle hitbox = new Rectangle();

        public int Level
        {
            set
            {
                mLevel = value;
            }
            get
            {
                return mLevel;
            }
        }

        /// <summary>
        /// This holds the current status of Humpty Dumpty. Its values ranges from ALIVE (1), 
        /// TIPPING (2), AND DEAD (0)
        /// </summary>
        private int currentStatus = 1;

        public int Status
        {
            get 
            {
                return currentStatus;
            }
            set
            {
                currentStatus = value;
            }
        }

        public float Rotation
        {
            get
            {
                return rotationalForce;
            }
        }

        /// <summary>
        /// This status represents Humpty Dumpty when alive.
        /// </summary>
        public const int ALIVE = 1;

        /// <summary>
        /// This status represents Humpty Dumpty when tipping.
        /// </summary>
        public const int TIPPING = 2;

        /// <summary>
        /// This status represents Humpty Dumpty when dead.
        /// </summary>
        public const int DEAD = 0;

        /// <summary>
        /// This value is the minimum value for the X axis on Humpty Dumpty's position.
        /// </summary>
        public const float minimumX = 50;

        /// <summary>
        /// This value is the maximum value for the X axis on Humpty Dumpty's position.
        /// </summary>
        public const float maximumX = 750;

        /// <summary>
        /// This value is the maximum rotational value of Humpty Dumpty. For the tipping value, refer to tippingThreshold.
        /// </summary>
        private const float maxThreshold = 1.0f;

        private const float constantY = 295;
        private const float fallAcceleration = 0.3F;
        private float fallSpeed = 0;

        private Texture2D texture;
        private Vector2 texturePosition = new Vector2(400, constantY);


        private Boolean isBalancing = false;
        

        private TextureHandler textureHandler;

        private float throwInterval = 2500;

        /// <summary>
        /// Wind variables
        /// </summary>
        private Boolean isWindy = false;
        private Vector2 windDirection = Vector2.Zero;
        private float windForce;
        private float currentIncreaseOfWind;
        private const float increaseOfWind = 0.041F;
        private const float increaseOfStrengthOfWind = 0.04F;
        private const float maximumWindCap = 4F;


        public float ThrowInterval
        {
            get { return throwInterval; }
            set { throwInterval = value; }
        }

        public float Force
        {
            set
            {
                rotationalForce = value;
            }
            get
            {
                return rotationalForce;
            }
        }

        public Vector2 Position
        {
            get
            {
                return texturePosition;
            }
        }

        public HumptyDumpty(TextureHandler textureHandler)
        {
            this.textureHandler = textureHandler;
            Initialize();
        }

        public void Initialize()
        {
            this.texture = textureHandler.getDebugTexture();

            // HitBox Initializations
            hitbox.Width = 64;
            hitbox.Height = 100;
            hitbox.X = (int)Position.X;
            hitbox.Y = (int)Position.Y;
        }


        private void applyWind()
        {
            if (windTime >= 5000 && isWindy == false)
            {
                isWindy = true;
                windDirection = (random.Next(2) == 0) ? new Vector2(1F, 0) : new Vector2(-1F, 0);
                windForce = 0F;
                windTime = 0;
                currentIncreaseOfWind = increaseOfWind;
            }
            else if (isWindy)
            {
                if (windTime >= 100)
                {
                    currentIncreaseOfWind += increaseOfStrengthOfWind;
                    windForce += currentIncreaseOfWind;
                    windTime = 0;
                    if (!isBalancing)
                        mForce.Y -= 0.8F;

                }
                mForce += windForce * windDirection;
                if (windForce > maximumWindCap)
                {
                    isWindy = false;
                    windTime = 0;
                    mForce.Y += fallAcceleration;
                    fallSpeed = 0F;
                }
            }
            
        }

        public void Reset()
        {
            mLevel = 1;
            currentStatus = ALIVE;
            rotationalForce = 0;
            throwInterval = 5000;
            windTime = 0;
            
        }

        public void Die()
        {

        }

        public void setBalance(Boolean b)
        {
            isBalancing = b;
        }

        private void changePosition()
        {
            if (rotationalForce < 0)
                mDirection.X = -1;
            else if (rotationalForce > 0)
                mDirection.X = 1;

            mForce += mDirection * (instantaneousSpeed * (Math.Abs(rotationalForce) + 0.1F) + 0.05F);

            if (isBalancing && isWindy) mForce *= 0.9F;
            else if (isBalancing) mForce *= 0.7F;

            if (!isWindy)
            {
                if (texturePosition.Y < constantY)
                {
                    fallSpeed += fallAcceleration;
                    mForce.Y += fallSpeed;
                }

            }


            // Force drag
            //mForce *= GameScreen.movementDrag;

            // Check first if HumptyDumpty is at the ends.
            if (mForce.X < -mSpeedCap)
                mForce.X = -mSpeedCap;
            else if (mForce.X > mSpeedCap)
                mForce.X = mSpeedCap;

            // If not, then apply the force to the position.
            
            texturePosition += mForce;


            if (texturePosition.X <= minimumX)
                texturePosition.X = minimumX + 1;
            else if (texturePosition.X >= maximumX)
                texturePosition.X = maximumX - 1;
            if (texturePosition.Y > constantY)
            {
                texturePosition.Y = constantY;
                fallSpeed = 0;
                mForce.Y = 0;
            }
        }

        /// <summary>
        /// This is for applying the rotation received by the accelerometer.
        /// </summary>
        /// <param name="gameTime">An object holding the time passed.</param>
        private void applyRotationalForce(Vector3 acceleration)
        {
            /*
            // positiveOrNegative() - Select either going to left or right
            // random.NextDouble() - Get magnitude of the random force
            // randomForceFactor - apply general factor to random force
            float randomForce;
            randomForce = GameScreen.positiveOrNegative() * (float)random.NextDouble() * GameScreen.rotationalForceFactor;

            // accelerometerFactor - apply general factor to accelerometer force
            float accelerationForce = acceleration.X * GameScreen.accelerometerFactor;
            rotationalForce += accelerationForce + randomForce;
            */
            rotationalForce = acceleration.X;
            /*
             * if (rotationalForce >= maxThreshold) rotationalForce = 1;
             * else if (rotationalForce <= -maxThreshold) rotationalForce = -1;
             */
        }



        private Color getColor()
        {
            if (currentStatus == DEAD)
                return Color.Red;

            Color c = Color.White;
            if (isDead())
            {
                c = Color.Red;
                currentStatus = DEAD;
                
            }
            else if (isTippingOver())
            {
                c = Color.Yellow;
                currentStatus = TIPPING;
            }
            else
            {
                currentStatus = ALIVE;
            }

            return c;
        }

        private Boolean isTippingOver()
        {
            return (rotationalForce >= GameScreen.warningThreshold || rotationalForce <= -GameScreen.warningThreshold);
        }

        private Boolean isDead()
        {
            return currentStatus == DEAD || (rotationalForce >= GameScreen.tippingThreshold || rotationalForce <= -GameScreen.tippingThreshold);
        }
        public void Update(GameTime gameTime, Vector3 acceleration)
        {

            // TRACE for bugs
            windTime += gameTime.ElapsedGameTime.Milliseconds;

            //applyWind();
            applyRotationalForce(acceleration);
            changePosition();
            

            // Hitbox has an offset
            hitbox.X = (int)Position.X;
            hitbox.Y = (int)Position.Y - hitbox.Height;

            if (isTippingOver())
            {
                hitbox.Width = 100;
                hitbox.Height = 64;
            }
            else
            {
                hitbox.Width = 64;
                hitbox.Height = 100;
            }

            checkStatus();

        }

        private void checkStatus()
        {
            if (isDead())
                currentStatus = HumptyDumpty.DEAD;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, texturePosition, null, getColor(), rotationalForce, new Vector2(0.5F, 1), new Vector2(64, 100), SpriteEffects.None, 0F);
            if (Game1.DEBUG)
            {
                Vector2 hitVectorPosition = new Vector2(hitbox.Location.X, hitbox.Location.Y);
                Vector2 hitVectorSize = new Vector2(hitbox.Width, hitbox.Height);
                spriteBatch.Draw(texture, hitVectorPosition, null, Color.Green, 0.0f, new Vector2(0.5f, 0), hitVectorSize, SpriteEffects.None, 0F);
            }
        }

    }
}
