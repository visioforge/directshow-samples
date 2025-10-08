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
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbAudioCaptureSource = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox48 = new System.Windows.Forms.GroupBox();
            this.label343 = new System.Windows.Forms.Label();
            this.edEncryptionKeyHEX = new System.Windows.Forms.TextBox();
            this.rbEncryptionKeyBinary = new System.Windows.Forms.RadioButton();
            this.btEncryptionOpenFile = new System.Windows.Forms.Button();
            this.edEncryptionKeyFile = new System.Windows.Forms.TextBox();
            this.rbEncryptionKeyFile = new System.Windows.Forms.RadioButton();
            this.edEncryptionKeyString = new System.Windows.Forms.TextBox();
            this.rbEncryptionKeyString = new System.Windows.Forms.RadioButton();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.rbEncryptionModeAES256 = new System.Windows.Forms.RadioButton();
            this.rbEncryptionModeAES128 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.edOutput = new System.Windows.Forms.TextBox();
            this.btSelectOutput = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.groupBox48.SuspendLayout();
            this.groupBox47.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Checked = true;
            this.rbFile.Location = new System.Drawing.Point(9, 33);
            this.rbFile.Margin = new System.Windows.Forms.Padding(2);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(68, 17);
            this.rbFile.TabIndex = 1;
            this.rbFile.TabStop = true;
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
            this.rbCamera.Location = new System.Drawing.Point(9, 81);
            this.rbCamera.Name = "rbCamera";
            this.rbCamera.Size = new System.Drawing.Size(59, 17);
            this.rbCamera.TabIndex = 6;
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
            this.btSourceStart.Location = new System.Drawing.Point(380, 396);
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
            this.btSourceStop.Location = new System.Drawing.Point(440, 396);
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
            // groupBox48
            // 
            this.groupBox48.Controls.Add(this.label343);
            this.groupBox48.Controls.Add(this.edEncryptionKeyHEX);
            this.groupBox48.Controls.Add(this.rbEncryptionKeyBinary);
            this.groupBox48.Controls.Add(this.btEncryptionOpenFile);
            this.groupBox48.Controls.Add(this.edEncryptionKeyFile);
            this.groupBox48.Controls.Add(this.rbEncryptionKeyFile);
            this.groupBox48.Controls.Add(this.edEncryptionKeyString);
            this.groupBox48.Controls.Add(this.rbEncryptionKeyString);
            this.groupBox48.Location = new System.Drawing.Point(380, 118);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Size = new System.Drawing.Size(306, 224);
            this.groupBox48.TabIndex = 40;
            this.groupBox48.TabStop = false;
            this.groupBox48.Text = "Encryption key type";
            // 
            // label343
            // 
            this.label343.AutoSize = true;
            this.label343.Location = new System.Drawing.Point(33, 199);
            this.label343.Name = "label343";
            this.label343.Size = new System.Drawing.Size(157, 13);
            this.label343.TabIndex = 10;
            this.label343.Text = "You can assign byte[] using API";
            // 
            // edEncryptionKeyHEX
            // 
            this.edEncryptionKeyHEX.Location = new System.Drawing.Point(36, 176);
            this.edEncryptionKeyHEX.Name = "edEncryptionKeyHEX";
            this.edEncryptionKeyHEX.Size = new System.Drawing.Size(258, 20);
            this.edEncryptionKeyHEX.TabIndex = 9;
            this.edEncryptionKeyHEX.Text = "enter hex data";
            // 
            // rbEncryptionKeyBinary
            // 
            this.rbEncryptionKeyBinary.AutoSize = true;
            this.rbEncryptionKeyBinary.Location = new System.Drawing.Point(14, 153);
            this.rbEncryptionKeyBinary.Name = "rbEncryptionKeyBinary";
            this.rbEncryptionKeyBinary.Size = new System.Drawing.Size(124, 17);
            this.rbEncryptionKeyBinary.TabIndex = 8;
            this.rbEncryptionKeyBinary.Text = "Binary data (v9 SDK)";
            this.rbEncryptionKeyBinary.UseVisualStyleBackColor = true;
            // 
            // btEncryptionOpenFile
            // 
            this.btEncryptionOpenFile.Location = new System.Drawing.Point(271, 114);
            this.btEncryptionOpenFile.Name = "btEncryptionOpenFile";
            this.btEncryptionOpenFile.Size = new System.Drawing.Size(23, 23);
            this.btEncryptionOpenFile.TabIndex = 7;
            this.btEncryptionOpenFile.Text = "...";
            this.btEncryptionOpenFile.UseVisualStyleBackColor = true;
            this.btEncryptionOpenFile.Click += new System.EventHandler(this.btEncryptionOpenFile_Click);
            // 
            // edEncryptionKeyFile
            // 
            this.edEncryptionKeyFile.Location = new System.Drawing.Point(36, 116);
            this.edEncryptionKeyFile.Name = "edEncryptionKeyFile";
            this.edEncryptionKeyFile.Size = new System.Drawing.Size(229, 20);
            this.edEncryptionKeyFile.TabIndex = 6;
            this.edEncryptionKeyFile.Text = "C:\\Samples\\!logo321.png";
            // 
            // rbEncryptionKeyFile
            // 
            this.rbEncryptionKeyFile.AutoSize = true;
            this.rbEncryptionKeyFile.Location = new System.Drawing.Point(14, 93);
            this.rbEncryptionKeyFile.Name = "rbEncryptionKeyFile";
            this.rbEncryptionKeyFile.Size = new System.Drawing.Size(87, 17);
            this.rbEncryptionKeyFile.TabIndex = 5;
            this.rbEncryptionKeyFile.Text = "File (v9 SDK)";
            this.rbEncryptionKeyFile.UseVisualStyleBackColor = true;
            // 
            // edEncryptionKeyString
            // 
            this.edEncryptionKeyString.Location = new System.Drawing.Point(36, 56);
            this.edEncryptionKeyString.Name = "edEncryptionKeyString";
            this.edEncryptionKeyString.Size = new System.Drawing.Size(258, 20);
            this.edEncryptionKeyString.TabIndex = 4;
            this.edEncryptionKeyString.Text = "100";
            // 
            // rbEncryptionKeyString
            // 
            this.rbEncryptionKeyString.AutoSize = true;
            this.rbEncryptionKeyString.Checked = true;
            this.rbEncryptionKeyString.Location = new System.Drawing.Point(14, 28);
            this.rbEncryptionKeyString.Name = "rbEncryptionKeyString";
            this.rbEncryptionKeyString.Size = new System.Drawing.Size(52, 17);
            this.rbEncryptionKeyString.TabIndex = 0;
            this.rbEncryptionKeyString.TabStop = true;
            this.rbEncryptionKeyString.Text = "String";
            this.rbEncryptionKeyString.UseVisualStyleBackColor = true;
            // 
            // groupBox47
            // 
            this.groupBox47.Controls.Add(this.rbEncryptionModeAES256);
            this.groupBox47.Controls.Add(this.rbEncryptionModeAES128);
            this.groupBox47.Location = new System.Drawing.Point(380, 29);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Size = new System.Drawing.Size(306, 83);
            this.groupBox47.TabIndex = 39;
            this.groupBox47.TabStop = false;
            this.groupBox47.Text = "Method";
            // 
            // rbEncryptionModeAES256
            // 
            this.rbEncryptionModeAES256.AutoSize = true;
            this.rbEncryptionModeAES256.Checked = true;
            this.rbEncryptionModeAES256.Location = new System.Drawing.Point(14, 51);
            this.rbEncryptionModeAES256.Name = "rbEncryptionModeAES256";
            this.rbEncryptionModeAES256.Size = new System.Drawing.Size(198, 17);
            this.rbEncryptionModeAES256.TabIndex = 1;
            this.rbEncryptionModeAES256.TabStop = true;
            this.rbEncryptionModeAES256.Text = "AES-256 (v9 encryption SDK output)";
            this.rbEncryptionModeAES256.UseVisualStyleBackColor = true;
            // 
            // rbEncryptionModeAES128
            // 
            this.rbEncryptionModeAES128.AutoSize = true;
            this.rbEncryptionModeAES128.Location = new System.Drawing.Point(14, 28);
            this.rbEncryptionModeAES128.Name = "rbEncryptionModeAES128";
            this.rbEncryptionModeAES128.Size = new System.Drawing.Size(198, 17);
            this.rbEncryptionModeAES128.TabIndex = 0;
            this.rbEncryptionModeAES128.Text = "AES-128 (v8 encryption SDK output)";
            this.rbEncryptionModeAES128.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 345);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Output file";
            // 
            // edOutput
            // 
            this.edOutput.Location = new System.Drawing.Point(380, 361);
            this.edOutput.Name = "edOutput";
            this.edOutput.Size = new System.Drawing.Size(277, 20);
            this.edOutput.TabIndex = 42;
            // 
            // btSelectOutput
            // 
            this.btSelectOutput.Location = new System.Drawing.Point(663, 359);
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
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(574, 400);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(112, 17);
            this.cbDebugMode.TabIndex = 37;
            this.cbDebugMode.Text = "Enable debugging";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 429);
            this.Controls.Add(this.btSelectOutput);
            this.Controls.Add(this.edOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox48);
            this.Controls.Add(this.groupBox47);
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
            this.Text = "Video Encryption SDK - Encryption Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox48.ResumeLayout(false);
            this.groupBox48.PerformLayout();
            this.groupBox47.ResumeLayout(false);
            this.groupBox47.PerformLayout();
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
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbAudioCaptureSource;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox48;
        private System.Windows.Forms.Label label343;
        private System.Windows.Forms.TextBox edEncryptionKeyHEX;
        private System.Windows.Forms.RadioButton rbEncryptionKeyBinary;
        private System.Windows.Forms.Button btEncryptionOpenFile;
        private System.Windows.Forms.TextBox edEncryptionKeyFile;
        private System.Windows.Forms.RadioButton rbEncryptionKeyFile;
        private System.Windows.Forms.TextBox edEncryptionKeyString;
        private System.Windows.Forms.RadioButton rbEncryptionKeyString;
        private System.Windows.Forms.GroupBox groupBox47;
        private System.Windows.Forms.RadioButton rbEncryptionModeAES256;
        private System.Windows.Forms.RadioButton rbEncryptionModeAES128;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edOutput;
        private System.Windows.Forms.Button btSelectOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox cbDebugMode;
    }
}

