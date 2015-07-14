using Semiodesk.Director;
using Semiodesk.TinyVirtuoso.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Semiodesk.TinyVirtuoso
{
    public class TinyVirtuoso
    {
        #region Members

        public IEnumerable<string> AvailableInstances { get { return _instances.Keys; } }

        public string DefaultInstance { get; set; }

        protected Dictionary<string, Virtuoso> _instances = new Dictionary<string, Virtuoso>();

        protected List<int> _usedPorts = new List<int>();

        /// <summary>
        /// Rootdirectory of TinyVirtuoso
        /// </summary>
        public DirectoryInfo RootDir { get; private set; }

        /// <summary>
        /// Directory of this assembly
        /// </summary>
        public DirectoryInfo CurrentDir { get; private set; }

        /// <summary>
        /// Contains all Databases
        /// </summary>
        public DirectoryInfo InstanceCollectionDir { get; private set; }

        FileInfo _templateConfig;

        FileInfo _executable;

        Random _rnd = new Random(DateTime.Now.Millisecond);
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new TinyVirtuoso.
        /// </summary>
        /// <param name="rootDir">Tells TinyVirtuoso where to store the databases, if the directory already contains databases these are made available. If no directory is given, one is created in the ApplicationData folder.</param>
        public TinyVirtuoso(DirectoryInfo rootDir = null, string defaultDatabase = null)
        {

            RootDir = GetRootDir(rootDir);
            CurrentDir = GetCurrentDir();

            if (!CheckVirtuoso())
                SetupVirtuoso();

            InstanceCollectionDir = GetInstanceCollectionDir();
            if (!InstanceCollectionDir.Exists)
                InstanceCollectionDir.Create();
            else
                LoadExistingInstances();

            _templateConfig = GetTemplateConfig();

            DefaultInstance = defaultDatabase;
        }

        #endregion

        #region Methods

        private DirectoryInfo GetRootDir(DirectoryInfo rootDir = null)
        {
            if (rootDir == null)
            {
                rootDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TinyVirtuoso"));
                if (!rootDir.Exists)
                {
                    rootDir.Create();
                }
            }
            return rootDir;
        }

        private DirectoryInfo GetCurrentDir()
        {
            Uri u = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            FileInfo asm = new FileInfo(u.LocalPath);
            return new DirectoryInfo(Path.Combine(asm.Directory.FullName, "TinyVirtuoso"));
        }

        private FileInfo GetTemplateConfig()
        {
            string path = Path.Combine(CurrentDir.FullName, "database");
            path = Path.Combine(path, "virtuoso.ini");
            var conf = new FileInfo(path);
            if (!conf.Exists)
                throw new FileNotFoundException("Database template file not found!", _templateConfig.FullName);
            return conf;
        }

        private DirectoryInfo GetInstanceCollectionDir()
        {
            var instanceCollectionDir = new DirectoryInfo(Path.Combine(RootDir.FullName, "databases"));
            return instanceCollectionDir;
        }

        void SetupVirtuoso()
        {
            DirectoryInfo targetPath = new DirectoryInfo(Path.Combine(RootDir.FullName, "virtuoso"));
            DirectoryInfo sourcePath = CurrentDir;

            if( targetPath.Exists )
                targetPath.Delete(true);

            DirectoryUtils.DirectoryCopy(sourcePath.FullName, targetPath.FullName, true);
            CheckVirtuoso();
        }

        bool CheckVirtuoso()
        {
            string path = Path.Combine(RootDir.FullName, "virtuoso");
            path = Path.Combine(path, "virtuoso-t.exe");
            _executable = new FileInfo(path);
            return _executable.Exists;
        }

        void LoadExistingInstances()
        {
            foreach (var db in InstanceCollectionDir.GetDirectories())
            {
                InitInstance(db);
            }
        }

        public void RenameInstance(string oldName, string newName)
        {

        }

        public string GetConnectionString(string username, string password)
        {
            return GetConnectionString(DefaultInstance, username, password);
        }

        public string GetConnectionString(string dbName, string username, string password)
        {
            Virtuoso v = _instances[dbName];

            var port = v.Configuration.Parameters.ServerPort;
            var res = port.Split(':');
            if (res.Count() > 1)
                port = res[1];
            else
                port = res[0];

            return string.Format("provider=virtuoso;host=localhost;port={0};uid={1};pw={2}",port, username, password );
        }

        public void Start(string instanceName = null)
        {
            if (string.IsNullOrEmpty(instanceName))
                instanceName = DefaultInstance;

            Virtuoso v = _instances[instanceName];
            v.Start();
        }

        public void Stop(string instanceName = null)
        {
            if (string.IsNullOrEmpty(instanceName))
                instanceName = DefaultInstance;

            Virtuoso v = _instances[instanceName];
            v.Stop();
        }

        private void InitInstance(DirectoryInfo instanceDir)
        {
            string dbName = instanceDir.Name;
            FileInfo targetConfig = new FileInfo(Path.Combine(instanceDir.FullName, "virtuoso.ini"));

            Virtuoso virt = new Virtuoso(_executable, targetConfig);

            int port;
            do
            {
                port = 1200 + _rnd.Next(10, 60);
            } while (!PortUtils.TestPort(port));

            virt.Configuration.Parameters.ServerPort = string.Format("localhost:{0}", port);
            virt.Configuration.SaveConfigFile();
            _instances.Add(dbName, virt);
            
        }

        public void CreateInstance(string instanceName=null)
        {
            if (string.IsNullOrEmpty(instanceName))
                instanceName = Guid.NewGuid().ToString();

            DirectoryInfo databaseDir = new DirectoryInfo(Path.Combine(InstanceCollectionDir.FullName, instanceName));
            if (databaseDir.Exists)
                throw new ArgumentException(string.Format("A database with the given name {0} exists already.", instanceName));

            databaseDir.Create();
            FileInfo targetConfig = new FileInfo(Path.Combine(databaseDir.FullName, "virtuoso.ini"));
            _templateConfig.CopyTo(targetConfig.FullName);

            InitInstance(databaseDir);
            
            if (DefaultInstance == null)
                DefaultInstance = instanceName;

        }

        public void SaveDatabase(DirectoryInfo dir, string fileName=null, string extension="zip")
        {
        }

        public void LoadDatabase(FileInfo dbFile)
        {

        }
        #endregion
    }
}
