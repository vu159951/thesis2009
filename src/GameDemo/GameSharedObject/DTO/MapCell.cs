using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameSharedObject.DTO
{
    public class MapCell
    {
        private Texture2D _background;
        private int _x;
        private int _y;
        private int _status;

        /// <summary>
        /// Background of the cell
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }
        /// <summary>
        /// X-coordinate of the cell.
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        /// <summary>
        /// Y-coordinate of a map cell in pixel.
        /// </summary>       
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        /// <summary>
        /// State of cell, Represent for the part of a terrain.
        /// Value = 0, mean no any terrains on it.
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public MapCell() 
        {
            this._background = null;
            this._x = 0;
            this._y = 0;
            this._status = 0;
        }
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="bgImage">Image for cell background</param>
        /// <param name="x">x-coordinate of the cell</param>
        /// <param name="y">y-coordinate of the cell</param>
        public MapCell(Texture2D image, int x, int y)
        {
            this._background = image;
            this._x = x;
            this._y = y;
            this._status = 0;
        }
    }
}
