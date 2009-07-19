using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Frames;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Controls
{
    public class GameTextbox: Textbox
    {
        public GameTextbox(Game game)
            : base(game)
        {
            this.Background = game.Content.Load<Texture2D>("Images\\MenuPanel\\menuPanel1");
            this.Font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.Text = "";
        }
    }
}
