using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject.Frames;

namespace GameSharedObject.Controls
{
    public class GameProgressbar: Progressbar
    {

        public GameProgressbar(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            // spriteBatch.Draw();
        }
    }
}
