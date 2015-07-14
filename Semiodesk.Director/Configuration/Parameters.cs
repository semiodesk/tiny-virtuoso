using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
{
    
    /// <summary>
    /// Server tuning, options
    /// </summary>
    public class Parameters : IniSectionWrapper
    {
        #region Members
        /// <summary>
        /// This is a Win32 specific option that forces Virtuoso to only run on one CPU in a multiprocessor environment.
        /// </summary>
        public bool? SingleCPU
        {
            get
            {
                return GetBoolData("SingleCPU");
            }
            set
            {
                SetBoolData("SingleCPU", value);
            }
        }


        /// <summary>
        /// [<IP Address>]:<port>
        /// This is the IP Address and port number where the server will start listening. You do not need to specify the listening IP Address but can do in a situation that you want the server to bind to a specific address only.
        /// </summary>
        public string ServerPort
        {
            get
            {
                return GetStringData("ServerPort");
            }
            set
            {
                SetStringData("ServerPort", value);
            }
        }

        /// <summary>
        /// This is the maximum number of threads used in the server. This should be close to the number of concurrent connections if heavy usage is expected. Different systems have different limitations on threads per process but a value of 100 should work in most places.
        /// </summary>
        public int? ServerThreads
        {
            get
            {
                return GetIntData("ServerThreads");
            }
            set
            {
                SetIntData("ServerThreads", value);
            }
        }


        /// <summary>
        /// Stack size of thread used for reading client messages and accepting connections.(default : 50 000 bytes)
        /// </summary>
        public int? ServerThreadSize
        {
            get
            {
                return GetIntData("ServerThreadSize");
            }
            set
            {
                SetIntData("ServerThreadSize", value);
            }
        }

        /// <summary>
        /// Stack size of the main thread (default : 100 000 bytes)
        /// </summary>
        public int? MainThreadSize
        {
            get
            {
                return GetIntData("MainThreadSize");
            }
            set
            {
                SetIntData("MainThreadSize", value);
            }
        }

        /// <summary>
        /// The interval in minutes (default : 0) after which threads in the thread pool should be released.
        /// </summary>
        public int? ThreadCleanupInterval
        {
            get
            {
                return GetIntData("ThreadCleanupInterval");
            }
            set
            {
                SetIntData("ThreadCleanupInterval", value);
            }
        }

        /// <summary>
        /// The maximum number of threads (default : 10) to leave in the thread queue after thread clean-up interval has expired.
        /// </summary>
        public int? ThreadThreshold
        {
            get
            {
                return GetIntData("ThreadThreshold");
            }
            set
            {
                SetIntData("ThreadThreshold", value);
            }
        }

        /// <summary>
        /// Defines the scheduler wake-up interval ( in minutes). By default is 0 i.e. the scheduler is disabled.
        /// </summary>
        public TimeSpan? SchedulerInterval
        {
            get
            {
                return GetTimespanData("SchedulerInterval");
            }
            set
            {
                SetTimespanData("SchedulerInterval", value);
            }
        }

        /// <summary>
        /// The interval in minutes (default : 0) after which allocated resources will be flushed..
        /// </summary>
        public TimeSpan? ResourcesCleanupInterval
        {
            get
            {
                return GetTimespanData("ResourcesCleanupInterval");
            }
            set
            {
                SetTimespanData("ResourcesCleanupInterval", value);
            }
        }

        /// <summary>
        /// Stack size of worker threads. This is the stack size for serving any client SQL statements or HTTP requests. This can be increased if the application uses recursive stored procedures or links in external code needing a large stack. (default 100 000 bytes)
        /// </summary>
        public int? FutureThreadSize
        {
            get
            {
                return GetIntData("FutureThreadSize");
            }
            set
            {
                SetIntData("FutureThreadSize", value);
            }
        }

        /// <summary>
        /// A Percentage that may be greater than 100%. This gives a percentage of the main .db file to which the temp db file may grow before starting to reclaim the oldest persistent hash index. Basically if a particular hash index is reusable (i.e. it references only table columns and the values in these columns have not changed) the engine keeps the hash index defined into the temp db for reuse. This parameter allows some control over the temp db file size.
        /// </summary>
        public float? TempAllocationPct
        {
            get
            {
                return GetFloatData("TempAllocationPct");
            }
            set
            {
                SetFloatData("TempAllocationPct", value);
            }
        }

        /// <summary>
        /// If this is non-zero, the database file(s) will be opened with the O_DIRECT option on platforms where this is supported. This has the effect of doing file system I/O from application buffers directly, bypassing caching by the operating system. This may be useful if a large fraction of RAM is configured as database buffers. If this is on, the file system cache will not grow at the expense of the database process, for example it is less likely to swap out memory that Virtuoso uses for its own database buffers. Mileage will vary according to operating system and version. For large databases where most of system memory is used for database buffers it is advisable to try this.
        /// </summary>
        public bool? O_DIRECT
        {
            get
            {
                return GetBoolData("O_DIRECT");
            }
            set
            {
                SetBoolData("O_DIRECT", value);
            }
        }

        /// <summary>
        /// This is the interval in minutes at which Virtuoso will automatically make a database checkpoint. The automatic checkpoint will not be made if there is less than MinAutoCheckpointSize bytes in the current transaction log. A checkpoint interval of 0 means that no periodic automatic checkpoints are made.
        /// Setting the value to -1 disables also the checkpoints made after a roll forward upon database startup. This setting should be used when backing up the database file(s). This guarantees that even if the server dies and restarts during the copy, no checkpoints that would change these files will take place and thus the backup is clean.
        /// the checkpoint_interval SQL function may be used to change the checkpoint interval value while the database is running.
        /// </summary>
        public TimeSpan? CheckpointInterval
        {
            get
            {
                return GetTimespanData("CheckpointInterval");
            }
            set
            {
                SetTimespanData("CheckpointInterval", value);
            }
        }

        /// <summary>
        /// This controls how the file system is synchronized after a checkpoint. Once the checkpoint has issued all write system calls it needs it can do one of the following depending on this setting:
        /// 0. - Continue, leave the OS to flush buffers when it will.
        /// 1. - Initiate the flush but do not wait for it to complete.
        /// 2. - Block until the OS has flushed all writes pertaining to any of the database files.
        /// </summary>
        public int? CheckpointSyncMode
        {
            get
            {
                return GetIntData("CheckpointSyncMode");
            }
            set
            {
                SetIntData("CheckpointSyncMode", value);
            }
        }

        /// <summary>
        /// This controls the check of page maps
        /// </summary>
        public bool? PageMapCheck
        {
            get
            {
                return GetBoolData("PageMapCheck");
            }
            set
            {
                SetBoolData("PageMapCheck", value);
            }
        }

        /// <summary>
        /// This controls the amount of RAM used by Virtuoso to cache database files. This has a critical performance impact and thus the value should be fairly high for large databases. Exceeding physical memory in this setting will have a significant negative impact. For a database-only server about 65% of available RAM could be configured for database buffers.
        /// Each buffer caches one 8K page of data and occupies approximately 8700 bytes of memory.
        /// </summary>
        public int? NumberOfBuffers
        {
            get
            {
                return GetIntData("NumberOfBuffers");
            }
            set
            {
                SetIntData("NumberOfBuffers", value);
            }
        }

        /// <summary>
        /// Specifies how many pages Virtuoso is allowed to remap. Remapping means that pages can consume the space of two actual pages until checkpoint. See the Checkpoint & Page Remapping section in the SQL Reference chapter for more information.
        /// </summary>
        public int? MaxCheckpointRemap
        {
            get
            {
                return GetIntData("MaxCheckpointRemap");
            }
            set
            {
                SetIntData("MaxCheckpointRemap", value);
            }
        }

        /// <summary>
        /// This setting should always be 0.
        /// </summary>
        public int? PrefixResultNames
        {
            get
            {
                return GetIntData("PrefixResultNames");
            }
            set
            {
                SetIntData("PrefixResultNames", value);
            }
        }

        /// <summary>
        /// This controls the case sensitivity of the Virtuoso SQL interpreter. The following values are supported:
        /// 0 - SQL is case sensitive and identifiers are stored in the case they are entered in. This is similar to the Progress or Informix default.
        /// 1 - SQL is case sensitive and Unquoted identifiers are converted to upper case when read. To use non-upper case identifiers, the identifiers must be quoted with double quotes ("). This is similar to Oracle.
        /// 2 - SQL is case-insensitive and identifiers are stored in the case where first seen. Unlike the situation in other modes, two identifiers differing only in case cannot exist. This is similar to the MS SQL Server 7 default behavior.
        /// Note:
        /// Once a database is created in one case mode the case mode should not be changed as that may change the interpretation of stored procedures etc.
        /// </summary>
        public int? CaseMode 
        {
            get
            {
                return GetIntData("CaseMode");
            }
            set
            {
                SetIntData("CaseMode", value);
            }
        }

        /// <summary>
        /// See CheckpointInterval.
        /// </summary>
        public int? MinAutoCheckpointSize
        {
            get
            {
                return GetIntData("MinAutoCheckpointSize");
            }
            set
            {
                SetIntData("MinAutoCheckpointSize", value);
            }
        }

        /// <summary>
        /// This is the size of transaction log in bytes after which an automatic checkpoint is initiated. If this is non-zero, whenever the transaction log size exceeds this size an automatic checkpoint is started. This will result in approximately like sized logs to be generated. This is useful with the CheckpointAuditTrail option for generating a trail of equal sized consecutive logs.
        /// </summary>
        public int? AutoCheckpointLogSize
        {
            get
            {
                return GetIntData("AutoCheckpointLogSize");
            }
            set
            {
                SetIntData("AutoCheckpointLogSize", value);
            }
        }

        /// <summary>
        /// If this is non-zero each checkpoint will start a new log and leave the old transaction log untouched. A 0 value indicates that the transaction log may be reused once the transactions in it have been written in a checkpoint.
        /// If it is important to keep an audit trail of all transactions in the database's life time then this option should be true. A new log file will be generated in the old log file's directory with a name ending with the date and time of the new log file's creation. Old log files can be manually copied into backup storage or deleted. Only one log file is active at one time. The newest log file may at any time be written to by the server, but that is the only log file Virtuoso has open at each time. Thus reading any logs is safe. Writing or deleting the active log file will result in loss of data, and possibly referential integrity, in the database. See the Back and Recovery chapter for more information on this and related parameters.
        /// </summary>
        public bool? CheckpointAuditTrail
        {
            get
            {
                return GetBoolData("CheckpointAuditTrail");
            }
            set
            {
                SetBoolData("CheckpointAuditTrail", value);
            }
        }

        /// <summary>
        /// If non-zero the system SQL function is enabled. This will allow a dba group user to run shell commands through SQL. This poses a potential security risk and hence the setting should normally be 0.
        /// </summary>
        public bool? AllowOSCalls
        {
            get
            {
                return GetBoolData("AllowOSCalls");
            }
            set
            {
                SetBoolData("AllowOSCalls", value);
            }
        }

        /// <summary>
        /// This is the maximum number of rows returned by a static cursor. Default = 5000
        /// </summary>
        public int? MaxStaticCursorRows
        {
            get
            {
                return GetIntData("MaxStaticCursorRows");
            }
            set
            {
                SetIntData("MaxStaticCursorRows", value);
            }
        }

        /// <summary>
        /// This is the amount of text data processed in one batch of the free-text index when doing a batch update or non-incrementally reindexing the data. Default : 10,000,000
        /// </summary>
        public int? FreeTextBatchSize
        {
            get
            {
                return GetIntData("FreeTextBatchSize");
            }
            set
            {
                SetIntData("FreeTextBatchSize", value);
            }
        }

        /// <summary>
        /// When set to 1, if an application prepares a statement with insufficient number of input parameters, the unspecified ones are assumed to be NULL.
        /// </summary>
        public bool? NullUnspecifiedParams
        {
            get
            {
                return GetBoolData("NullUnspecifiedParams");
            }
            set
            {
                SetBoolData("NullUnspecifiedParams", value);
            }
        }

        /// <summary>
        /// Defines a sorting order according to SYS_COLLATIONS. The name supplied to this parameter must be in charsets_list(1).
        /// </summary>

        public string Collation
        {
            get
            {
                return GetStringData("Collation");
            }
            set
            {
                SetStringData("Collation", value);
            }
        }

        /// <summary>
        /// <path> := <absolute_path> or <relative_path> comma-delimited list of OS directories allowed for file operations such as file_to_string(). The server base directory (the directory containing this INI file) must appear on this list in order to enable File DSNs to work. On Windows use in the path "\".
        /// The Virtuoso ISQL utility can be used to check the Server DirsAllowed params as follows:
        ///   SQL> select server_root (), virtuoso_ini_path ();
        /// The above should show in the result the server working directory and INI file name.
        /// Also you can check the relevant INI setting by running following statement via ISQL command line utility:
        ///   SQL> select cfg_item_value (virtuoso_ini_path (), 'Parameters', 'DirsAllowed');
        /// </summary>
        public List<string> DirsAllowed
        {
            get
            {
                return GetStringListData("DirsAllowed");
            }
            set
            {
                SetStringListData("DirsAllowed", value);
            }
        }

        /// <summary>
        /// <path> := <absolute_path> or <relative_path> OS directories denied for file operations. See Virtuoso ACL's for information on functions that are restricted.
        /// </summary>
        public List<string> DirsDenied
        {
            get
            {
                return GetStringListData("DirsDenied");
            }
            set
            {
                SetStringListData("DirsDenied", value);
            }
        }

        /// <summary>
        /// <path> := <absolute_path> or <relative_path> OS directory containig VADs files. When set, enables automatic update of vads on server startup. On Windows use in the path "\".
        /// </summary>
        public string VADInstallDir
        {
            get
            {
                return GetStringData("VADInstallDir");
            }
            set
            {
                SetStringData("VADInstallDir", value);
            }
        }

        /// <summary>
        /// Specifies the port on which the server listens for incoming SSL CLI requests.
        /// </summary>
        public int? SSLServerPort
        {
            get
            {
                return GetIntData("SSLServerPort");
            }
            set
            {
                SetIntData("SSLServerPort", value);
            }
        }

        /// <summary>
        /// The SSL certificate to use (same meaning as the SSLCertificate in HTTPServer section)
        /// </summary>
        public string SSLCertificate
        {
            get
            {
                return GetStringData("SSLCertificate");
            }
            set
            {
                SetStringData("SSLCertificate", value);
            }
        }

        /// <summary>
        /// The server's private key (same meaning as the SSLCertificate in HTTPServer section)
        /// </summary>
        public string SSLPrivateKey
        {
            get
            {
                return GetStringData("SSLPrivateKey");
            }
            set
            {
                SetStringData("SSLPrivateKey", value);
            }
        }


        /// <summary>
        /// This parameter governs the maximum number of partial or full join orders that the Virtuoso SQL Optimized compiler will calculate per select statement. When MaxOptimizeLayouts has been reached, the best execution plan reached thus far will be used. The default value is 1000, specifying 0 will try all possible orders and guarantee that the best plan is reached.
        /// </summary>
        public int? MaxOptimizeLayouts
        {
            get
            {
                return GetIntData("MaxOptimizeLayouts");
            }
            set
            {
                SetIntData("MaxOptimizeLayouts", value);
            }
        }

        /// <summary>
        /// The default value is 0. If non-zero, this specifies that the SQL compiler should stop considering alternative execution plans after the elapsed compilation time exceeds the best run time estimate times the parameter. For example, if this is 2, then compilation stops after using twice the time of the best plan reached thus far. Enabling this option increases performance when processing short running queries that are each executed once. Using this with long running queries or prepared parametrized queries is not useful and may lead to non-optimal plans being selected.
        /// </summary>
        public bool? StopCompilerWhenXOverRunTime
        {
            get
            {
                return GetBoolData("StopCompilerWhenXOverRunTime");
            }
            set
            {
                SetBoolData("StopCompilerWhenXOverRunTime", value);
            }
        }


        /// <summary>
        /// This parameter accepts a comma-delimited list of tracing options to activate by default. Enabled trace options will list there respective errors in the virtuoso.log file when encountered. Valid options are:
        /// user_log
        /// failed_log
        /// compile
        /// ddl_log
        /// client_sql
        /// errors
        /// dsn
        /// sql_send
        /// transact
        /// remote_transact
        /// exec
        /// soap
        /// If an invalid option is set then this error will be listed in the virtuoso.log file upon server startup. Virtuoso will continue to log selected options unless the trace_off() function is called for that item.
        /// </summary>
        public int? TraceOn
        {
            get
            {
                return GetIntData("TraceOn");
            }
            set
            {
                SetIntData("TraceOn", value);
            }
        }

        /// <summary>
        /// LiteMode = 0/1 (default 0)
        /// Runs server in lite mode. When Lite mode is on:
        /// 
        /// • the web services are not initialized i.e. no web server, dav, soap, pop3 etc.

        ///     • the replication is stopped
        ///     • the pl debugging is disabled
        ///     • plugins are disabled
        ///     • rendezvous is disabled
        ///     • the relevant tables to the above are not created
        ///     • the index tree maps is set to 8 if no other setting is given
        ///     • memory reserve is not allocated

        /// • affects DisableTcpSocket. So DisableTcpSocket setting is treated as 1 when LiteMode=1, regardless of value in INI file 
        /// </summary>
        public bool? LiteMode
        {
            get
            {
                return GetBoolData("LiteMode");
            }
            set
            {
                SetBoolData("LiteMode", value);
            }
        }

        /// <summary>
        /// RdfFreeTextRulesSize = 10 or more 
        /// The size of hash to control rdf free text index
        /// </summary>
        public int? RdfFreeTextRulesSize
        {
            get
            {
                return GetIntData("RdfFreeTextRulesSize");
            }
            set
            {
                SetIntData("RdfFreeTextRulesSize", value);
            }
        }

        /// <summary>
        /// IndexTreeMaps = 2 -1024 (power of 2)
        /// Size of index tree maps, larger is better for speed but consume memory, 
        /// Defaults:
        /// LiteMode 2 
        /// 'normal' mode 256
        /// </summary>
        public int? IndexTreeMaps
        {
            get
            {
                return GetIntData("IndexTreeMaps");
            }
            set
            {
                SetIntData("IndexTreeMaps", value);
            }
        }
        #endregion


        #region Constructor
        public Parameters(SectionData data)
            : base(data)
        { }
        #endregion
    }
}
