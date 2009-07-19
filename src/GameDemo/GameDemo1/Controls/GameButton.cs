using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Controls
{
    public class GameButton: GameSharedObject.Frames.Button
    {
        private String _text;
        private SpriteFont _font;
        private Color _foreColor;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        #region Local Variable
        private Texture2D bgNormal;
        private Texture2D bgMouseEnter;
        private Texture2D bgMouseDown;
        #endregion

        public GameButton(Game game)
            : base(game)
        {
            bgNormal = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseLeave");
            bgMouseEnter = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseEnter");
            bgMouseDown = Game.Content.Load<Texture2D>("Images\\Button\\JXOnlineI\\MouseDown");
            this.Font = game.Content.Load<SpriteFont>("Images\\Button\\JXOnlineI\\Font");
            this.Background = bgNormal;

            this.ForeColor = Color.White;
            this.Size = new System.Drawing.Size(269, 38);
            _text = "New button";
        }
        protected override void OnMouseDown(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgMouseDown;
            base.OnMouseDown(e);
        }
        protected override void OnMouseEnter(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgMouseEnter;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(GameSharedObject.Frames.MouseEventArgs e)
        {
            this.Background = bgNormal;
            base.OnMouseLeave(e);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 size = _font.MeasureString(_text);
            Vector2 pos = new Vector2((this.Size.Width - size.X) / 2 + this.Parent.Location.X + 5,
                (this.Size.Height - size.Y) / 2 + this.Parent.Location.Y + 6);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
            
        }
    }
}
