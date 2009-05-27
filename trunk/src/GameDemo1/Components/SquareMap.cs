using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDemo1.HelperObjects;
using GameDemo1.Input;

namespace GameDemo1.Components
{
    public class SquareMap : Map
    {
        public readonly System.Drawing.Size CELL_SIZE = new System.Drawing.Size(64, 64);
        public readonly Point ROOT_Vector2 = new Point(0,0);

        public SquareMap(Game game, string pathSpecificationFile, Vector2 currentrootcoordiante): base(game)
        {
            this._currentRootCoordinate = currentrootcoordiante;
            this._pathSpecificationFile = pathSpecificationFile;
            Config.CURRENT_CELL_SIZE = CELL_SIZE;
            Transform = new SquareTransform(ROOT_Vector2, CELL_SIZE.Width, CELL_SIZE.Height);

            MatrixMgr matrixmgr = new MatrixMgr();
            matrixmgr.Read(this._pathSpecificationFile);
            this._bgMatrix = matrixmgr.Matrix;
            this.LoadMapCells(this._bgMatrix);
        }

        protected override void ScrollingMapByKeyBoard()
        {
            this.keyState = Keyboard.GetState(); // get key
            if (keyState.IsKeyDown(Keys.Up))
            {
                this._currentRootCoordinate.Y -= Config.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                this._currentRootCoordinate.Y += Config.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (Config.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = Config.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                this._currentRootCoordinate.X -= Config.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                this._currentRootCoordinate.X += Config.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (Config.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = Config.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
            Config.CURRENT_COORDINATE = this._currentRootCoordinate;
            return;
        }
        protected override void ScrollingMapByMouse()
        {
            this.mouseState = Mouse.GetState();
            if (mouseState.X <= 0)
            {
                this._currentRootCoordinate.X -= Config.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (mouseState.Y <= 0)
            {
                this._currentRootCoordinate.Y -= Config.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (mouseState.X >= Game.Window.ClientBounds.Width - Config.CURSOR_SIZE.Width)
            {
                this._currentRootCoordinate.X += Config.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (Config.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = Config.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
            if (mouseState.Y >= Game.Window.ClientBounds.Height - Config.CURSOR_SIZE.Height)
            {
                this._currentRootCoordinate.Y += Config.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (Config.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = Config.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            Config.CURRENT_COORDINATE = this._currentRootCoordinate;
            return;
        }
        protected override void DrawBackGround()
        {
            int i1 = (int)this._currentRootCoordinate.X / CELL_SIZE.Width;// get x index of cell at start view area
            int j1 = (int)this._currentRootCoordinate.Y / CELL_SIZE.Height;// get y index of cell at start view area
            int i2 = (int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width) / CELL_SIZE.Width; // get x index of cell at end view area
            int j2 = (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height) / CELL_SIZE.Height;// get y index of cell at end view area
            {
                for (int i = i1; i <= i2; i++){
                    for (int j = j1; j <= j2; j++){
                        try
                        {
                            // draw cell in above index
                            Rectangle recToDraw = new Rectangle((int)(this.cells[i, j].X - this._currentRootCoordinate.X), (int)(this.cells[i, j].Y - this._currentRootCoordinate.Y), CELL_SIZE.Width, CELL_SIZE.Height);// calculating new postion of cell with current root coodinate
                            spriteBatch.Draw(this.cells[i, j].Background, recToDraw, Color.White);
                        } catch
                        { }
                    }
                }
            }
        }
        protected override void LoadMapCells(int[,] matrixmap)
        {
            this.cells = new MapCell[Config.MAP_SIZE_IN_CELL.Width, Config.MAP_SIZE_IN_CELL.Height];
            for (int i = 0; i < Config.MAP_SIZE_IN_CELL.Width; i++){
                for (int j = 0; j < Config.MAP_SIZE_IN_CELL.Height; j++){
                    //this.cells[i, j] = new MapCell(imageofcell, i * CELL_SIZE.Width, j * CELL_SIZE.Height);
                    this.cells[i, j] = new MapCell(Game.Content.Load<Texture2D>(Config.PATH_TO_SQUARE_MAP_IMAGE + "BG0002"), i * CELL_SIZE.Width, j * CELL_SIZE.Height);
                }
            }
        }
    }
}
