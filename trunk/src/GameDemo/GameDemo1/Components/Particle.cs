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
using GameDemo1.DTO;

namespace GameDemo1.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Particle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private ParticleDTO _particleInfo;        
        private Vector2 _position;
        private int _indexImage;

        public ParticleDTO ParticleInfo
        {
            get { return _particleInfo; }
            set { _particleInfo = value; }
        }
        public int IndexImage
        {
            get { return _indexImage; }
            set { _indexImage = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }       
        public Particle(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            this._position = Vector2.Zero;
            this._indexImage = 0;
            this._particleInfo = new ParticleDTO();
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

            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here            
            base.Draw(gameTime);
        }                
    }
}