﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
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

        public %className%(Game game, string pathspecificationfile, string particleSpecificationFile, Vector2 position, int codeFaction)
            : base(game)
        {
			%extAttribute%
            this.Position = position;
            this.CodeFaction = codeFaction;
            this.PathSpecificationFile = pathspecificationfile;//get file for specification
            this.MovingVector = Vector2.Zero;
            this.CurrentIndex = 0;
            this.WhomIHit = null;// người bị đó tấn công
            this.PlayerContainer = null; // player mà nó trực thuộc
            this.StructureContainer = null;// player mà nó trực thuộc

            // lấy tập hình particle
            ParticleDataReader particleReader = new ParticleDataReader();
            this.ParticleAttack = new Particle(game);
            if (particleSpecificationFile != "")
            {
                this.ParticleAttack.ParticleInfo = particleReader.Load(particleSpecificationFile);
            }

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

        #endregion
    }
}
