using System;
using System.Diagnostics;

namespace ExtrenalAppRestart
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Restarting....");
            try {
                Process proc = new Process();
                proc.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/YPBBT.exe";
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
            } catch (Exception) { }
        }
      
    }
}
