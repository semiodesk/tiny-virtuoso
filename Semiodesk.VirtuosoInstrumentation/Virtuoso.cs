using Semiodesk.VirtuosoInstrumentation.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Semiodesk.VirtuosoInstrumentation.Windows;


namespace Semiodesk.VirtuosoInstrumentation
{
    public class Virtuoso : IDisposable
    {
        #region Members
        FileInfo _binary;
        FileInfo _configFile;
        VirtuosoConfig _config;

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
                int? port = Util.GetPort(_config.Parameters.ServerPort);
                if (!port.HasValue)
                    throw new ArgumentException("No valid port given.");

#if WINDOWS
                _starter = new Win32VirtuosoStarter(port.Value);
#else
                _starter = new NativeVirtuosoStarter();
#endif
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
