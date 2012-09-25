using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace humpydumpy
{
    public class TextureHandler
    {
        private List<Texture2D> listOfTextures = new List<Texture2D>();
        private List<Texture2D> projectiles = new List<Texture2D>();
        private Texture2D backgroundTexture;
        private Texture2D debug;
        private Texture2D loading;
        private SpriteFont font;
        private Texture2D mainMenuScreen;
        public Texture2D MainMenuScreen { get; set; }
        private Texture2D howToScreen;
        public Texture2D HowToScreen { get; set; }
        private Texture2D aboutScreen;
        public Texture2D AboutScreen { get; set; }

        public TextureHandler(GraphicsDevice gd)
        {
            // Texture initializations
            debug = new Texture2D(gd, 1, 1);
            debug.SetData<Color>(new Color[] { Color.White });
            
        }

        public void setLoadingImage(Texture2D loading)
        {
            this.loading = loading;
        }

        public Texture2D getLoadingImage() { return loading; }

        public void setBackground(Texture2D bg)
        {
            backgroundTexture = bg;
        }

        public void addProjectile(Texture2D projectile)
        {
            projectiles.Add(projectile);
        }

        public Texture2D getProjectile(ProjectileType p)
        {
            return projectiles.ElementAt((int)p);
        }

        public Texture2D getBackground()
        {
            return backgroundTexture;
        }

        public void addTexture(Texture2D item)
        {
            listOfTextures.Add(item);
        }

        public Texture2D getTexture(int i)
        {
            if (i < 0 || i >= listOfTextures.Count)
                return null;

            return listOfTextures.ElementAt(i);
        }

        public Texture2D getTexture(String name)
        {
            foreach (Texture2D td in listOfTextures)
            {
                if (td.Name.Equals(name))
                    return td;
            }
            return null;
        }
        public Texture2D getDebugTexture()
        {
            return debug;
        }


        public void setFont(SpriteFont font)
        {
            this.font = font;
        }

        public SpriteFont getFont()
        {
            return font;
        }
    }
}
