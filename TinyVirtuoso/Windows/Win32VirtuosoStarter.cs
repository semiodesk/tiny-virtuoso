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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Semiodesk.TinyVirtuoso.Utils;

namespace Semiodesk.VirtuosoInstrumentation.Windows
{
    class Win32VirtuosoStarter : IProcessStarter
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

        public int Port { get; set; }
         
        private Job _job;
        private int _targetPort;
        private DirectoryInfo _binDir;
        private DirectoryInfo _workingDir;
        #endregion

        #region Constructor
        public Win32VirtuosoStarter(int targetPort, DirectoryInfo binDir = null, DirectoryInfo workingDir = null)
        {
            Port = targetPort;
            _binDir = binDir;
            _workingDir = workingDir;
        }
        #endregion

        ~Win32VirtuosoStarter()
        {
            Dispose(false);
        }

        #region Methods

        public bool Start(bool waitOnStartup = true, TimeSpan? timeout = null)
        {
            _job = new Job();
            
            STARTUPINFO si = new STARTUPINFO();
            si.wShowWindow = 0;


            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            bool success = Win32Process.CreateProcess(Executable, string.Format("{0} {1}", Executable, Parameter),
                IntPtr.Zero, IntPtr.Zero, false,
                ProcessCreationFlags.NORMAL_PRIORITY_CLASS | 
                ProcessCreationFlags.CREATE_NO_WINDOW |
                ProcessCreationFlags.STARTF_USESTDHANDLES |
                ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB,
                IntPtr.Zero, _workingDir.FullName, ref si, out pi);

            _job.AddProcess(pi.hProcess);
            _process = Process.GetProcessById((int)pi.dwProcessId);

            if (success && waitOnStartup)
            {
                double time = 0;
                if (timeout.HasValue)
                    time = timeout.Value.TotalMilliseconds;
                while (!_serverStartOccured)
                {
                    _serverStartOccured = !PortUtils.IsPortFree(Port);
                    if (!_serverStartOccured)
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
            }
            


            return success;
            
        }


        public bool Stop(bool force = false)
        {
            bool res = true;
            if (ProcessRunning)
            {
                Util.SendCtrlC(_process.Id, _binDir);
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
            }
            return res;
        }


        protected void Dispose(bool fromDispose)
        {
            _process.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}

