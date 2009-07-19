using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Frames;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Controls
{
    public class GameLabel: Label
    {
        public GameLabel(Game game)
            : base(game)
        {
            this.Font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.ForeColor = Color.White;
            this.Text = "New label";
        }
    }
}
