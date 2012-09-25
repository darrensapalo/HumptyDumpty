using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{
    public class TutorialScreen : Screen
    {
        Rectangle play = new Rectangle(265, 180, 220, 80);
        Rectangle about = new Rectangle(265, 260, 220, 80);
        Rectangle how = new Rectangle(265, 340, 220, 80);
        Rectangle music = new Rectangle(700, 410, 70, 70);

        public TutorialScreen(Game1 game, TextureHandler textureHandler) { this.textureHandler = textureHandler; this.game = game; }
        public override void Initialize(GraphicsDevice graphicsDevice) { }
        public override void Update(GameTime gameTime, TouchCollection tc)
        {
            foreach (TouchLocation tl in tc)
            {
                if (play.Contains((int)tl.Position.X, (int)tl.Position.Y))
                {
                    game.changeScreen(ScreenType.MainMenuScreen);
                }
                else if (about.Contains((int)tl.Position.X, (int)tl.Position.Y))
                {
                    game.changeScreen(ScreenType.AboutScreen);
                }
                else if (how.Contains((int)tl.Position.X, (int)tl.Position.Y))
                {
                    game.changeScreen(ScreenType.TutorialScreen);
                }
                else if (music.Contains((int)tl.Position.X, (int)tl.Position.Y))
                {
                    
                }
            }
            base.Update(gameTime, tc);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureHandler.HowToScreen, Vector2.Zero, Color.White);
            spriteBatch.Draw(textureHandler.getDebugTexture(), play, Color.Red * 0.2F);
            spriteBatch.Draw(textureHandler.getDebugTexture(), about, Color.Blue * 0.2F);
            spriteBatch.Draw(textureHandler.getDebugTexture(), how, Color.Green * 0.2F);
            spriteBatch.Draw(textureHandler.getDebugTexture(), music, Color.GreenYellow * 0.2F);
            
            base.Draw(spriteBatch);
        }
    }
}
