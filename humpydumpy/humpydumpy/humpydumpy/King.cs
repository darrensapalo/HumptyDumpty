using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace humpydumpy
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class King
    {
        private TextureHandler textureHandler;
        private float elapsedProjectileTime = 0;
        private Vector2 force = new Vector2(0, 0);
        private LinkedList<Projectile> thrownList = new LinkedList<Projectile>();
        private HumptyDumpty humptyDumpty;
        private SpriteFont font;
        private Vector2 position;
        private Boolean hasThrown;

        public const int THROW_DELAY = 500;
        private float THROW_INTERVAL = 2500;

        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        public King(TextureHandler t, HumptyDumpty h)
        {
            humptyDumpty = h;
            textureHandler = t;
            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            font = textureHandler.getFont();
            // TODO: Add your initialization code here
            position = new Vector2(400, 400);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            Random r = new Random();

            // TODO: Add your update code here
            elapsedProjectileTime += gameTime.ElapsedGameTime.Milliseconds;
            THROW_INTERVAL = humptyDumpty.ThrowInterval;

            if (humptyDumpty.Status != HumptyDumpty.DEAD)
            {
                if (elapsedProjectileTime >= THROW_INTERVAL && !hasThrown)
                {
                    Projectile p = new Projectile(textureHandler, humptyDumpty);
                    p.setPosition(position);
                    p.Initialize();
                    thrownList.AddLast(p);
                    hasThrown = true;
                    THROW_INTERVAL -= 50;
                    THROW_INTERVAL = MathHelper.Clamp(THROW_INTERVAL, 800, 2500);
                }
                else if (elapsedProjectileTime >= THROW_INTERVAL + THROW_DELAY && hasThrown)
                {
                    elapsedProjectileTime = 0;
                    hasThrown = false;
                }
                else if (elapsedProjectileTime < THROW_INTERVAL)
                {
                    float speed = MathHelper.Clamp(Math.Abs(humptyDumpty.Position.X - position.X) / 5, 0, 8);
                    if (humptyDumpty.Position.X < position.X)
                        position.X -= speed;
                    else
                        position.X += speed;
                }
            }
            humptyDumpty.ThrowInterval = THROW_INTERVAL;
            
            force *= 0.9f;
            
            // Update all projectiles
            for (int x = 0; x < thrownList.Count; x++)
            {
                Projectile p = thrownList.ElementAt(x);
                p.Update(gameTime, humptyDumpty);
                if (p.getPosition().Y > 480)
                    thrownList.Remove(p);
            }

            if (position.X < HumptyDumpty.minimumX)
                force *= -1;
            else if (position.X > HumptyDumpty.maximumX)
                force *= -1;

        }

        public void Reset()
        {
            thrownList.Clear();
            elapsedProjectileTime = -500;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // King
            Texture2D t = textureHandler.getTexture("king");
            spriteBatch.Draw(t, new Rectangle((int)position.X, (int)position.Y, 50, 70), null, Color.White, 0.0F, new Vector2(0.5F, 0), SpriteEffects.None, 0.0F);

            // Projectiles
            foreach (Projectile p in thrownList)
            {
                p.Draw(spriteBatch, font);
            }
        }
    }
}
