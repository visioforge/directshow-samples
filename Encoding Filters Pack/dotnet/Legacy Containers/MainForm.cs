using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.LegacyContainers
{
    internal sealed class MainForm : Form
    {
        private readonly RadioButton _fileSource = new RadioButton();
        private readonly RadioButton _cameraSource = new RadioButton();
        private readonly TextBox _sourceFile = new TextBox();
        private readonly Button _browseSource = new Button();
        private readonly ComboBox _cameras = new ComboBox();
        private readonly ComboBox _audioDevices = new ComboBox();
        private readonly ComboBox _formats = new ComboBox();
        private readonly ComboBox _tvSystem = new ComboBox();
        private readonly NumericUpDown _width = new NumericUpDown();
        private readonly NumericUpDown _height = new NumericUpDown();
        private readonly NumericUpDown _videoBitrate = new NumericUpDown();
        private readonly CheckBox _encodeAudio = new CheckBox();
        private readonly NumericUpDown _audioBitrate = new NumericUpDown();
        private readonly NumericUpDown _sampleRate = new NumericUpDown();
        private readonly NumericUpDown _channels = new NumericUpDown();
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
            Text = "Encoding Filters Pack — legacy containers";
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
            _fileSource.SetBounds(16, 24, 60, 24);
            _fileSource.CheckedChanged += delegate { UpdateSourceMode(); };
            _cameraSource.Text = "Camera";
            _cameraSource.SetBounds(80, 24, 80, 24);

            _sourceFile.SetBounds(16, 52, 250, 24);
            _browseSource.Text = "...";
            _browseSource.SetBounds(274, 50, 36, 26);
            _browseSource.Click += delegate { BrowseSource(); };

            _cameras.DropDownStyle = ComboBoxStyle.DropDownList;
            _cameras.SetBounds(16, 84, 294, 24);

            var audioLabel = new Label();
            audioLabel.Text = "Audio device";
            audioLabel.SetBounds(16, 112, 120, 18);
            _audioDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            _audioDevices.SetBounds(16, 130, 294, 24);

            var formatLabel = new Label();
            formatLabel.Text = "Container";
            formatLabel.SetBounds(16, 160, 80, 18);
            _formats.DropDownStyle = ComboBoxStyle.DropDownList;
            _formats.Items.Add(new FormatChoice("FLV", 0, ".flv"));
            _formats.Items.Add(new FormatChoice("MPEG-1", 1, ".mpg"));
            _formats.Items.Add(new FormatChoice("MPEG-1 VCD", 2, ".mpg"));
            _formats.Items.Add(new FormatChoice("MPEG-2", 3, ".mpg"));
            _formats.Items.Add(new FormatChoice("MPEG-2 SVCD", 5, ".mpg"));
            _formats.Items.Add(new FormatChoice("MPEG-2 DVD", 6, ".mpg"));
            _formats.SelectedIndex = 0;
            _formats.SetBounds(16, 178, 180, 24);
            _formats.SelectedIndexChanged += delegate { ApplyExtension(); };

            _tvSystem.DropDownStyle = ComboBoxStyle.DropDownList;
            _tvSystem.Items.Add("Default");
            _tvSystem.Items.Add("PAL");
            _tvSystem.Items.Add("NTSC");
            _tvSystem.SelectedIndex = 0;
            _tvSystem.SetBounds(204, 178, 106, 24);

            var sizeLabel = new Label();
            sizeLabel.Text = "Output size";
            sizeLabel.SetBounds(16, 210, 90, 18);
            _width.Minimum = 160;
            _width.Maximum = 1920;
            _width.Increment = 2;
            _width.Value = 1280;
            _width.SetBounds(110, 206, 70, 24);
            _height.Minimum = 120;
            _height.Maximum = 1080;
            _height.Increment = 2;
            _height.Value = 720;
            _height.SetBounds(190, 206, 70, 24);

            var bitrateLabel = new Label();
            bitrateLabel.Text = "Video kbps";
            bitrateLabel.SetBounds(16, 240, 80, 18);
            _videoBitrate.Minimum = 100;
            _videoBitrate.Maximum = 20000;
            _videoBitrate.Value = 4000;
            _videoBitrate.SetBounds(110, 236, 90, 24);

            _encodeAudio.Text = "Audio";
            _encodeAudio.Checked = true;
            _encodeAudio.SetBounds(210, 236, 70, 24);

            var audioRateLabel = new Label();
            audioRateLabel.Text = "Audio kbps / Hz / ch";
            audioRateLabel.SetBounds(16, 270, 140, 18);
            _audioBitrate.Minimum = 32;
            _audioBitrate.Maximum = 384;
            _audioBitrate.Value = 128;
            _audioBitrate.SetBounds(16, 288, 70, 24);
            _sampleRate.Minimum = 8000;
            _sampleRate.Maximum = 48000;
            _sampleRate.Increment = 100;
            _sampleRate.Value = 48000;
            _sampleRate.SetBounds(96, 288, 90, 24);
            _channels.Minimum = 1;
            _channels.Maximum = 2;
            _channels.Value = 2;
            _channels.SetBounds(196, 288, 50, 24);

            var outputLabel = new Label();
            outputLabel.Text = "Output";
            outputLabel.SetBounds(16, 322, 80, 18);
            _output.SetBounds(16, 340, 250, 24);
            _browseOutput.Text = "...";
            _browseOutput.SetBounds(274, 338, 36, 26);
            _browseOutput.Click += delegate { BrowseOutput(); };

            _start.Text = "Start";
            _start.SetBounds(16, 380, 140, 32);
            _start.Click += delegate { OnStart(); };
            _stop.Text = "Stop";
            _stop.Enabled = false;
            _stop.SetBounds(170, 380, 140, 32);
            _stop.Click += delegate { OnStop(); };

            _status.Text = "Idle.";
            _status.AutoEllipsis = true;
            _status.SetBounds(16, 422, 320, 48);

            options.Controls.AddRange(new Control[]
            {
                _fileSource, _cameraSource, _sourceFile, _browseSource, _cameras,
                audioLabel, _audioDevices, formatLabel, _formats, _tvSystem, sizeLabel, _width, _height,
                bitrateLabel, _videoBitrate, _encodeAudio, audioRateLabel, _audioBitrate, _sampleRate, _channels,
                outputLabel, _output, _browseOutput, _start, _stop, _status
            });

            _preview.BackColor = Color.Black;
            _preview.SetBounds(376, 8, 596, 544);
            Controls.Add(options);
            Controls.Add(_preview);

            _output.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "VisioForge",
                "encode.flv");
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

        private void ApplyExtension()
        {
            var format = (FormatChoice)_formats.SelectedItem;
            if (format == null || string.IsNullOrEmpty(_output.Text))
            {
                return;
            }

            var directory = Path.GetDirectoryName(_output.Text);
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            _output.Text = Path.Combine(directory, "encode" + format.Extension);
        }

        private void BrowseSource()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv|All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _sourceFile.Text = dialog.FileName;
                }
            }
        }

        private void BrowseOutput()
        {
            var format = (FormatChoice)_formats.SelectedItem;
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = format.Caption + "|*" + format.Extension;
                dialog.FileName = "encode" + format.Extension;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _output.Text = dialog.FileName;
                }
            }
        }

        private void OnStart()
        {
            var format = (FormatChoice)_formats.SelectedItem;
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
                    format.Format,
                    (int)_width.Value,
                    (int)_height.Value,
                    (int)_videoBitrate.Value,
                    _encodeAudio.Checked,
                    (int)_audioBitrate.Value,
                    (int)_sampleRate.Value,
                    (int)_channels.Value,
                    _tvSystem.SelectedIndex,
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
