
namespace FFMPEG_Source_Demo
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
            components = new System.ComponentModel.Container();
            btSelectFile = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            pnScreen = new System.Windows.Forms.Panel();
            btStop = new System.Windows.Forms.Button();
            btPause = new System.Windows.Forms.Button();
            btResume = new System.Windows.Forms.Button();
            btStart = new System.Windows.Forms.Button();
            tbSpeed = new System.Windows.Forms.TrackBar();
            label16 = new System.Windows.Forms.Label();
            lbTime = new System.Windows.Forms.Label();
            tbTimeline = new System.Windows.Forms.TrackBar();
            label2 = new System.Windows.Forms.Label();
            edConnectionTimeOut = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            cbBufferingMode = new System.Windows.Forms.ComboBox();
            cbUseGPU = new System.Windows.Forms.CheckBox();
            tmProgress = new System.Windows.Forms.Timer(components);
            edFilename = new System.Windows.Forms.TextBox();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            label4 = new System.Windows.Forms.Label();
            cbVideoStream = new System.Windows.Forms.ComboBox();
            cbAudioStream = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)tbSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbTimeline).BeginInit();
            SuspendLayout();
            // 
            // btSelectFile
            // 
            btSelectFile.Location = new System.Drawing.Point(658, 44);
            btSelectFile.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btSelectFile.Name = "btSelectFile";
            btSelectFile.Size = new System.Drawing.Size(40, 44);
            btSelectFile.TabIndex = 0;
            btSelectFile.Text = "...";
            btSelectFile.UseVisualStyleBackColor = true;
            btSelectFile.Click += btSelectFile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 17);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(142, 25);
            label1.TabIndex = 1;
            label1.Text = "URL or file name";
            // 
            // pnScreen
            // 
            pnScreen.BackColor = System.Drawing.Color.Black;
            pnScreen.Location = new System.Drawing.Point(730, 17);
            pnScreen.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            pnScreen.Name = "pnScreen";
            pnScreen.Size = new System.Drawing.Size(912, 825);
            pnScreen.TabIndex = 3;
            // 
            // btStop
            // 
            btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btStop.Location = new System.Drawing.Point(315, 277);
            btStop.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btStop.Name = "btStop";
            btStop.Size = new System.Drawing.Size(77, 44);
            btStop.TabIndex = 19;
            btStop.Text = "Stop";
            btStop.UseVisualStyleBackColor = true;
            btStop.Click += btStop_Click;
            // 
            // btPause
            // 
            btPause.Location = new System.Drawing.Point(218, 277);
            btPause.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btPause.Name = "btPause";
            btPause.Size = new System.Drawing.Size(87, 44);
            btPause.TabIndex = 18;
            btPause.Text = "Pause";
            btPause.UseVisualStyleBackColor = true;
            btPause.Click += btPause_Click;
            // 
            // btResume
            // 
            btResume.Location = new System.Drawing.Point(107, 277);
            btResume.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btResume.Name = "btResume";
            btResume.Size = new System.Drawing.Size(102, 44);
            btResume.TabIndex = 17;
            btResume.Text = "Resume";
            btResume.UseVisualStyleBackColor = true;
            btResume.Click += btResume_Click;
            // 
            // btStart
            // 
            btStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btStart.Location = new System.Drawing.Point(25, 277);
            btStart.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btStart.Name = "btStart";
            btStart.Size = new System.Drawing.Size(72, 44);
            btStart.TabIndex = 16;
            btStart.Text = "Start";
            btStart.UseVisualStyleBackColor = true;
            btStart.Click += btStart_Click;
            // 
            // tbSpeed
            // 
            tbSpeed.Location = new System.Drawing.Point(550, 217);
            tbSpeed.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            tbSpeed.Maximum = 25;
            tbSpeed.Minimum = 5;
            tbSpeed.Name = "tbSpeed";
            tbSpeed.Size = new System.Drawing.Size(148, 69);
            tbSpeed.TabIndex = 15;
            tbSpeed.Value = 10;
            tbSpeed.Scroll += tbSpeed_Scroll;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(552, 187);
            label16.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(62, 25);
            label16.TabIndex = 14;
            label16.Text = "Speed";
            // 
            // lbTime
            // 
            lbTime.AutoSize = true;
            lbTime.Location = new System.Drawing.Point(375, 229);
            lbTime.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lbTime.Name = "lbTime";
            lbTime.Size = new System.Drawing.Size(155, 25);
            lbTime.TabIndex = 13;
            lbTime.Text = "00:00:00/00:00:00";
            // 
            // tbTimeline
            // 
            tbTimeline.Location = new System.Drawing.Point(25, 202);
            tbTimeline.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            tbTimeline.Maximum = 100;
            tbTimeline.Name = "tbTimeline";
            tbTimeline.Size = new System.Drawing.Size(345, 69);
            tbTimeline.TabIndex = 12;
            tbTimeline.Scroll += tbTimeline_Scroll;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 104);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(208, 25);
            label2.TabIndex = 22;
            label2.Text = "Connection timeout (ms)";
            // 
            // edConnectionTimeOut
            // 
            edConnectionTimeOut.Location = new System.Drawing.Point(230, 98);
            edConnectionTimeOut.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            edConnectionTimeOut.Name = "edConnectionTimeOut";
            edConnectionTimeOut.Size = new System.Drawing.Size(72, 31);
            edConnectionTimeOut.TabIndex = 23;
            edConnectionTimeOut.Text = "10000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(333, 104);
            label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(136, 25);
            label3.TabIndex = 24;
            label3.Text = "Buffering mode";
            // 
            // cbBufferingMode
            // 
            cbBufferingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBufferingMode.FormattingEnabled = true;
            cbBufferingMode.Items.AddRange(new object[] { "Auto", "On", "Off" });
            cbBufferingMode.Location = new System.Drawing.Point(473, 98);
            cbBufferingMode.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbBufferingMode.Name = "cbBufferingMode";
            cbBufferingMode.Size = new System.Drawing.Size(106, 33);
            cbBufferingMode.TabIndex = 25;
            // 
            // cbUseGPU
            // 
            cbUseGPU.AutoSize = true;
            cbUseGPU.Location = new System.Drawing.Point(592, 102);
            cbUseGPU.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbUseGPU.Name = "cbUseGPU";
            cbUseGPU.Size = new System.Drawing.Size(106, 29);
            cbUseGPU.TabIndex = 26;
            cbUseGPU.Text = "Use GPU";
            cbUseGPU.UseVisualStyleBackColor = true;
            // 
            // tmProgress
            // 
            tmProgress.Tick += tmProgress_Tick;
            // 
            // edFilename
            // 
            edFilename.Location = new System.Drawing.Point(25, 48);
            edFilename.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            edFilename.Name = "edFilename";
            edFilename.Size = new System.Drawing.Size(621, 31);
            edFilename.TabIndex = 2;
            edFilename.Text = "c:\\";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(20, 383);
            label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(117, 25);
            label4.TabIndex = 27;
            label4.Text = "Video stream";
            // 
            // cbVideoStream
            // 
            cbVideoStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbVideoStream.FormattingEnabled = true;
            cbVideoStream.Location = new System.Drawing.Point(160, 377);
            cbVideoStream.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbVideoStream.Name = "cbVideoStream";
            cbVideoStream.Size = new System.Drawing.Size(536, 33);
            cbVideoStream.TabIndex = 28;
            cbVideoStream.SelectedIndexChanged += cbVideoStream_SelectedIndexChanged;
            // 
            // cbAudioStream
            // 
            cbAudioStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbAudioStream.FormattingEnabled = true;
            cbAudioStream.Location = new System.Drawing.Point(160, 429);
            cbAudioStream.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbAudioStream.Name = "cbAudioStream";
            cbAudioStream.Size = new System.Drawing.Size(536, 33);
            cbAudioStream.TabIndex = 30;
            cbAudioStream.SelectedIndexChanged += cbAudioStream_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(20, 435);
            label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(119, 25);
            label5.TabIndex = 29;
            label5.Text = "Audio stream";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1662, 865);
            Controls.Add(cbAudioStream);
            Controls.Add(label5);
            Controls.Add(cbVideoStream);
            Controls.Add(label4);
            Controls.Add(cbUseGPU);
            Controls.Add(cbBufferingMode);
            Controls.Add(label3);
            Controls.Add(edConnectionTimeOut);
            Controls.Add(label2);
            Controls.Add(btStop);
            Controls.Add(btPause);
            Controls.Add(btResume);
            Controls.Add(btStart);
            Controls.Add(tbSpeed);
            Controls.Add(label16);
            Controls.Add(lbTime);
            Controls.Add(tbTimeline);
            Controls.Add(pnScreen);
            Controls.Add(edFilename);
            Controls.Add(label1);
            Controls.Add(btSelectFile);
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Name = "Form1";
            Text = "FFMPEG Source Demo";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)tbSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbTimeline).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btSelectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnScreen;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btPause;
        private System.Windows.Forms.Button btResume;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.TrackBar tbSpeed;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.TrackBar tbTimeline;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edConnectionTimeOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBufferingMode;
        private System.Windows.Forms.CheckBox cbUseGPU;
        private System.Windows.Forms.Timer tmProgress;
        private System.Windows.Forms.TextBox edFilename;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbVideoStream;
        private System.Windows.Forms.ComboBox cbAudioStream;
        private System.Windows.Forms.Label label5;
    }
}

