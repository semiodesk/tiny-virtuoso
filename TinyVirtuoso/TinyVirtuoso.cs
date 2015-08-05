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


using Semiodesk.VirtuosoInstrumentation;
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

        public DirectoryInfo TargetBinPath { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new TinyVirtuoso.
        /// </summary>
        /// <param name="rootDir">Tells TinyVirtuoso where to store the databases, if the directory already contains databases these are made available. If no directory is given, one is created in the ApplicationData folder.</param>
        public TinyVirtuoso(DirectoryInfo rootDir = null)
        {
            RootDir = GetRootDir(rootDir);
            CurrentDir = GetCurrentDir();
            TargetBinPath = GetTargetBinPath();

            if (!CheckVirtuoso())
                SetupVirtuoso();

            InstanceCollectionDir = GetInstanceCollectionDir();
            if (!InstanceCollectionDir.Exists)
                InstanceCollectionDir.Create();
            else
                LoadExistingInstances();

            _templateConfig = GetTemplateConfig();
        }

        #endregion

        #region Methods
        #region Public Interface
        public string GetConnectionString(string instanceName, string username, string password)
        {
            Virtuoso v = _instances[instanceName];

            int? port = Util.GetPort(v.Configuration.Parameters.ServerPort);

            return string.Format("provider=virtuoso;host=localhost;port={0};uid={1};pw={2}", port, username, password);
        }

        public string GetOdbcConnectionString(string instanceName, string username, string password)
        {
            Virtuoso v = _instances[instanceName];
            int? port = Util.GetPort(v.Configuration.Parameters.ServerPort);
            return "Server=localhost:" + port + ";uid=" + username + ";pwd=" + password + ";Charset=utf-8";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="waitForStartup"></param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        public bool Start(string instanceName, bool waitForStartup = true, int timeout = -1)
        {
            if (string.IsNullOrEmpty(instanceName) || !_instances.ContainsKey(instanceName))
                throw new ArgumentException(string.Format("No instance with key {0} found", instanceName));

            Virtuoso v = _instances[instanceName];
            TimeSpan? timespan = null;
            if (timeout > 0)
                timespan = TimeSpan.FromMilliseconds(timeout);
            return v.Start(waitForStartup, timespan);
        }

        public void Stop(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName) || !_instances.ContainsKey(instanceName))
                throw new ArgumentException(string.Format("No instance with key {0} found", instanceName));

            Virtuoso v = _instances[instanceName];
            v.Stop();
        }

        public void CreateInstance(string instanceName)
        {
            DirectoryInfo databaseDir = new DirectoryInfo(Path.Combine(InstanceCollectionDir.FullName, instanceName));
            if (databaseDir.Exists)
                throw new ArgumentException(string.Format("A database with the given name {0} exists already.", instanceName));

            databaseDir.Create();
            FileInfo targetConfig = new FileInfo(Path.Combine(databaseDir.FullName, "virtuoso.ini"));
            _templateConfig.CopyTo(targetConfig.FullName);

            InitInstance(databaseDir);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootDir"></param>
        /// <returns></returns>
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

        private DirectoryInfo GetTargetBinPath()
        {
            return new DirectoryInfo(Path.Combine(RootDir.FullName, "virtuoso"));
        }

        private DirectoryInfo GetInstanceCollectionDir()
        {
            var instanceCollectionDir = new DirectoryInfo(Path.Combine(RootDir.FullName, "databases"));
            return instanceCollectionDir;
        }

        private void SetupVirtuoso()
        {
            DirectoryInfo sourcePath = CurrentDir;

            if (TargetBinPath.Exists)
                TargetBinPath.Delete(true);

            DirectoryUtils.DirectoryCopy(sourcePath.FullName, TargetBinPath.FullName, true);
            CheckVirtuoso();
        }

        private bool CheckVirtuoso()
        {
            DirectoryInfo sourcePath = CurrentDir;
            int charCount = sourcePath.FullName.Count() + 1;

            foreach (var x in sourcePath.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string rel = x.FullName.Remove(0, charCount);
                FileInfo f = new FileInfo(Path.Combine(TargetBinPath.FullName, rel));
                if (!f.Exists)
                    return false;

                if (f.Name == "virtuoso-t.exe")
                {
                    _executable = f;
                }
            }

            return true;
        }

        private void LoadExistingInstances()
        {
            foreach (var db in InstanceCollectionDir.GetDirectories())
            {
                InitInstance(db);
            }
        }

        private void InitInstance(DirectoryInfo instanceDir)
        {
            string dbName = instanceDir.Name;
            FileInfo targetConfig = new FileInfo(Path.Combine(instanceDir.FullName, "virtuoso.ini"));

            Virtuoso virt = new Virtuoso(_executable, targetConfig);
            virt.EnvironmentDir = TargetBinPath;
            int port;
            do
            {
                port = 1200 + _rnd.Next(10, 60);
            } while (!PortUtils.TestPort(port));

            virt.Configuration.Parameters.ServerPort = string.Format("localhost:{0}", port);
            virt.Configuration.SaveConfigFile();

            virt.EnvironmentDir = TargetBinPath;
            _instances.Add(dbName, virt);

        }
        #endregion

        #endregion
    }
}