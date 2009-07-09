﻿using System;
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
            MapCreator.CONST_CEILING_LIMIT = Convert.ToInt32(txtCeiling.Value);/* * 0.01f*/ ;
            MapCreator.CONST_FLOOR_LIMIT = Convert.ToInt32(txtFloor.Value); /* * 0.01f */ ;
            MapCreator.CONST_EPSILON = Convert.ToInt32(txtEpsilon.Value); /* * 0.1f */ ;

            MapCreator.Load(Application.StartupPath + "\\" + @"Specification\MapCellData");
            _mapMatrix = MapCreator.Generate(w, h);
            MatrixMgr.Save(Application.StartupPath + "\\Map\\" + txtFileName.Text, _mapMatrix);
        }
    }
}