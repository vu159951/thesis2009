namespace GameDemo1
{
    partial class frmMapCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.NumericUpDown();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFloor = new System.Windows.Forms.NumericUpDown();
            this.txtCeiling = new System.Windows.Forms.NumericUpDown();
            this.txtEpsilon = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMinValue = new System.Windows.Forms.NumericUpDown();
            this.txtDistance = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.imageListView1 = new MapImage.ImageListView();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCeiling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEpsilon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map Size";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(84, 7);
            this.txtWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.txtWidth.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(70, 22);
            this.txtWidth.TabIndex = 0;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "x";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(180, 7);
            this.txtHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.txtHeight.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(70, 22);
            this.txtHeight.TabIndex = 1;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHeight.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(12, 283);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(147, 32);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Create map";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ngưỡng trên";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(84, 213);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(168, 22);
            this.txtFileName.TabIndex = 7;
            this.txtFileName.Text = "Map_2.txt";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Output";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Ngưỡng dưới";
            // 
            // txtFloor
            // 
            this.txtFloor.Location = new System.Drawing.Point(143, 80);
            this.txtFloor.Name = "txtFloor";
            this.txtFloor.Size = new System.Drawing.Size(70, 22);
            this.txtFloor.TabIndex = 3;
            this.txtFloor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFloor.Value = new decimal(new int[] {
            41,
            0,
            0,
            0});
            // 
            // txtCeiling
            // 
            this.txtCeiling.Location = new System.Drawing.Point(143, 48);
            this.txtCeiling.Name = "txtCeiling";
            this.txtCeiling.Size = new System.Drawing.Size(70, 22);
            this.txtCeiling.TabIndex = 2;
            this.txtCeiling.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCeiling.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // txtEpsilon
            // 
            this.txtEpsilon.Location = new System.Drawing.Point(143, 113);
            this.txtEpsilon.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtEpsilon.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtEpsilon.Name = "txtEpsilon";
            this.txtEpsilon.Size = new System.Drawing.Size(70, 22);
            this.txtEpsilon.TabIndex = 4;
            this.txtEpsilon.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEpsilon.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 111);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Epsilon";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(14, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(274, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "Lưu ý: Ngưỡng dưới phải nhỏ hơn ngưỡng trên";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 145);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Giá trị tối thiểu";
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(143, 143);
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(70, 22);
            this.txtMinValue.TabIndex = 5;
            this.txtMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinValue.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(143, 175);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(70, 22);
            this.txtDistance.TabIndex = 6;
            this.txtDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDistance.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 177);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "Khoảng cách->max";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(301, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "Choose the first cell";
            // 
            // imageListView1
            // 
            this.imageListView1.Location = new System.Drawing.Point(295, 33);
            this.imageListView1.Margin = new System.Windows.Forms.Padding(4);
            this.imageListView1.Name = "imageListView1";
            this.imageListView1.Path = null;
            this.imageListView1.Size = new System.Drawing.Size(166, 283);
            this.imageListView1.TabIndex = 9;
            // 
            // frmMapCreator
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 329);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.imageListView1);
            this.Controls.Add(this.txtDistance);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtEpsilon);
            this.Controls.Add(this.txtCeiling);
            this.Controls.Add(this.txtMinValue);
            this.Controls.Add(this.txtFloor);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapCreator";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Background Creator";
            this.Load += new System.EventHandler(this.frmMapCreator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFloor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCeiling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEpsilon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtHeight;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown txtFloor;
        private System.Windows.Forms.NumericUpDown txtCeiling;
        private System.Windows.Forms.NumericUpDown txtEpsilon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown txtMinValue;
        private System.Windows.Forms.NumericUpDown txtDistance;
        private System.Windows.Forms.Label label9;
        private MapImage.ImageListView imageListView1;
        private System.Windows.Forms.Label label10;
    }
}