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
    public class RhombusMap : Map
    {
        public readonly System.Drawing.Size CELL_SIZE = new System.Drawing.Size(97, 49);// kích thước 1 cell hình thoi sẽ vẽ
        public readonly Point ROOT_Vector2;// điểm làm gốc

        public RhombusMap(Game game, string pathSpecificationFile, Vector2 currentrootcoordiante)
            : base(game)
        {
            this._currentRootCoordinate = currentrootcoordiante;
            this._pathSpecificationFile = pathSpecificationFile;
            Config.CURRENT_CELL_SIZE = CELL_SIZE;
            ROOT_Vector2 = new Point((CELL_SIZE.Width >> 1) * (Config.MAP_SIZE_IN_CELL.Width - 1), 0);
            Transform = new RomhbusTransform(ROOT_Vector2, CELL_SIZE.Width, CELL_SIZE.Height);

            // get matrix from file and load map
            // lấy ma trận số từ file đặc tả
            MatrixMgr matrixmgr = new MatrixMgr();
            matrixmgr.Read(this._pathSpecificationFile);
            this._bgMatrix = matrixmgr.Matrix;
            this.LoadMapCells(this._bgMatrix); // load các cell dựa vào ma trận số
        }

        /// <summary>
        /// Scroll map bằng phím -> tính lại tọa độ cho góc trái trên viewport theo hệ tọa độ toàn map
        /// </summary>
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

        /// <summary>
        /// Scroll map bằng chuột -> tính lại tọa độ cho góc trái trên viewport theo hệ tọa độ toàn map
        /// </summary>
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

        /// <summary>
        /// Tính toán và vẽ các cell vào viewport
        /// </summary>
        protected override void DrawBackGround()
        {
            // calculate which cells to draw
            // tính toán ra các cell cần phải vẽ hiện tại -> chỉ các cell nằm trong view port hiện tại mới được vẽ ra màn hình, các cell ngoài vùng view port sẽ ko được vẽ
            Point cell1 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width),(int) (this._currentRootCoordinate.Y)));
            Point cell2 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X), (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height)));
            Point cell3 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X), (int)(this._currentRootCoordinate.Y)));
            Point cell4 = transform.PointToCell(new Point((int)(this._currentRootCoordinate.X + Game.Window.ClientBounds.Width), (int)(this._currentRootCoordinate.Y + Game.Window.ClientBounds.Height)));

            int i1 = (int)cell3.Y;// C
            if (i1 < 0)
            {
                i1 = 0;
            }
            if (i1 >= Config.MAP_SIZE_IN_CELL.Height)
            {
                i1 = Config.MAP_SIZE_IN_CELL.Height - 1;
            }

            int i2 = (int)cell4.Y + 2;// D
            if (i2 < 0)
            {
                i2 = 0;
            }
            if (i2 >= Config.MAP_SIZE_IN_CELL.Height)
            {
                i2 = Config.MAP_SIZE_IN_CELL.Height - 1;
            }

            int j1 = (int)cell1.X;// A
            if (j1 < 0)
            {
                j1 = 0;
            }
            if (j1 >= Config.MAP_SIZE_IN_CELL.Width)
            {
                j1 = Config.MAP_SIZE_IN_CELL.Width - 1;
            }

            int j2 = (int)cell2.X + 2;// B
            if (j2 < 0)
            {
                j2 = 0;
            }
            if (j2 >= Config.MAP_SIZE_IN_CELL.Width)
            {
                j2 = Config.MAP_SIZE_IN_CELL.Width - 1;
            }

            for (int i = i1; i <= i2; i++)
            {
                for (int j = j1; j <= j2; j++) // với các cell được phéo vẽ
                {
                    try
                    {
                        // tính ra vị trí sẽ vẽ cell trong view port dựa và position của cell trong hệ tọa độ map
                        // và tọa độ của góc trái trên của view port
                        Rectangle recToDraw = new Rectangle(
                            (int)(this.cells[i, j].X - this._currentRootCoordinate.X),
                            (int)(this.cells[i, j].Y - this._currentRootCoordinate.Y),
                            CELL_SIZE.Width, CELL_SIZE.Height);
                        spriteBatch.Draw(this.cells[i, j].Background, recToDraw, Color.WhiteSmoke);
                    }
                    catch
                    {
                        int a = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Load các cell dựa vào ma trận số
        /// </summary>
        /// <param name="matrixmap"></param>
        protected override void LoadMapCells(int[,] matrixmap)
        {
            // khởi tạo mảng cell 2 chiều
            this.cells = new MapCell[Config.MAP_SIZE_IN_CELL.Width, Config.MAP_SIZE_IN_CELL.Height];

            int HalfOfCellSizeX = CELL_SIZE.Width >> 1;
            int HalfOfCellSizeY = CELL_SIZE.Height>> 1;

            for (int i = 0; i < Config.MAP_SIZE_IN_CELL.Height; i++){
                for (int j = 0; j < Config.MAP_SIZE_IN_CELL.Width; j++)
                {
                    // tính vị trí của cell trên map
                    int x = (int)ROOT_Vector2.X - HalfOfCellSizeX * j + HalfOfCellSizeX * i;
                    int y = (int)ROOT_Vector2.Y + HalfOfCellSizeY * j + HalfOfCellSizeY * i;
                    // từ mãng số -> cell sẽ được load từ hình nào
                    int dir = (matrixmap[i, j] >> 7) + 1;
                    int file = matrixmap[i, j] - (dir-1) * 128;
                    string temp = file.ToString();
                    for (int m = 1; m <= 5 - file.ToString().Length; m++)
                    {
                        temp = "0" + temp;
                    }
                    this.cells[i, j] = new MapCell(Game.Content.Load<Texture2D>(Config.PATH_TO_RHOMBUS_MAP_IMAGE + "BG00" + dir + "//ter" + temp), x, y);
                }
            }
        }
    }
}
