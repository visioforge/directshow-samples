using System;
using System.Windows.Forms;

namespace VisioForge.Samples.FileEncode
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
