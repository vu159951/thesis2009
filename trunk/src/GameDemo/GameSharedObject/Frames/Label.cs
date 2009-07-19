using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Label : Control
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

        public Label(Game game)
            : base(game) { }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // TODO: Add your draw code here
            Vector2 pos = new Vector2(
                this.Parent.Location.X + this.Location.X + 3,
                this.Parent.Location.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
        }
        protected override bool IsMouseOnControl(MouseState state)
        {
            if (state.X >= this.Location.X + this.Parent.Location.X &&
                state.X <= this.Location.X + this.Parent.Location.X + this.Size.Width &&
                state.Y >= this.Location.Y + this.Parent.Location.Y &&
                state.Y <= this.Location.Y + this.Parent.Location.Y + this.Size.Height)
                return true;
            return false;

        }
    }
}
