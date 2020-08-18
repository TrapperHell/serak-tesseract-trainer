using System.Diagnostics;
using System.IO;

namespace SerakTesseractTrainer
{
    public static class TesseractExecutor
    {
        public static void cmdExcute(string command, string parameter, string projectFolder)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.WorkingDirectory = projectFolder;
            //info.CreateNoWindow = true;
            info.Arguments = parameter;
            info.FileName = Path.Combine(Configuration.TesseractPath, command);

            using (Process proc = Process.Start(info))
            {
                proc.WaitForExit();
            }
        }
    }
}