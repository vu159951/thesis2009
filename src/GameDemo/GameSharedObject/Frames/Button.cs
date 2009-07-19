using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Button : Control
    {
        private bool isDown;
        public delegate void ClickHandler(object sender, EventArgs e);
        public event ClickHandler Click;
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                this.Click(this, e);
        }

        public Button(Game game)
            : base(game) { isDown = false; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isDown = true;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDown)
                this.OnClick(new EventArgs());
            isDown = false;
        }

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
            spriteBatch.Draw(_background,
                new Rectangle(this.Location.X + this.Parent.Location.X, 
                        this.Location.Y + this.Parent.Location.Y, 
                        this.Size.Width, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));
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
