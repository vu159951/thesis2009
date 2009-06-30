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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StoneResourceCenter : ResourceCenter
    {
        public StoneResourceCenter(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }

        public StoneResourceCenter(Game game, string pathspecificationfile, Vector2 position)
            : base(game)
        {
            this.Position = position;            
            this.Info = new ResourceCenterDTO();
            this.Info = GlobalDTO.RESOURCECENTER_DATA_READER.Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];
            this.ResourceInfo = new Resource(((ResourceCenterDTO)this.Info).ResourceInfo["NameResource"].Value, int.Parse(((ResourceCenterDTO)this.Info).ResourceInfo["Container"].Value));
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}