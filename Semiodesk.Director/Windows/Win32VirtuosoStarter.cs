#if WINDOWS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Semiodesk.Director.Windows
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

        #endregion

        #region Constructor
        public Win32VirtuosoStarter()
        {
        }
        #endregion

        #region Methods

        public bool Start(bool waitOnStartup = true)
        {
            _job = new Job();
            
            STARTUPINFO si = new STARTUPINFO();
            si.wShowWindow = 1;

            //SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            //sa.nLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(sa);
            //sa.lpSecurityDescriptor = IntPtr.Zero;
            //sa.bInheritHandle = true;

            //Win32Process.CreatePipe(out _stdErrHandle,
            //   out si.hStdOutput,
            //   ref sa, 4096 );

            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            bool success = Win32Process.CreateProcess(Executable, string.Format("{0} {1}", Executable, Parameter),
                IntPtr.Zero, IntPtr.Zero, false,
                ProcessCreationFlags.NORMAL_PRIORITY_CLASS | 
                ProcessCreationFlags.CREATE_NEW_CONSOLE |
                //ProcessCreationFlags.CREATE_NO_WINDOW |
                ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB,
                IntPtr.Zero, null, ref si, out pi);

            _job.AddProcess(pi.hProcess);
            
            
            return success;
            
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