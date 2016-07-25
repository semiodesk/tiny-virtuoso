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

using Semiodesk.VirtuosoInstrumentation.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Semiodesk.VirtuosoInstrumentation.Windows;
using Semiodesk.TinyVirtuoso.Utils;
using Semiodesk.VirtuosoInstrumentation;


namespace Semiodesk.TinyVirtuoso
{
    public class Virtuoso : IDisposable
    {
        #region Members
        FileInfo _binary;
        FileInfo _configFile;
        VirtuosoConfig _config;
       
        internal DirectoryInfo EnvironmentDir { get; set; }

        IProcessStarter _starter;

        public bool IsOnline
        {
            get
            {
                return ProcessRunning;
            }
        }

        public bool ProcessRunning
        {
            get
            {
                if (_starter != null)
                    return _starter.ProcessRunning;
                else
                    return false;
            }
            
        }

        Random _rnd = new Random(DateTime.Now.Millisecond);

        public bool AutoPort { get; private set; }

        public VirtuosoConfig Configuration { get { return _config; } }

        public DirectoryInfo DataDirectory { get { return _configFile.Directory; } }
        #endregion

        #region Constructor
        public Virtuoso(FileInfo binary, FileInfo config, bool autoPort)
        {
            AutoPort = autoPort;
            _binary = binary;
            _configFile = config;
            _config = new VirtuosoConfig(_configFile);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the 
        /// </summary>
        public bool Start(bool waitOnStartup = true, TimeSpan? timeout = null)
        {
            bool res = false;
            _config.Locked = true;
            if (_starter == null)
            {
                int? port;
                if (AutoPort)
                {

                    do
                    {
                        port = 35000 + _rnd.Next(10, 60);
                    } while (!PortUtils.TestPort(port.Value));
                    Configuration.Parameters.ServerPort = string.Format("localhost:{0}", port);
                    Configuration.SaveConfigFile();
                }
                else 
                {
                    port = PortUtils.GetPort(_config.Parameters.ServerPort);
                    if (!port.HasValue)
                        throw new ArgumentException("No valid port given.");
                }

                string param = "";
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    _starter = new UnixVirtuosoStarter(port.Value, _binary.Directory, _configFile.Directory);
                    param = string.Format("-f -c \"{0}\"", _configFile.FullName);
                }
                else
                {
                    _starter = new Win32VirtuosoStarter(port.Value, _binary.Directory, _configFile.Directory);
                    param = string.Format("-f -c \"{0}\"", _configFile.FullName);
                }

                _starter.Executable = _binary.FullName;
                _starter.Parameter = param;
                res = _starter.Start(waitOnStartup, timeout);
            }
            return res;
        }

        public string GetTrinityConnectionString(string username = "dba", string password = "dba")
        {
            int? port = PortUtils.GetPort(Configuration.Parameters.ServerPort);

            return string.Format("provider=virtuoso;host=localhost;port={0};uid={1};pw={2}", port, username, password);
        }

        public string GetAdoNetConnectionString(string username = "dba", string password = "dba")
        {
            int? port = PortUtils.GetPort(Configuration.Parameters.ServerPort);
            return "Server=localhost:" + port + ";uid=" + username + ";pwd=" + password + ";Charset=utf-8";
        }

        public void Stop(bool force = false)
        {
            if( _starter != null )
                _starter.Stop(force);
            if( _config != null )
                _config.Locked = false;
        }


        ~Virtuoso()
        {
            Dispose(false); 
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RemoveLock()
        {
            try
            {
                foreach (var x in _configFile.Directory.GetFiles("*.lck"))
                {
                    x.Delete();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Virtuoso seems to be running still. Try to shut it down manually.");
            }
        }
        

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_starter != null && ProcessRunning)
                    Stop(true);
            }
        }
        #endregion
    }
}
