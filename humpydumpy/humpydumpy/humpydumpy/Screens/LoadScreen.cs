using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{
    public class LoadScreen : Screen
    {
        Vector2 position;
        Texture2D loading;
        int loadTime;
        float rate = 0F;
        Boolean fadeIn, fadeOut, finished;
        public LoadScreen(Game1 game, TextureHandler textureHandler)
        {
            this.textureHandler = textureHandler;
            this.game = game;
            loading = textureHandler.getLoadingImage();
            fadeIn = true;
            finished = fadeOut = false;
        }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            loading = textureHandler.getLoadingImage();
            position = new Vector2(graphicsDevice.DisplayMode.Height / 2 - loading.Height / 2, graphicsDevice.DisplayMode.Width / 2 - loading.Width / 2);
        }

        public override void Update(GameTime gameTime, TouchCollection tc)
        {
            loadTime += gameTime.ElapsedGameTime.Milliseconds;
            if (fadeIn && rate < 1)
            {
                rate += 0.05F;
                loadTime = 0;
            }
            else if (fadeOut && rate > 0)
            {
                rate -= 0.05F;
                loadTime = 0;
            }
            else if (fadeIn && rate >= 1)
            {
                if (loadTime > 2000)
                {
                    fadeIn = false;
                    fadeOut = true;
                    loadTime = 0;
                }
            }
            else if (fadeOut && rate <= 0)
            {
                fadeIn = false;
                fadeOut = false;
                loadTime = 0;
                
            }
            else if (!fadeOut && !fadeIn)
            {
                if (loadTime > 500)
                {
                    finished = true;
                }
            }

            

            base.Update(gameTime, tc);
        }

        public Boolean isFinishedLoading()
        {
            return finished;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(loading, position, Color.White * rate);
            base.Draw(spriteBatch);
        }
    }
}
