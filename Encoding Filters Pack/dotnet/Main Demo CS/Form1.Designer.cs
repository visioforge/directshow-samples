namespace Main_Demo
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
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.edSourceFile = new System.Windows.Forms.TextBox();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.rbCamera = new System.Windows.Forms.RadioButton();
            this.cbVideoCaptureSource = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSourceVideoFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSourceFrameRate = new System.Windows.Forms.ComboBox();
            this.btSourceStart = new System.Windows.Forms.Button();
            this.btSourceStop = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbAudioCaptureSource = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.edOutput = new System.Windows.Forms.TextBox();
            this.btSelectOutput = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbMP4v10 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.rbMP4v11 = new System.Windows.Forms.RadioButton();
            this.edGPUAvailable = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Location = new System.Drawing.Point(9, 33);
            this.rbFile.Margin = new System.Windows.Forms.Padding(2);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(68, 17);
            this.rbFile.TabIndex = 1;
            this.rbFile.Text = "Video file";
            this.rbFile.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(115, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Video source";
            // 
            // edSourceFile
            // 
            this.edSourceFile.Location = new System.Drawing.Point(26, 55);
            this.edSourceFile.Name = "edSourceFile";
            this.edSourceFile.Size = new System.Drawing.Size(276, 20);
            this.edSourceFile.TabIndex = 4;
            this.edSourceFile.Text = "C:\\Samples\\!video.avi";
            // 
            // btOpenFile
            // 
            this.btOpenFile.Location = new System.Drawing.Point(308, 53);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(24, 23);
            this.btOpenFile.TabIndex = 5;
            this.btOpenFile.Text = "...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // rbCamera
            // 
            this.rbCamera.AutoSize = true;
            this.rbCamera.Checked = true;
            this.rbCamera.Location = new System.Drawing.Point(9, 81);
            this.rbCamera.Name = "rbCamera";
            this.rbCamera.Size = new System.Drawing.Size(59, 17);
            this.rbCamera.TabIndex = 6;
            this.rbCamera.TabStop = true;
            this.rbCamera.Text = "Device";
            this.rbCamera.UseVisualStyleBackColor = true;
            // 
            // cbVideoCaptureSource
            // 
            this.cbVideoCaptureSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoCaptureSource.FormattingEnabled = true;
            this.cbVideoCaptureSource.Location = new System.Drawing.Point(26, 121);
            this.cbVideoCaptureSource.Name = "cbVideoCaptureSource";
            this.cbVideoCaptureSource.Size = new System.Drawing.Size(306, 21);
            this.cbVideoCaptureSource.TabIndex = 7;
            this.cbVideoCaptureSource.SelectedIndexChanged += new System.EventHandler(this.cbVideoCaptureSource_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Format";
            // 
            // cbSourceVideoFormat
            // 
            this.cbSourceVideoFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceVideoFormat.FormattingEnabled = true;
            this.cbSourceVideoFormat.Location = new System.Drawing.Point(26, 170);
            this.cbSourceVideoFormat.Name = "cbSourceVideoFormat";
            this.cbSourceVideoFormat.Size = new System.Drawing.Size(234, 21);
            this.cbSourceVideoFormat.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Frame rate";
            // 
            // cbSourceFrameRate
            // 
            this.cbSourceFrameRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceFrameRate.FormattingEnabled = true;
            this.cbSourceFrameRate.Location = new System.Drawing.Point(266, 170);
            this.cbSourceFrameRate.Name = "cbSourceFrameRate";
            this.cbSourceFrameRate.Size = new System.Drawing.Size(66, 21);
            this.cbSourceFrameRate.TabIndex = 11;
            // 
            // btSourceStart
            // 
            this.btSourceStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStart.Location = new System.Drawing.Point(9, 355);
            this.btSourceStart.Name = "btSourceStart";
            this.btSourceStart.Size = new System.Drawing.Size(54, 23);
            this.btSourceStart.TabIndex = 12;
            this.btSourceStart.Text = "Start";
            this.btSourceStart.UseVisualStyleBackColor = true;
            this.btSourceStart.Click += new System.EventHandler(this.btSourceStart_Click);
            // 
            // btSourceStop
            // 
            this.btSourceStop.Enabled = false;
            this.btSourceStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStop.Location = new System.Drawing.Point(69, 355);
            this.btSourceStop.Name = "btSourceStop";
            this.btSourceStop.Size = new System.Drawing.Size(54, 23);
            this.btSourceStop.TabIndex = 13;
            this.btSourceStop.Text = "Stop";
            this.btSourceStop.UseVisualStyleBackColor = true;
            this.btSourceStop.Click += new System.EventHandler(this.btSourceStop_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Video source";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(23, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Audio source";
            // 
            // cbAudioCaptureSource
            // 
            this.cbAudioCaptureSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioCaptureSource.FormattingEnabled = true;
            this.cbAudioCaptureSource.Location = new System.Drawing.Point(26, 219);
            this.cbAudioCaptureSource.Name = "cbAudioCaptureSource";
            this.cbAudioCaptureSource.Size = new System.Drawing.Size(306, 21);
            this.cbAudioCaptureSource.TabIndex = 34;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(574, 355);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(112, 17);
            this.cbDebugMode.TabIndex = 37;
            this.cbDebugMode.Text = "Enable debugging";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(469, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 38;
            this.label2.Text = "Output settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Output file";
            // 
            // edOutput
            // 
            this.edOutput.Location = new System.Drawing.Point(9, 328);
            this.edOutput.Name = "edOutput";
            this.edOutput.Size = new System.Drawing.Size(648, 20);
            this.edOutput.TabIndex = 42;
            // 
            // btSelectOutput
            // 
            this.btSelectOutput.Location = new System.Drawing.Point(663, 326);
            this.btSelectOutput.Name = "btSelectOutput";
            this.btSelectOutput.Size = new System.Drawing.Size(23, 23);
            this.btSelectOutput.TabIndex = 43;
            this.btSelectOutput.Text = "...";
            this.btSelectOutput.UseVisualStyleBackColor = true;
            this.btSelectOutput.Click += new System.EventHandler(this.btSelectOutput_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Encrypted files|*.enc|All files|*.*";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.edGPUAvailable);
            this.panel1.Controls.Add(this.rbMP4v11);
            this.panel1.Controls.Add(this.rbMP4v10);
            this.panel1.Location = new System.Drawing.Point(380, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 92);
            this.panel1.TabIndex = 49;
            // 
            // rbMP4v10
            // 
            this.rbMP4v10.AutoSize = true;
            this.rbMP4v10.Checked = true;
            this.rbMP4v10.Location = new System.Drawing.Point(16, 7);
            this.rbMP4v10.Name = "rbMP4v10";
            this.rbMP4v10.Size = new System.Drawing.Size(164, 17);
            this.rbMP4v10.TabIndex = 47;
            this.rbMP4v10.TabStop = true;
            this.rbMP4v10.Text = "MP4 (CPU / Intel QuickSync)";
            this.rbMP4v10.UseVisualStyleBackColor = true;
            this.rbMP4v10.Click += new System.EventHandler(this.rbMP4v10_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(377, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Demo app uses default parameters";
            // 
            // rbMP4v11
            // 
            this.rbMP4v11.AutoSize = true;
            this.rbMP4v11.Location = new System.Drawing.Point(16, 33);
            this.rbMP4v11.Name = "rbMP4v11";
            this.rbMP4v11.Size = new System.Drawing.Size(201, 17);
            this.rbMP4v11.TabIndex = 48;
            this.rbMP4v11.TabStop = true;
            this.rbMP4v11.Text = "MP4 (CPU / Intel, Nvidia, AMD GPU)";
            this.rbMP4v11.UseVisualStyleBackColor = true;
            // 
            // edGPUAvailable
            // 
            this.edGPUAvailable.Location = new System.Drawing.Point(80, 56);
            this.edGPUAvailable.Name = "edGPUAvailable";
            this.edGPUAvailable.ReadOnly = true;
            this.edGPUAvailable.Size = new System.Drawing.Size(211, 20);
            this.edGPUAvailable.TabIndex = 49;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Available";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 389);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btSelectOutput);
            this.Controls.Add(this.edOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDebugMode);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbAudioCaptureSource);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btSourceStop);
            this.Controls.Add(this.btSourceStart);
            this.Controls.Add(this.cbSourceFrameRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSourceVideoFormat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbVideoCaptureSource);
            this.Controls.Add(this.rbCamera);
            this.Controls.Add(this.btOpenFile);
            this.Controls.Add(this.edSourceFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbFile);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Video Encoding Filters SDK Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rbFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edSourceFile;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.RadioButton rbCamera;
        private System.Windows.Forms.ComboBox cbVideoCaptureSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSourceVideoFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSourceFrameRate;
        private System.Windows.Forms.Button btSourceStart;
        private System.Windows.Forms.Button btSourceStop;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbAudioCaptureSource;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edOutput;
        private System.Windows.Forms.Button btSelectOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbMP4v10;
        private System.Windows.Forms.RadioButton rbMP4v11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox edGPUAvailable;
        private System.Windows.Forms.Label label7;
    }
}

