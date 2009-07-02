using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameDemo1.DTO;
using GameDemo1.Data;

namespace GameDemo1
{
    public partial class frmMapCreator : Form
    {
        private MatrixDTO _mapMatrix;
        public MatrixDTO MapMatrix
        {
            get { return _mapMatrix; }
            set { _mapMatrix = value; }
        }

        public frmMapCreator()
        {
            InitializeComponent();
            _mapMatrix = new MatrixDTO();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int w = Convert.ToInt32(txtWidth.Value);
            int h = Convert.ToInt32(txtHeight.Value);
            MapCreator.Load(Application.StartupPath + "\\" + @"Specification\MapCellData");
            _mapMatrix = MapCreator.Generate(w, h);
            MatrixMgr.Save(Application.StartupPath + "\\Map\\" + "Map_2.txt", _mapMatrix);
        }
    }
}
