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
using GameSharedObject.Components;


namespace GameSharedObject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Technology : Microsoft.Xna.Framework.GameComponent
    {
        private string _nameTech;
        private List<Structure> _upgradeStructure;

        public List<Structure> UpgradeStructure
        {
            get { return _upgradeStructure; }
            set { _upgradeStructure = value; }
        }
        public string NameTech
        {
            get { return _nameTech; }
            set { _nameTech = value; }
        }
        // ----------------------------------------------------------------------------------------------------
        //                      Methods
        // ----------------------------------------------------------------------------------------------------
        public Technology(Game game)
            : base(game)
        {
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
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}