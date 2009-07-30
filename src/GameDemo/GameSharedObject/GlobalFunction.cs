using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GameSharedObject.Components;
using System.Runtime.InteropServices;
using GameSharedObject.DTO;

namespace GameSharedObject
{
    public class GlobalFunction
    {
        /// <summary>
        /// Tạo ra vecto di chuyển dựa vào tọa độ điểm đầu và cuối
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="currentPosition"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static Vector2 CreateMovingVector(Point endPoint, Vector2 currentPosition, int speed)
        {
            Vector2 result = Vector2.Zero;
            float x = endPoint.X - (int)currentPosition.X;
            float y = endPoint.Y - (int)currentPosition.Y;
            float tempx = Math.Abs(x);
            float tempy = Math.Abs(y);

            if (tempx > tempy)
            {
                result.X = speed;
                result.Y = speed * tempy / tempx;
            }
            else if (tempx < tempy)
            {
                result.Y = speed;
                result.X = speed * tempx / tempy;
            }
            if (x < 0)
            {
                result.X = -result.X;
            }
            if (y < 0)
            {
                result.Y = -result.Y;
            }
            return result;
        }
        public static bool PointInRectangle(Rectangle rec, Point p)
        {
            if (p.X >= rec.X && p.X <= rec.X + rec.Width &&
                p.Y >= rec.Y && p.Y <= rec.Y + rec.Height)
                return true;
            return false;
        }
        public static void SetOccupiedCellsToMatrix(Sprite terrain)
        {
            Point from = GlobalDTO.MANAGER_GAME.Map.Transform.PointToCell(new Point((int)terrain.Position.X, (int)terrain.Position.Y + (terrain.Size.Height / 4)));
            Point to = GlobalDTO.MANAGER_GAME.Map.Transform.PointToCell(new Point((int)(terrain.Position.X + terrain.Size.Width), (int)(terrain.Position.Y + terrain.Size.Height)));
            for (int i = from.X + 2; i < to.X + 2; i++)
            {
                for (int j = from.Y + 1; j < to.Y + 1; j++)
                {
                    try
                    {
                        GlobalDTO.MANAGER_GAME.Map.OccupiedMatrix[i, j] = 1;
                    }
                    catch
                    { }
                }
            }            
        }

        #region Alert for testing
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        public static void MessageBox(String message)
        {
            MessageBox(new IntPtr(0), message, "Function test", 0);
        }
        #endregion
    }
}
