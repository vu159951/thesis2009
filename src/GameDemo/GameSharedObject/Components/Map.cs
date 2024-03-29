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

namespace GameSharedObject.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable.
    /// </summary>
    public abstract class Map : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Properties

        protected MapCell[,] cells; // matrix for drawing, get from _bgMatrix, ma trận các cell trong map
        protected Viewport _viewArea; // vùng viewport
        protected Vector2 _currentRootCoordinate; // vị trí của góc trái trên của viewport trong hệ toa độ toàn map
        protected int[,] _bgMatrix;// ma trận số biểu diễn lát nền map bằng hình thoi hoặc hình vuông
        private int[,] _occupiedMatrix;
        protected Transform transform; // đối tượng chuyển đổi cell <-> pixel
        protected string _pathSpecificationFile; // đường dẫn đến file mô tả map

        protected SpriteBatch spriteBatch = null;
        protected KeyboardState keyState;
        protected MouseState mouseState;

        public int[,] BgMatrix
        {
            get { return _bgMatrix; }
            set { _bgMatrix = value; }
        }
        public int[,] OccupiedMatrix
        {
            get { return _occupiedMatrix; }
            set { _occupiedMatrix = value; }
        }
        public string PathSpecificationFile
        {
            get { return _pathSpecificationFile; }
            set { _pathSpecificationFile = value; }
        }
        public Transform Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        public MapCell[,] Cells
        {
            get { return cells; }
            set { cells = value; }
        }
        public Viewport ViewArea
        {
            get { return _viewArea; }
            set { _viewArea = value; }
        }
        public Vector2 CurrentRootCoordinate
        {
            get { return _currentRootCoordinate; }
            set { _currentRootCoordinate = value; }
        }

        #endregion

        #region basic Method
        public Map(Game game) : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
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
            this.ScrollingMapByKeyBoard();
            this.ScrollingMapByMouse();            
            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your draw code here
            // nếu đang trong quá trình loading thì tạm thời chưa vẽ ra màn hình
            if (GlobalDTO.CURRENT_MODEGAME == "Playing")
            {
                this.DrawBackGround();
            }
            else
            {
                return;
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Function
        /// <summary>
        /// Abstract funtion
        /// </summary>
        protected abstract void DrawBackGround();// draw cell on map
        protected abstract void LoadMapCells(int[,] matrixmap); // moad map from matrix[,]
        protected abstract void ScrollingMapByKeyBoard(); // scroll map by keyboard
        protected abstract void ScrollingMapByMouse(); // scroll map by mouse
        #endregion
    }
}