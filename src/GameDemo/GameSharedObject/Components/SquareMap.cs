﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameSharedObject.DTO;
using GameSharedObject.Data;

namespace GameSharedObject.Components
{
    public class SquareMap : Map
    {
        public readonly System.Drawing.Size CELL_SIZE = new System.Drawing.Size(64, 64); // kích thước cell hình uông để lát nền map
        public readonly Point ROOT_Vector2 = new Point(0,0); // vị trí gốc của map

        public SquareMap(Game game, string pathSpecificationFile, Vector2 currentrootcoordiante): base(game)
        {
            this._currentRootCoordinate = currentrootcoordiante;
            this._pathSpecificationFile = pathSpecificationFile;
            GlobalDTO.CURRENT_CELL_SIZE = CELL_SIZE;
            Transform = new SquareTransform(ROOT_Vector2, CELL_SIZE.Width, CELL_SIZE.Height);

            // load ma trận số mô tả cách lát nền và thực hiện lát nền cho map
            this._bgMatrix = MatrixMgr.Read(this._pathSpecificationFile).Data;
            this.LoadMapCells(this._bgMatrix);// load cell hình để lát nền
        }

        /// <summary>
        ///  Scroll map bằng phím
        /// </summary>
        protected override void ScrollingMapByKeyBoard()
        {
            this.keyState = Keyboard.GetState(); // get key
            if (keyState.IsKeyDown(Keys.Up))
            {
                this._currentRootCoordinate.Y -= GlobalDTO.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                this._currentRootCoordinate.Y += GlobalDTO.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (GlobalDTO.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = GlobalDTO.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                this._currentRootCoordinate.X -= GlobalDTO.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                this._currentRootCoordinate.X += GlobalDTO.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (GlobalDTO.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = GlobalDTO.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
            GlobalDTO.CURRENT_COORDINATE = this._currentRootCoordinate;
            return;
        }

        /// <summary>
        /// Scrool map bằng chuột
        /// </summary>
        protected override void ScrollingMapByMouse()
        {
            this.mouseState = Mouse.GetState();
            if (mouseState.X <= 0)
            {
                this._currentRootCoordinate.X -= GlobalDTO.SPEED_SCROLL.X; // scroll left
                if (this._currentRootCoordinate.X < 0)
                {
                    this._currentRootCoordinate.X = 0;
                }
            }
            if (mouseState.Y <= 0)
            {
                this._currentRootCoordinate.Y -= GlobalDTO.SPEED_SCROLL.Y;// scrool up
                if (this._currentRootCoordinate.Y < 0)// if can't scroll continuous, stand here
                {
                    this._currentRootCoordinate.Y = 0;
                }
            }
            if (mouseState.X >= Game.Window.ClientBounds.Width - GlobalDTO.CURSOR_SIZE.Width)
            {
                this._currentRootCoordinate.X += GlobalDTO.SPEED_SCROLL.X; // scroll right
                if (this._currentRootCoordinate.X > (GlobalDTO.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width))
                {
                    this._currentRootCoordinate.X = GlobalDTO.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width - Game.Window.ClientBounds.Width;
                }
            }
            if (mouseState.Y >= Game.Window.ClientBounds.Height - GlobalDTO.CURSOR_SIZE.Height)
            {
                this._currentRootCoordinate.Y += GlobalDTO.SPEED_SCROLL.Y;// scrool down
                if (this._currentRootCoordinate.Y > (GlobalDTO.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height))
                {
                    this._currentRootCoordinate.Y = GlobalDTO.MAP_SIZE_IN_CELL.Height * CELL_SIZE.Height - Game.Window.ClientBounds.Height;
                }
            }
            GlobalDTO.CURRENT_COORDINATE = this._currentRootCoordinate;
            return;
        }

        /// <summary>
        /// Tính ra các cell sẽ vẽ trong viewport -> chỉ có cell trong view port mới được vẽ
        /// </summary>
        protected override void DrawBackGround()
        {
            int i1 = (int)this._currentRootCoordinate.X / CELL_SIZE.Width;// get x index of cell at start view area
            int j1 = (int)this._currentRootCoordinate.Y / CELL_SIZE.Height;// get y index of cell at start view area
            int i2 = (int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width) / CELL_SIZE.Width; // get x index of cell at end view area
            int j2 = (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height) / CELL_SIZE.Height;// get y index of cell at end view area
            {
                for (int i = i1; i <= i2; i++){
                    for (int j = j1; j <= j2; j++){ // với mỗi cell trong view port
                        try
                        {
                            // draw cell in above index
                            // tính ra vị trí và vẽ chúng
                            Rectangle recToDraw = new Rectangle((int)(this.cells[i, j].X - this._currentRootCoordinate.X), (int)(this.cells[i, j].Y - this._currentRootCoordinate.Y), CELL_SIZE.Width, CELL_SIZE.Height);// calculating new postion of cell with current root coodinate
                            spriteBatch.Draw(this.cells[i, j].Background, recToDraw, Color.White);
                        } catch
                        { }
                    }
                }
            }
        }

        /// <summary>
        /// Load hình của cell lên để lát vào map
        /// </summary>
        /// <param name="matrixmap"></param>
        protected override void LoadMapCells(int[,] matrixmap)
        {
            this.cells = new MapCell[GlobalDTO.MAP_SIZE_IN_CELL.Width, GlobalDTO.MAP_SIZE_IN_CELL.Height];
            for (int i = 0; i < GlobalDTO.MAP_SIZE_IN_CELL.Width; i++){
                for (int j = 0; j < GlobalDTO.MAP_SIZE_IN_CELL.Height; j++){
                    //this.cells[i, j] = new MapCell(imageofcell, i * CELL_SIZE.Width, j * CELL_SIZE.Height);
                    this.cells[i, j] = new MapCell(Game.Content.Load<Texture2D>(GlobalDTO.RES_SQUARE_MAP_PATH + "BG0002"), i * CELL_SIZE.Width, j * CELL_SIZE.Height);
                }
            }
        }
    }
}
