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
using GameSharedObject.DTO;

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class CursorGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties

        private Texture2D _currentTexture;        
        private Texture2D _textureNomal;// hình ảnh trỏ chuột
        private Texture2D _textureSpecial;        
        private Vector2 position;// vị trí trên màn hình Viewport(không phải so với toàn mp)
        private Rectangle _boundRec;

        public Texture2D CurrentTexture
        {
            get { return _currentTexture; }
            set { _currentTexture = value; }
        }
        public Texture2D TextureSpecial
        {
            get { return _textureSpecial; }
            set { _textureSpecial = value; }
        }
        public Rectangle BoundRec
        {
            get { return _boundRec; }
            set { _boundRec = value; }
        }
        public Texture2D TextureNomal
        {
            get { return _textureNomal; }
            set { _textureNomal = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }        
        protected SpriteBatch spriteBatch;

        #endregion

        #region basic method        
        public CursorGame(Game game)
            : base(game)
        {
            spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this._textureNomal = game.Content.Load<Texture2D>(GlobalDTO.RES_CURSOR_PATH + GlobalDTO.CURSOR_NOMAL);
            this._textureSpecial = game.Content.Load<Texture2D>(GlobalDTO.RES_CURSOR_PATH + GlobalDTO.CURSOR_SPECIAL);
            this._currentTexture = this._textureNomal;
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
            if (this.position.X > this.Game.Window.ClientBounds.Width - this._textureNomal.Width)
            {
                this.position.X = this.Game.Window.ClientBounds.Width - this._textureNomal.Width;
            }
            if (this.position.Y > this.Game.Window.ClientBounds.Height - this._textureNomal.Height)
            {
                this.position.Y = this.Game.Window.ClientBounds.Height - this._textureNomal.Height;
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
            // xác định hình chữ nhật bao ngoài để vẽ con trỏ
            this._boundRec = new Rectangle((int)this.position.X, (int)this.position.Y, GlobalDTO.CURSOR_SIZE.Width, GlobalDTO.CURSOR_SIZE.Height);
            if (this._currentTexture == this._textureNomal || this._currentTexture == this._textureSpecial)
            {
                spriteBatch.Draw(this._currentTexture, this._boundRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(this._currentTexture, this._boundRec, Color.Green);
            }

            base.Draw(gameTime);
        }
        #endregion
    }
}