using System;
using System.Windows.Forms;

namespace VisioForge.Samples.H264V10
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
