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

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class CursorGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // =====================================================================================
        //          Properties
        // =====================================================================================
        private Texture2D texture;
        private Vector2 position;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected SpriteBatch spriteBatch;

        // ===============================================================================================
        //              Method
        // ===============================================================================================
        public CursorGame(Game game, Texture2D texture)
            : base(game)
        {
            spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this.texture = texture;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Called when graphics resources need to be loaded.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Add your load content code here

            base.LoadContent();
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Add your unload content code here

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            MouseState mouse = Mouse.GetState();
            if (this.position.X != mouse.X || this.position.Y != mouse.Y)
            {

                this.position = new Vector2(mouse.X, mouse.Y);

            }
            if (this.position.X < 0)
            {
                this.position.X = 0;
            }
            if (this.position.Y < 0)
            {
                this.position.Y = 0;
            }
            if (this.position.X > this.Game.Window.ClientBounds.Width - this.texture.Width)
            {
                this.position.X = this.Game.Window.ClientBounds.Width - this.texture.Width;
            }
            if (this.position.Y > this.Game.Window.ClientBounds.Height - this.texture.Height)
            {
                this.position.Y = this.Game.Window.ClientBounds.Height - this.texture.Height;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here
            Rectangle rec = new Rectangle((int)this.position.X, (int)this.position.Y, Config.CURSOR_SIZE.Width, Config.CURSOR_SIZE.Height);
            spriteBatch.Draw(this.texture, rec, Color.White);

            base.Draw(gameTime);
        }
    }
}