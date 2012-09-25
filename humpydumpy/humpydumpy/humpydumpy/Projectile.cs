using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using humpydumpy.Screens;


namespace humpydumpy
{
    public enum ProjectileType
    {
        Apple = 0,
        Banana,
        Lemon,
        Mango,
        Orange,
        Slipper,
        Tomato
    }

    class Projectile
    {
        private Vector2 position = new Vector2();
        private Texture2D texture;

        private Vector2 source;
        private Vector2 destination;
        public Rectangle hitbox;


        private Rectangle trajectory;
        private HumptyDumpty humptyDumpty = null;

        private float elapsedLifeSpanInSeconds = 0f;
        public Vector2 getDestination() { return destination; }
        public void setDestination(Vector2 value) { destination = value; }

        /// <summary>
        /// This is the factor that affects the acceleration. The closer the number is
        /// to zero, the faster the projectile is.
        /// 
        /// Alternatively, we can have a minimum and a maximum acceleration factor.
        /// 
        /// DEPRECATED: A new method getAccelerationFactor was created to base the 
        /// acceleration on Humpty Dumpty's current level (starts at 1).
        /// </summary>
        public const float accelerationFactor = 50f;

        /// <summary>
        /// This factor affects the randomness of the acceleration factor.
        /// </summary>
        public const float randomAccelerationFactor = 1.5f;

        /// <summary>
        /// This is the number of seconds to wait before the projectile starts to move.
        /// </summary>
        public const float secondsToWait = 0.6f;

        public const float accelerationUpwards = 15 + 5;

        private ProjectileType projectileType;
        private float rotation;
        private float kValue;
        private float speed;
        private Vector2 acceleration;
        public Projectile(TextureHandler t, HumptyDumpty humptyDumpty)
        {
            setDestination(humptyDumpty.Position);
            hitbox = new Rectangle((int)position.X, (int)position.Y, 10, 10);
            trajectory = new Rectangle((int)position.X, (int)position.Y, 2, 100);
            this.humptyDumpty = humptyDumpty;

            Random r = new Random();
            projectileType = (ProjectileType)r.Next(7);
            texture = t.getProjectile(projectileType);

        }
        public void Initialize()
        {
#if PARABOLIC
            //acceleration = (destination - position) / getAcceleration(humptyDumpty);
            movement = new Vector2(destination.X - source.X, 0);
            source += -movement;
            position = source;
            initK();
#else
            acceleration = Vector2.Zero;
            acceleration.Y = accelerationUpwards;
#endif
        }

        public float getAcceleration(HumptyDumpty humptyDumpty)
        {
            float baseAccelerationFactor = 90f;
            Random random = new Random();



            int value = humptyDumpty.Level;
            switch (value) // humptyDumpty.Level
            {
                case 5: baseAccelerationFactor = 22; break;
                case 4: baseAccelerationFactor = 30; break;
                case 3: baseAccelerationFactor = 33; break;
                case 2: baseAccelerationFactor = 37; break;
                case 1: baseAccelerationFactor = 40; break;
                default: baseAccelerationFactor = 20; break;
            }

            return baseAccelerationFactor + GameScreen.positiveOrNegative() * (float)(random.NextDouble() + 1);
        }

        private void initK()
        {
            kValue = (source.Y - destination.Y) / ((source.X - destination.X) * (source.X - destination.X));
            speed = Math.Abs(source.X - destination.X) / 30;
        }

        private float findY(float x)
        {
            return (x - source.X) * (x - source.X) + kValue;
        }

        private void setPosition()
        {
#if PARABOLIC
            if (source.X < destination.X)
                movement.X += speed;
            else
                movement.X -= speed;
            //movement.Y = 4 * kValue * movement.X * movement.X - 150;
            float x = (movement.X + source.X) - destination.X;
            movement.Y = kValue * (x * x) + destination.Y - 65;
            position = new Vector2(movement.X + source.X, movement.Y);
#else
            position.Y -= acceleration.Y;
            acceleration.Y -= 0.5F;
#endif

        }

        public void Update(GameTime gameTime, HumptyDumpty humptyDumpty)
        {
            elapsedLifeSpanInSeconds += gameTime.ElapsedGameTime.Milliseconds;
            if (this.humptyDumpty == null)
                this.humptyDumpty = humptyDumpty;

 
            if (elapsedLifeSpanInSeconds > secondsToWait * 1000)
            {
                hitbox.X = (int)position.X;
                hitbox.Y = (int)position.Y;
                if (hitbox.Intersects(humptyDumpty.hitbox))
                {
                    humptyDumpty.Status = HumptyDumpty.DEAD;
                }
                setPosition();
                rotation += getAcceleration(humptyDumpty) / 150.0F;
            }
        }

        private double getTangentAngle()
        {
            return (double) (Math.Abs((position.Y - humptyDumpty.Position.Y)) / Math.Abs(position.X - humptyDumpty.Position.X) );
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(12.5F, 12.5F), new Vector2(1.5F), SpriteEffects.None, 0F);
            
            //spriteBatch.Draw(textureProjectileTrajectory, trajectory, null, Color.White, rotation, new Vector2(0, 1), SpriteEffects.None, 0);
            if (Game1.DEBUG)
                spriteBatch.DrawString(font, "Destination: " + destination.ToString(), position, Color.Black);
        }
        public void setPosition(Vector2 p)
        {
            source = position = p;
        }

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
