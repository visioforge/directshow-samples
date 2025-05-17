namespace VideoMixerDemo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edFilename1 = new System.Windows.Forms.TextBox();
            this.edFilename2 = new System.Windows.Forms.TextBox();
            this.edFilename3 = new System.Windows.Forms.TextBox();
            this.btSelectFile1 = new System.Windows.Forms.Button();
            this.btSelectFile3 = new System.Windows.Forms.Button();
            this.btSelectFile2 = new System.Windows.Forms.Button();
            this.pnScreen = new System.Windows.Forms.Panel();
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.edOutputWidth = new System.Windows.Forms.TextBox();
            this.edOutputHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.edFile1Y = new System.Windows.Forms.TextBox();
            this.edFile1X = new System.Windows.Forms.TextBox();
            this.edFile1Height = new System.Windows.Forms.TextBox();
            this.edFile1Width = new System.Windows.Forms.TextBox();
            this.edFile2Height = new System.Windows.Forms.TextBox();
            this.edFile2Width = new System.Windows.Forms.TextBox();
            this.edFile2Y = new System.Windows.Forms.TextBox();
            this.edFile2X = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.edFile3Height = new System.Windows.Forms.TextBox();
            this.edFile3Width = new System.Windows.Forms.TextBox();
            this.edFile3Y = new System.Windows.Forms.TextBox();
            this.edFile3X = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.edBGImage = new System.Windows.Forms.TextBox();
            this.btSelectBGImage = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pnBGImage = new System.Windows.Forms.Panel();
            this.cbTextLogo = new System.Windows.Forms.CheckBox();
            this.cbImageLogo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "File 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "File 3";
            // 
            // edFilename1
            // 
            this.edFilename1.Location = new System.Drawing.Point(15, 25);
            this.edFilename1.Name = "edFilename1";
            this.edFilename1.Size = new System.Drawing.Size(262, 20);
            this.edFilename1.TabIndex = 3;
            this.edFilename1.Text = "c:\\samples\\!video.avi";
            // 
            // edFilename2
            // 
            this.edFilename2.Location = new System.Drawing.Point(15, 73);
            this.edFilename2.Name = "edFilename2";
            this.edFilename2.Size = new System.Drawing.Size(262, 20);
            this.edFilename2.TabIndex = 4;
            this.edFilename2.Text = "c:\\samples\\!video.mp4";
            // 
            // edFilename3
            // 
            this.edFilename3.Location = new System.Drawing.Point(15, 121);
            this.edFilename3.Name = "edFilename3";
            this.edFilename3.Size = new System.Drawing.Size(262, 20);
            this.edFilename3.TabIndex = 5;
            this.edFilename3.Text = "c:\\samples\\!video2.wmv";
            // 
            // btSelectFile1
            // 
            this.btSelectFile1.Location = new System.Drawing.Point(283, 23);
            this.btSelectFile1.Name = "btSelectFile1";
            this.btSelectFile1.Size = new System.Drawing.Size(26, 23);
            this.btSelectFile1.TabIndex = 6;
            this.btSelectFile1.Text = "...";
            this.btSelectFile1.UseVisualStyleBackColor = true;
            this.btSelectFile1.Click += new System.EventHandler(this.btSelectFile1_Click);
            // 
            // btSelectFile3
            // 
            this.btSelectFile3.Location = new System.Drawing.Point(283, 119);
            this.btSelectFile3.Name = "btSelectFile3";
            this.btSelectFile3.Size = new System.Drawing.Size(26, 23);
            this.btSelectFile3.TabIndex = 7;
            this.btSelectFile3.Text = "...";
            this.btSelectFile3.UseVisualStyleBackColor = true;
            this.btSelectFile3.Click += new System.EventHandler(this.btSelectFile3_Click);
            // 
            // btSelectFile2
            // 
            this.btSelectFile2.Location = new System.Drawing.Point(283, 71);
            this.btSelectFile2.Name = "btSelectFile2";
            this.btSelectFile2.Size = new System.Drawing.Size(26, 23);
            this.btSelectFile2.TabIndex = 8;
            this.btSelectFile2.Text = "...";
            this.btSelectFile2.UseVisualStyleBackColor = true;
            this.btSelectFile2.Click += new System.EventHandler(this.btSelectFile2_Click);
            // 
            // pnScreen
            // 
            this.pnScreen.BackColor = System.Drawing.Color.Black;
            this.pnScreen.Location = new System.Drawing.Point(315, 12);
            this.pnScreen.Name = "pnScreen";
            this.pnScreen.Size = new System.Drawing.Size(473, 370);
            this.pnScreen.TabIndex = 9;
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(632, 394);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 10;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(713, 394);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 11;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Output resolution";
            // 
            // edOutputWidth
            // 
            this.edOutputWidth.Location = new System.Drawing.Point(15, 172);
            this.edOutputWidth.Name = "edOutputWidth";
            this.edOutputWidth.Size = new System.Drawing.Size(42, 20);
            this.edOutputWidth.TabIndex = 13;
            this.edOutputWidth.Text = "1920";
            // 
            // edOutputHeight
            // 
            this.edOutputHeight.Location = new System.Drawing.Point(63, 172);
            this.edOutputHeight.Name = "edOutputHeight";
            this.edOutputHeight.Size = new System.Drawing.Size(42, 20);
            this.edOutputHeight.TabIndex = 14;
            this.edOutputHeight.Text = "1080";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "File 1 position (x, y, width, height)";
            // 
            // edFile1Y
            // 
            this.edFile1Y.Location = new System.Drawing.Point(63, 224);
            this.edFile1Y.Name = "edFile1Y";
            this.edFile1Y.Size = new System.Drawing.Size(42, 20);
            this.edFile1Y.TabIndex = 17;
            this.edFile1Y.Text = "0";
            // 
            // edFile1X
            // 
            this.edFile1X.Location = new System.Drawing.Point(15, 224);
            this.edFile1X.Name = "edFile1X";
            this.edFile1X.Size = new System.Drawing.Size(42, 20);
            this.edFile1X.TabIndex = 16;
            this.edFile1X.Text = "0";
            // 
            // edFile1Height
            // 
            this.edFile1Height.Location = new System.Drawing.Point(167, 224);
            this.edFile1Height.Name = "edFile1Height";
            this.edFile1Height.Size = new System.Drawing.Size(42, 20);
            this.edFile1Height.TabIndex = 19;
            this.edFile1Height.Text = "540";
            // 
            // edFile1Width
            // 
            this.edFile1Width.Location = new System.Drawing.Point(119, 224);
            this.edFile1Width.Name = "edFile1Width";
            this.edFile1Width.Size = new System.Drawing.Size(42, 20);
            this.edFile1Width.TabIndex = 18;
            this.edFile1Width.Text = "960";
            // 
            // edFile2Height
            // 
            this.edFile2Height.Location = new System.Drawing.Point(167, 271);
            this.edFile2Height.Name = "edFile2Height";
            this.edFile2Height.Size = new System.Drawing.Size(42, 20);
            this.edFile2Height.TabIndex = 24;
            this.edFile2Height.Text = "540";
            // 
            // edFile2Width
            // 
            this.edFile2Width.Location = new System.Drawing.Point(119, 271);
            this.edFile2Width.Name = "edFile2Width";
            this.edFile2Width.Size = new System.Drawing.Size(42, 20);
            this.edFile2Width.TabIndex = 23;
            this.edFile2Width.Text = "960";
            // 
            // edFile2Y
            // 
            this.edFile2Y.Location = new System.Drawing.Point(63, 271);
            this.edFile2Y.Name = "edFile2Y";
            this.edFile2Y.Size = new System.Drawing.Size(42, 20);
            this.edFile2Y.TabIndex = 22;
            this.edFile2Y.Text = "0";
            // 
            // edFile2X
            // 
            this.edFile2X.Location = new System.Drawing.Point(15, 271);
            this.edFile2X.Name = "edFile2X";
            this.edFile2X.Size = new System.Drawing.Size(42, 20);
            this.edFile2X.TabIndex = 21;
            this.edFile2X.Text = "960";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "File 2 position (x, y, width, height)";
            // 
            // edFile3Height
            // 
            this.edFile3Height.Location = new System.Drawing.Point(167, 321);
            this.edFile3Height.Name = "edFile3Height";
            this.edFile3Height.Size = new System.Drawing.Size(42, 20);
            this.edFile3Height.TabIndex = 29;
            this.edFile3Height.Text = "540";
            // 
            // edFile3Width
            // 
            this.edFile3Width.Location = new System.Drawing.Point(119, 321);
            this.edFile3Width.Name = "edFile3Width";
            this.edFile3Width.Size = new System.Drawing.Size(42, 20);
            this.edFile3Width.TabIndex = 28;
            this.edFile3Width.Text = "960";
            // 
            // edFile3Y
            // 
            this.edFile3Y.Location = new System.Drawing.Point(63, 321);
            this.edFile3Y.Name = "edFile3Y";
            this.edFile3Y.Size = new System.Drawing.Size(42, 20);
            this.edFile3Y.TabIndex = 27;
            this.edFile3Y.Text = "540";
            // 
            // edFile3X
            // 
            this.edFile3X.Location = new System.Drawing.Point(15, 321);
            this.edFile3X.Name = "edFile3X";
            this.edFile3X.Size = new System.Drawing.Size(42, 20);
            this.edFile3X.TabIndex = 26;
            this.edFile3X.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 305);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "File 3 position (x, y, width, height)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 355);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Background image";
            // 
            // edBGImage
            // 
            this.edBGImage.Location = new System.Drawing.Point(15, 371);
            this.edBGImage.Name = "edBGImage";
            this.edBGImage.Size = new System.Drawing.Size(262, 20);
            this.edBGImage.TabIndex = 31;
            this.edBGImage.Text = "c:\\Samples\\pics\\1.jpg";
            // 
            // btSelectBGImage
            // 
            this.btSelectBGImage.Location = new System.Drawing.Point(283, 369);
            this.btSelectBGImage.Name = "btSelectBGImage";
            this.btSelectBGImage.Size = new System.Drawing.Size(26, 23);
            this.btSelectBGImage.TabIndex = 32;
            this.btSelectBGImage.Text = "...";
            this.btSelectBGImage.UseVisualStyleBackColor = true;
            this.btSelectBGImage.Click += new System.EventHandler(this.btSelectBGImage_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 399);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "Background color";
            // 
            // pnBGImage
            // 
            this.pnBGImage.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnBGImage.Location = new System.Drawing.Point(119, 399);
            this.pnBGImage.Name = "pnBGImage";
            this.pnBGImage.Size = new System.Drawing.Size(20, 20);
            this.pnBGImage.TabIndex = 34;
            this.pnBGImage.Click += new System.EventHandler(this.pnBGImage_Click);
            // 
            // cbTextLogo
            // 
            this.cbTextLogo.AutoSize = true;
            this.cbTextLogo.Location = new System.Drawing.Point(15, 438);
            this.cbTextLogo.Name = "cbTextLogo";
            this.cbTextLogo.Size = new System.Drawing.Size(104, 17);
            this.cbTextLogo.TabIndex = 35;
            this.cbTextLogo.Text = "Sample text logo";
            this.cbTextLogo.UseVisualStyleBackColor = true;
            // 
            // cbImageLogo
            // 
            this.cbImageLogo.AutoSize = true;
            this.cbImageLogo.Location = new System.Drawing.Point(142, 438);
            this.cbImageLogo.Name = "cbImageLogo";
            this.cbImageLogo.Size = new System.Drawing.Size(115, 17);
            this.cbImageLogo.TabIndex = 36;
            this.cbImageLogo.Text = "Sample image logo";
            this.cbImageLogo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.cbImageLogo);
            this.Controls.Add(this.cbTextLogo);
            this.Controls.Add(this.pnBGImage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btSelectBGImage);
            this.Controls.Add(this.edBGImage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.edFile3Height);
            this.Controls.Add(this.edFile3Width);
            this.Controls.Add(this.edFile3Y);
            this.Controls.Add(this.edFile3X);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.edFile2Height);
            this.Controls.Add(this.edFile2Width);
            this.Controls.Add(this.edFile2Y);
            this.Controls.Add(this.edFile2X);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.edFile1Height);
            this.Controls.Add(this.edFile1Width);
            this.Controls.Add(this.edFile1Y);
            this.Controls.Add(this.edFile1X);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edOutputHeight);
            this.Controls.Add(this.edOutputWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.pnScreen);
            this.Controls.Add(this.btSelectFile2);
            this.Controls.Add(this.btSelectFile3);
            this.Controls.Add(this.btSelectFile1);
            this.Controls.Add(this.edFilename3);
            this.Controls.Add(this.edFilename2);
            this.Controls.Add(this.edFilename1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Video Mixer Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edFilename1;
        private System.Windows.Forms.TextBox edFilename2;
        private System.Windows.Forms.TextBox edFilename3;
        private System.Windows.Forms.Button btSelectFile1;
        private System.Windows.Forms.Button btSelectFile3;
        private System.Windows.Forms.Button btSelectFile2;
        private System.Windows.Forms.Panel pnScreen;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edOutputWidth;
        private System.Windows.Forms.TextBox edOutputHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edFile1Y;
        private System.Windows.Forms.TextBox edFile1X;
        private System.Windows.Forms.TextBox edFile1Height;
        private System.Windows.Forms.TextBox edFile1Width;
        private System.Windows.Forms.TextBox edFile2Height;
        private System.Windows.Forms.TextBox edFile2Width;
        private System.Windows.Forms.TextBox edFile2Y;
        private System.Windows.Forms.TextBox edFile2X;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox edFile3Height;
        private System.Windows.Forms.TextBox edFile3Width;
        private System.Windows.Forms.TextBox edFile3Y;
        private System.Windows.Forms.TextBox edFile3X;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox edBGImage;
        private System.Windows.Forms.Button btSelectBGImage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnBGImage;
        private System.Windows.Forms.CheckBox cbTextLogo;
        private System.Windows.Forms.CheckBox cbImageLogo;
    }
}

