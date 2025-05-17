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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btCameraStop1 = new System.Windows.Forms.Button();
            this.btCameraStartPreview1 = new System.Windows.Forms.Button();
            this.pnScreen1 = new System.Windows.Forms.Panel();
            this.cbCameraFrameRate1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCameraVideoFormat1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbCameraFrameRate2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCameraVideoFormat2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pnScreen2 = new System.Windows.Forms.Panel();
            this.btCameraStop2 = new System.Windows.Forms.Button();
            this.btCameraStart2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btCameraStartCapture1 = new System.Windows.Forms.Button();
            this.cbDebugMode = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.edImagesPerSecond = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.edImagesHeight = new System.Windows.Forms.TextBox();
            this.edImagesWidth = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btOpenFolderWithImages = new System.Windows.Forms.Button();
            this.edImagesPath = new System.Windows.Forms.TextBox();
            this.rbFolderWithImages = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.cbAudioCaptureSource = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btSourceStop = new System.Windows.Forms.Button();
            this.btSourceStart = new System.Windows.Forms.Button();
            this.cbSourceFrameRate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSourceVideoFormat = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbVideoCaptureSource = new System.Windows.Forms.ComboBox();
            this.rbCamera = new System.Windows.Forms.RadioButton();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.edSourceFile = new System.Windows.Forms.TextBox();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btSourceImageLogoAdd = new System.Windows.Forms.Button();
            this.btSourceImageLogoSelect = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.edSourceImageLogo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btSourceAddTextLogo = new System.Windows.Forms.Button();
            this.btSouceTextLogoFont = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.edSourceTextLogo = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tmPusher = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.Location = new System.Drawing.Point(362, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(3, 458);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(484, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Virtual camera 1";
            // 
            // btCameraStop1
            // 
            this.btCameraStop1.Enabled = false;
            this.btCameraStop1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCameraStop1.Location = new System.Drawing.Point(630, 355);
            this.btCameraStop1.Name = "btCameraStop1";
            this.btCameraStop1.Size = new System.Drawing.Size(98, 23);
            this.btCameraStop1.TabIndex = 15;
            this.btCameraStop1.Text = "Stop";
            this.btCameraStop1.UseVisualStyleBackColor = true;
            this.btCameraStop1.Click += new System.EventHandler(this.btCameraStop_Click);
            // 
            // btCameraStartPreview1
            // 
            this.btCameraStartPreview1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCameraStartPreview1.Location = new System.Drawing.Point(372, 355);
            this.btCameraStartPreview1.Name = "btCameraStartPreview1";
            this.btCameraStartPreview1.Size = new System.Drawing.Size(111, 23);
            this.btCameraStartPreview1.TabIndex = 14;
            this.btCameraStartPreview1.Text = "Start preview";
            this.btCameraStartPreview1.UseVisualStyleBackColor = true;
            this.btCameraStartPreview1.Click += new System.EventHandler(this.btCameraStart_Click);
            // 
            // pnScreen1
            // 
            this.pnScreen1.BackColor = System.Drawing.Color.Black;
            this.pnScreen1.Location = new System.Drawing.Point(370, 33);
            this.pnScreen1.Name = "pnScreen1";
            this.pnScreen1.Size = new System.Drawing.Size(358, 267);
            this.pnScreen1.TabIndex = 16;
            // 
            // cbCameraFrameRate1
            // 
            this.cbCameraFrameRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameraFrameRate1.FormattingEnabled = true;
            this.cbCameraFrameRate1.Location = new System.Drawing.Point(630, 323);
            this.cbCameraFrameRate1.Name = "cbCameraFrameRate1";
            this.cbCameraFrameRate1.Size = new System.Drawing.Size(98, 21);
            this.cbCameraFrameRate1.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(628, 307);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Frame rate";
            // 
            // cbCameraVideoFormat1
            // 
            this.cbCameraVideoFormat1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameraVideoFormat1.FormattingEnabled = true;
            this.cbCameraVideoFormat1.Location = new System.Drawing.Point(372, 323);
            this.cbCameraVideoFormat1.Name = "cbCameraVideoFormat1";
            this.cbCameraVideoFormat1.Size = new System.Drawing.Size(253, 21);
            this.cbCameraVideoFormat1.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(368, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Format";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Location = new System.Drawing.Point(733, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(3, 460);
            this.panel2.TabIndex = 21;
            // 
            // cbCameraFrameRate2
            // 
            this.cbCameraFrameRate2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameraFrameRate2.FormattingEnabled = true;
            this.cbCameraFrameRate2.Location = new System.Drawing.Point(915, 323);
            this.cbCameraFrameRate2.Name = "cbCameraFrameRate2";
            this.cbCameraFrameRate2.Size = new System.Drawing.Size(65, 21);
            this.cbCameraFrameRate2.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(913, 307);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Frame rate";
            // 
            // cbCameraVideoFormat2
            // 
            this.cbCameraVideoFormat2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameraVideoFormat2.FormattingEnabled = true;
            this.cbCameraVideoFormat2.Location = new System.Drawing.Point(743, 323);
            this.cbCameraVideoFormat2.Name = "cbCameraVideoFormat2";
            this.cbCameraVideoFormat2.Size = new System.Drawing.Size(166, 21);
            this.cbCameraVideoFormat2.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(739, 305);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Format";
            // 
            // pnScreen2
            // 
            this.pnScreen2.BackColor = System.Drawing.Color.Black;
            this.pnScreen2.Location = new System.Drawing.Point(743, 33);
            this.pnScreen2.Name = "pnScreen2";
            this.pnScreen2.Size = new System.Drawing.Size(358, 267);
            this.pnScreen2.TabIndex = 25;
            // 
            // btCameraStop2
            // 
            this.btCameraStop2.Enabled = false;
            this.btCameraStop2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCameraStop2.Location = new System.Drawing.Point(1047, 321);
            this.btCameraStop2.Name = "btCameraStop2";
            this.btCameraStop2.Size = new System.Drawing.Size(54, 23);
            this.btCameraStop2.TabIndex = 24;
            this.btCameraStop2.Text = "Stop";
            this.btCameraStop2.UseVisualStyleBackColor = true;
            this.btCameraStop2.Click += new System.EventHandler(this.btCameraStop2_Click);
            // 
            // btCameraStart2
            // 
            this.btCameraStart2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCameraStart2.Location = new System.Drawing.Point(987, 321);
            this.btCameraStart2.Name = "btCameraStart2";
            this.btCameraStart2.Size = new System.Drawing.Size(54, 23);
            this.btCameraStart2.TabIndex = 23;
            this.btCameraStart2.Text = "Start";
            this.btCameraStart2.UseVisualStyleBackColor = true;
            this.btCameraStart2.Click += new System.EventHandler(this.btCameraStart2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(857, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Virtual camera 2";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btCameraStartCapture1
            // 
            this.btCameraStartCapture1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCameraStartCapture1.Location = new System.Drawing.Point(488, 355);
            this.btCameraStartCapture1.Name = "btCameraStartCapture1";
            this.btCameraStartCapture1.Size = new System.Drawing.Size(136, 23);
            this.btCameraStartCapture1.TabIndex = 36;
            this.btCameraStartCapture1.Text = "Start capture to AVI";
            this.btCameraStartCapture1.UseVisualStyleBackColor = true;
            this.btCameraStartCapture1.Click += new System.EventHandler(this.btCameraStartCapture1_Click);
            // 
            // cbDebugMode
            // 
            this.cbDebugMode.AutoSize = true;
            this.cbDebugMode.Location = new System.Drawing.Point(9, 529);
            this.cbDebugMode.Name = "cbDebugMode";
            this.cbDebugMode.Size = new System.Drawing.Size(112, 17);
            this.cbDebugMode.TabIndex = 37;
            this.cbDebugMode.Text = "Enable debugging";
            this.cbDebugMode.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(350, 458);
            this.tabControl1.TabIndex = 38;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edImagesPerSecond);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.edImagesHeight);
            this.tabPage1.Controls.Add(this.edImagesWidth);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btOpenFolderWithImages);
            this.tabPage1.Controls.Add(this.edImagesPath);
            this.tabPage1.Controls.Add(this.rbFolderWithImages);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.cbAudioCaptureSource);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.btSourceStop);
            this.tabPage1.Controls.Add(this.btSourceStart);
            this.tabPage1.Controls.Add(this.cbSourceFrameRate);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cbSourceVideoFormat);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cbVideoCaptureSource);
            this.tabPage1.Controls.Add(this.rbCamera);
            this.tabPage1.Controls.Add(this.btOpenFile);
            this.tabPage1.Controls.Add(this.edSourceFile);
            this.tabPage1.Controls.Add(this.rbFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(342, 432);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // edImagesPerSecond
            // 
            this.edImagesPerSecond.Location = new System.Drawing.Point(176, 301);
            this.edImagesPerSecond.Margin = new System.Windows.Forms.Padding(2);
            this.edImagesPerSecond.Name = "edImagesPerSecond";
            this.edImagesPerSecond.Size = new System.Drawing.Size(52, 20);
            this.edImagesPerSecond.TabIndex = 58;
            this.edImagesPerSecond.Text = "1";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(174, 286);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 13);
            this.label16.TabIndex = 57;
            this.label16.Text = "Images per second";
            // 
            // edImagesHeight
            // 
            this.edImagesHeight.Location = new System.Drawing.Point(98, 301);
            this.edImagesHeight.Margin = new System.Windows.Forms.Padding(2);
            this.edImagesHeight.Name = "edImagesHeight";
            this.edImagesHeight.Size = new System.Drawing.Size(52, 20);
            this.edImagesHeight.TabIndex = 56;
            this.edImagesHeight.Text = "720";
            // 
            // edImagesWidth
            // 
            this.edImagesWidth.Location = new System.Drawing.Point(30, 301);
            this.edImagesWidth.Margin = new System.Windows.Forms.Padding(2);
            this.edImagesWidth.Name = "edImagesWidth";
            this.edImagesWidth.Size = new System.Drawing.Size(52, 20);
            this.edImagesWidth.TabIndex = 55;
            this.edImagesWidth.Text = "1280";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(84, 303);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(12, 13);
            this.label15.TabIndex = 54;
            this.label15.Text = "x";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 286);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Resolution";
            // 
            // btOpenFolderWithImages
            // 
            this.btOpenFolderWithImages.Location = new System.Drawing.Point(310, 255);
            this.btOpenFolderWithImages.Name = "btOpenFolderWithImages";
            this.btOpenFolderWithImages.Size = new System.Drawing.Size(24, 23);
            this.btOpenFolderWithImages.TabIndex = 52;
            this.btOpenFolderWithImages.Text = "...";
            this.btOpenFolderWithImages.UseVisualStyleBackColor = true;
            this.btOpenFolderWithImages.Click += new System.EventHandler(this.btOpenFolderWithImages_Click);
            // 
            // edImagesPath
            // 
            this.edImagesPath.Location = new System.Drawing.Point(30, 257);
            this.edImagesPath.Name = "edImagesPath";
            this.edImagesPath.Size = new System.Drawing.Size(276, 20);
            this.edImagesPath.TabIndex = 51;
            this.edImagesPath.Text = "c:\\Samples\\pics";
            // 
            // rbFolderWithImages
            // 
            this.rbFolderWithImages.AutoSize = true;
            this.rbFolderWithImages.Location = new System.Drawing.Point(12, 236);
            this.rbFolderWithImages.Margin = new System.Windows.Forms.Padding(2);
            this.rbFolderWithImages.Name = "rbFolderWithImages";
            this.rbFolderWithImages.Size = new System.Drawing.Size(112, 17);
            this.rbFolderWithImages.TabIndex = 50;
            this.rbFolderWithImages.TabStop = true;
            this.rbFolderWithImages.Text = "Folder with images";
            this.rbFolderWithImages.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(26, 187);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 13);
            this.label14.TabIndex = 49;
            this.label14.Text = "Audio source";
            // 
            // cbAudioCaptureSource
            // 
            this.cbAudioCaptureSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioCaptureSource.FormattingEnabled = true;
            this.cbAudioCaptureSource.Location = new System.Drawing.Point(30, 203);
            this.cbAudioCaptureSource.Name = "cbAudioCaptureSource";
            this.cbAudioCaptureSource.Size = new System.Drawing.Size(306, 21);
            this.cbAudioCaptureSource.TabIndex = 48;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 89);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 47;
            this.label13.Text = "Video source";
            // 
            // btSourceStop
            // 
            this.btSourceStop.Enabled = false;
            this.btSourceStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStop.Location = new System.Drawing.Point(280, 354);
            this.btSourceStop.Name = "btSourceStop";
            this.btSourceStop.Size = new System.Drawing.Size(54, 23);
            this.btSourceStop.TabIndex = 46;
            this.btSourceStop.Text = "Stop";
            this.btSourceStop.UseVisualStyleBackColor = true;
            this.btSourceStop.Click += new System.EventHandler(this.btSourceStop_Click);
            // 
            // btSourceStart
            // 
            this.btSourceStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSourceStart.Location = new System.Drawing.Point(220, 354);
            this.btSourceStart.Name = "btSourceStart";
            this.btSourceStart.Size = new System.Drawing.Size(54, 23);
            this.btSourceStart.TabIndex = 45;
            this.btSourceStart.Text = "Start";
            this.btSourceStart.UseVisualStyleBackColor = true;
            this.btSourceStart.Click += new System.EventHandler(this.btSourceStart_Click);
            // 
            // cbSourceFrameRate
            // 
            this.cbSourceFrameRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceFrameRate.FormattingEnabled = true;
            this.cbSourceFrameRate.Location = new System.Drawing.Point(270, 154);
            this.cbSourceFrameRate.Name = "cbSourceFrameRate";
            this.cbSourceFrameRate.Size = new System.Drawing.Size(66, 21);
            this.cbSourceFrameRate.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Frame rate";
            // 
            // cbSourceVideoFormat
            // 
            this.cbSourceVideoFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceVideoFormat.FormattingEnabled = true;
            this.cbSourceVideoFormat.Location = new System.Drawing.Point(30, 154);
            this.cbSourceVideoFormat.Name = "cbSourceVideoFormat";
            this.cbSourceVideoFormat.Size = new System.Drawing.Size(234, 21);
            this.cbSourceVideoFormat.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Format";
            // 
            // cbVideoCaptureSource
            // 
            this.cbVideoCaptureSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoCaptureSource.FormattingEnabled = true;
            this.cbVideoCaptureSource.Location = new System.Drawing.Point(30, 105);
            this.cbVideoCaptureSource.Name = "cbVideoCaptureSource";
            this.cbVideoCaptureSource.Size = new System.Drawing.Size(306, 21);
            this.cbVideoCaptureSource.TabIndex = 40;
            this.cbVideoCaptureSource.SelectedIndexChanged += new System.EventHandler(this.cbVideoCaptureSource_SelectedIndexChanged);
            // 
            // rbCamera
            // 
            this.rbCamera.AutoSize = true;
            this.rbCamera.Location = new System.Drawing.Point(12, 65);
            this.rbCamera.Name = "rbCamera";
            this.rbCamera.Size = new System.Drawing.Size(59, 17);
            this.rbCamera.TabIndex = 39;
            this.rbCamera.Text = "Device";
            this.rbCamera.UseVisualStyleBackColor = true;
            // 
            // btOpenFile
            // 
            this.btOpenFile.Location = new System.Drawing.Point(310, 37);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(24, 23);
            this.btOpenFile.TabIndex = 38;
            this.btOpenFile.Text = "...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // edSourceFile
            // 
            this.edSourceFile.Location = new System.Drawing.Point(30, 39);
            this.edSourceFile.Name = "edSourceFile";
            this.edSourceFile.Size = new System.Drawing.Size(276, 20);
            this.edSourceFile.TabIndex = 37;
            this.edSourceFile.Text = "c:\\Samples\\!video.mp4";
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Checked = true;
            this.rbFile.Location = new System.Drawing.Point(12, 17);
            this.rbFile.Margin = new System.Windows.Forms.Padding(2);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(68, 17);
            this.rbFile.TabIndex = 36;
            this.rbFile.TabStop = true;
            this.rbFile.Text = "Video file";
            this.rbFile.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(342, 432);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Effects";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 227);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(195, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "A lot of other effects available using API";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btSourceImageLogoAdd);
            this.groupBox2.Controls.Add(this.btSourceImageLogoSelect);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.edSourceImageLogo);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 96);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image logo";
            // 
            // btSourceImageLogoAdd
            // 
            this.btSourceImageLogoAdd.Location = new System.Drawing.Point(123, 63);
            this.btSourceImageLogoAdd.Name = "btSourceImageLogoAdd";
            this.btSourceImageLogoAdd.Size = new System.Drawing.Size(67, 23);
            this.btSourceImageLogoAdd.TabIndex = 3;
            this.btSourceImageLogoAdd.Text = "Add";
            this.btSourceImageLogoAdd.UseVisualStyleBackColor = true;
            this.btSourceImageLogoAdd.Click += new System.EventHandler(this.btSourceImageLogoAdd_Click);
            // 
            // btSourceImageLogoSelect
            // 
            this.btSourceImageLogoSelect.Location = new System.Drawing.Point(270, 37);
            this.btSourceImageLogoSelect.Name = "btSourceImageLogoSelect";
            this.btSourceImageLogoSelect.Size = new System.Drawing.Size(47, 23);
            this.btSourceImageLogoSelect.TabIndex = 2;
            this.btSourceImageLogoSelect.Text = "Select";
            this.btSourceImageLogoSelect.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "File name";
            // 
            // edSourceImageLogo
            // 
            this.edSourceImageLogo.Location = new System.Drawing.Point(17, 37);
            this.edSourceImageLogo.Name = "edSourceImageLogo";
            this.edSourceImageLogo.Size = new System.Drawing.Size(247, 20);
            this.edSourceImageLogo.TabIndex = 0;
            this.edSourceImageLogo.Text = "c:\\logo.jpg";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btSourceAddTextLogo);
            this.groupBox1.Controls.Add(this.btSouceTextLogoFont);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.edSourceTextLogo);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 110);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text logo";
            // 
            // btSourceAddTextLogo
            // 
            this.btSourceAddTextLogo.Location = new System.Drawing.Point(123, 77);
            this.btSourceAddTextLogo.Name = "btSourceAddTextLogo";
            this.btSourceAddTextLogo.Size = new System.Drawing.Size(67, 23);
            this.btSourceAddTextLogo.TabIndex = 3;
            this.btSourceAddTextLogo.Text = "Add";
            this.btSourceAddTextLogo.UseVisualStyleBackColor = true;
            this.btSourceAddTextLogo.Click += new System.EventHandler(this.btSourceAddTextLogo_Click);
            // 
            // btSouceTextLogoFont
            // 
            this.btSouceTextLogoFont.Location = new System.Drawing.Point(270, 50);
            this.btSouceTextLogoFont.Name = "btSouceTextLogoFont";
            this.btSouceTextLogoFont.Size = new System.Drawing.Size(47, 23);
            this.btSouceTextLogoFont.TabIndex = 2;
            this.btSouceTextLogoFont.Text = "Font";
            this.btSouceTextLogoFont.UseVisualStyleBackColor = true;
            this.btSouceTextLogoFont.Click += new System.EventHandler(this.btSouceTextLogoFont_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Text";
            // 
            // edSourceTextLogo
            // 
            this.edSourceTextLogo.Location = new System.Drawing.Point(16, 52);
            this.edSourceTextLogo.Name = "edSourceTextLogo";
            this.edSourceTextLogo.Size = new System.Drawing.Size(247, 20);
            this.edSourceTextLogo.TabIndex = 0;
            this.edSourceTextLogo.Text = "Hello!";
            // 
            // tmPusher
            // 
            this.tmPusher.Tick += new System.EventHandler(this.tmPusher_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 472);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cbDebugMode);
            this.Controls.Add(this.btCameraStartCapture1);
            this.Controls.Add(this.cbCameraFrameRate2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbCameraVideoFormat2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pnScreen2);
            this.Controls.Add(this.btCameraStop2);
            this.Controls.Add(this.btCameraStart2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cbCameraFrameRate1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCameraVideoFormat1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pnScreen1);
            this.Controls.Add(this.btCameraStop1);
            this.Controls.Add(this.btCameraStartPreview1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Virtual Camera SDK demo (C# WinForms)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCameraStop1;
        private System.Windows.Forms.Button btCameraStartPreview1;
        private System.Windows.Forms.Panel pnScreen1;
        private System.Windows.Forms.ComboBox cbCameraFrameRate1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCameraVideoFormat1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbCameraFrameRate2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbCameraVideoFormat2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnScreen2;
        private System.Windows.Forms.Button btCameraStop2;
        private System.Windows.Forms.Button btCameraStart2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btCameraStartCapture1;
        private System.Windows.Forms.CheckBox cbDebugMode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbAudioCaptureSource;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btSourceStop;
        private System.Windows.Forms.Button btSourceStart;
        private System.Windows.Forms.ComboBox cbSourceFrameRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSourceVideoFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbVideoCaptureSource;
        private System.Windows.Forms.RadioButton rbCamera;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.TextBox edSourceFile;
        private System.Windows.Forms.RadioButton rbFile;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btSourceImageLogoAdd;
        private System.Windows.Forms.Button btSourceImageLogoSelect;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox edSourceImageLogo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btSourceAddTextLogo;
        private System.Windows.Forms.Button btSouceTextLogoFont;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox edSourceTextLogo;
        private System.Windows.Forms.RadioButton rbFolderWithImages;
        private System.Windows.Forms.Button btOpenFolderWithImages;
        private System.Windows.Forms.TextBox edImagesPath;
        private System.Windows.Forms.TextBox edImagesPerSecond;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox edImagesHeight;
        private System.Windows.Forms.TextBox edImagesWidth;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer tmPusher;
    }
}

