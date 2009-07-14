namespace ResAnalyzing
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnGen = new System.Windows.Forms.Button();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.chboExportImage = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInputFolder = new System.Windows.Forms.Button();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cusDataGridViewEchelon1 = new ResAnalyzing.CusDataGridViewEchelon();
            this.customDataGridView1 = new ResAnalyzing.CustomDataGridView();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txtInfoName = new System.Windows.Forms.TextBox();
            this.txtInfoValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboPropertyType = new System.Windows.Forms.ComboBox();
            this.btnAddInfo = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cboInfoType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGen
            // 
            this.btnGen.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGen.Location = new System.Drawing.Point(397, 256);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(121, 275);
            this.btnGen.TabIndex = 0;
            this.btnGen.Text = "Generate";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(397, 89);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(121, 21);
            this.cboType.TabIndex = 7;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // chboExportImage
            // 
            this.chboExportImage.AutoSize = true;
            this.chboExportImage.Checked = true;
            this.chboExportImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chboExportImage.Location = new System.Drawing.Point(397, 233);
            this.chboExportImage.Name = "chboExportImage";
            this.chboExportImage.Size = new System.Drawing.Size(72, 17);
            this.chboExportImage.TabIndex = 14;
            this.chboExportImage.Text = "Export file";
            this.chboExportImage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOutputFolder);
            this.groupBox1.Controls.Add(this.txtOutputFolder);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnInputFolder);
            this.groupBox1.Controls.Add(this.txtInputFolder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(28, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 100);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folder Path";
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Location = new System.Drawing.Point(398, 55);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnOutputFolder.TabIndex = 18;
            this.btnOutputFolder.Text = "Browse";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(77, 58);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(312, 20);
            this.txtOutputFolder.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Output Path:";
            // 
            // btnInputFolder
            // 
            this.btnInputFolder.Location = new System.Drawing.Point(398, 22);
            this.btnInputFolder.Name = "btnInputFolder";
            this.btnInputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnInputFolder.TabIndex = 15;
            this.btnInputFolder.Text = "Browse";
            this.btnInputFolder.UseVisualStyleBackColor = true;
            this.btnInputFolder.Click += new System.EventHandler(this.btnInputFolder_Click);
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Location = new System.Drawing.Point(77, 25);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(312, 20);
            this.txtInputFolder.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Input Path:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cboInfoType);
            this.groupBox2.Controls.Add(this.cusDataGridViewEchelon1);
            this.groupBox2.Controls.Add(this.customDataGridView1);
            this.groupBox2.Controls.Add(this.btnRemove);
            this.groupBox2.Controls.Add(this.txtInfoName);
            this.groupBox2.Controls.Add(this.txtInfoValue);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboPropertyType);
            this.groupBox2.Controls.Add(this.btnAddInfo);
            this.groupBox2.Controls.Add(this.btnClearAll);
            this.groupBox2.Location = new System.Drawing.Point(28, 222);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 329);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // cusDataGridViewTri1
            // 
            this.cusDataGridViewEchelon1.ItemList = null;
            this.cusDataGridViewEchelon1.Location = new System.Drawing.Point(14, 53);
            this.cusDataGridViewEchelon1.Name = "cusDataGridViewTri1";
            this.cusDataGridViewEchelon1.Size = new System.Drawing.Size(308, 182);
            this.cusDataGridViewEchelon1.TabIndex = 31;
            this.cusDataGridViewEchelon1.OnSelectedItem += new ResAnalyzing.CusDataGridViewEchelon.Process(this.cusDataGridViewTri1_OnSelectedItem);
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.ItemList = null;
            this.customDataGridView1.Location = new System.Drawing.Point(14, 53);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.Size = new System.Drawing.Size(308, 182);
            this.customDataGridView1.TabIndex = 30;
            this.customDataGridView1.OnSelectedItem += new ResAnalyzing.CustomDataGridView.Process(this.customDataGridView1_OnSelectedItem);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(328, 93);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(29, 96);
            this.btnRemove.TabIndex = 29;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtInfoName
            // 
            this.txtInfoName.Location = new System.Drawing.Point(60, 269);
            this.txtInfoName.Name = "txtInfoName";
            this.txtInfoName.Size = new System.Drawing.Size(218, 20);
            this.txtInfoName.TabIndex = 24;
            // 
            // txtInfoValue
            // 
            this.txtInfoValue.Location = new System.Drawing.Point(60, 295);
            this.txtInfoValue.Name = "txtInfoValue";
            this.txtInfoValue.Size = new System.Drawing.Size(218, 20);
            this.txtInfoValue.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Sprite property type :";
            // 
            // cboPropertyType
            // 
            this.cboPropertyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPropertyType.FormattingEnabled = true;
            this.cboPropertyType.Location = new System.Drawing.Point(123, 26);
            this.cboPropertyType.Name = "cboPropertyType";
            this.cboPropertyType.Size = new System.Drawing.Size(140, 21);
            this.cboPropertyType.TabIndex = 19;
            this.cboPropertyType.SelectedIndexChanged += new System.EventHandler(this.cboPropertyType_SelectedIndexChanged);
            // 
            // btnAddInfo
            // 
            this.btnAddInfo.Location = new System.Drawing.Point(284, 268);
            this.btnAddInfo.Name = "btnAddInfo";
            this.btnAddInfo.Size = new System.Drawing.Size(67, 47);
            this.btnAddInfo.TabIndex = 13;
            this.btnAddInfo.Text = "Add info";
            this.btnAddInfo.UseVisualStyleBackColor = true;
            this.btnAddInfo.Click += new System.EventHandler(this.btnAddInfo_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(269, 24);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(82, 23);
            this.btnClearAll.TabIndex = 18;
            this.btnClearAll.Text = "Clear all";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Sprite Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label7.Location = new System.Drawing.Point(132, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(350, 46);
            this.label7.TabIndex = 23;
            this.label7.Text = "ResXml Generator";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ResAnalyzing.Properties.Resources.img;
            this.pictureBox1.Location = new System.Drawing.Point(28, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // cboInfoType
            // 
            this.cboInfoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInfoType.FormattingEnabled = true;
            this.cboInfoType.Items.AddRange(new object[] {
            "None",
            "Unit",
            "Structure",
            "Technology",
            "ResourceCenter "});
            this.cboInfoType.Location = new System.Drawing.Point(123, 242);
            this.cboInfoType.Name = "cboInfoType";
            this.cboInfoType.Size = new System.Drawing.Size(140, 21);
            this.cboInfoType.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(74, 245);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Type: ";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 549);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chboExportImage);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.btnGen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Ressouce XML Generator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox chboExportImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnInputFolder;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboPropertyType;
        private System.Windows.Forms.Button btnAddInfo;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;       
        private System.Windows.Forms.TextBox txtInfoName;
        private System.Windows.Forms.TextBox txtInfoValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemove;
        private CustomDataGridView customDataGridView1;
        private CusDataGridViewEchelon cusDataGridViewEchelon1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboInfoType;       
    }
}

