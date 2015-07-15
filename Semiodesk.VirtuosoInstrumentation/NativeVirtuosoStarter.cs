using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Semiodesk.VirtuosoInstrumentation
{
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

        #endregion
        #region Constructor
        public NativeVirtuosoStarter()
        {
            _process = new Process();

           
            
        }
        #endregion

        #region Methods

        public bool Start(bool waitOnStartup = true, TimeSpan? timeout = null)
        {

            _process.StartInfo.FileName = Executable;
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

                _serverStartOccured = false;
                _process.CancelErrorRead();
                _process.ErrorDataReceived -= _process_ErrorDataReceived;
                _process = null;

            }
            return res;
        }

        #endregion
    }
}
