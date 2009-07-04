using Microsoft.Xna.Framework;

namespace GameSharedObject
{
    public class SquareTransform : Transform
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
        public SquareTransform()
        {
            _root = new Point(0, 0);
            _width = 100;
            _height = 50;
            Transform.CURRENT = this;
        }
        public SquareTransform(Point p, int w, int h)
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

            cell.X = (p.X - _root.X) / _width;
            cell.Y = (p.Y - _root.Y) / _height;
            return cell;
        }
        public override Point CenterToCell(Point p)
        {
            p.X -= _width / 2;
            p.Y -= _height / 2;

            return PointToCell(p);
        }
        public override Point CellToPoint(Point cell)
        {
            Point p = new Point();

            p.X = (cell.X * _width) - _root.X;
            p.Y = (cell.Y * _height) - _root.Y;

            return p;
        }
        public override Point CellToCenter(Point cell)
        {
            Point p = CellToPoint(cell);
            p.X += _width / 2;
            p.Y += _height / 2;

            return p;
        }
        #endregion
    }
}
