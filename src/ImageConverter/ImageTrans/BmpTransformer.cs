using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ImageTrans
{
    public class BmpTransformer
    {
        public static Bitmap MakeTransparent(Bitmap Bmp, Color TransColor)
        {
            ColorMap[] map = new ColorMap[] { new ColorMap() };
            map[0].OldColor = TransColor;
            map[0].NewColor = Color.Transparent;
            ImageAttributes imgAttr = new ImageAttributes();
            imgAttr.SetRemapTable(map, ColorAdjustType.Bitmap);
            Bmp.MakeTransparent(TransColor);

            return Bmp;
        }
        public static Bitmap Scale(Bitmap Bmp, int newWidth, int newHeight)
        {
            Bitmap scaledBitmap = new Bitmap(newWidth, newHeight);

            // Scale the bitmap in high quality mode.
            using (Graphics gr = Graphics.FromImage(scaledBitmap))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.DrawImage(Bmp, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, Bmp.Width, Bmp.Height), GraphicsUnit.Pixel);
            }

            // Copy original Bitmap's EXIF tags to new bitmap.
            foreach (PropertyItem propertyItem in Bmp.PropertyItems){
                scaledBitmap.SetPropertyItem(propertyItem);
            }

            return scaledBitmap;
        }
        public static Bitmap Scale(Bitmap Bmp, float ScaleFactorX, float ScaleFactorY)
        {
            int scaleWidth = (int)Math.Max(Bmp.Width * ScaleFactorX, 1.0f);
            int scaleHeight = (int)Math.Max(Bmp.Height * ScaleFactorY, 1.0f);

            Bitmap scaledBitmap = new Bitmap(scaleWidth, scaleHeight);

            // Scale the bitmap in high quality mode.
            using (Graphics gr = Graphics.FromImage(scaledBitmap)){
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.DrawImage(Bmp, new Rectangle(0, 0, scaleWidth, scaleHeight), new Rectangle(0, 0, Bmp.Width, Bmp.Height), GraphicsUnit.Pixel);
            }

            // Copy original Bitmap's EXIF tags to new bitmap.
            foreach (PropertyItem propertyItem in Bmp.PropertyItems){
                scaledBitmap.SetPropertyItem(propertyItem);
            }

            return scaledBitmap;
        }
        public static Bitmap ScaleVector(Bitmap Bmp, int borderWidth, int borderHeight)
        {
            Bitmap result;
            int newWidth = 0, newHeight = 0;
            int ws = borderWidth - Bmp.Width;
            int hs = borderHeight - Bmp.Height;

            if (ws < 0 || hs < 0){
                // border nhỏ hơn size của image
                if (ws < hs){
                    newWidth = borderWidth;
                    newHeight = Convert.ToInt32( ((double)borderWidth / Bmp.Width/*Hệ số*/) * Bmp.Height );
                }else if (ws > hs){
                    newWidth = Convert.ToInt32( ((double)borderHeight / Bmp.Height/*Hệ số*/) * Bmp.Width );
                    newHeight = borderHeight;
                }
                result = Scale(Bmp,newWidth, newHeight);
            } else{
                // border lớn hơn size của image
                if (ws < hs){
                    newWidth = borderWidth;
                    newHeight = Convert.ToInt32(((double)borderWidth / Bmp.Width/*Hệ số*/) * Bmp.Height); 
                }else if (ws > hs){
                    newWidth = Convert.ToInt32(((double)borderHeight / Bmp.Height/*Hệ số*/) * Bmp.Width); 
                    newHeight = borderHeight;
                }
                result = Scale(Bmp, newWidth, newHeight);
            }
            return result;
        }
        public static Bitmap GetImageWithMask(Bitmap bmp, Bitmap maskBmp, Color maskColor)
        {
            // This functon used to mask(multiply) two images bitmap.
            int width = (bmp.Width < maskBmp.Width) ? bmp.Width :maskBmp.Width;
            int height = (bmp.Height < maskBmp.Height) ? bmp.Height : maskBmp.Height;
            Bitmap result = new Bitmap(width, height);

            for (int row = 0; row < result.Width; row++){
                for (int col = 0; col < result.Height; col++){
                    if (maskBmp.GetPixel(row, col).ToArgb() == maskColor.ToArgb())
                        result.SetPixel(row, col, bmp.GetPixel(row, col));
                }
            }
            return result;
        }
        public static Bitmap Rotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            PixelFormat pf = default(PixelFormat);
            if (bkColor == Color.Transparent){
                pf = PixelFormat.Format32bppArgb;
            }else{
                pf = bmp.PixelFormat;
            }

            Bitmap tempImg = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tempImg);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            //Using System.Drawing.Drawing2D.Matrix class 
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pf);
            g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();
            return newImg;
        }
        public static Bitmap FlipHorizontal(Bitmap Bmp)
        {
            Bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return Bmp;
        }
        public static Bitmap FlipVertical(Bitmap Bmp)
        {
            Bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return Bmp;
        }
    }
}
