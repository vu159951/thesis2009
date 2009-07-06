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
    public class %className%: Unit
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
            // this.Position = position;
            // this.CodeFaction = codeFaction;
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.MovingVector = Vector2.Zero;// now is IDLE
            this.CurrentIndex = 0;
            this.WhomIHit = null;// người bị đó tấn công
            this.PlayerContainer = null; // player mà nó trực thuộc
            this.StructureContainer = null;// player mà nó trực thuộc

            UnitDataReader unitReader = new UnitDataReader();
            this.Info = new UnitDTO();
            this.Info = unitReader.Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];            
            this.GetInformationUnit(); // lấy thông tin trong file đặc tả
			
			// Khởu tạo attack particle
			Dictionary<string, ItemInfo> infoList = ((UnitDTO)this.Info).InformationList;
            if (infoList.ContainsKey("AttackParticle") && !String.IsNullOrEmpty(infoList["AttackParticle"].Value))
                this.ParticleAttack = new Particle(game, infoList["AttackParticle"].Value);
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
            %className% unit = new %className%(GlobalDTO.GAME);
            unit.BoundRectangle = this.BoundRectangle;
            unit.CodeFaction = this.CodeFaction;
            unit.Color = this.Color;
            unit.CurrentDirection = new DirectionInfo(this.CurrentDirection);
            unit.CurrentHealth = this.CurrentHealth;
            unit.CurrentIndex = this.CurrentIndex;
            unit.CurrentStatus = new StatusInfo(this.CurrentStatus);
            unit.EndPoint = this.EndPoint;
            unit.FlagBeAttacked = this.FlagBeAttacked;
            unit.Info = this.Info;
            unit.MovingVector = this.MovingVector;
            unit.ParticleAttack = this.ParticleAttack;
            unit.PathSpecificationFile = this.PathSpecificationFile;
            unit.PercentSize = this.PercentSize;
            unit.PlayerContainer = new Player(GlobalDTO.GAME);
            unit.Position = this.Position;
            unit.RequirementResources = this.RequirementResources;
            unit.SelectedFlag = this.SelectedFlag;
            unit.SelectedImage = this.SelectedImage;
            unit.StructureContainer = this.StructureContainer;
            unit.TimeToBuyFinish = this.TimeToBuyFinish;
            unit.WhomIHit = this.WhomIHit;
            return unit;
        }		
        #endregion
    }
}
