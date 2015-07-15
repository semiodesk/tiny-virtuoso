using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation
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

        public static bool TestPortOpen(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    return false;
                    break;
                }
            }
            return true;
        }

        public static int? GetPort(string hostWithPort)
        {
            var port = hostWithPort;
            var tmp = port.Split(':');
            if (tmp.Count() > 1)
                port = tmp[1];
            else
                port = tmp[0];
            int res;
            if (int.TryParse(port, out res))
                return res;

            return null;
        }
    }
}
