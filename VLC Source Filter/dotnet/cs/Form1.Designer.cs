
namespace VLC_Source_Demo
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
            tmProgress = new System.Windows.Forms.Timer(components);
            edFilename = new System.Windows.Forms.TextBox();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            cbUseCustomParams = new System.Windows.Forms.CheckBox();
            edCustomParams = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
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
            pnScreen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnScreen.BackColor = System.Drawing.Color.Black;
            pnScreen.Location = new System.Drawing.Point(730, 17);
            pnScreen.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            pnScreen.Name = "pnScreen";
            pnScreen.Size = new System.Drawing.Size(912, 825);
            pnScreen.TabIndex = 3;
            pnScreen.Resize += pnScreen_Resize;
            // 
            // btStop
            // 
            btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            btStop.Location = new System.Drawing.Point(315, 196);
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
            btPause.Location = new System.Drawing.Point(218, 196);
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
            btResume.Location = new System.Drawing.Point(107, 196);
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
            btStart.Location = new System.Drawing.Point(25, 196);
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
            tbSpeed.Location = new System.Drawing.Point(550, 136);
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
            label16.Location = new System.Drawing.Point(552, 106);
            label16.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(62, 25);
            label16.TabIndex = 14;
            label16.Text = "Speed";
            // 
            // lbTime
            // 
            lbTime.AutoSize = true;
            lbTime.Location = new System.Drawing.Point(375, 148);
            lbTime.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lbTime.Name = "lbTime";
            lbTime.Size = new System.Drawing.Size(155, 25);
            lbTime.TabIndex = 13;
            lbTime.Text = "00:00:00/00:00:00";
            // 
            // tbTimeline
            // 
            tbTimeline.Location = new System.Drawing.Point(25, 121);
            tbTimeline.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            tbTimeline.Maximum = 100;
            tbTimeline.Name = "tbTimeline";
            tbTimeline.Size = new System.Drawing.Size(345, 69);
            tbTimeline.TabIndex = 12;
            tbTimeline.Scroll += tbTimeline_Scroll;
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
            edFilename.Text = "c:\\Samples\\!video.mp4";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // cbUseCustomParams
            // 
            cbUseCustomParams.AutoSize = true;
            cbUseCustomParams.Location = new System.Drawing.Point(25, 254);
            cbUseCustomParams.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbUseCustomParams.Name = "cbUseCustomParams";
            cbUseCustomParams.Size = new System.Drawing.Size(226, 29);
            cbUseCustomParams.TabIndex = 20;
            cbUseCustomParams.Text = "Use Custom Parameters";
            cbUseCustomParams.UseVisualStyleBackColor = true;
            // 
            // edCustomParams
            // 
            edCustomParams.Location = new System.Drawing.Point(25, 326);
            edCustomParams.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            edCustomParams.Multiline = true;
            edCustomParams.Name = "edCustomParams";
            edCustomParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            edCustomParams.Size = new System.Drawing.Size(673, 150);
            edCustomParams.TabIndex = 22;
            edCustomParams.Text = "--no-video-title-show\r\n--clock-jitter=0\r\n--network-caching=300\r\n";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(25, 295);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(469, 25);
            label2.TabIndex = 21;
            label2.Text = "Custom VLC Parameters (one per line or space-separated):";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1662, 865);
            Controls.Add(edCustomParams);
            Controls.Add(label2);
            Controls.Add(cbUseCustomParams);
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
            Text = "VLC Source Demo";
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
        private System.Windows.Forms.Timer tmProgress;
        private System.Windows.Forms.TextBox edFilename;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox cbUseCustomParams;
        private System.Windows.Forms.TextBox edCustomParams;
        private System.Windows.Forms.Label label2;
    }
}

