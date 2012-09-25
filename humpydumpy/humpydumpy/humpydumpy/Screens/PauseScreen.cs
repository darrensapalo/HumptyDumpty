using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{
    public class PauseScreen : Screen
    {

        public PauseScreen(Game1 game, TextureHandler textureHandler) { this.textureHandler = textureHandler; this.game = game;  }
        public override void Initialize(GraphicsDevice graphicsDevice) { }
        public override void Update(GameTime gameTime, TouchCollection tc)
        {
            base.Update(gameTime, tc);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
