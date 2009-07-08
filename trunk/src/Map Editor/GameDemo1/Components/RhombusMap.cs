using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDemo1.DTO;
using GameDemo1.Data;

namespace GameDemo1.Components
{
    public class RhombusMap : Map
    {
        public readonly System.Drawing.Size CELL_SIZE = new System.Drawing.Size(97, 49);
        public readonly Point ROOT_Vector2;
        public SpriteFont font;

        public RhombusMap(Game game, string pathSpecificationFile, Vector2 currentrootcoordiante)
            : base(game)
        {
            this._currentRootCoordinate = currentrootcoordiante;
            this._pathSpecificationFile = pathSpecificationFile;
            Config.CURRENT_CELL_SIZE = CELL_SIZE;
            ROOT_Vector2 = new Point((CELL_SIZE.Width >> 1) * (Config.MAP_SIZE_IN_CELL.Width - 1), 0);
            Transform = new RomhbusTransform(ROOT_Vector2, CELL_SIZE.Width, CELL_SIZE.Height);

            font = game.Content.Load<SpriteFont>(Config.PATH_TO_FONT);
            // get matrix from file and load map
            this._bgMatrix = MatrixMgr.Read(this._pathSpecificationFile).Data;
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
            this.mouseState = Mouse.GetState(); // get mouse
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
            // calculate which cells to draw
            Point cell1 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width),(int) (this._currentRootCoordinate.Y)));
            Point cell2 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X), (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height)));
            Point cell3 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X), (int)(this._currentRootCoordinate.Y)));
            Point cell4 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width), (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height)));

            int i1 = (int)cell3.Y;// C
            if (i1 < 0)
            {
                i1 = 0;
            }
            if (i1 > Config.MAP_SIZE_IN_CELL.Height)
            {
                i1 = Config.MAP_SIZE_IN_CELL.Height;
            }

            int i2 = (int)cell4.Y + 2;// D
            if (i2 < 0)
            {
                i2 = 0;
            }
            if (i2 > Config.MAP_SIZE_IN_CELL.Height)
            {
                i2 = Config.MAP_SIZE_IN_CELL.Height;
            }

            int j1 = (int)cell1.X;// A
            if (j1 < 0)
            {
                j1 = 0;
            }
            if (j1 > Config.MAP_SIZE_IN_CELL.Width)
            {
                j1 = Config.MAP_SIZE_IN_CELL.Width;
            }

            int j2 = (int)cell2.X + 2;// B
            if (j2 < 0)
            {
                j2 = 0;
            }
            if (j2 > Config.MAP_SIZE_IN_CELL.Width)
            {
                j2 = Config.MAP_SIZE_IN_CELL.Width;
            }

            for (int i = i1; i <= i2; i++)
            {
                for (int j = j1; j <= j2; j++)
                {
                    try
                    {
                        // draw cell in above index
                        Rectangle recToDraw = new Rectangle(
                            (int)(this.cells[i, j].X - this._currentRootCoordinate.X),
                            (int)(this.cells[i, j].Y - this._currentRootCoordinate.Y),
                            CELL_SIZE.Width, CELL_SIZE.Height);// calculating new postion of cell with current root coodinate
                        spriteBatch.Draw(this.cells[i, j].Background, recToDraw, Color.White);
                        spriteBatch.DrawString(
                            font,
                            this._bgMatrix[i, j].ToString(),
                            new Vector2(
                            this.cells[i, j].X - this._currentRootCoordinate.X + this.CELL_SIZE.Width / 2 - 7,
                            this.cells[i, j].Y - this._currentRootCoordinate.Y + this.CELL_SIZE.Height / 2 - 5),
                            Color.Red);
                    }
                    catch
                    { }
                }
            }
        }

        protected override void LoadMapCells(int[,] matrixmap)
        {
            this.cells = new MapCell[Config.MAP_SIZE_IN_CELL.Width, Config.MAP_SIZE_IN_CELL.Height];

            // Vector2 I = new Vector2(((Config.MAP_SIZE_IN_CELL.Width * CELL_SIZE.Width) >> 1) - CELL_SIZE.Width >> 1, 0);
            int HalfOfCellSizeX = CELL_SIZE.Width >> 1;
            int HalfOfCellSizeY = CELL_SIZE.Height>> 1;

            for (int j = 0; j < Config.MAP_SIZE_IN_CELL.Height; j++){
                for (int i = 0; i < Config.MAP_SIZE_IN_CELL.Width; i++)
                {
                    int x = (int)ROOT_Vector2.X - HalfOfCellSizeX * j + HalfOfCellSizeX * i;
                    int y = (int)ROOT_Vector2.Y + HalfOfCellSizeY * j + HalfOfCellSizeY * i;
                    string fileName = matrixmap[i, j].ToString("0000");
                    this.cells[i, j] = new MapCell(Game.Content.Load<Texture2D>(Config.PATH_TO_RHOMBUS_MAP_IMAGE + fileName), x, y);
                }
            }
        }
    }
}
