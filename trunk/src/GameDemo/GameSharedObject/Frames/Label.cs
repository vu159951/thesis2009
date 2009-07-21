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
        public delegate void TextChangedHandler(object sender, EventArgs e);
        public event TextChangedHandler TextChanged;
        protected void OnTextChanged(EventArgs e)
        {
            if (this.TextChanged != null)
                this.TextChanged(this, e);
        }

        private String _text;
        private SpriteFont _font;
        private Color _foreColor;

        public String Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                this.OnTextChanged(new EventArgs());
            }
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

        #region Local variable
        private Point parent;
        #endregion

        public Label(Game game)
            : base(game) {
                parent = new Point(0, 0);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
            if (this.Parent == null){
                parent.X = 0;
                parent.Y = 0;
            }else{
                parent.X = this.Location.X;
                parent.Y = this.Location.Y;
            }
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
               parent.X + this.Location.X + 3,
                parent.Y + this.Location.Y);
            spriteBatch.DrawString(_font, _text, pos, _foreColor);
        }
        protected override bool IsMouseOnControl(MouseState state)
        {
            if (state.X >= this.Location.X + this.parent.X &&
                state.X <= this.Location.X + parent.X + this.Size.Width &&
                state.Y >= this.Location.Y + parent.Y &&
                state.Y <= this.Location.Y + parent.Y + this.Size.Height)
                return true;
            return false;

        }
    }
}
