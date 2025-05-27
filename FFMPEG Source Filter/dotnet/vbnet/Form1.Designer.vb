Namespace FFMPEG_Source_Demo
    Partial Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container()
            btSelectFile = New System.Windows.Forms.Button()
            label1 = New System.Windows.Forms.Label()
            pnScreen = New System.Windows.Forms.Panel()
            btStop = New System.Windows.Forms.Button()
            btPause = New System.Windows.Forms.Button()
            btResume = New System.Windows.Forms.Button()
            btStart = New System.Windows.Forms.Button()
            tbSpeed = New System.Windows.Forms.TrackBar()
            label16 = New System.Windows.Forms.Label()
            lbTime = New System.Windows.Forms.Label()
            tbTimeline = New System.Windows.Forms.TrackBar()
            label2 = New System.Windows.Forms.Label()
            edConnectionTimeOut = New System.Windows.Forms.TextBox()
            label3 = New System.Windows.Forms.Label()
            cbBufferingMode = New System.Windows.Forms.ComboBox()
            cbUseGPU = New System.Windows.Forms.CheckBox()
            tmProgress = New System.Windows.Forms.Timer(components)
            edFilename = New System.Windows.Forms.TextBox()
            openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
            label4 = New System.Windows.Forms.Label()
            cbVideoStream = New System.Windows.Forms.ComboBox()
            cbAudioStream = New System.Windows.Forms.ComboBox()
            label5 = New System.Windows.Forms.Label()
            CType(tbSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(tbTimeline, System.ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' btSelectFile
            ' 
            btSelectFile.Location = New System.Drawing.Point(658, 44)
            btSelectFile.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            btSelectFile.Name = "btSelectFile"
            btSelectFile.Size = New System.Drawing.Size(40, 44)
            btSelectFile.TabIndex = 0
            btSelectFile.Text = "..."
            btSelectFile.UseVisualStyleBackColor = True
            AddHandler btSelectFile.Click, AddressOf btSelectFile_Click
            ' 
            ' label1
            ' 
            label1.AutoSize = True
            label1.Location = New System.Drawing.Point(20, 17)
            label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label1.Name = "label1"
            label1.Size = New System.Drawing.Size(142, 25)
            label1.TabIndex = 1
            label1.Text = "URL or file name"
            ' 
            ' pnScreen
            ' 
            pnScreen.BackColor = System.Drawing.Color.Black
            pnScreen.Location = New System.Drawing.Point(730, 17)
            pnScreen.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            pnScreen.Name = "pnScreen"
            pnScreen.Size = New System.Drawing.Size(912, 825)
            pnScreen.TabIndex = 3
            ' 
            ' btStop
            ' 
            btStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204)
            btStop.Location = New System.Drawing.Point(315, 277)
            btStop.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            btStop.Name = "btStop"
            btStop.Size = New System.Drawing.Size(77, 44)
            btStop.TabIndex = 19
            btStop.Text = "Stop"
            btStop.UseVisualStyleBackColor = True
            AddHandler btStop.Click, AddressOf btStop_Click
            ' 
            ' btPause
            ' 
            btPause.Location = New System.Drawing.Point(218, 277)
            btPause.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            btPause.Name = "btPause"
            btPause.Size = New System.Drawing.Size(87, 44)
            btPause.TabIndex = 18
            btPause.Text = "Pause"
            btPause.UseVisualStyleBackColor = True
            AddHandler btPause.Click, AddressOf btPause_Click
            ' 
            ' btResume
            ' 
            btResume.Location = New System.Drawing.Point(107, 277)
            btResume.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            btResume.Name = "btResume"
            btResume.Size = New System.Drawing.Size(102, 44)
            btResume.TabIndex = 17
            btResume.Text = "Resume"
            btResume.UseVisualStyleBackColor = True
            AddHandler btResume.Click, AddressOf btResume_Click
            ' 
            ' btStart
            ' 
            btStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204)
            btStart.Location = New System.Drawing.Point(25, 277)
            btStart.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            btStart.Name = "btStart"
            btStart.Size = New System.Drawing.Size(72, 44)
            btStart.TabIndex = 16
            btStart.Text = "Start"
            btStart.UseVisualStyleBackColor = True
            AddHandler btStart.Click, AddressOf btStart_Click
            ' 
            ' tbSpeed
            ' 
            tbSpeed.Location = New System.Drawing.Point(550, 217)
            tbSpeed.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            tbSpeed.Maximum = 25
            tbSpeed.Minimum = 5
            tbSpeed.Name = "tbSpeed"
            tbSpeed.Size = New System.Drawing.Size(148, 69)
            tbSpeed.TabIndex = 15
            tbSpeed.Value = 10
            AddHandler tbSpeed.Scroll, AddressOf tbSpeed_Scroll
            ' 
            ' label16
            ' 
            label16.AutoSize = True
            label16.Location = New System.Drawing.Point(552, 187)
            label16.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label16.Name = "label16"
            label16.Size = New System.Drawing.Size(62, 25)
            label16.TabIndex = 14
            label16.Text = "Speed"
            ' 
            ' lbTime
            ' 
            lbTime.AutoSize = True
            lbTime.Location = New System.Drawing.Point(375, 229)
            lbTime.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            lbTime.Name = "lbTime"
            lbTime.Size = New System.Drawing.Size(155, 25)
            lbTime.TabIndex = 13
            lbTime.Text = "00:00:00/00:00:00"
            ' 
            ' tbTimeline
            ' 
            tbTimeline.Location = New System.Drawing.Point(25, 202)
            tbTimeline.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            tbTimeline.Maximum = 100
            tbTimeline.Name = "tbTimeline"
            tbTimeline.Size = New System.Drawing.Size(345, 69)
            tbTimeline.TabIndex = 12
            AddHandler tbTimeline.Scroll, AddressOf tbTimeline_Scroll
            ' 
            ' label2
            ' 
            label2.AutoSize = True
            label2.Location = New System.Drawing.Point(20, 104)
            label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label2.Name = "label2"
            label2.Size = New System.Drawing.Size(208, 25)
            label2.TabIndex = 22
            label2.Text = "Connection timeout (ms)"
            ' 
            ' edConnectionTimeOut
            ' 
            edConnectionTimeOut.Location = New System.Drawing.Point(230, 98)
            edConnectionTimeOut.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            edConnectionTimeOut.Name = "edConnectionTimeOut"
            edConnectionTimeOut.Size = New System.Drawing.Size(72, 31)
            edConnectionTimeOut.TabIndex = 23
            edConnectionTimeOut.Text = "10000"
            ' 
            ' label3
            ' 
            label3.AutoSize = True
            label3.Location = New System.Drawing.Point(333, 104)
            label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label3.Name = "label3"
            label3.Size = New System.Drawing.Size(136, 25)
            label3.TabIndex = 24
            label3.Text = "Buffering mode"
            ' 
            ' cbBufferingMode
            ' 
            cbBufferingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            cbBufferingMode.FormattingEnabled = True
            cbBufferingMode.Items.AddRange(New Object() {"Auto", "On", "Off"})
            cbBufferingMode.Location = New System.Drawing.Point(473, 98)
            cbBufferingMode.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            cbBufferingMode.Name = "cbBufferingMode"
            cbBufferingMode.Size = New System.Drawing.Size(106, 33)
            cbBufferingMode.TabIndex = 25
            ' 
            ' cbUseGPU
            ' 
            cbUseGPU.AutoSize = True
            cbUseGPU.Location = New System.Drawing.Point(592, 102)
            cbUseGPU.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            cbUseGPU.Name = "cbUseGPU"
            cbUseGPU.Size = New System.Drawing.Size(106, 29)
            cbUseGPU.TabIndex = 26
            cbUseGPU.Text = "Use GPU"
            cbUseGPU.UseVisualStyleBackColor = True
            ' 
            ' tmProgress
            ' 
            AddHandler tmProgress.Tick, AddressOf tmProgress_Tick
            ' 
            ' edFilename
            ' 
            edFilename.Location = New System.Drawing.Point(25, 48)
            edFilename.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            edFilename.Name = "edFilename"
            edFilename.Size = New System.Drawing.Size(621, 31)
            edFilename.TabIndex = 2
            edFilename.Text = "c:\"
            ' 
            ' openFileDialog1
            ' 
            openFileDialog1.FileName = "openFileDialog1"
            ' 
            ' label4
            ' 
            label4.AutoSize = True
            label4.Location = New System.Drawing.Point(20, 383)
            label4.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label4.Name = "label4"
            label4.Size = New System.Drawing.Size(117, 25)
            label4.TabIndex = 27
            label4.Text = "Video stream"
            ' 
            ' cbVideoStream
            ' 
            cbVideoStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            cbVideoStream.FormattingEnabled = True
            cbVideoStream.Location = New System.Drawing.Point(160, 377)
            cbVideoStream.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            cbVideoStream.Name = "cbVideoStream"
            cbVideoStream.Size = New System.Drawing.Size(536, 33)
            cbVideoStream.TabIndex = 28
            AddHandler cbVideoStream.SelectedIndexChanged, AddressOf cbVideoStream_SelectedIndexChanged
            ' 
            ' cbAudioStream
            ' 
            cbAudioStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            cbAudioStream.FormattingEnabled = True
            cbAudioStream.Location = New System.Drawing.Point(160, 429)
            cbAudioStream.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            cbAudioStream.Name = "cbAudioStream"
            cbAudioStream.Size = New System.Drawing.Size(536, 33)
            cbAudioStream.TabIndex = 30
            AddHandler cbAudioStream.SelectedIndexChanged, AddressOf cbAudioStream_SelectedIndexChanged
            ' 
            ' label5
            ' 
            label5.AutoSize = True
            label5.Location = New System.Drawing.Point(20, 435)
            label5.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label5.Name = "label5"
            label5.Size = New System.Drawing.Size(119, 25)
            label5.TabIndex = 29
            label5.Text = "Audio stream"
            ' 
            ' Form1
            ' 
            AutoScaleDimensions = New System.Drawing.SizeF(10F, 25F)
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            ClientSize = New System.Drawing.Size(1662, 865)
            Controls.Add(cbAudioStream)
            Controls.Add(label5)
            Controls.Add(cbVideoStream)
            Controls.Add(label4)
            Controls.Add(cbUseGPU)
            Controls.Add(cbBufferingMode)
            Controls.Add(label3)
            Controls.Add(edConnectionTimeOut)
            Controls.Add(label2)
            Controls.Add(btStop)
            Controls.Add(btPause)
            Controls.Add(btResume)
            Controls.Add(btStart)
            Controls.Add(tbSpeed)
            Controls.Add(label16)
            Controls.Add(lbTime)
            Controls.Add(tbTimeline)
            Controls.Add(pnScreen)
            Controls.Add(edFilename)
            Controls.Add(label1)
            Controls.Add(btSelectFile)
            Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            Name = "Form1"
            Text = "FFMPEG Source Demo"
            AddHandler Load, AddressOf Form1_Load
            CType(tbSpeed, System.ComponentModel.ISupportInitialize).EndInit()
            CType(tbTimeline, System.ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()
        End Sub

#End Region

        Private btSelectFile As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label
        Private pnScreen As System.Windows.Forms.Panel
        Private btStop As System.Windows.Forms.Button
        Private btPause As System.Windows.Forms.Button
        Private btResume As System.Windows.Forms.Button
        Private btStart As System.Windows.Forms.Button
        Private tbSpeed As System.Windows.Forms.TrackBar
        Private label16 As System.Windows.Forms.Label
        Private lbTime As System.Windows.Forms.Label
        Private tbTimeline As System.Windows.Forms.TrackBar
        Private label2 As System.Windows.Forms.Label
        Private edConnectionTimeOut As System.Windows.Forms.TextBox
        Private label3 As System.Windows.Forms.Label
        Private cbBufferingMode As System.Windows.Forms.ComboBox
        Private cbUseGPU As System.Windows.Forms.CheckBox
        Private tmProgress As System.Windows.Forms.Timer
        Private edFilename As System.Windows.Forms.TextBox
        Private openFileDialog1 As System.Windows.Forms.OpenFileDialog
        Private label4 As System.Windows.Forms.Label
        Private cbVideoStream As System.Windows.Forms.ComboBox
        Private cbAudioStream As System.Windows.Forms.ComboBox
        Private label5 As System.Windows.Forms.Label
    End Class
End Namespace 