using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using GameDemo1.DTO;

namespace GameDemo1.Data
{
    public class MatrixMgr
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);

        public static MatrixDTO Read(string binaryFilePath)
        {
            // 4 byte dau: chiều rộng ma trận
            // 4 byte kế: chiều cao ma trận
            // 4*n byte kế là các giá trị trong ma trận
            byte[] num = new byte[4];
            FileStream rd = new FileStream(binaryFilePath, FileMode.Open, FileAccess.Read);
            MatrixDTO matrix = new MatrixDTO();

            rd.Read(num, 0, num.Length);
            matrix.Width = System.BitConverter.ToInt32(num, 0);
            rd.Read(num, 0, num.Length);
            matrix.Height = System.BitConverter.ToInt32(num, 0);
            matrix.Data = new int[matrix.Width, matrix.Height];

            for (int j = 0; j < matrix.Height; j++)
                for (int i = 0; i < matrix.Width; i++){
                    rd.Read(num, 0, num.Length);
                    matrix.Data[i, j] = System.BitConverter.ToInt32(num, 0);
                }
            rd.Close();
            return matrix;
        }
        public static void Save(string binaryFilePath, MatrixDTO matrix)
        {
            // 4 byte dau: chiều rộng ma trận
            // 4 byte kế: chiều cao ma trận
            // 4*n byte kế là các giá trị trong ma trận
            FileStream wr = new FileStream(binaryFilePath, FileMode.Create, FileAccess.Write);
            wr.Write(System.BitConverter.GetBytes(matrix.Width), 0, sizeof(int));
            wr.Write(System.BitConverter.GetBytes(matrix.Height), 0, sizeof(int));
            for (int i = 0; i < matrix.Height; i++)
                for (int j = 0; j < matrix.Width; j++){
                    wr.Write(System.BitConverter.GetBytes(matrix.Data[i, j]), 0, sizeof(int));
                }
            wr.Close();
        }
        public static MatrixDTO ReadTextFile(string textFilePath, int width, int height)
        {
            StreamReader rd = new StreamReader(textFilePath);
            int index = 0;

            MatrixDTO matrix = new MatrixDTO(height, width);
            while (!rd.EndOfStream){
                String[] arr = rd.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++){
                    matrix.Data[index, i] = int.Parse(arr[i]);
                }
                index++;
            }
            return matrix;
        }
        public static void SaveTextFile(string textFilePath, MatrixDTO matrix)
        {
            StreamWriter sw = new StreamWriter(textFilePath);
            for (int j = 0; j < matrix.Height; j++){
                for (int i = 0; i < matrix.Width; i++){
                    sw.Write(matrix.Data[i, j].ToString() + " ");
                }
                sw.WriteLine();

            }
            sw.Close();
        }
        public static MatrixDTO Randomize(int maxValue, MatrixDTO matrix)
        {
            for (int j = 0; j < matrix.Height; j++){
                for (int i = 0; i < matrix.Width; i++){
                    matrix.Data[i, j] = rnd.Next(0, maxValue);
                }
            }
            return matrix;
        }
    }
}
