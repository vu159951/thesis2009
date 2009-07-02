using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameDemo1.DTO
{
    public class MatrixDTO
    {
        private int[,] _data;
        private int _width;
        private int _height;

        public int[,] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public int Width
        {
          get { return _width; }
          set { _width = value; }
        }
        public int Height
        {
          get { return _height; }
          set { _height = value; }
        }

        public MatrixDTO() { 
            _width = 1;
            _height = 1;
        }
        public MatrixDTO(int matrixWidth, int matrixHeight)
        {
            _width = matrixWidth;
            _height = matrixHeight; 
            _data = new int[_width, _height];    
        }
    }
}
