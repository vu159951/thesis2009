using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject
{
    public class frmMain: GameScene
    {
        private GameMenu menu;

        public frmMain(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.Background = game.Content.Load<Texture2D>("Images\\Background\\style001");
            this.menu = new GameMenu(game);

            this.menu.Location = new Point(320, 250);
        }

        public void ShowControls()
        {
            this.Game.Components.Add(this.menu);
            this.menu.ShowControls();
        }
    }
}
