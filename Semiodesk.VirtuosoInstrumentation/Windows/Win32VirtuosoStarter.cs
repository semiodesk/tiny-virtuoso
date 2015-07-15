#if WINDOWS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

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

        private IntPtr _stdErrHandle;
        private Job _job;
        SafeFileHandle safeHandle;
        private StreamReader _standardError;
        private int _targetPort;
        #endregion

        #region Constructor
        public Win32VirtuosoStarter(int targetPort)
        {
            _targetPort = targetPort;
        }
        #endregion

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
                IntPtr.Zero, null, ref si, out pi);

            _job.AddProcess(pi.hProcess);

            if (success && waitOnStartup)
            {
                double time = 0;
                if (timeout.HasValue)
                    time = timeout.Value.TotalMilliseconds;
                while (!_serverStartOccured)
                {
                    _serverStartOccured = !Util.TestPortOpen(_targetPort);
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


        void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string error = e.Data;
            
        }


        public bool Stop(bool force = false)
        {
            bool res = true;
            if (ProcessRunning)
            {
                Util.SendCtrlC(_process.Id);
                if (!_process.WaitForExit(1000) && force)
                {
                    try
                    {
                        _process.Kill();
                    }
                    catch (InvalidOperationException e)
                    {
                        res= false;
                    }
                }
            }
            return res;
        }

        #endregion
    }
}

#endif