using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Controls
{
    public class GameScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D background;

        protected Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }
        protected SpriteBatch spriteBatch;

        public GameScene(Game game)
            : base(game)
        {
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(background, new Rectangle(0,0, this.Game.Window.ClientBounds.Width, this.Game.Window.ClientBounds.Height), Color.White);
        }
    }
}
