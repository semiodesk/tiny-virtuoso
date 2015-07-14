using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Semiodesk.Director
{
    public class Util
    {
        public static bool SendCtrlC(int pid)
        {
            var process = new Process();

            process.StartInfo.FileName = "CtrlCSender.exe";
            process.StartInfo.Arguments = pid.ToString();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            if( !process.HasExited)
                process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}
