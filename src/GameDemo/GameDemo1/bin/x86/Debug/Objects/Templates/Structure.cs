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
using GameSharedObject.DTO;
using GameSharedObject.Data;
using GameSharedObject.Components;


namespace %asmNamespace%.Objects
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class %className% : Structure
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
            this.Position = position;
            this.CodeFaction = codeFaction;
            this.Info = new StructureDTO();
            DataReader rd = new StructureDataReader();
            this.Info = rd.Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];
            this.CurrentUpgradeInfo = ((StructureDTO)this.Info).UpgradeList[1];
            this.GetInformationStructure(); // lấy thông tin trong file đặc tả
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