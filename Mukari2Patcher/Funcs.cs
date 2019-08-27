using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Mukari2Patcher
{
    internal class Funcs
    {
        public static void Start()
        {
            if (!File.Exists(Configs.Settings.BinaryName))
            {
                MessageBox.Show(Texts.GetText("MISSINGBINARY", Configs.Settings.BinaryName));

                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Configs.Settings.BinaryName;
                /*startInfo.Arguments = "--argument";
                startInfo.Arguments = id;
                MessageBox.Show(id);*/
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show(Texts.GetText("UNKNOWNERROR", exception.Message));
                Application.Current.Shutdown();
            }
        }
    }
}