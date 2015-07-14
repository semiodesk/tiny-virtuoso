using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director
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
        bool Start(bool waitOnStartup = true);
        bool Stop(bool force = false);
        #endregion

    }
}
