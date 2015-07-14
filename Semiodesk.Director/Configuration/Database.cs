using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
{
    /// <summary>
    /// Location of database files
    /// </summary>
    public class Database : IniSectionWrapper
    {
        #region Members
        /// <summary>
        /// For a single file database, this is the relative path of the file in the format appropriate to the platform. The path is resolved relative to the directory that is current at server startup. All other paths are interpreted similarly. 
        /// </summary>
        public string DatabaseFile 
        {
            get
            {
                return GetStringData("DatabaseFile");
            }
            set
            {
                SetStringData("DatabaseFile", value);
            }
        }

        /// <summary>
        /// This is the transaction log file. If this parameter is omitted, which should never be the case in practice, the database will run without log, meaning that it cannot recover transactions committed since last checkpoint if it should abnormally terminate. There is always a single log file for one server. The file grows as transactions get committed until a checkpoint is reached at which time the transactions are applied to the database file and the trx file is reclaimed, unless CheckpointAuditTrail is enabled. 
        /// </summary>
        public string TransactionFile
        {
            get
            {
                return GetStringData("TransactionFile");
            }
            set
            {
                SetStringData("TransactionFile", value);
            }
        }

        /// <summary>
        /// This file logs database error messages, e.g. 'out of disk'. By viewing this the dba can trace problems and see at which times the server has started, checkpoints have been made, etc. 
        /// </summary>
        public string ErrorLogFile
        {
            get
            {
                return GetStringData("ErrorLogFile");
            }
            set
            {
                SetStringData("ErrorLogFile", value);
            }
        }

        /// <summary>
        /// This controls what events get logged into the database error log. This should always be 7.
        /// </summary>
        public int? ErrorLogLevel
        {
            get
            {
                return GetIntData("ErrorLogLevel");
            }
            set
            {
                SetIntData("ErrorLogLevel", value);
            }
        }

        /// <summary>
        /// This optional parameter can be used to manually specify the location of the Virtuoso lock (.lck) file. This can be relative or the full path to the lock file. Virtuoso, by default, creates a file with the same name as the DatabaseFile but with the extension of .lck. This file exists when the Virtuoso server is running to prevent it starting multiple times using the same parameters, and should be automatically removed by the server upon exit. However, not all file systems support file locking, such as NFS, therefore this parameter can be set to keep the lock file on a more appropriate file system.
        /// </summary>
        public string LockFile
        {
            get
            {
                return GetStringData("LockFile");
            }
            set
            {
                SetStringData("LockFile", value);
            }
        }

        /// <summary>
        /// This is the size that the database file automatically grows (in 8k pages) when the current file is not large enough. Default = minimum = 100. The parameter has no effect if striping is set.
        /// </summary>
        public string FileExtend
        {
            get
            {
                return GetStringData("FileExtend");
            }
            set
            {
                SetStringData("FileExtend", value);
            }
        }

        /// <summary>
        /// A non-zero value will enable the settings in [Striping] to take effect. If this is the case the DatabaseFile parameter is ignored and the files in [striping] are used.
        /// </summary>
        public bool? Striping
        {
            get
            {
                return GetBoolData("Striping");
            }
            set
            {
                SetBoolData("Striping", value);
            }
        }

        /// <summary>
        /// If this is non-zero log segmentation is enabled. This is only used for crash dumps where several files may be needed to accommodate the recovery logs. If non-zero, this will be followed by entries of the form Log1=...
        /// </summary>
        public string LogSegments
        {
            get
            {
                return GetStringData("DatabaseFile");
            }
            set
            {
                SetStringData("DatabaseFile", value);
            }
        }


        //public string XA_Persistent_File { get; set; }


        //public string MaxCheckpointRemap { get; set; }


        internal string TempStorage
        {
            get
            {
                return GetStringData("TempStorage");
            }
            set
            {
                SetStringData("TempStorage", value);
            }
        }

        /// <summary>
        /// Virtuoso can writes log worthy messages to the system log (Unix based operating systems including Linux) or the Windows Event Log (Windows operating systems). Messages are written in the system/event log before the virtuoso.log file is opened, therefore errors due to absence of virtuoso.ini log are loggable there. This system/event logging can be enabled using this option, by default it is set to 0 meaning off.
        /// On Unix/Linux messages are written as "Virtuoso" events.
        /// On Windows messages are written in the Application event log.
        /// </summary>
        public bool? Syslog
        {
            get
            {
                return GetBoolData("Syslog");
            }
            set
            {
                SetBoolData("Syslog", value);
            }
        }
        #endregion

        #region Constructor
        public Database(SectionData data)
            : base(data)
        { }
        #endregion

    }

}
