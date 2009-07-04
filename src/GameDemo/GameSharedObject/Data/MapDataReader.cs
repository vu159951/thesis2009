using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameSharedObject.Data
{
    public class MapDataReader
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private int AMOUNT_OF_PICTURES = 15;
        private int x;
        private int y;

        private int[,] _matrix;
        public int[,] Matrix
        {
          get { return _matrix; }
          set { _matrix = value; }
        }

        public MapDataReader()
        {
            _matrix = new int[1 ,1];
        }
        public MapDataReader(int Width, int Height)
        {
            x = Width;
            y = Height;
            _matrix = new int[x,y];
        }

        public void Read(string filePath)
        {
            byte[] num = new byte[4];
            FileStream rd = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            rd.Read(num, 0, num.Length);
            x = System.BitConverter.ToInt32(num, 0);
            rd.Read(num, 0, num.Length);
            y = System.BitConverter.ToInt32(num, 0);
            _matrix = new int[x, y];
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++){
                    rd.Read(num, 0, num.Length);
                    _matrix[i, j] = System.BitConverter.ToInt32(num, 0);
                }
            rd.Close();
        }
        public void Save(string filePath)
        {
            FileStream wr = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            wr.Write(System.BitConverter.GetBytes(x), 0, sizeof(int));
            wr.Write(System.BitConverter.GetBytes(y), 0, sizeof(int));
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++){
                    wr.Write(System.BitConverter.GetBytes(_matrix[i, j]), 0, sizeof(int));
                }
            wr.Close();
        }
        public void Randomize()
        {
            int[,] kq = new int[x, y];
            
            for (int i = 0; i < y; i++){
                for (int j = 0; j < x; j++){
                    kq[i, j] = rnd.Next(0, AMOUNT_OF_PICTURES);
                }
            }
            _matrix = kq;
        }
    }
}
