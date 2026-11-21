using System;
using System.Windows.Forms;

namespace VisioForge.Samples.AudioEncode
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
