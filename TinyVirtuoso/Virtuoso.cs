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


namespace Semiodesk.VirtuosoInstrumentation
{
    public class Virtuoso : IDisposable
    {
        #region Members
        FileInfo _binary;
        FileInfo _configFile;
        VirtuosoConfig _config;
       
        public DirectoryInfo EnvironmentDir { get; set; }

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
                return _starter.ProcessRunning;
            }
            
        }

        public VirtuosoConfig Configuration { get { return _config; } }
        #endregion

        #region Constructor
        public Virtuoso(FileInfo binary, FileInfo config)
        {
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
                int? port = PortUtils.GetPort(_config.Parameters.ServerPort);
                if (!port.HasValue)
                    throw new ArgumentException("No valid port given.");

                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                    _starter = new NativeVirtuosoStarter(workingDir: _configFile.Directory);
                else
                    _starter = new Win32VirtuosoStarter(port.Value, EnvironmentDir);

                _starter.Executable = _binary.FullName;
                _starter.Parameter = string.Format("-f -c {0}", _configFile.FullName);
                res = _starter.Start(waitOnStartup, timeout);
            }
            return res;
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
