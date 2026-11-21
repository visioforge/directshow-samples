using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.WebM
{
    internal sealed class MainForm : Form
    {
        private readonly RadioButton _fileSource = new RadioButton();
        private readonly RadioButton _cameraSource = new RadioButton();
        private readonly TextBox _sourceFile = new TextBox();
        private readonly Button _browseSource = new Button();
        private readonly ComboBox _cameras = new ComboBox();
        private readonly ComboBox _audioDevices = new ComboBox();
        private readonly NumericUpDown _videoBitrate = new NumericUpDown();
        private readonly ComboBox _codecs = new ComboBox();
        private readonly CheckBox _encodeAudio = new CheckBox();
        private readonly TextBox _output = new TextBox();
        private readonly Button _browseOutput = new Button();
        private readonly Button _start = new Button();
        private readonly Button _stop = new Button();
        private readonly Label _status = new Label();
        private readonly Panel _preview = new Panel();
        private readonly Session _session = new Session();
        private DsDevice[] _videoDevices = new DsDevice[0];
        private DsDevice[] _audioDevicesList = new DsDevice[0];
        private bool _running;

        public MainForm()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = "Encoding Filters Pack — WebM";
            ClientSize = new Size(980, 560);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font(Font.FontFamily, 9f);

            BuildUi();
            LoadDevices();
            Shown += delegate { _session.PlacePreview(_preview.ClientSize.Width, _preview.ClientSize.Height); };
            FormClosed += delegate { Shutdown(); };
            _preview.Resize += delegate { _session.PlacePreview(_preview.ClientSize.Width, _preview.ClientSize.Height); };
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Session.GraphNotifyMessage && _running)
            {
                string error;
                if (_session.DrainEvents(out error))
                {
                    Shutdown();
                    _status.Text = error ?? "Finished.";
                }
            }

            base.WndProc(ref m);
        }

        private void BuildUi()
        {
            var options = new GroupBox();
            options.Text = "Encode";
            options.SetBounds(8, 8, 360, 544);

            _fileSource.Text = "File";
            _fileSource.Checked = true;
            _fileSource.SetBounds(16, 28, 60, 24);
            _fileSource.CheckedChanged += delegate { UpdateSourceMode(); };

            _cameraSource.Text = "Camera";
            _cameraSource.SetBounds(80, 28, 80, 24);

            _sourceFile.SetBounds(16, 58, 250, 24);
            _browseSource.Text = "...";
            _browseSource.SetBounds(274, 56, 36, 26);
            _browseSource.Click += delegate { BrowseSource(); };

            _cameras.DropDownStyle = ComboBoxStyle.DropDownList;
            _cameras.SetBounds(16, 92, 294, 24);

            var audioLabel = new Label();
            audioLabel.Text = "Audio device";
            audioLabel.SetBounds(16, 122, 120, 20);
            _audioDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            _audioDevices.SetBounds(16, 142, 294, 24);

            var bitrateLabel = new Label();
            bitrateLabel.Text = "Video bitrate (kbps)";
            bitrateLabel.SetBounds(16, 176, 150, 20);
            _videoBitrate.Minimum = 100;
            _videoBitrate.Maximum = 100000;
            _videoBitrate.Value = 2000;
            _videoBitrate.SetBounds(170, 172, 140, 24);

            var codecLabel = new Label();
            codecLabel.Text = "Codecs";
            codecLabel.SetBounds(16, 206, 140, 20);
            _codecs.DropDownStyle = ComboBoxStyle.DropDownList;

            // FFMPEG_SINK_VIDEO_CODEC VP8 11, VP9 12; FFMPEG_SINK_AUDIO_CODEC VORBIS 5, OPUS 6.
            _codecs.Items.Add(new CodecChoice("VP8 + Vorbis", 11, 5));
            _codecs.Items.Add(new CodecChoice("VP9 + Opus", 12, 6));
            _codecs.SelectedIndex = 0;
            _codecs.SetBounds(16, 226, 294, 24);

            _encodeAudio.Text = "Audio";
            _encodeAudio.Checked = true;
            _encodeAudio.SetBounds(16, 264, 160, 24);

            var outputLabel = new Label();
            outputLabel.Text = "Output";
            outputLabel.SetBounds(16, 300, 80, 20);
            _output.SetBounds(16, 320, 250, 24);
            _browseOutput.Text = "...";
            _browseOutput.SetBounds(274, 318, 36, 26);
            _browseOutput.Click += delegate { BrowseOutput(); };

            _start.Text = "Start";
            _start.SetBounds(16, 366, 140, 32);
            _start.Click += delegate { OnStart(); };
            _stop.Text = "Stop";
            _stop.Enabled = false;
            _stop.SetBounds(170, 366, 140, 32);
            _stop.Click += delegate { OnStop(); };

            _status.Text = "Idle.";
            _status.AutoEllipsis = true;
            _status.SetBounds(16, 410, 320, 48);

            options.Controls.AddRange(new Control[]
            {
                _fileSource, _cameraSource, _sourceFile, _browseSource, _cameras,
                audioLabel, _audioDevices, bitrateLabel, _videoBitrate, codecLabel, _codecs,
                _encodeAudio, outputLabel, _output, _browseOutput, _start, _stop, _status
            });

            _preview.BackColor = Color.Black;
            _preview.SetBounds(376, 8, 596, 544);

            Controls.Add(options);
            Controls.Add(_preview);

            _output.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "VisioForge",
                "encode.webm");
            UpdateSourceMode();
        }

        private void LoadDevices()
        {
            _videoDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            foreach (var device in _videoDevices)
            {
                _cameras.Items.Add(device.Name);
            }

            if (_cameras.Items.Count > 0)
            {
                _cameras.SelectedIndex = 0;
            }

            _audioDevices.Items.Add("(none)");
            _audioDevicesList = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
            foreach (var device in _audioDevicesList)
            {
                _audioDevices.Items.Add(device.Name);
            }

            _audioDevices.SelectedIndex = _audioDevices.Items.Count > 1 ? 1 : 0;
        }

        private void UpdateSourceMode()
        {
            var file = _fileSource.Checked;
            _sourceFile.Enabled = file;
            _browseSource.Enabled = file;
            _cameras.Enabled = !file;
            _audioDevices.Enabled = !file;
        }

        private void BrowseSource()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv;*.webm|All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _sourceFile.Text = dialog.FileName;
                }
            }
        }

        private void BrowseOutput()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "WebM|*.webm";
                dialog.FileName = "encode.webm";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _output.Text = dialog.FileName;
                }
            }
        }

        private void OnStart()
        {
            var output = _output.Text.Trim();
            if (output.Length == 0)
            {
                MessageBox.Show(this, "Choose an output path.", Text);
                return;
            }

            try
            {
                var directory = Path.GetDirectoryName(output);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                DsDevice video = null;
                DsDevice audio = null;
                if (_cameraSource.Checked)
                {
                    if (_cameras.SelectedIndex < 0)
                    {
                        MessageBox.Show(this, "No camera is selected.", Text);
                        return;
                    }

                    video = _videoDevices[_cameras.SelectedIndex];
                    if (_audioDevices.SelectedIndex > 0)
                    {
                        audio = _audioDevicesList[_audioDevices.SelectedIndex - 1];
                    }
                }
                else if (!File.Exists(_sourceFile.Text))
                {
                    MessageBox.Show(this, "Choose a source file.", Text);
                    return;
                }


                _running = true;
                _session.Start(
                    Handle,
                    _preview.Handle,
                    _preview.ClientSize.Width,
                    _preview.ClientSize.Height,
                    _fileSource.Checked,
                    _sourceFile.Text,
                    video,
                    audio,
                    (CodecChoice)_codecs.SelectedItem,
                    (int)_videoBitrate.Value,
                    _encodeAudio.Checked,
                    output);

                _start.Enabled = false;
                _stop.Enabled = true;
                _status.Text = "Encoding.";
            }
            catch (Exception ex)
            {
                Shutdown();
                _status.Text = ex.Message;
                MessageBox.Show(this, ex.Message, Text);
            }
        }

        private void OnStop()
        {
            _status.Text = "Stopping...";
            _status.Refresh();
            Shutdown();
            _status.Text = "Stopped.";
        }

        private void Shutdown()
        {
            _running = false;
            _session.Stop();
            _start.Enabled = true;
            _stop.Enabled = false;
        }
    }
}
