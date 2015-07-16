using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation
{
    public interface IProcessStarter
    {
        #region Members
        string Executable { get; set; }
        string Parameter { get; set; }
        bool ProcessRunning { get; }
        bool Started { get; }
        #endregion


        #region Constructor
        #endregion

        #region Methods
        bool Start(bool waitOnStartup = true, TimeSpan? timeout = null);
        bool Stop(bool force = false, DirectoryInfo binDir = null);
        #endregion

    }
}
