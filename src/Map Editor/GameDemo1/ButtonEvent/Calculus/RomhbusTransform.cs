using System;
using Microsoft.Xna.Framework;

namespace GameDemo1
{
    public class RomhbusTransform : Transform
    {   
        #region Properties
        public override Point Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = new Point(value.X, value.Y);
            }
        }

        public override int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public override int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        #endregion

        #region Constructors
        public RomhbusTransform()
        {
            _root = new Point(300, 0);
            _width = 100;
            _height = 50;
            Transform.CURRENT = this;
        }

        public RomhbusTransform(Point p, int w, int h)
        {           
            _root = new Point(p.X, p.Y);
            _width = w;
            _height = h;
            Transform.CURRENT = this;
        }
        #endregion

        #region Member functions
        public override Point PointToCell(Point p)
        {
            Point cell = new Point();

            Point I = new Point(_root.Y / _height - 1, _root.X / _width);
            int w = 2 * _height;

            // Tọa độ hình thoi
            int i = (p.Y - _root.Y) / _height;
            int j = (p.X) / w;

            int x = i - j + I.Y;
            int y = i + j - I.Y;

            // Tọa độ chuột tương đối trong hình chữ nhật bao hình thoi
            int tx = p.X % w;
            int ty = (p.Y - _root.Y) % _height;

            // Độ lệch so với ô thứ 0
            int[] dx = { 0, 0, -1, 1, 0 };
            int[] dy = { 0, -1, 0, 0, 1 };

            // Xác định index của dx, dy
            int index;
            if (tx <= _height)
                if (ty <= _height / 2)
                {
                    if (tx + 2 * ty - _height < 0)
                        index = 1;
                    else
                        index = 0;
                }
                else
                {
                    if (tx - 2 * ty + _height < 0)
                        index = 3;
                    else
                        index = 0;
                }
            else
                if (ty <= _height / 2)
                {
                    if (tx - 2 * ty - _height > 0)
                        index = 2;
                    else
                        index = 0;
                }
                else
                {
                    if (tx + 2 * ty - 3 * _height > 0)
                        index = 4;
                    else
                        index = 0;
                }

            x += dx[index];
            y += dy[index];

            cell.X = x;
            cell.Y = y;
            return cell;
        }

        public override Point CenterToCell(Point p)
        {
            p.X -= _height;
            p.Y -= _height / 2;

            return PointToCell(p);
        }

        public override Point CellToPoint(Point cell)
        {
            Point p = new Point();

            p.X = _root.X - (cell.X - cell.Y) * _height;
            p.Y = _root.Y + (cell.X + cell.Y) * _height / 2;

            return p;
        }

        public override Point CellToCenter(Point cell)
        {
            Point p = CellToPoint(cell);
            p.X += _height;
            p.Y += _height / 2;

            return p;
        }
        #endregion
    }
}