using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameSharedObject;
using GameSharedObject.DTO;
using GameSharedObject.Data;
using GameSharedObject.Components;

namespace %asmNamespace%.Objects
{
    public class %className%: %className%Unit
    {
        #region Basic methods
        // =====================================================================================================
        // ============================================= basic Methods ================================================
        // =====================================================================================================
        public %className%(Game game)
            :base(game)
        {        

        }

        public %className%(Game game, string pathspecificationfile)
            : base(game)
        {
			%extAttribute%
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.MovingVector = Vector2.Zero;
            this.CurrentIndex = 0;
            this.WhomIHit = null;// người bị đó tấn công
            this.PlayerContainer = null; // player mà nó trực thuộc
            this.StructureContainer = null;// player mà nó trực thuộc

            // loại tài nguyên hiện tài mà %className% đang khai thác
            this.CurrentResourceExploiting = null;
            // mõ tài nguyên hiện tại mà %className% đang khai thác
            this.CurrentResourceCenterExploiting = null;
            // số lượng khai thác tối đa
            this.MaxExploit = 100;
            // tốc độ khai thác
            this.SpeedExploit = 1;

            UnitDataReader unitReader = new UnitDataReader();
            this.Info = new UnitDTO();
            this.Info = unitReader.Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];            
            this.GetInformationUnit(); // lấy thông tin trong file đặc tả
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
            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
		
		public override object Clone()
        {
            %className% pUnit = new %className%(GlobalDTO.GAME);
            pUnit.BoundRectangle = this.BoundRectangle;
            pUnit.CodeFaction = this.CodeFaction;
            pUnit.Color = this.Color;
            pUnit.CurrentDirection = new DirectionInfo(this.CurrentDirection);
            pUnit.CurrentHealth = this.CurrentHealth;
            pUnit.CurrentIndex = this.CurrentIndex;
            pUnit.CurrentResourceCenterExploiting = this.CurrentResourceCenterExploiting;
            pUnit.CurrentResourceExploiting = new Resource();
            pUnit.CurrentStatus = new StatusInfo(this.CurrentStatus);
            pUnit.FlagBeAttacked = false;
            pUnit.Info = this.Info;
            pUnit.MovingVector = Vector2.Zero;
            pUnit.ParticleAttack = this.ParticleAttack;
            pUnit.PathSpecificationFile = this.PathSpecificationFile;
            pUnit.PercentSize = this.PercentSize;
            pUnit.PlayerContainer = new Player(GlobalDTO.GAME);
            pUnit.Position = this.Position;
            pUnit.RequirementResources = this.RequirementResources;
            pUnit.SelectedFlag = this.SelectedFlag;
            pUnit.SelectedImage = this.SelectedImage;
            pUnit.SpeedExploit = this.SpeedExploit;
            pUnit.StructureContainer = this.StructureContainer;
            pUnit.TimeToBuyFinish = this.TimeToBuyFinish;
            pUnit.WhomIHit = this.WhomIHit;
            return pUnit;
        }
        #endregion
    }
}
