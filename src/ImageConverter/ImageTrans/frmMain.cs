using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageTrans
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select your input forlder";
            dlg.ShowNewFolderButton = true;
            if (dlg.ShowDialog() == DialogResult.OK){
                txtPath.Text = dlg.SelectedPath;
            }
        }
        private void btnAction_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPath.Text.Trim()))
            {
                if (rdFlipX.Checked)
                    DoFlipX(txtPath.Text.Trim());
                if (rdFlipY.Checked)
                    DoFlipY(txtPath.Text.Trim());
                if (rdMask.Checked)
                    DoMaskImage(txtPath.Text.Trim());
                if (rdBorderStand.Checked)
                    DoBorderStandlization(txtPath.Text);
                if (rdScale.Checked){
                    Directory.CreateDirectory(txtPath.Text.TrimEnd('\\') + "@");
                    DoScaleVector(txtPath.Text, txtPath.Text.TrimEnd('\\') + "@", Convert.ToInt32(rdScaleXValue.Value), Convert.ToInt32(rdScaleYValue.Value));
                }
                if (rdRemoveBg.Checked){
                    string output = txtPath.Text.TrimEnd('\\') + "#";
                    Directory.CreateDirectory(output);
                    RemoveBackground(txtPath.Text.TrimEnd('\\'), output); 
                }
                MessageBox.Show("Thực hiện xong tác vụ!");
            }
        }


        private void DoFlipX(string folderPath)
        {
            String[] files = Directory.GetFiles(folderPath);
            foreach(String file in files){
                BmpTransformer.FlipHorizontal(new Bitmap(file)).Save(file);
            }
        }
        private void DoFlipY(string folderPath)
        {
            String[] files = Directory.GetFiles(folderPath);
            foreach (String file in files){
                BmpTransformer.FlipVertical(new Bitmap(file)).Save(file);
            }
        }
        private void DoMaskImage(string folderPath)
        {
            String[] files = Directory.GetFiles(folderPath, "*.bmp");

            foreach (String file in files){
                if (file[file.Length - 5] == 'M' || file[file.Length - 5] == 'I')
                    continue;
                String name = Path.GetFileNameWithoutExtension(file);
                String ext = Path.GetExtension(file);
                String dirPath = Path.GetDirectoryName(file).TrimEnd('\\');

                BmpTransformer.GetImageWithMask(
                    new Bitmap(file),
                    new Bitmap(dirPath + '\\' + name + 'M' + ext),
                    Color.Black).Save(dirPath + '\\' + name + ".png");
            }
        }
        private void DoScaleVector(string inFolder, string outFolder, int WidthOfBorder, int HeightOfBorder)
        {
            String[] files = Directory.GetFiles(inFolder, "*.png");
            foreach (String file in files){
                BmpTransformer.ScaleVector(new Bitmap(file), WidthOfBorder, HeightOfBorder).Save(
                    outFolder.TrimEnd('\\') + '\\' + Path.GetFileName(file));
            }
        }
        private void DoBorderStandlization(string folderPath)
        {
            folderPath = folderPath.Trim();
            Size maxPoint;
            Size maxBorder = FindBorderConst(folderPath, out maxPoint);
            String[] files = Directory.GetFiles(folderPath, "*.png");

            using (StreamReader sr = new StreamReader(Directory.GetFiles(folderPath, "*.csv")[0])){
                foreach (String file in files){
                    Bitmap bmp = new Bitmap(maxBorder.Width, maxBorder.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    if (sr.EndOfStream)
                        throw new Exception("Invalid csv specification file!");
                    Point p = ParsePoint(sr.ReadLine());
                    g.DrawImage(Image.FromFile(file), new Point(maxPoint.Width - p.X, maxPoint.Height - p.Y));
                    String dirPath = Path.GetDirectoryName(file).TrimEnd('\\');
                    Directory.CreateDirectory(dirPath + "@");
                    bmp.Save(dirPath + "@" + '\\' + Path.GetFileName(file));
                }
            }
        }
        private void RemoveBackground(string folderPath, string outputFolder){
            String[] files = Directory.GetFiles(folderPath);
            foreach (String file in files){
                string outputFIle = outputFolder.Trim('\\') + "\\" + Path.GetFileNameWithoutExtension(file) + ".png";
                Bitmap bmp = new Bitmap(file);
                BmpTransformer.MakeTransparent(bmp, bmp.GetPixel(0, 0)).Save(outputFIle);
            }
        }

        private Point ParsePoint(String text)
        {
            String[] ss = text.Split(new String[] { ","}, StringSplitOptions.RemoveEmptyEntries);
            Point p = new Point();
            p.X = int.Parse(ss[0].Trim());
            p.Y = int.Parse(ss[1].Trim());
            return p;
        }
        private Size FindBorderConst(String folderPath, out Size maxPoint)
        {
            Size topleft = new Size(0, 0);
            Size rightbottom = new Size(0, 0);
            String[] files = Directory.GetFiles(folderPath, "*.png");

            if (files.Length == 0)
                throw new Exception("Không tìm thấy các tập tin png dành cho thống kê");
            using(StreamReader sr = new StreamReader(Directory.GetFiles(folderPath, "*.csv")[0])){
                for (int i = 0; i < files.Length; i++){
                    String file = files[i];
                    if (sr.EndOfStream)
                        throw new Exception("Invalid csv specification file!");
                    Point p = ParsePoint(sr.ReadLine());
                    Size imgSize = new Bitmap(file).Size;
                    if (topleft.Width < p.X)
                        topleft.Width = p.X;
                    if (topleft.Height < p.Y)
                        topleft.Height = p.Y;
                    if (rightbottom.Width < imgSize.Width - p.X)
                        rightbottom.Width = imgSize.Width - p.X;
                    if (rightbottom.Height < imgSize.Height - p.Y)
                        rightbottom.Height = imgSize.Height - p.Y;
                }
            }
            maxPoint = topleft;
            return new Size(topleft.Width + rightbottom.Width, topleft.Height + rightbottom.Height);
        }
    }
}