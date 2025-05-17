namespace VideoEffects
{
    partial class Form1
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
            this.btStop = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.pnScreen = new System.Windows.Forms.Panel();
            this.btSelectFile = new System.Windows.Forms.Button();
            this.edFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.cbImageLogo = new System.Windows.Forms.CheckBox();
            this.cbTextLogo = new System.Windows.Forms.CheckBox();
            this.cbVerticalFlip = new System.Windows.Forms.CheckBox();
            this.cbHorizontalFlip = new System.Windows.Forms.CheckBox();
            this.cbVerticalMirror = new System.Windows.Forms.CheckBox();
            this.cbHorizontalMirror = new System.Windows.Forms.CheckBox();
            this.cbGrayscale = new System.Windows.Forms.CheckBox();
            this.cbInvert = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbResize = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.edResizeWidth = new System.Windows.Forms.TextBox();
            this.edResizeHeight = new System.Windows.Forms.TextBox();
            this.cbCrop = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.edCropLeft = new System.Windows.Forms.TextBox();
            this.edCropTop = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.edCropBottom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.edCropRight = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbRotate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(713, 406);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 17;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(632, 406);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 16;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // pnScreen
            // 
            this.pnScreen.BackColor = System.Drawing.Color.Black;
            this.pnScreen.Location = new System.Drawing.Point(315, 24);
            this.pnScreen.Name = "pnScreen";
            this.pnScreen.Size = new System.Drawing.Size(473, 370);
            this.pnScreen.TabIndex = 15;
            // 
            // btSelectFile
            // 
            this.btSelectFile.Location = new System.Drawing.Point(283, 35);
            this.btSelectFile.Name = "btSelectFile";
            this.btSelectFile.Size = new System.Drawing.Size(26, 23);
            this.btSelectFile.TabIndex = 14;
            this.btSelectFile.Text = "...";
            this.btSelectFile.UseVisualStyleBackColor = true;
            this.btSelectFile.Click += new System.EventHandler(this.btSelectFile_Click);
            // 
            // edFilename
            // 
            this.edFilename.Location = new System.Drawing.Point(15, 37);
            this.edFilename.Name = "edFilename";
            this.edFilename.Size = new System.Drawing.Size(262, 20);
            this.edFilename.TabIndex = 13;
            this.edFilename.Text = "c:\\samples\\!video.mp4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "File";
            // 
            // cbImageLogo
            // 
            this.cbImageLogo.AutoSize = true;
            this.cbImageLogo.Location = new System.Drawing.Point(142, 73);
            this.cbImageLogo.Name = "cbImageLogo";
            this.cbImageLogo.Size = new System.Drawing.Size(115, 17);
            this.cbImageLogo.TabIndex = 38;
            this.cbImageLogo.Text = "Sample image logo";
            this.cbImageLogo.UseVisualStyleBackColor = true;
            this.cbImageLogo.CheckedChanged += new System.EventHandler(this.cbImageLogo_CheckedChanged);
            // 
            // cbTextLogo
            // 
            this.cbTextLogo.AutoSize = true;
            this.cbTextLogo.Location = new System.Drawing.Point(15, 73);
            this.cbTextLogo.Name = "cbTextLogo";
            this.cbTextLogo.Size = new System.Drawing.Size(104, 17);
            this.cbTextLogo.TabIndex = 37;
            this.cbTextLogo.Text = "Sample text logo";
            this.cbTextLogo.UseVisualStyleBackColor = true;
            this.cbTextLogo.CheckedChanged += new System.EventHandler(this.cbTextLogo_CheckedChanged);
            // 
            // cbVerticalFlip
            // 
            this.cbVerticalFlip.AutoSize = true;
            this.cbVerticalFlip.Location = new System.Drawing.Point(15, 96);
            this.cbVerticalFlip.Name = "cbVerticalFlip";
            this.cbVerticalFlip.Size = new System.Drawing.Size(77, 17);
            this.cbVerticalFlip.TabIndex = 39;
            this.cbVerticalFlip.Text = "Vertical flip";
            this.cbVerticalFlip.UseVisualStyleBackColor = true;
            this.cbVerticalFlip.CheckedChanged += new System.EventHandler(this.cbVerticalFlip_CheckedChanged);
            // 
            // cbHorizontalFlip
            // 
            this.cbHorizontalFlip.AutoSize = true;
            this.cbHorizontalFlip.Location = new System.Drawing.Point(142, 96);
            this.cbHorizontalFlip.Name = "cbHorizontalFlip";
            this.cbHorizontalFlip.Size = new System.Drawing.Size(89, 17);
            this.cbHorizontalFlip.TabIndex = 40;
            this.cbHorizontalFlip.Text = "Horizontal flip";
            this.cbHorizontalFlip.UseVisualStyleBackColor = true;
            this.cbHorizontalFlip.CheckedChanged += new System.EventHandler(this.cbHorizontalFlip_CheckedChanged);
            // 
            // cbVerticalMirror
            // 
            this.cbVerticalMirror.AutoSize = true;
            this.cbVerticalMirror.Location = new System.Drawing.Point(15, 119);
            this.cbVerticalMirror.Name = "cbVerticalMirror";
            this.cbVerticalMirror.Size = new System.Drawing.Size(89, 17);
            this.cbVerticalMirror.TabIndex = 41;
            this.cbVerticalMirror.Text = "Vertical mirror";
            this.cbVerticalMirror.UseVisualStyleBackColor = true;
            this.cbVerticalMirror.CheckedChanged += new System.EventHandler(this.cbVerticalMirror_CheckedChanged);
            // 
            // cbHorizontalMirror
            // 
            this.cbHorizontalMirror.AutoSize = true;
            this.cbHorizontalMirror.Location = new System.Drawing.Point(142, 119);
            this.cbHorizontalMirror.Name = "cbHorizontalMirror";
            this.cbHorizontalMirror.Size = new System.Drawing.Size(101, 17);
            this.cbHorizontalMirror.TabIndex = 42;
            this.cbHorizontalMirror.Text = "Horizontal mirror";
            this.cbHorizontalMirror.UseVisualStyleBackColor = true;
            this.cbHorizontalMirror.CheckedChanged += new System.EventHandler(this.cbHorizontalMirror_CheckedChanged);
            // 
            // cbGrayscale
            // 
            this.cbGrayscale.AutoSize = true;
            this.cbGrayscale.Location = new System.Drawing.Point(15, 142);
            this.cbGrayscale.Name = "cbGrayscale";
            this.cbGrayscale.Size = new System.Drawing.Size(73, 17);
            this.cbGrayscale.TabIndex = 43;
            this.cbGrayscale.Text = "Grayscale";
            this.cbGrayscale.UseVisualStyleBackColor = true;
            this.cbGrayscale.CheckedChanged += new System.EventHandler(this.cbGrayscale_CheckedChanged);
            // 
            // cbInvert
            // 
            this.cbInvert.AutoSize = true;
            this.cbInvert.Location = new System.Drawing.Point(142, 142);
            this.cbInvert.Name = "cbInvert";
            this.cbInvert.Size = new System.Drawing.Size(53, 17);
            this.cbInvert.TabIndex = 44;
            this.cbInvert.Text = "Invert";
            this.cbInvert.UseVisualStyleBackColor = true;
            this.cbInvert.CheckedChanged += new System.EventHandler(this.cbInvert_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(312, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "More effects are available using API.";
            // 
            // cbResize
            // 
            this.cbResize.AutoSize = true;
            this.cbResize.Location = new System.Drawing.Point(15, 221);
            this.cbResize.Name = "cbResize";
            this.cbResize.Size = new System.Drawing.Size(58, 17);
            this.cbResize.TabIndex = 46;
            this.cbResize.Text = "Resize";
            this.cbResize.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Resize / crop / rotate should be enabled before starting.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "x";
            // 
            // edResizeWidth
            // 
            this.edResizeWidth.Location = new System.Drawing.Point(90, 219);
            this.edResizeWidth.Name = "edResizeWidth";
            this.edResizeWidth.Size = new System.Drawing.Size(41, 20);
            this.edResizeWidth.TabIndex = 49;
            this.edResizeWidth.Text = "1280";
            // 
            // edResizeHeight
            // 
            this.edResizeHeight.Location = new System.Drawing.Point(155, 219);
            this.edResizeHeight.Name = "edResizeHeight";
            this.edResizeHeight.Size = new System.Drawing.Size(41, 20);
            this.edResizeHeight.TabIndex = 50;
            this.edResizeHeight.Text = "720";
            // 
            // cbCrop
            // 
            this.cbCrop.AutoSize = true;
            this.cbCrop.Location = new System.Drawing.Point(15, 255);
            this.cbCrop.Name = "cbCrop";
            this.cbCrop.Size = new System.Drawing.Size(48, 17);
            this.cbCrop.TabIndex = 51;
            this.cbCrop.Text = "Crop";
            this.cbCrop.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Left";
            // 
            // edCropLeft
            // 
            this.edCropLeft.Location = new System.Drawing.Point(90, 281);
            this.edCropLeft.Name = "edCropLeft";
            this.edCropLeft.Size = new System.Drawing.Size(41, 20);
            this.edCropLeft.TabIndex = 53;
            this.edCropLeft.Text = "0";
            // 
            // edCropTop
            // 
            this.edCropTop.Location = new System.Drawing.Point(90, 307);
            this.edCropTop.Name = "edCropTop";
            this.edCropTop.Size = new System.Drawing.Size(41, 20);
            this.edCropTop.TabIndex = 55;
            this.edCropTop.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 54;
            this.label6.Text = "Top";
            // 
            // edCropBottom
            // 
            this.edCropBottom.Location = new System.Drawing.Point(193, 307);
            this.edCropBottom.Name = "edCropBottom";
            this.edCropBottom.Size = new System.Drawing.Size(41, 20);
            this.edCropBottom.TabIndex = 59;
            this.edCropBottom.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(147, 310);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 58;
            this.label7.Text = "Bottom";
            // 
            // edCropRight
            // 
            this.edCropRight.Location = new System.Drawing.Point(193, 281);
            this.edCropRight.Name = "edCropRight";
            this.edCropRight.Size = new System.Drawing.Size(41, 20);
            this.edCropRight.TabIndex = 57;
            this.edCropRight.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(147, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 56;
            this.label8.Text = "Right";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 343);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 60;
            this.label9.Text = "Rotate";
            // 
            // cbRotate
            // 
            this.cbRotate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRotate.FormattingEnabled = true;
            this.cbRotate.Items.AddRange(new object[] {
            "None",
            "90",
            "180",
            "270"});
            this.cbRotate.Location = new System.Drawing.Point(90, 340);
            this.cbRotate.Name = "cbRotate";
            this.cbRotate.Size = new System.Drawing.Size(144, 21);
            this.cbRotate.TabIndex = 61;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 437);
            this.Controls.Add(this.cbRotate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.edCropBottom);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.edCropRight);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.edCropTop);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.edCropLeft);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCrop);
            this.Controls.Add(this.edResizeHeight);
            this.Controls.Add(this.edResizeWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbResize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbInvert);
            this.Controls.Add(this.cbGrayscale);
            this.Controls.Add(this.cbHorizontalMirror);
            this.Controls.Add(this.cbVerticalMirror);
            this.Controls.Add(this.cbHorizontalFlip);
            this.Controls.Add(this.cbVerticalFlip);
            this.Controls.Add(this.cbImageLogo);
            this.Controls.Add(this.cbTextLogo);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.pnScreen);
            this.Controls.Add(this.btSelectFile);
            this.Controls.Add(this.edFilename);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Video Effects Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Panel pnScreen;
        private System.Windows.Forms.Button btSelectFile;
        private System.Windows.Forms.TextBox edFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox cbImageLogo;
        private System.Windows.Forms.CheckBox cbTextLogo;
        private System.Windows.Forms.CheckBox cbVerticalFlip;
        private System.Windows.Forms.CheckBox cbHorizontalFlip;
        private System.Windows.Forms.CheckBox cbVerticalMirror;
        private System.Windows.Forms.CheckBox cbHorizontalMirror;
        private System.Windows.Forms.CheckBox cbGrayscale;
        private System.Windows.Forms.CheckBox cbInvert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbResize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edResizeWidth;
        private System.Windows.Forms.TextBox edResizeHeight;
        private System.Windows.Forms.CheckBox cbCrop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edCropLeft;
        private System.Windows.Forms.TextBox edCropTop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox edCropBottom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox edCropRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbRotate;
    }
}

