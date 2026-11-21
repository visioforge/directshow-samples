using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.AudioEncode
{
    internal sealed class MainForm : Form
    {
        private readonly RadioButton _fileSource = new RadioButton();
        private readonly RadioButton _deviceSource = new RadioButton();
        private readonly TextBox _sourceFile = new TextBox();
        private readonly Button _browseSource = new Button();
        private readonly ComboBox _devices = new ComboBox();
        private readonly ComboBox _formats = new ComboBox();
        private readonly Label _rateLabel = new Label();
        private readonly NumericUpDown _bitrate = new NumericUpDown();
        private readonly TextBox _output = new TextBox();
        private readonly Button _browseOutput = new Button();
        private readonly Button _start = new Button();
        private readonly Button _stop = new Button();
        private readonly Label _status = new Label();
        private readonly Session _session = new Session();
        private DsDevice[] _audioDevices = new DsDevice[0];
        private bool _running;

        public MainForm()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Text = "Encoding Filters Pack — audio";
            ClientSize = new Size(420, 360);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font(Font.FontFamily, 9f);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            BuildUi();
            LoadDevices();
            FormClosed += delegate { Shutdown(); };
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
            _fileSource.Text = "File";
            _fileSource.Checked = true;
            _fileSource.SetBounds(16, 16, 60, 24);
            _fileSource.CheckedChanged += delegate { UpdateSourceMode(); };

            _deviceSource.Text = "Capture device";
            _deviceSource.SetBounds(80, 16, 140, 24);

            _sourceFile.SetBounds(16, 48, 300, 24);
            _browseSource.Text = "...";
            _browseSource.SetBounds(324, 46, 36, 26);
            _browseSource.Click += delegate { BrowseSource(); };

            _devices.DropDownStyle = ComboBoxStyle.DropDownList;
            _devices.SetBounds(16, 84, 344, 24);

            var formatLabel = new Label();
            formatLabel.Text = "Format";
            formatLabel.SetBounds(16, 118, 80, 20);
            _formats.DropDownStyle = ComboBoxStyle.DropDownList;
            _formats.Items.Add(new FormatChoice("MP3 (LAME)", 0, ".mp3"));
            _formats.Items.Add(new FormatChoice("AAC in MP4", 1, ".mp4"));
            _formats.Items.Add(new FormatChoice("FLAC in Matroska", 2, ".mka"));
            _formats.SelectedIndex = 0;
            _formats.SetBounds(16, 138, 200, 24);
            _formats.SelectedIndexChanged += delegate { ApplyFormatUi(); };

            _rateLabel.Text = "Bitrate (kbps)";
            _rateLabel.SetBounds(230, 118, 120, 20);
            _bitrate.Minimum = 32;
            _bitrate.Maximum = 320;
            _bitrate.Increment = 16;
            _bitrate.Value = 128;
            _bitrate.SetBounds(230, 138, 130, 24);

            var outputLabel = new Label();
            outputLabel.Text = "Output";
            outputLabel.SetBounds(16, 176, 80, 20);
            _output.SetBounds(16, 196, 300, 24);
            _browseOutput.Text = "...";
            _browseOutput.SetBounds(324, 194, 36, 26);
            _browseOutput.Click += delegate { BrowseOutput(); };

            _start.Text = "Start";
            _start.SetBounds(16, 240, 160, 32);
            _start.Click += delegate { OnStart(); };
            _stop.Text = "Stop";
            _stop.Enabled = false;
            _stop.SetBounds(190, 240, 160, 32);
            _stop.Click += delegate { OnStop(); };

            _status.Text = "Idle.";
            _status.AutoEllipsis = true;
            _status.SetBounds(16, 284, 380, 48);

            Controls.AddRange(new Control[]
            {
                _fileSource, _deviceSource, _sourceFile, _browseSource, _devices,
                formatLabel, _formats, _rateLabel, _bitrate, outputLabel, _output, _browseOutput,
                _start, _stop, _status
            });

            _output.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "VisioForge",
                "audio.mp3");
            UpdateSourceMode();
        }

        private void LoadDevices()
        {
            _audioDevices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
            foreach (var device in _audioDevices)
            {
                _devices.Items.Add(device.Name);
            }

            if (_devices.Items.Count > 0)
            {
                _devices.SelectedIndex = 0;
            }
        }

        private void UpdateSourceMode()
        {
            var file = _fileSource.Checked;
            _sourceFile.Enabled = file;
            _browseSource.Enabled = file;
            _devices.Enabled = !file;
        }

        private void ApplyFormatUi()
        {
            var format = (FormatChoice)_formats.SelectedItem;
            if (format == null)
            {
                return;
            }

            // FLAC is lossless and has no bitrate.
            _bitrate.Enabled = format.Kind != 2;
            ApplyExtension();
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

            _output.Text = Path.Combine(directory, "audio" + format.Extension);
        }

        private void BrowseSource()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Media files|*.mp4;*.mkv;*.mov;*.avi;*.mp3;*.wav;*.m4a|All files|*.*";
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
                dialog.Filter = format.Kind == 2 ? "Matroska audio|*.mka" : format.Kind == 0 ? "MP3|*.mp3" : "MP4|*.mp4";
                dialog.FileName = "audio" + format.Extension;
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

                DsDevice device = null;
                if (_deviceSource.Checked)
                {
                    if (_devices.SelectedIndex < 0)
                    {
                        MessageBox.Show(this, "No audio device is selected.", Text);
                        return;
                    }

                    device = _audioDevices[_devices.SelectedIndex];
                }
                else if (!File.Exists(_sourceFile.Text))
                {
                    MessageBox.Show(this, "Choose a source file.", Text);
                    return;
                }

                _running = true;
                _session.Start(
                    Handle,
                    _fileSource.Checked,
                    _sourceFile.Text,
                    device,
                    format.Kind,
                    (int)_bitrate.Value,
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
