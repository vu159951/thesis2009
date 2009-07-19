using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GameSharedObject.Frames
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Form : Control
    {
        private Game _game;
        private ControlCollection _controls;
        public ControlCollection Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }
        
        public Form(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            _game = game;
            _controls = new ControlCollection();
            _controls.Adding += new ControlCollection.AddingHandler(_controls_Adding);
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
                new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height),
                new Color(Color.White, (float)this._opacity * 0.01f));
        }

        private void _controls_Adding(object sender, Control item)
        {
            item.Parent = this;
            _game.Components.Add(item);
        }

        protected override bool IsMouseOnControl(MouseState state)
        {
            if (state.X >= this.Location.X && state.X <= this.Location.X + this.Size.Width)
                if (state.Y >= this.Location.Y && state.Y <= this.Location.Y + this.Size.Height)
                {
                    // Check for mouse is on child controls
                    for (int i = _controls.Count - 1; i >= 0; i--){
                        Control ctl = _controls[i];
                        if (!(ctl is Form)){
                            if (state.X >= this.Location.X + ctl.Location.X &&
                                state.X <= this.Location.X + ctl.Location.X + ctl.Size.Width &&
                                state.Y >= this.Location.Y + ctl.Location.Y &&
                                state.Y <= this.Location.Y + ctl.Location.Y + ctl.Size.Height)
                                return false;
                            }
                    }
                    return true;
                }
            return false;
        }
    }
}
