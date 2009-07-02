using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GameDemo1
{
    public partial class frmMain : Form
    {
        public static bool EditMode { get; set; }

        public frmMain()
        {
            InitializeComponent();
            EditMode = false;
        }

        public string map = "";
        private string filename = "";
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                this.filename = this.textBox1.Text;
                this.map = this.listBox1.SelectedItem.ToString();
                EditMode = true;
                this.Close();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("Map//");
            foreach (FileInfo f in dir.GetFiles())
            {
                this.listBox1.Items.Add(f.Name.Substring(0, f.Name.Length - 4));
            }
            this.listBox1.SelectedIndex = 0;
        }

        private void btnNewMap_Click(object sender, EventArgs e)
        {
            frmMapCreator frm = new frmMapCreator();
            frm.ShowDialog();

            // Load again
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo("Map//");
            foreach (FileInfo f in dir.GetFiles()){
                this.listBox1.Items.Add(f.Name.Substring(0, f.Name.Length - 4));
            }
            this.listBox1.SelectedIndex = 0;
        }
    }
}
