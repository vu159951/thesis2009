using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace ImageTrans
{
    public class AgeOfEmpire
    {
        public readonly Size IMAGE_SIZE = new Size(128, 128);
        public const String INPUT_FOLDER_PATH = @"";
        public const String OUTPUT_FOLDER_PATH = @"";
        public const String LISTNAME_FILE_PATH = @"";
        public const String SPLITTER = "------";

        private String folder;
        private Dictionary<string, string> _nameList = new Dictionary<string, string>();

        public AgeOfEmpire(String folder)
        {
        }
        #region Initialization
        public void Initialize()
        {
            this.LoadNameList(LISTNAME_FILE_PATH);
        }
        private String FilterString(String text)
        {
            int pos = -1;
            pos = text.IndexOf('\\');
            if (pos == -1)
                pos = text.IndexOf('/');
            if(pos == -1)
                pos = text.IndexOf('|');
            text = text.Substring(0, pos);
            text.Replace("*", "");
            text.Replace("?", "");
            text.Replace("\"", "");
            text.Replace("<", "");
            text.Replace(">", "");
            return text;
        }
        private void LoadNameList(String filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream){
                    String line = sr.ReadLine();
                    if (!String.IsNullOrEmpty(line.Trim())){
                        String[] p = line.Split(new string[] { SPLITTER }, StringSplitOptions.RemoveEmptyEntries);
                        if (p.Length == 2){
                            _nameList.Add(p[0], FilterString(p[1]));
                        }
                    }
                }
            }
        }
        #endregion

        public void DoProcess()
        {
        }
        public void ProcessFolder(String inFolder, String outFolder)
        {
        }
        public Bitmap GetAnImage(String filePath, Point position)
        {
            String name = Path.GetFileNameWithoutExtension(filePath);
            String ext = Path.GetExtension(filePath);
            String folderPath = Path.GetDirectoryName(filePath);

            Bitmap temp, result = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            Bitmap bmp = new Bitmap(filePath);
            Bitmap mask = new Bitmap(folderPath.TrimEnd('\\') + '\\' + name + 'M' + ext);
            temp = BmpTransformer.GetImageWithMask(bmp, mask, Color.Black);

            // Còn đang viết dở ở đây =,="
            temp = BmpTransformer.ScaleVector(temp, IMAGE_SIZE.Width, IMAGE_SIZE.Height);


            return result;
        }
    }
}