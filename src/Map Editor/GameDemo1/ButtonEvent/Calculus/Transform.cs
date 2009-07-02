using System;
using Microsoft.Xna.Framework;

namespace GameDemo1
{
    public abstract class Transform
    {
        protected Point _root;    // Tọa độ điểm bắt đầu coi là gốc
        protected int _width;  // Chiều rộng
        protected int _height; // Chiều cao

        public abstract Point Root { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public static Transform CURRENT;

        public abstract Point PointToCell(Point p);
        public abstract Point CenterToCell(Point p);
        public abstract Point CellToPoint(Point cell);
        public abstract Point CellToCenter(Point cell);
        public static Vector2 PointToVector2(Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }
        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
    }
}
