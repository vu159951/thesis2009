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
