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
using System.Drawing;

namespace ImageTransform
{
    public class Converter
    {
        public static Texture2D Bitmap2Texture2D(GraphicsDevice graphics, Bitmap bmp)
        {
            Microsoft.Xna.Framework.Graphics.Color[] pixels = new Microsoft.Xna.Framework.Graphics.Color[bmp.Width * bmp.Height];
            for (int y = 0; y < bmp.Height; y++){
                for (int x = 0; x < bmp.Width; x++)
                {
                    System.Drawing.Color c = bmp.GetPixel(x, y);
                    pixels[(y * bmp.Width) + x] = new Microsoft.Xna.Framework.Graphics.Color(c.R, c.G, c.B, c.A);
                }
            }

            Texture2D myTex = new Texture2D(
              graphics,
              bmp.Width,
              bmp.Height,
              1,
              TextureUsage.None,
              SurfaceFormat.Color);

            myTex.SetData<Microsoft.Xna.Framework.Graphics.Color>(pixels);
            return myTex;
        }
    }
}
