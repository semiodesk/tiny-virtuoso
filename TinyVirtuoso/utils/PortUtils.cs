﻿// LICENSE:
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
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Net.Sockets;

namespace Semiodesk.TinyVirtuoso.Utils
{
    public class PortUtils
    {
        public static bool TestPort(int port)
        {
            return IsPortFree(port);			
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

		public static bool IsPortFree(int port)
		{
			bool result = false;
			try
			{
                TcpClient cl = new TcpClient();
                IAsyncResult r = cl.BeginConnect("127.0.0.1", port, null, null);
                r.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10), true);
                result = !cl.Connected;
                cl.Close();

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
