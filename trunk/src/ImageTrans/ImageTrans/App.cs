using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace ImageTrans
{
    public class App
    {
        private String savePath = "";

        public void Transform()
        {
            //Bitmap b = new Bitmap(@"C:\Documents and Settings\kesfaw\Desktop\Ggra0043201.bmp");
            //Bitmap m = new Bitmap(@"C:\Documents and Settings\kesfaw\Desktop\Ggra0043201M.bmp");
            Bitmap a = new Bitmap(@"C:\Documents and Settings\kesfaw\Desktop\Ggra0112711.bmp");
            Bitmap b = BmpTransformer.FlipHorizontal(a);
            b.Save(@"C:\Documents and Settings\kesfaw\Desktop\RS128x128.png");
            return;
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Environment.CurrentDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dlg.SelectedPath);

                // Get path string
                savePath = getStorePath(dlg.SelectedPath).Trim('\\') + "\\" + Path.GetFileName(dlg.SelectedPath)/*Path.GetRandomFileName()*/ + "2009";

                // create path folders
                Directory.CreateDirectory(savePath);

                foreach (String f in files){
                    this.doItNow(f);
                }
            }
        }
        private void doItNow(String path)
        {
            Bitmap bmp = new Bitmap(path);
            BmpTransformer.MakeTransparent(bmp, bmp.GetPixel(0,0));
            bmp.Save(savePath.Trim('\\') + "\\" + getFileName(path) + ".png");
        }
        private String getStorePath(String SourcePath)
        {
            return Directory.GetParent(SourcePath).FullName;
        }
        private String getFileName(String filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
    }
}
