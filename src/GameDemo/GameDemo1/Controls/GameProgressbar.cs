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
            this.barTexture = game.Content.Load<Texture2D>("Images\\Button\\Progressbar\\bar");
            this.Background = game.Content.Load<Texture2D>("Images\\Button\\Progressbar\\bgProgressbar");
            this.font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.ForeColor = Color.BurlyWood;
        }
    }
}
