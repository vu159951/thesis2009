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
using System.IO;
using GameSharedObject.Data;

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Particle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties        
        private ParticleDTO _particleInfo;// thông tin particle   
        private Vector2 _position;// vị trí phát particle
        private int _indexImage;// index của hình hiện tại

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
        #endregion

        #region Basic method
        
        public Particle(Game game, string name)
            : base(game)
        {
            // TODO: Construct any child components here
            string path = GlobalDTO.SPEC_PARTICLE_PATH + name + GlobalDTO.SPEC_EXTENSION;
            if (!File.Exists(path))
                throw new Exception("Error! File not found.");
            this._particleInfo = (new ParticleDataReader()).Load(path);
            this._position = Vector2.Zero;
            this._indexImage = 0;            
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
        #endregion
    }
}