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
using GameSharedObject.Data;

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public class Terrain : Sprite
    {
        #region Properties
        #endregion

        #region Basic function

        public Terrain(Game game)
            : base(game) 
        {

        }
        public Terrain(Game game, string pathspecificationfile, Vector2 position)
            : base(game)
        {
            // TODO: Construct any child components here
            this.PercentSize = 0.5f; // set kích thước theo tỷ lệ phần trăm
            this.Position = position;// for position // set vị trí tính theo tọa độ map
            this.PathSpecificationFile = pathspecificationfile;// get path to specification file // chỉ định đường dẫn tới xml đặc tả
            //this.GetSetOfTexturesForSprite(pathspecificationfile);// get texture // lấy tập ảnh
            this.CodeFaction = 0; // is neutral object // mã của phe trung lập

            this.Info = new TerrainDTO();
            this.Info = (new TerrainDataReader()).Load(pathspecificationfile);
            this.CurrentStatus = this.Info.Action[StatusList.IDLE.Name];
            this.CurrentDirection = this.Info.Action[this.CurrentStatus.Name].DirectionInfo[DirectionList.S.Name];            
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