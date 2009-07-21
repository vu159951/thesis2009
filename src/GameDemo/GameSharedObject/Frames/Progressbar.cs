using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Progressbar: Control
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

        public Progressbar(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: nothing
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // TODO: Add your draw code here
            spriteBatch.Draw(_background,
                new Rectangle(this.Location.X + this.Parent.Location.X, this.Location.Y + this.Parent.Location.X, this.Size.Width, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));

            Vector2 size = _font.MeasureString(_text);
            Vector2 pos = new Vector2(
                this.Parent.Location.X + this.Location.X + 5,
                this.Parent.Location.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // TODO: nothing
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // TODO: nothing
        }
        protected override bool IsMouseOnControl(Microsoft.Xna.Framework.Input.MouseState state)
        {
            throw new NotImplementedException();
        }
    }
}
