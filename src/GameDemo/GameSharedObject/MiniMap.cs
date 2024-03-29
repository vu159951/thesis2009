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
using GameSharedObject.DTO;

namespace GameSharedObject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MiniMap : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties
        private Point _rootPosition = Point.Zero; // vị trí đặt minimap
        private Texture2D _background;// nền mini map
        private Texture2D _displayPoint;// điểm biểu thị cho các thành phần minimap
        private Texture2D _viewport;// khung vùng nhìn

        public Texture2D Viewport
        {
            get { return _viewport; }
            set { _viewport = value; }
        }
        public Texture2D DisplayPoint
        {
            get { return _displayPoint; }
            set { _displayPoint = value; }
        }
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }
        public Point RootPosition
        {
            get { return _rootPosition; }
            set { _rootPosition = value; }
        }

        private SpriteBatch spriteBatch;// spritebatch obj // sprite để vẽ hình
        #endregion

        #region Basic Method
        public MiniMap(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public MiniMap(Game game, Point position)
            :base(game)
        {
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            this._background = game.Content.Load<Texture2D>(GlobalDTO.RES_MINI_MAP_PATH + GlobalDTO.MINIMAP);
            this._rootPosition = position;
            this._displayPoint = game.Content.Load<Texture2D>(GlobalDTO.RES_MINI_MAP_PATH + "Point");
            this._viewport = game.Content.Load<Texture2D>(GlobalDTO.RES_MINI_MAP_PATH + "Rectangle");
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

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (GlobalDTO.CURRENT_MODEGAME == "Playing")
            {
                spriteBatch.Draw(this._background, new Rectangle(this._rootPosition.X, this._rootPosition.Y, this._background.Width, this._background.Height), Color.White);
                float percent = this._background.Height * 1.0f / (GlobalDTO.CURRENT_CELL_SIZE.Height * GlobalDTO.MAP_SIZE_IN_CELL.Height * 1.0f);
                for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListUnitOnMap.Count; i++)// vẽ vị trí các unit trên map với màu theo player
                {
                    // tính ra vị trí vẽ unit trên mini map hợp với vị trí thực của unit trên map
                    Vector2 position = new Vector2(GlobalDTO.MANAGER_GAME.ListUnitOnMap[i].Position.X * percent + this._rootPosition.X, GlobalDTO.MANAGER_GAME.ListUnitOnMap[i].Position.Y * percent + this._rootPosition.Y);
                    spriteBatch.Draw(this._displayPoint, new Rectangle((int)position.X, (int)position.Y, 2, 2), ((Unit)GlobalDTO.MANAGER_GAME.ListUnitOnMap[i]).PlayerContainer.Color);
                }
                for (int i = 0; i < GlobalDTO.MANAGER_GAME.ListStructureOnMap.Count; i++) // vẽ vị trí các structure trên map với màu player
                {
                    Vector2 position = new Vector2(GlobalDTO.MANAGER_GAME.ListStructureOnMap[i].Position.X * percent + this._rootPosition.X, GlobalDTO.MANAGER_GAME.ListStructureOnMap[i].Position.Y * percent + this._rootPosition.Y);
                    spriteBatch.Draw(this._displayPoint, new Rectangle((int)position.X, (int)position.Y, 5, 5), ((Structure)GlobalDTO.MANAGER_GAME.ListStructureOnMap[i]).PlayerContainer.Color);
                }

                // vẽ khung view port cho mini map
                spriteBatch.Draw(this._viewport, new Rectangle((int)(GlobalDTO.CURRENT_COORDINATE.X * percent) + this._rootPosition.X, (int)(GlobalDTO.CURRENT_COORDINATE.Y * percent) + this._rootPosition.Y, (int)(GlobalDTO.SCREEN_SIZE.Width * percent), (int)(GlobalDTO.SCREEN_SIZE.Height * percent)), Color.White);
            }
            base.Draw(gameTime);
        }
        #endregion
    }
}