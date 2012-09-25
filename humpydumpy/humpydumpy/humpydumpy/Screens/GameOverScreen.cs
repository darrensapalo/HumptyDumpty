using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input.Touch;

namespace humpydumpy.Screens
{
    public class GameOverScreen : Screen
    {

        public GameOverScreen(Game1 game, TextureHandler textureHandler) { this.textureHandler = textureHandler; this.game = game;  }
        public override void Initialize(GraphicsDevice graphicsDevice) { }
        public override void Update(GameTime gameTime, TouchCollection tc)
        {
            if (tc.Count > 0)
            {
                game.changeScreen(ScreenType.MainMenuScreen);
                game.getHumptyDumpty().Status = HumptyDumpty.ALIVE;
            }
            base.Update(gameTime, tc);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /*
            // show score
            // show message
            List<String> MBOptions = new List<string>();
            String youLose = "Humpty Dumpty has failed to amuse the King.";
            MBOptions.Add("Play again!");
            Guide.BeginShowMessageBox("Hard to Please", youLose, MBOptions, 0, MessageBoxIcon.Alert, GameOverMessage, null);
             */
            int score = game.getScore();
            spriteBatch.DrawString(textureHandler.getFont(), "You've lost. Your score is " + score +" Do you wish to play again?", new Vector2(80, 180), Color.White);
            base.Draw(spriteBatch);
        }

    }
}
