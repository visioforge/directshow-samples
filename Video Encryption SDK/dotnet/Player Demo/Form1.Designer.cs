namespace Player_Demo
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
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.btSourceStop = new System.Windows.Forms.Button();
            this.btSourceStart = new System.Windows.Forms.Button();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.edSourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnScreen = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox48.SuspendLayout();
            this.groupBox47.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox48.Location = new System.Drawing.Point(455, 106);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Size = new System.Drawing.Size(306, 224);
            this.groupBox48.TabIndex = 48;
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
            this.groupBox47.Location = new System.Drawing.Point(455, 17);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Size = new System.Drawing.Size(306, 83);
            this.groupBox47.TabIndex = 47;
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
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(649, 388);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(112, 17);
            this.cbDebugMode.TabIndex = 46;
            this.cbDebugMode.Text = "Enable debugging";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // btSourceStop
            // 
            this.btSourceStop.Enabled = false;
            this.btSourceStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStop.Location = new System.Drawing.Point(515, 384);
            this.btSourceStop.Name = "btSourceStop";
            this.btSourceStop.Size = new System.Drawing.Size(54, 23);
            this.btSourceStop.TabIndex = 45;
            this.btSourceStop.Text = "Stop";
            this.btSourceStop.UseVisualStyleBackColor = true;
            this.btSourceStop.Click += new System.EventHandler(this.btSourceStop_Click);
            // 
            // btSourceStart
            // 
            this.btSourceStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStart.Location = new System.Drawing.Point(455, 384);
            this.btSourceStart.Name = "btSourceStart";
            this.btSourceStart.Size = new System.Drawing.Size(54, 23);
            this.btSourceStart.TabIndex = 44;
            this.btSourceStart.Text = "Start";
            this.btSourceStart.UseVisualStyleBackColor = true;
            this.btSourceStart.Click += new System.EventHandler(this.btSourceStart_Click);
            // 
            // btOpenFile
            // 
            this.btOpenFile.Location = new System.Drawing.Point(425, 31);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(24, 23);
            this.btOpenFile.TabIndex = 43;
            this.btOpenFile.Text = "...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // edSourceFile
            // 
            this.edSourceFile.Location = new System.Drawing.Point(12, 33);
            this.edSourceFile.Name = "edSourceFile";
            this.edSourceFile.Size = new System.Drawing.Size(407, 20);
            this.edSourceFile.TabIndex = 42;
            this.edSourceFile.Text = "C:\\Samples\\!video.avi";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Video file";
            // 
            // pnScreen
            // 
            this.pnScreen.BackColor = System.Drawing.Color.Black;
            this.pnScreen.Location = new System.Drawing.Point(12, 68);
            this.pnScreen.Name = "pnScreen";
            this.pnScreen.Size = new System.Drawing.Size(437, 337);
            this.pnScreen.TabIndex = 50;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 419);
            this.Controls.Add(this.pnScreen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox48);
            this.Controls.Add(this.groupBox47);
            this.Controls.Add(this.cbDebugMode);
            this.Controls.Add(this.btSourceStop);
            this.Controls.Add(this.btSourceStart);
            this.Controls.Add(this.btOpenFile);
            this.Controls.Add(this.edSourceFile);
            this.Name = "Form1";
            this.Text = "Video Encryption SDK - Encryption Demo";
            this.groupBox48.ResumeLayout(false);
            this.groupBox48.PerformLayout();
            this.groupBox47.ResumeLayout(false);
            this.groupBox47.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.Button btSourceStop;
        private System.Windows.Forms.Button btSourceStart;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.TextBox edSourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnScreen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

