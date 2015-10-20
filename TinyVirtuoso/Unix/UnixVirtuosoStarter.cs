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
using System.Text;
using System.Threading;

namespace Semiodesk.VirtuosoInstrumentation
{
    /// <summary>
    /// 
    /// </summary>
    class NativeVirtuosoStarter : IProcessStarter
    {
        #region Members

        Process _process;

        private bool _serverStartOccured = false;

        public string Executable
        {
            get;
            set;
        }

        public string Parameter
        {
            get;
            set;
            
        }

        public bool ProcessRunning
        {
            get
            {
                return _process != null && !_process.HasExited;
            }

        }

        public bool Started
        {
            get
            {
                return _serverStartOccured;
            }
        }

        public DirectoryInfo _binDir;
        public DirectoryInfo _workingDir;

        #endregion

        #region Constructor
        public NativeVirtuosoStarter(DirectoryInfo binDir = null, DirectoryInfo workingDir = null)
        {
            _process = new Process();

            _binDir = binDir;
            _workingDir = workingDir;
            
        }
        #endregion

        #region Methods

        public bool Start(bool waitOnStartup = true, TimeSpan? timeout = null)
        {
            SetExecutable();

            _process.StartInfo.FileName = Executable;
            _process.StartInfo.WorkingDirectory = _workingDir.FullName;
            _process.StartInfo.Arguments = Parameter;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.ErrorDataReceived += _process_ErrorDataReceived;

            //_process.OutputDataReceived += _process_OutputDataReceived;
            _process.Start();

            //_process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            if (waitOnStartup)
            {
                double time = 0;
                if (timeout.HasValue)
                    time = timeout.Value.TotalMilliseconds;
                while (!_serverStartOccured)
                {
                    Thread.Sleep(10);
                    if (timeout.HasValue)
                    {
                        time -= 10;
                        if (time <= 0)
                            break;
                    }
                }
            }
            return true;
        }


        void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string error = e.Data;
            if (!_serverStartOccured && !string.IsNullOrEmpty(error))
            {
                if (error.Contains("Server online at"))
                    _serverStartOccured = true;
            }
        }

        void _process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string res = e.Data;
        }

        public bool Stop(bool force = false)
        {
            bool res = true;
            if (ProcessRunning)
            {
                SendSigint(_process.Id);
                //Util.SendCtrlC(_process.Id, _binDir);
                if (!_process.WaitForExit(1000) && force)
                {
                    try
                    {
                        _process.Kill();
                    }
                    catch (InvalidOperationException)
                    {
                        res= false;
                    }
                }

                _serverStartOccured = false;
                _process.CancelErrorRead();
                _process.ErrorDataReceived -= _process_ErrorDataReceived;
                _process = null;

            }
            return res;
        }

        private void SendSigint(int pid)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "kill";
            proc.StartInfo.Arguments = string.Format("-SIGINT {0}", pid);
            proc.Start();
            proc.WaitForExit();
        }

        private void SetExecutable()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "chmod";
            proc.StartInfo.Arguments = string.Format("+x {0}", Executable);
            proc.Start();
            proc.WaitForExit();
        }

        #endregion
    }
}
