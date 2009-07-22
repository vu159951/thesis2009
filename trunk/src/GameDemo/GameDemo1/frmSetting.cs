using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject.Frames;

namespace GameSharedObject
{
    public class frmSetting : GameScene
    {
        private Form menu;
        private GameCheckbox chk1;
        private GameCheckbox chk2;
        private GameCheckbox chk3;
        private GameCheckbox chk4;
        private GameCheckbox chk5;
        private GameCheckbox chk6;

        public frmSetting(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.menu = new GameMenu(game);

            this.Background = game.Content.Load<Texture2D>("Images\\Background\\style001");
            this.menu.Background = game.Content.Load<Texture2D>("Images\\Button\\Menu\\menu02");
            this.menu.Location = new Point((int)((game.Window.ClientBounds.Width - this.menu.Size.Width)/2), 250);
        }

        public void ShowControls()
        {
            this.Game.Components.Add(this.menu);
        }
        public void UnLoad()
        {
            this.Game.Components.Remove(this);
        }
    }
}
