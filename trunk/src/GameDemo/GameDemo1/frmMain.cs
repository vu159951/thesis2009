using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSharedObject.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject.Components;

namespace GameSharedObject
{
    public class frmMain: GameScene
    {
        private CursorGame _cursor; // cursor game// con trỏ chuột của game
        private GameMenu menu;
        public GameMenu Menu
        {
            get { return menu; }
            set { menu = value; }
        }

        public frmMain(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.Background = game.Content.Load<Texture2D>("Images\\Background\\style001");
            this.menu = new GameMenu(game);
            this._cursor = new CursorGame(game);

            this.menu.Location = new Point((int)((game.Window.ClientBounds.Width - menu.Size.Width)/2), 250);
        }

        public void ShowControls()
        {
            this.Game.Components.Add(this.menu);
            this.menu.ShowControls();
            this.Game.Components.Add(_cursor);
        }
        public void UnLoad()
        {
            this.Game.Components.Remove(_cursor);
            this.menu.UnLoad();
            this.Game.Components.Remove(this);
        }
    }
}
