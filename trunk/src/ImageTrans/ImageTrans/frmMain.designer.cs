namespace ImageTrans
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
            this.btnAction = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdScaleYValue = new System.Windows.Forms.NumericUpDown();
            this.rdScaleXValue = new System.Windows.Forms.NumericUpDown();
            this.rdMask = new System.Windows.Forms.RadioButton();
            this.rdFlipY = new System.Windows.Forms.RadioButton();
            this.rdScale = new System.Windows.Forms.RadioButton();
            this.rdBorderStand = new System.Windows.Forms.RadioButton();
            this.rdFlipX = new System.Windows.Forms.RadioButton();
            this.rdRemoveBg = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdScaleYValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdScaleXValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAction
            // 
            this.btnAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAction.Location = new System.Drawing.Point(15, 178);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(84, 29);
            this.btnAction.TabIndex = 0;
            this.btnAction.Text = "Action";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Directory";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(94, 6);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(327, 20);
            this.txtPath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(427, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(37, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rdScaleYValue);
            this.groupBox1.Controls.Add(this.rdScaleXValue);
            this.groupBox1.Controls.Add(this.rdMask);
            this.groupBox1.Controls.Add(this.rdFlipY);
            this.groupBox1.Controls.Add(this.rdScale);
            this.groupBox1.Controls.Add(this.rdBorderStand);
            this.groupBox1.Controls.Add(this.rdRemoveBg);
            this.groupBox1.Controls.Add(this.rdFlipX);
            this.groupBox1.Location = new System.Drawing.Point(15, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 140);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "x";
            // 
            // rdScaleYValue
            // 
            this.rdScaleYValue.Location = new System.Drawing.Point(380, 42);
            this.rdScaleYValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.rdScaleYValue.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.rdScaleYValue.Name = "rdScaleYValue";
            this.rdScaleYValue.Size = new System.Drawing.Size(63, 20);
            this.rdScaleYValue.TabIndex = 1;
            this.rdScaleYValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rdScaleYValue.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // rdScaleXValue
            // 
            this.rdScaleXValue.Location = new System.Drawing.Point(296, 42);
            this.rdScaleXValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.rdScaleXValue.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.rdScaleXValue.Name = "rdScaleXValue";
            this.rdScaleXValue.Size = new System.Drawing.Size(60, 20);
            this.rdScaleXValue.TabIndex = 1;
            this.rdScaleXValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rdScaleXValue.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // rdMask
            // 
            this.rdMask.AutoSize = true;
            this.rdMask.Checked = true;
            this.rdMask.Location = new System.Drawing.Point(6, 65);
            this.rdMask.Name = "rdMask";
            this.rdMask.Size = new System.Drawing.Size(129, 17);
            this.rdMask.TabIndex = 0;
            this.rdMask.TabStop = true;
            this.rdMask.Text = "Get Image with MASK";
            this.rdMask.UseVisualStyleBackColor = true;
            // 
            // rdFlipY
            // 
            this.rdFlipY.AutoSize = true;
            this.rdFlipY.Location = new System.Drawing.Point(6, 42);
            this.rdFlipY.Name = "rdFlipY";
            this.rdFlipY.Size = new System.Drawing.Size(78, 17);
            this.rdFlipY.TabIndex = 0;
            this.rdFlipY.Text = "Flip vertical";
            this.rdFlipY.UseVisualStyleBackColor = true;
            // 
            // rdScale
            // 
            this.rdScale.AutoSize = true;
            this.rdScale.Location = new System.Drawing.Point(183, 42);
            this.rdScale.Name = "rdScale";
            this.rdScale.Size = new System.Drawing.Size(107, 17);
            this.rdScale.TabIndex = 0;
            this.rdScale.Text = "Scale with border";
            this.rdScale.UseVisualStyleBackColor = true;
            // 
            // rdBorderStand
            // 
            this.rdBorderStand.AutoSize = true;
            this.rdBorderStand.Location = new System.Drawing.Point(183, 19);
            this.rdBorderStand.Name = "rdBorderStand";
            this.rdBorderStand.Size = new System.Drawing.Size(116, 17);
            this.rdBorderStand.TabIndex = 0;
            this.rdBorderStand.Text = "Standardlize border";
            this.rdBorderStand.UseVisualStyleBackColor = true;
            // 
            // rdFlipX
            // 
            this.rdFlipX.AutoSize = true;
            this.rdFlipX.Location = new System.Drawing.Point(6, 19);
            this.rdFlipX.Name = "rdFlipX";
            this.rdFlipX.Size = new System.Drawing.Size(91, 17);
            this.rdFlipX.TabIndex = 0;
            this.rdFlipX.Text = "Flip horiziontal";
            this.rdFlipX.UseVisualStyleBackColor = true;
            // 
            // rdRemoveBg
            // 
            this.rdRemoveBg.AutoSize = true;
            this.rdRemoveBg.Location = new System.Drawing.Point(6, 88);
            this.rdRemoveBg.Name = "rdRemoveBg";
            this.rdRemoveBg.Size = new System.Drawing.Size(125, 17);
            this.rdRemoveBg.TabIndex = 0;
            this.rdRemoveBg.Text = "Remove background";
            this.rdRemoveBg.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 219);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAction);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdScaleYValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdScaleXValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdFlipX;
        private System.Windows.Forms.RadioButton rdFlipY;
        private System.Windows.Forms.RadioButton rdMask;
        private System.Windows.Forms.RadioButton rdBorderStand;
        private System.Windows.Forms.RadioButton rdScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown rdScaleYValue;
        private System.Windows.Forms.NumericUpDown rdScaleXValue;
        private System.Windows.Forms.RadioButton rdRemoveBg;
    }
}