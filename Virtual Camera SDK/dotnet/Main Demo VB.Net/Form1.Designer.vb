<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        btCameraStartCapture1 = New Button()
        label14 = New Label()
        cbAudioCaptureSource = New ComboBox()
        label13 = New Label()
        label12 = New Label()
        groupBox2 = New GroupBox()
        btSourceImageLogoAdd = New Button()
        btSourceImageLogoSelect = New Button()
        label11 = New Label()
        edSourceImageLogo = New TextBox()
        groupBox1 = New GroupBox()
        btSourceAddTextLogo = New Button()
        btSouceTextLogoFont = New Button()
        label10 = New Label()
        edSourceTextLogo = New TextBox()
        cbCameraFrameRate2 = New ComboBox()
        label7 = New Label()
        cbCameraVideoFormat2 = New ComboBox()
        label8 = New Label()
        pnScreen2 = New Panel()
        btCameraStop2 = New Button()
        btCameraStart2 = New Button()
        label9 = New Label()
        panel2 = New Panel()
        cbCameraFrameRate1 = New ComboBox()
        label5 = New Label()
        cbCameraVideoFormat1 = New ComboBox()
        label6 = New Label()
        pnScreen1 = New Panel()
        btCameraStop1 = New Button()
        btCameraStartPreview1 = New Button()
        btSourceStop = New Button()
        btSourceStart = New Button()
        cbSourceFrameRate = New ComboBox()
        label4 = New Label()
        cbSourceVideoFormat = New ComboBox()
        label3 = New Label()
        cbVideoCaptureSource = New ComboBox()
        rbCamera = New RadioButton()
        btOpenFile = New Button()
        edSourceFile = New TextBox()
        label2 = New Label()
        label1 = New Label()
        rbFile = New RadioButton()
        panel1 = New Panel()
        openFileDialog1 = New OpenFileDialog()
        fontDialog1 = New FontDialog()
        groupBox2.SuspendLayout()
        groupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' btCameraStartCapture1
        ' 
        btCameraStartCapture1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btCameraStartCapture1.Location = New System.Drawing.Point(773, 690)
        btCameraStartCapture1.Margin = New Padding(5, 6, 5, 6)
        btCameraStartCapture1.Name = "btCameraStartCapture1"
        btCameraStartCapture1.Size = New System.Drawing.Size(227, 44)
        btCameraStartCapture1.TabIndex = 73
        btCameraStartCapture1.Text = "Start capture to AVI"
        btCameraStartCapture1.UseVisualStyleBackColor = True
        ' 
        ' label14
        ' 
        label14.AutoSize = True
        label14.Location = New System.Drawing.Point(42, 398)
        label14.Margin = New Padding(5, 0, 5, 0)
        label14.Name = "label14"
        label14.Size = New System.Drawing.Size(117, 25)
        label14.TabIndex = 72
        label14.Text = "Audio source"
        ' 
        ' cbAudioCaptureSource
        ' 
        cbAudioCaptureSource.DropDownStyle = ComboBoxStyle.DropDownList
        cbAudioCaptureSource.FormattingEnabled = True
        cbAudioCaptureSource.Location = New System.Drawing.Point(47, 429)
        cbAudioCaptureSource.Margin = New Padding(5, 6, 5, 6)
        cbAudioCaptureSource.Name = "cbAudioCaptureSource"
        cbAudioCaptureSource.Size = New System.Drawing.Size(507, 33)
        cbAudioCaptureSource.TabIndex = 71
        ' 
        ' label13
        ' 
        label13.AutoSize = True
        label13.Location = New System.Drawing.Point(42, 210)
        label13.Margin = New Padding(5, 0, 5, 0)
        label13.Name = "label13"
        label13.Size = New System.Drawing.Size(115, 25)
        label13.TabIndex = 70
        label13.Text = "Video source"
        ' 
        ' label12
        ' 
        label12.AutoSize = True
        label12.Location = New System.Drawing.Point(13, 962)
        label12.Margin = New Padding(5, 0, 5, 0)
        label12.Name = "label12"
        label12.Size = New System.Drawing.Size(329, 25)
        label12.TabIndex = 69
        label12.Text = "A lot of other effects available using API"
        ' 
        ' groupBox2
        ' 
        groupBox2.Controls.Add(btSourceImageLogoAdd)
        groupBox2.Controls.Add(btSourceImageLogoSelect)
        groupBox2.Controls.Add(label11)
        groupBox2.Controls.Add(edSourceImageLogo)
        groupBox2.Location = New System.Drawing.Point(18, 765)
        groupBox2.Margin = New Padding(5, 6, 5, 6)
        groupBox2.Name = "groupBox2"
        groupBox2.Padding = New Padding(5, 6, 5, 6)
        groupBox2.Size = New System.Drawing.Size(538, 185)
        groupBox2.TabIndex = 68
        groupBox2.TabStop = False
        groupBox2.Text = "Image logo"
        ' 
        ' btSourceImageLogoAdd
        ' 
        btSourceImageLogoAdd.Location = New System.Drawing.Point(205, 121)
        btSourceImageLogoAdd.Margin = New Padding(5, 6, 5, 6)
        btSourceImageLogoAdd.Name = "btSourceImageLogoAdd"
        btSourceImageLogoAdd.Size = New System.Drawing.Size(112, 44)
        btSourceImageLogoAdd.TabIndex = 3
        btSourceImageLogoAdd.Text = "Add"
        btSourceImageLogoAdd.UseVisualStyleBackColor = True
        ' 
        ' btSourceImageLogoSelect
        ' 
        btSourceImageLogoSelect.Location = New System.Drawing.Point(450, 71)
        btSourceImageLogoSelect.Margin = New Padding(5, 6, 5, 6)
        btSourceImageLogoSelect.Name = "btSourceImageLogoSelect"
        btSourceImageLogoSelect.Size = New System.Drawing.Size(78, 44)
        btSourceImageLogoSelect.TabIndex = 2
        btSourceImageLogoSelect.Text = "Select"
        btSourceImageLogoSelect.UseVisualStyleBackColor = True
        ' 
        ' label11
        ' 
        label11.AutoSize = True
        label11.Location = New System.Drawing.Point(23, 40)
        label11.Margin = New Padding(5, 0, 5, 0)
        label11.Name = "label11"
        label11.Size = New System.Drawing.Size(87, 25)
        label11.TabIndex = 1
        label11.Text = "File name"
        ' 
        ' edSourceImageLogo
        ' 
        edSourceImageLogo.Location = New System.Drawing.Point(28, 71)
        edSourceImageLogo.Margin = New Padding(5, 6, 5, 6)
        edSourceImageLogo.Name = "edSourceImageLogo"
        edSourceImageLogo.Size = New System.Drawing.Size(409, 31)
        edSourceImageLogo.TabIndex = 0
        edSourceImageLogo.Text = "c:\logo.jpg"
        ' 
        ' groupBox1
        ' 
        groupBox1.Controls.Add(btSourceAddTextLogo)
        groupBox1.Controls.Add(btSouceTextLogoFont)
        groupBox1.Controls.Add(label10)
        groupBox1.Controls.Add(edSourceTextLogo)
        groupBox1.Location = New System.Drawing.Point(18, 542)
        groupBox1.Margin = New Padding(5, 6, 5, 6)
        groupBox1.Name = "groupBox1"
        groupBox1.Padding = New Padding(5, 6, 5, 6)
        groupBox1.Size = New System.Drawing.Size(538, 212)
        groupBox1.TabIndex = 67
        groupBox1.TabStop = False
        groupBox1.Text = "Text logo"
        ' 
        ' btSourceAddTextLogo
        ' 
        btSourceAddTextLogo.Location = New System.Drawing.Point(205, 148)
        btSourceAddTextLogo.Margin = New Padding(5, 6, 5, 6)
        btSourceAddTextLogo.Name = "btSourceAddTextLogo"
        btSourceAddTextLogo.Size = New System.Drawing.Size(112, 44)
        btSourceAddTextLogo.TabIndex = 3
        btSourceAddTextLogo.Text = "Add"
        btSourceAddTextLogo.UseVisualStyleBackColor = True
        ' 
        ' btSouceTextLogoFont
        ' 
        btSouceTextLogoFont.Location = New System.Drawing.Point(450, 96)
        btSouceTextLogoFont.Margin = New Padding(5, 6, 5, 6)
        btSouceTextLogoFont.Name = "btSouceTextLogoFont"
        btSouceTextLogoFont.Size = New System.Drawing.Size(78, 44)
        btSouceTextLogoFont.TabIndex = 2
        btSouceTextLogoFont.Text = "Font"
        btSouceTextLogoFont.UseVisualStyleBackColor = True
        ' 
        ' label10
        ' 
        label10.AutoSize = True
        label10.Location = New System.Drawing.Point(23, 65)
        label10.Margin = New Padding(5, 0, 5, 0)
        label10.Name = "label10"
        label10.Size = New System.Drawing.Size(42, 25)
        label10.TabIndex = 1
        label10.Text = "Text"
        ' 
        ' edSourceTextLogo
        ' 
        edSourceTextLogo.Location = New System.Drawing.Point(27, 100)
        edSourceTextLogo.Margin = New Padding(5, 6, 5, 6)
        edSourceTextLogo.Name = "edSourceTextLogo"
        edSourceTextLogo.Size = New System.Drawing.Size(409, 31)
        edSourceTextLogo.TabIndex = 0
        edSourceTextLogo.Text = "Hello!"
        ' 
        ' cbCameraFrameRate2
        ' 
        cbCameraFrameRate2.DropDownStyle = ComboBoxStyle.DropDownList
        cbCameraFrameRate2.FormattingEnabled = True
        cbCameraFrameRate2.Location = New System.Drawing.Point(1487, 629)
        cbCameraFrameRate2.Margin = New Padding(5, 6, 5, 6)
        cbCameraFrameRate2.Name = "cbCameraFrameRate2"
        cbCameraFrameRate2.Size = New System.Drawing.Size(106, 33)
        cbCameraFrameRate2.TabIndex = 66
        ' 
        ' label7
        ' 
        label7.AutoSize = True
        label7.Location = New System.Drawing.Point(1482, 598)
        label7.Margin = New Padding(5, 0, 5, 0)
        label7.Name = "label7"
        label7.Size = New System.Drawing.Size(96, 25)
        label7.TabIndex = 65
        label7.Text = "Frame rate"
        ' 
        ' cbCameraVideoFormat2
        ' 
        cbCameraVideoFormat2.DropDownStyle = ComboBoxStyle.DropDownList
        cbCameraVideoFormat2.FormattingEnabled = True
        cbCameraVideoFormat2.Location = New System.Drawing.Point(1200, 629)
        cbCameraVideoFormat2.Margin = New Padding(5, 6, 5, 6)
        cbCameraVideoFormat2.Name = "cbCameraVideoFormat2"
        cbCameraVideoFormat2.Size = New System.Drawing.Size(274, 33)
        cbCameraVideoFormat2.TabIndex = 64
        ' 
        ' label8
        ' 
        label8.AutoSize = True
        label8.Location = New System.Drawing.Point(1193, 594)
        label8.Margin = New Padding(5, 0, 5, 0)
        label8.Name = "label8"
        label8.Size = New System.Drawing.Size(69, 25)
        label8.TabIndex = 63
        label8.Text = "Format"
        ' 
        ' pnScreen2
        ' 
        pnScreen2.BackColor = Drawing.Color.Black
        pnScreen2.Location = New System.Drawing.Point(1198, 71)
        pnScreen2.Margin = New Padding(5, 6, 5, 6)
        pnScreen2.Name = "pnScreen2"
        pnScreen2.Size = New System.Drawing.Size(597, 513)
        pnScreen2.TabIndex = 62
        ' 
        ' btCameraStop2
        ' 
        btCameraStop2.Enabled = False
        btCameraStop2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btCameraStop2.Location = New System.Drawing.Point(1705, 625)
        btCameraStop2.Margin = New Padding(5, 6, 5, 6)
        btCameraStop2.Name = "btCameraStop2"
        btCameraStop2.Size = New System.Drawing.Size(90, 44)
        btCameraStop2.TabIndex = 61
        btCameraStop2.Text = "Stop"
        btCameraStop2.UseVisualStyleBackColor = True
        ' 
        ' btCameraStart2
        ' 
        btCameraStart2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btCameraStart2.Location = New System.Drawing.Point(1605, 625)
        btCameraStart2.Margin = New Padding(5, 6, 5, 6)
        btCameraStart2.Name = "btCameraStart2"
        btCameraStart2.Size = New System.Drawing.Size(90, 44)
        btCameraStart2.TabIndex = 60
        btCameraStart2.Text = "Start"
        btCameraStart2.UseVisualStyleBackColor = True
        ' 
        ' label9
        ' 
        label9.AutoSize = True
        label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CByte(204))
        label9.Location = New System.Drawing.Point(1388, 19)
        label9.Name = "label9"
        label9.Size = New System.Drawing.Size(185, 29)
        label9.TabIndex = 59
        label9.Text = "Virtual camera 2"
        ' 
        ' panel2
        ' 
        panel2.BackColor = Drawing.Color.DarkGray
        panel2.Location = New System.Drawing.Point(1183, 15)
        panel2.Margin = New Padding(3, 4, 3, 4)
        panel2.Name = "panel2"
        panel2.Size = New System.Drawing.Size(5, 885)
        panel2.TabIndex = 58
        ' 
        ' cbCameraFrameRate1
        ' 
        cbCameraFrameRate1.DropDownStyle = ComboBoxStyle.DropDownList
        cbCameraFrameRate1.FormattingEnabled = True
        cbCameraFrameRate1.Location = New System.Drawing.Point(1010, 629)
        cbCameraFrameRate1.Margin = New Padding(5, 6, 5, 6)
        cbCameraFrameRate1.Name = "cbCameraFrameRate1"
        cbCameraFrameRate1.Size = New System.Drawing.Size(161, 33)
        cbCameraFrameRate1.TabIndex = 57
        ' 
        ' label5
        ' 
        label5.AutoSize = True
        label5.Location = New System.Drawing.Point(1005, 598)
        label5.Margin = New Padding(5, 0, 5, 0)
        label5.Name = "label5"
        label5.Size = New System.Drawing.Size(96, 25)
        label5.TabIndex = 56
        label5.Text = "Frame rate"
        ' 
        ' cbCameraVideoFormat1
        ' 
        cbCameraVideoFormat1.DropDownStyle = ComboBoxStyle.DropDownList
        cbCameraVideoFormat1.FormattingEnabled = True
        cbCameraVideoFormat1.Location = New System.Drawing.Point(578, 629)
        cbCameraVideoFormat1.Margin = New Padding(5, 6, 5, 6)
        cbCameraVideoFormat1.Name = "cbCameraVideoFormat1"
        cbCameraVideoFormat1.Size = New System.Drawing.Size(419, 33)
        cbCameraVideoFormat1.TabIndex = 55
        ' 
        ' label6
        ' 
        label6.AutoSize = True
        label6.Location = New System.Drawing.Point(573, 598)
        label6.Margin = New Padding(5, 0, 5, 0)
        label6.Name = "label6"
        label6.Size = New System.Drawing.Size(69, 25)
        label6.TabIndex = 54
        label6.Text = "Format"
        ' 
        ' pnScreen1
        ' 
        pnScreen1.BackColor = Drawing.Color.Black
        pnScreen1.Location = New System.Drawing.Point(577, 71)
        pnScreen1.Margin = New Padding(5, 6, 5, 6)
        pnScreen1.Name = "pnScreen1"
        pnScreen1.Size = New System.Drawing.Size(597, 513)
        pnScreen1.TabIndex = 53
        ' 
        ' btCameraStop1
        ' 
        btCameraStop1.Enabled = False
        btCameraStop1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btCameraStop1.Location = New System.Drawing.Point(1010, 690)
        btCameraStop1.Margin = New Padding(5, 6, 5, 6)
        btCameraStop1.Name = "btCameraStop1"
        btCameraStop1.Size = New System.Drawing.Size(163, 44)
        btCameraStop1.TabIndex = 52
        btCameraStop1.Text = "Stop"
        btCameraStop1.UseVisualStyleBackColor = True
        ' 
        ' btCameraStartPreview1
        ' 
        btCameraStartPreview1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btCameraStartPreview1.Location = New System.Drawing.Point(578, 690)
        btCameraStartPreview1.Margin = New Padding(5, 6, 5, 6)
        btCameraStartPreview1.Name = "btCameraStartPreview1"
        btCameraStartPreview1.Size = New System.Drawing.Size(185, 44)
        btCameraStartPreview1.TabIndex = 51
        btCameraStartPreview1.Text = "Start preview"
        btCameraStartPreview1.UseVisualStyleBackColor = True
        ' 
        ' btSourceStop
        ' 
        btSourceStop.Enabled = False
        btSourceStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btSourceStop.Location = New System.Drawing.Point(468, 487)
        btSourceStop.Margin = New Padding(5, 6, 5, 6)
        btSourceStop.Name = "btSourceStop"
        btSourceStop.Size = New System.Drawing.Size(90, 44)
        btSourceStop.TabIndex = 50
        btSourceStop.Text = "Stop"
        btSourceStop.UseVisualStyleBackColor = True
        ' 
        ' btSourceStart
        ' 
        btSourceStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CByte(204))
        btSourceStart.Location = New System.Drawing.Point(368, 487)
        btSourceStart.Margin = New Padding(5, 6, 5, 6)
        btSourceStart.Name = "btSourceStart"
        btSourceStart.Size = New System.Drawing.Size(90, 44)
        btSourceStart.TabIndex = 49
        btSourceStart.Text = "Start"
        btSourceStart.UseVisualStyleBackColor = True
        ' 
        ' cbSourceFrameRate
        ' 
        cbSourceFrameRate.DropDownStyle = ComboBoxStyle.DropDownList
        cbSourceFrameRate.FormattingEnabled = True
        cbSourceFrameRate.Location = New System.Drawing.Point(447, 335)
        cbSourceFrameRate.Margin = New Padding(5, 6, 5, 6)
        cbSourceFrameRate.Name = "cbSourceFrameRate"
        cbSourceFrameRate.Size = New System.Drawing.Size(107, 33)
        cbSourceFrameRate.TabIndex = 48
        ' 
        ' label4
        ' 
        label4.AutoSize = True
        label4.Location = New System.Drawing.Point(442, 304)
        label4.Margin = New Padding(5, 0, 5, 0)
        label4.Name = "label4"
        label4.Size = New System.Drawing.Size(96, 25)
        label4.TabIndex = 47
        label4.Text = "Frame rate"
        ' 
        ' cbSourceVideoFormat
        ' 
        cbSourceVideoFormat.DropDownStyle = ComboBoxStyle.DropDownList
        cbSourceVideoFormat.FormattingEnabled = True
        cbSourceVideoFormat.Location = New System.Drawing.Point(47, 335)
        cbSourceVideoFormat.Margin = New Padding(5, 6, 5, 6)
        cbSourceVideoFormat.Name = "cbSourceVideoFormat"
        cbSourceVideoFormat.Size = New System.Drawing.Size(387, 33)
        cbSourceVideoFormat.TabIndex = 46
        ' 
        ' label3
        ' 
        label3.AutoSize = True
        label3.Location = New System.Drawing.Point(42, 304)
        label3.Margin = New Padding(5, 0, 5, 0)
        label3.Name = "label3"
        label3.Size = New System.Drawing.Size(69, 25)
        label3.TabIndex = 45
        label3.Text = "Format"
        ' 
        ' cbVideoCaptureSource
        ' 
        cbVideoCaptureSource.DropDownStyle = ComboBoxStyle.DropDownList
        cbVideoCaptureSource.FormattingEnabled = True
        cbVideoCaptureSource.Location = New System.Drawing.Point(47, 240)
        cbVideoCaptureSource.Margin = New Padding(5, 6, 5, 6)
        cbVideoCaptureSource.Name = "cbVideoCaptureSource"
        cbVideoCaptureSource.Size = New System.Drawing.Size(507, 33)
        cbVideoCaptureSource.TabIndex = 44
        ' 
        ' rbCamera
        ' 
        rbCamera.AutoSize = True
        rbCamera.Location = New System.Drawing.Point(18, 163)
        rbCamera.Margin = New Padding(5, 6, 5, 6)
        rbCamera.Name = "rbCamera"
        rbCamera.Size = New System.Drawing.Size(89, 29)
        rbCamera.TabIndex = 43
        rbCamera.Text = "Device"
        rbCamera.UseVisualStyleBackColor = True
        ' 
        ' btOpenFile
        ' 
        btOpenFile.Location = New System.Drawing.Point(517, 110)
        btOpenFile.Margin = New Padding(5, 6, 5, 6)
        btOpenFile.Name = "btOpenFile"
        btOpenFile.Size = New System.Drawing.Size(40, 44)
        btOpenFile.TabIndex = 42
        btOpenFile.Text = "..."
        btOpenFile.UseVisualStyleBackColor = True
        ' 
        ' edSourceFile
        ' 
        edSourceFile.Location = New System.Drawing.Point(47, 113)
        edSourceFile.Margin = New Padding(5, 6, 5, 6)
        edSourceFile.Name = "edSourceFile"
        edSourceFile.Size = New System.Drawing.Size(457, 31)
        edSourceFile.TabIndex = 41
        edSourceFile.Text = "c:\Samples\!video.mp4"
        ' 
        ' label2
        ' 
        label2.AutoSize = True
        label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CByte(204))
        label2.Location = New System.Drawing.Point(767, 19)
        label2.Name = "label2"
        label2.Size = New System.Drawing.Size(185, 29)
        label2.TabIndex = 40
        label2.Text = "Virtual camera 1"
        ' 
        ' label1
        ' 
        label1.AutoSize = True
        label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12F, Drawing.FontStyle.Regular, Drawing.GraphicsUnit.Point, CByte(204))
        label1.Location = New System.Drawing.Point(195, 19)
        label1.Name = "label1"
        label1.Size = New System.Drawing.Size(155, 29)
        label1.TabIndex = 39
        label1.Text = "Video source"
        ' 
        ' rbFile
        ' 
        rbFile.AutoSize = True
        rbFile.Checked = True
        rbFile.Location = New System.Drawing.Point(18, 71)
        rbFile.Margin = New Padding(3, 4, 3, 4)
        rbFile.Name = "rbFile"
        rbFile.Size = New System.Drawing.Size(111, 29)
        rbFile.TabIndex = 38
        rbFile.TabStop = True
        rbFile.Text = "Video file"
        rbFile.UseVisualStyleBackColor = True
        ' 
        ' panel1
        ' 
        panel1.BackColor = Drawing.Color.DarkGray
        panel1.Location = New System.Drawing.Point(565, 19)
        panel1.Margin = New Padding(3, 4, 3, 4)
        panel1.Name = "panel1"
        panel1.Size = New System.Drawing.Size(5, 881)
        panel1.TabIndex = 37
        ' 
        ' openFileDialog1
        ' 
        openFileDialog1.FileName = "openFileDialog1"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New System.Drawing.SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New System.Drawing.Size(1817, 1012)
        Controls.Add(btCameraStartCapture1)
        Controls.Add(label14)
        Controls.Add(cbAudioCaptureSource)
        Controls.Add(label13)
        Controls.Add(label12)
        Controls.Add(groupBox2)
        Controls.Add(groupBox1)
        Controls.Add(cbCameraFrameRate2)
        Controls.Add(label7)
        Controls.Add(cbCameraVideoFormat2)
        Controls.Add(label8)
        Controls.Add(pnScreen2)
        Controls.Add(btCameraStop2)
        Controls.Add(btCameraStart2)
        Controls.Add(label9)
        Controls.Add(panel2)
        Controls.Add(cbCameraFrameRate1)
        Controls.Add(label5)
        Controls.Add(cbCameraVideoFormat1)
        Controls.Add(label6)
        Controls.Add(pnScreen1)
        Controls.Add(btCameraStop1)
        Controls.Add(btCameraStartPreview1)
        Controls.Add(btSourceStop)
        Controls.Add(btSourceStart)
        Controls.Add(cbSourceFrameRate)
        Controls.Add(label4)
        Controls.Add(cbSourceVideoFormat)
        Controls.Add(label3)
        Controls.Add(cbVideoCaptureSource)
        Controls.Add(rbCamera)
        Controls.Add(btOpenFile)
        Controls.Add(edSourceFile)
        Controls.Add(label2)
        Controls.Add(label1)
        Controls.Add(rbFile)
        Controls.Add(panel1)
        Margin = New Padding(5, 6, 5, 6)
        Name = "Form1"
        Text = "Virtual Camera SDK Demo (VB.Net WinForms)"
        groupBox2.ResumeLayout(False)
        groupBox2.PerformLayout()
        groupBox1.ResumeLayout(False)
        groupBox1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Private WithEvents btCameraStartCapture1 As System.Windows.Forms.Button
    Private WithEvents label14 As System.Windows.Forms.Label
    Private WithEvents cbAudioCaptureSource As System.Windows.Forms.ComboBox
    Private WithEvents label13 As System.Windows.Forms.Label
    Private WithEvents label12 As System.Windows.Forms.Label
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents btSourceImageLogoAdd As System.Windows.Forms.Button
    Private WithEvents btSourceImageLogoSelect As System.Windows.Forms.Button
    Private WithEvents label11 As System.Windows.Forms.Label
    Private WithEvents edSourceImageLogo As System.Windows.Forms.TextBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents btSourceAddTextLogo As System.Windows.Forms.Button
    Private WithEvents btSouceTextLogoFont As System.Windows.Forms.Button
    Private WithEvents label10 As System.Windows.Forms.Label
    Private WithEvents edSourceTextLogo As System.Windows.Forms.TextBox
    Private WithEvents cbCameraFrameRate2 As System.Windows.Forms.ComboBox
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents cbCameraVideoFormat2 As System.Windows.Forms.ComboBox
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents pnScreen2 As System.Windows.Forms.Panel
    Private WithEvents btCameraStop2 As System.Windows.Forms.Button
    Private WithEvents btCameraStart2 As System.Windows.Forms.Button
    Private WithEvents label9 As System.Windows.Forms.Label
    Private WithEvents panel2 As System.Windows.Forms.Panel
    Private WithEvents cbCameraFrameRate1 As System.Windows.Forms.ComboBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents cbCameraVideoFormat1 As System.Windows.Forms.ComboBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents pnScreen1 As System.Windows.Forms.Panel
    Private WithEvents btCameraStop1 As System.Windows.Forms.Button
    Private WithEvents btCameraStartPreview1 As System.Windows.Forms.Button
    Private WithEvents btSourceStop As System.Windows.Forms.Button
    Private WithEvents btSourceStart As System.Windows.Forms.Button
    Private WithEvents cbSourceFrameRate As System.Windows.Forms.ComboBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents cbSourceVideoFormat As System.Windows.Forms.ComboBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents cbVideoCaptureSource As System.Windows.Forms.ComboBox
    Private WithEvents rbCamera As System.Windows.Forms.RadioButton
    Private WithEvents btOpenFile As System.Windows.Forms.Button
    Private WithEvents edSourceFile As System.Windows.Forms.TextBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents rbFile As System.Windows.Forms.RadioButton
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Private WithEvents fontDialog1 As System.Windows.Forms.FontDialog

End Class
