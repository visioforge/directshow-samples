Namespace VLC_Source_Demo
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
            tmProgress = New System.Windows.Forms.Timer(components)
            edFilename = New System.Windows.Forms.TextBox()
            openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
            DirectCast(tbSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(tbTimeline, System.ComponentModel.ISupportInitialize).BeginInit()
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
            btStop.Location = New System.Drawing.Point(315, 196)
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
            btPause.Location = New System.Drawing.Point(218, 196)
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
            btResume.Location = New System.Drawing.Point(107, 196)
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
            btStart.Location = New System.Drawing.Point(25, 196)
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
            tbSpeed.Location = New System.Drawing.Point(550, 136)
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
            label16.Location = New System.Drawing.Point(552, 106)
            label16.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            label16.Name = "label16"
            label16.Size = New System.Drawing.Size(62, 25)
            label16.TabIndex = 14
            label16.Text = "Speed"
            ' 
            ' lbTime
            ' 
            lbTime.AutoSize = True
            lbTime.Location = New System.Drawing.Point(375, 148)
            lbTime.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
            lbTime.Name = "lbTime"
            lbTime.Size = New System.Drawing.Size(155, 25)
            lbTime.TabIndex = 13
            lbTime.Text = "00:00:00/00:00:00"
            ' 
            ' tbTimeline
            ' 
            tbTimeline.Location = New System.Drawing.Point(25, 121)
            tbTimeline.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
            tbTimeline.Maximum = 100
            tbTimeline.Name = "tbTimeline"
            tbTimeline.Size = New System.Drawing.Size(345, 69)
            tbTimeline.TabIndex = 12
            AddHandler tbTimeline.Scroll, AddressOf tbTimeline_Scroll
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
            edFilename.Text = "c:\Samples\!video.mp4"
            ' 
            ' openFileDialog1
            ' 
            openFileDialog1.FileName = "openFileDialog1"
            ' 
            ' Form1
            ' 
            AutoScaleDimensions = New System.Drawing.SizeF(10F, 25F)
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            ClientSize = New System.Drawing.Size(1662, 865)
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
            Text = "VLC Source Demo"
            AddHandler Load, AddressOf Form1_Load
            DirectCast(tbSpeed, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(tbTimeline, System.ComponentModel.ISupportInitialize).EndInit()
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
        Private tmProgress As System.Windows.Forms.Timer
        Private edFilename As System.Windows.Forms.TextBox
        Private openFileDialog1 As System.Windows.Forms.OpenFileDialog
    End Class
End Namespace 