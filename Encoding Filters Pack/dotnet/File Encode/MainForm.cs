using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisioForge.DirectShowAPI;
using VisioForge.DirectShowLib;

namespace VisioForge.Samples.FileEncode
{
    internal sealed class MainForm : Form
    {
        private readonly RadioButton _fileSource = new RadioButton();
        private readonly RadioButton _cameraSource = new RadioButton();
        private readonly TextBox _sourceFile = new TextBox();
        private readonly Button _browseSource = new Button();
        private readonly ComboBox _cameras = new ComboBox();
        private readonly ComboBox _audioDevices = new ComboBox();
        private readonly ComboBox _encoders = new ComboBox();
        private readonly NumericUpDown _videoBitrate = new NumericUpDown();
        private readonly ComboBox _rateControl = new ComboBox();
        private readonly ComboBox _containers = new ComboBox();
        private readonly CheckBox _encodeAudio = new CheckBox();
        private readonly NumericUpDown _audioBitrate = new NumericUpDown();
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
            Text = "Encoding Filters Pack — file encode";
            ClientSize = new Size(980, 560);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font(Font.FontFamily, 9f);

            BuildUi();
            LoadDevices();
            LoadEncoders();
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

            var encoderLabel = new Label();
            encoderLabel.Text = "Video encoder";
            encoderLabel.SetBounds(16, 176, 140, 20);
            _encoders.DropDownStyle = ComboBoxStyle.DropDownList;
            _encoders.SetBounds(16, 196, 294, 24);

            var bitrateLabel = new Label();
            bitrateLabel.Text = "Video bitrate (kbps)";
            bitrateLabel.SetBounds(16, 230, 150, 20);
            _videoBitrate.Minimum = 100;
            _videoBitrate.Maximum = 100000;
            _videoBitrate.Value = 4000;
            _videoBitrate.SetBounds(170, 226, 140, 24);

            var rateLabel = new Label();
            rateLabel.Text = "Rate control";
            rateLabel.SetBounds(16, 260, 140, 20);
            _rateControl.DropDownStyle = ComboBoxStyle.DropDownList;
            _rateControl.Items.Add(VFMFCommonRateControlMode.CBR);
            _rateControl.Items.Add(VFMFCommonRateControlMode.PeakConstrainedVBR);
            _rateControl.Items.Add(VFMFCommonRateControlMode.Quality);
            _rateControl.SelectedIndex = 0;
            _rateControl.SetBounds(16, 280, 294, 24);

            var containerLabel = new Label();
            containerLabel.Text = "Container";
            containerLabel.SetBounds(16, 314, 140, 20);
            _containers.DropDownStyle = ComboBoxStyle.DropDownList;
            _containers.Items.Add(new MuxChoice("MP4", VFMFMuxer.MP4, ".mp4"));
            _containers.Items.Add(new MuxChoice("MKV", VFMFMuxer.MKV, ".mkv"));
            _containers.Items.Add(new MuxChoice("MOV", VFMFMuxer.MOV, ".mov"));
            _containers.Items.Add(new MuxChoice("MPEG-TS", VFMFMuxer.MPEG_TS, ".ts"));
            _containers.Items.Add(new MuxChoice("HLS (folder)", VFMFMuxer.HLS, null));
            _containers.SelectedIndex = 0;
            _containers.SetBounds(16, 334, 294, 24);
            _containers.SelectedIndexChanged += delegate { ApplyContainerExtension(); };

            _encodeAudio.Text = "AAC audio";
            _encodeAudio.Checked = true;
            _encodeAudio.SetBounds(16, 370, 120, 24);
            var audioRateLabel = new Label();
            audioRateLabel.Text = "kbps";
            audioRateLabel.SetBounds(250, 374, 40, 20);
            _audioBitrate.Minimum = 32;
            _audioBitrate.Maximum = 512;
            _audioBitrate.Value = 128;
            _audioBitrate.SetBounds(150, 370, 90, 24);

            var outputLabel = new Label();
            outputLabel.Text = "Output";
            outputLabel.SetBounds(16, 406, 80, 20);
            _output.SetBounds(16, 426, 250, 24);
            _browseOutput.Text = "...";
            _browseOutput.SetBounds(274, 424, 36, 26);
            _browseOutput.Click += delegate { BrowseOutput(); };

            _start.Text = "Start";
            _start.SetBounds(16, 466, 140, 32);
            _start.Click += delegate { OnStart(); };
            _stop.Text = "Stop";
            _stop.Enabled = false;
            _stop.SetBounds(170, 466, 140, 32);
            _stop.Click += delegate { OnStop(); };

            _status.Text = "Idle.";
            _status.AutoEllipsis = true;
            _status.SetBounds(16, 506, 320, 24);

            options.Controls.AddRange(new Control[]
            {
                _fileSource, _cameraSource, _sourceFile, _browseSource, _cameras,
                audioLabel, _audioDevices, encoderLabel, _encoders, bitrateLabel, _videoBitrate,
                rateLabel, _rateControl, containerLabel, _containers, _encodeAudio, _audioBitrate,
                audioRateLabel, outputLabel, _output, _browseOutput, _start, _stop, _status
            });

            _preview.BackColor = Color.Black;
            _preview.SetBounds(376, 8, 596, 544);

            Controls.Add(options);
            Controls.Add(_preview);

            _output.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "VisioForge",
                "encode.mp4");
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

        private void LoadEncoders()
        {
            FiltersAvailableInfo info;
            var listed = true;
            try
            {
                info = MFTFilterEnum.GetFiltersAvailable(out _);
            }
            catch (Exception ex)
            {
                info = new FiltersAvailableInfo();
                listed = false;
                _status.Text = "Hardware encoders were not listed. " + ex.Message;
            }

            AddEncoder("H.264 software", VFMFVideoEncoder.MS_H264, !listed || info.H264_CPU);
            AddEncoder("H.264 Intel QuickSync", VFMFVideoEncoder.QSV_H264, info.QSV_H264);
            AddEncoder("H.264 NVIDIA NVENC", VFMFVideoEncoder.NVENC_H264, info.NVENC_H264);
            AddEncoder("H.264 AMD", VFMFVideoEncoder.AMD_H264, info.AMD_H264);
            AddEncoder("H.265 software", VFMFVideoEncoder.MS_H265, true);
            AddEncoder("H.265 Intel QuickSync", VFMFVideoEncoder.QSV_H265, info.QSV_H265);
            AddEncoder("H.265 NVIDIA NVENC", VFMFVideoEncoder.NVENC_H265, info.NVENC_H265);
            AddEncoder("H.265 AMD", VFMFVideoEncoder.AMD_H265, info.AMD_H265);

            for (var i = 0; i < _encoders.Items.Count; i++)
            {
                if (((EncoderChoice)_encoders.Items[i]).Available)
                {
                    _encoders.SelectedIndex = i;
                    break;
                }
            }

            if (listed && !info.AAC)
            {
                _encodeAudio.Checked = false;
                _encodeAudio.Enabled = false;
            }
        }

        private void AddEncoder(string caption, VFMFVideoEncoder encoder, bool available)
        {
            _encoders.Items.Add(new EncoderChoice(caption, encoder, available));
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
                dialog.Filter = "Media|*.mp4;*.mov;*.mkv;*.avi;*.wmv;*.mpg;*.mpeg;*.ts|All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _sourceFile.Text = dialog.FileName;
                }
            }
        }

        private void BrowseOutput()
        {
            var mux = (MuxChoice)_containers.SelectedItem;
            if (mux.Extension == null)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        _output.Text = dialog.SelectedPath;
                    }
                }

                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = mux.Caption + "|*" + mux.Extension;
                dialog.FileName = Path.GetFileName(_output.Text);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _output.Text = dialog.FileName;
                }
            }
        }

        private void ApplyContainerExtension()
        {
            var mux = (MuxChoice)_containers.SelectedItem;
            if (mux == null || mux.Extension == null || string.IsNullOrEmpty(_output.Text))
            {
                return;
            }

            var directory = Path.GetDirectoryName(_output.Text);
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            _output.Text = Path.Combine(directory, "encode" + mux.Extension);
        }

        private void OnStart()
        {
            var encoder = (EncoderChoice)_encoders.SelectedItem;
            if (encoder == null || !encoder.Available)
            {
                MessageBox.Show(this, "That encoder is not installed on this PC.", Text);
                return;
            }

            var mux = (MuxChoice)_containers.SelectedItem;
            var output = _output.Text.Trim();
            if (output.Length == 0)
            {
                MessageBox.Show(this, "Choose an output path.", Text);
                return;
            }

            try
            {
                if (mux.Extension == null)
                {
                    Directory.CreateDirectory(output);
                }
                else
                {
                    var directory = Path.GetDirectoryName(output);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
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
                    encoder,
                    (int)_videoBitrate.Value,
                    (VFMFCommonRateControlMode)_rateControl.SelectedItem,
                    mux,
                    _encodeAudio.Checked,
                    (int)_audioBitrate.Value,
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
