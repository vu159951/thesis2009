using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GameSharedObject;
using GameSharedObject.DTO;
using GameSharedObject.Data;
using GameSharedObject.Components;


namespace %asmNamespace%.Objects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class %className% : ResourceCenter
    {
        public %className%(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }

        public %className%(Game game, string pathspecificationfile)
            : base(game)
        {
			%extAttribute%
            this.Info = new ResourceCenterDTO();
            this.Info = (new ResourceCenterDataReader()).Load(pathspecificationfile);
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

		public override object Clone()
        {
            %className% resourcecenter = new %className%(GlobalDTO.GAME);
            resourcecenter.BoundRectangle = this.BoundRectangle;
            resourcecenter.CodeFaction = this.CodeFaction;
            resourcecenter.Color = this.Color;
            resourcecenter.CurrentDirection = new DirectionInfo(this.CurrentDirection);
            resourcecenter.CurrentIndex = this.CurrentIndex;
            resourcecenter.CurrentStatus = new StatusInfo(this.CurrentStatus);
            resourcecenter.Info = this.Info;
            resourcecenter.PathSpecificationFile = this.PathSpecificationFile;
            resourcecenter.PercentSize = this.PercentSize;
            resourcecenter.Position = this.Position;
            resourcecenter.ResourceInfo = this.ResourceInfo;
            resourcecenter.SelectedFlag = this.SelectedFlag;
            resourcecenter.SelectedImage = this.SelectedImage;            
            return resourcecenter;
        }
    }
}