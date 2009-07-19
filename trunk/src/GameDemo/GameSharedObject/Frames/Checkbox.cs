using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.Frames
{
    public class Checkbox : Button
    {
        private bool _isChecked;

        public delegate void CheckChangedHandler(object sender, EventArgs e);
        public event CheckChangedHandler CheckChanged;
        protected virtual void OnCheckChanged(EventArgs e)
        {
            if (CheckChanged != null)
                this.CheckChanged(this, e);
        }

        #region Variable Local
        protected Texture2D bgCheck;
        protected Texture2D bgUncheck;
        #endregion

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public Checkbox(Game game)
            : base(game) {
            this._isChecked = false;
            this.Background = bgUncheck;
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


        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (_isChecked){
                _isChecked = false;
                this._background = bgUncheck;
                this.OnCheckChanged(new EventArgs());
            }else if (!_isChecked){
                _isChecked = true;
                this._background = bgCheck;
                this.OnCheckChanged(new EventArgs());
            }

        }
        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here
            spriteBatch.Draw(_background,
                new Rectangle(this.Location.X + this.Parent.Location.X,
                        this.Location.Y + this.Parent.Location.Y,
                        this.Size.Height, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));
        }
        protected override bool IsMouseOnControl(MouseState state)
        {
            return base.IsMouseOnControl(state);
        }
    }
}
