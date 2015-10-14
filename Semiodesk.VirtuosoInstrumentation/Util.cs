// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Net.Sockets;

namespace Semiodesk.VirtuosoInstrumentation
{
    public class Util
    {
        public static bool SendCtrlC(int pid, DirectoryInfo senderBinDir = null)
        {
            var process = new Process();
            string binName =  "CtrlCSender.exe";
            FileInfo sender;
            if( senderBinDir == null )
                sender = new FileInfo(binName);
            else
                sender = new FileInfo(Path.Combine(senderBinDir.FullName, binName));

            process.StartInfo.FileName = sender.FullName;
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
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                return TryConnect(port);
            else
                return SearchPort(port);
        }

        public static bool SearchPort(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TryConnect(int port)
        {
            bool result = false;
            try
            {
                Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new System.Net.IPAddress(new byte[] {127, 0, 0, 1}), port);
                socket.Close();
            }catch(SocketException ex) 
            {
                if (ex.ErrorCode == 10061)
                    result = true;
                else
                    throw ex;
            }
            return result;

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
