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
    public class %className% : ResearchStructure
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
            this.Info = new StructureDTO();
            StructureDataReader rd = new StructureDataReader();
            this.Info = rd.Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.ES.Name];
            this.CurrentUpgradeInfo = ((StructureDTO)this.Info).UpgradeList[1];
            this.GetInformationStructure(); // lấy thông tin trong file đặc tả
            this.GetListTechnology(); // lấy danh sách các tech
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
            %className% structure = new %className%(GlobalDTO.GAME);
            structure.ModelUnitList = this.ModelUnitList;
            structure.BoundRectangle = this.BoundRectangle;
            structure.CodeFaction = this.CodeFaction;
            structure.Color = this.Color;
            structure.CurrentDirection = new DirectionInfo(this.CurrentDirection);
            structure.CurrentHealth = this.CurrentHealth;
            structure.CurrentIndex = this.CurrentIndex;
            structure.CurrentStatus = new StatusInfo(this.CurrentStatus);
            structure.CurrentUpgradeInfo = this.CurrentUpgradeInfo;
            structure.FlagAttacked = this.FlagAttacked;
            structure.Info = this.Info;
            structure.PathSpecificationFile = this.PathSpecificationFile;
            structure.PercentSize = this.PercentSize;
            structure.PlayerContainer = new Player(GlobalDTO.GAME);
            structure.Position = this.Position;
            structure.RequirementResources = this.RequirementResources;
            structure.SelectedFlag = this.SelectedFlag;
            structure.SelectedImage = this.SelectedImage;            
            structure.TimeToBuildFinish = this.TimeToBuildFinish;
            structure.OwnerUnitList = new List<Sprite>();
            structure.DelayTimeToBuild = this.DelayTimeToBuild;
            structure.ListTechnology = this.ListTechnology;
            return structure;
        }
    }
}