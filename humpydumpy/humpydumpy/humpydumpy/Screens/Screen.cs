using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{


    public abstract class Screen
    {


        protected TextureHandler textureHandler;
        protected Game1 game;
        public virtual void Initialize(GraphicsDevice graphicsDevice) { }
        public virtual void Update(GameTime gameTime, TouchCollection tc){ }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
