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
using System.IO;
using GameSharedObject.Components;
using GameSharedObject.DTO;
using GameSharedObject.Data;


namespace GameSharedObject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Technology : Sprite
    {
        #region Properties        
        private TechnologyDTO _techInfo;        

        public TechnologyDTO TechInfo
        {
            get { return _techInfo; }
            set { _techInfo = value; }
        }
        #endregion

        #region Basic method
        public Technology(Game game, string name)
            : base(game)
        {
            // TODO: Construct any child components here
            string path = GlobalDTO.SPEC_TECH_PATH + name + GlobalDTO.SPEC_EXTENSION;
            if (!File.Exists(path))
                throw new Exception("Error! File not found.");
            this._techInfo = (new TechnologyDataReader()).Load(path);
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
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
        #endregion
    }
}