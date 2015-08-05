// LICENSE:
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

using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Semiodesk.VirtuosoInstrumentation.Configuration
{
    /// <summary>
    /// Taken from http://docs.openlinksw.com/virtuoso/databaseadmsrv.html
    /// </summary>
    /// <see href="http://docs.openlinksw.com/virtuoso/databaseadmsrv.html"/>
    public class VirtuosoConfig
    {
        #region Members
        FileInfo _configFile;
        IniData _data;

        public Database Database { get; private set; }
        public TempDatabase TempDatabase { get; private set; }
        public Parameters Parameters { get; private set; }
        public Sparql Sparql { get; private set; }

        private IniSectionWrapper[] _iniSections;

        private bool _locked = false;
        public bool Locked
        {
            get
            {
                return _locked;
            }
            internal set
            {
                _locked = value;
                foreach (var sec in _iniSections)
                    sec.Locked = value;

            }
        }
        #endregion

        #region Constructor
        public VirtuosoConfig(string file)
        {
            _configFile = new FileInfo(file);
            if( _configFile.Exists )
                LoadConfigFile();
        }

        public VirtuosoConfig(FileInfo file)
        {
            _configFile = file;
            if (file.Exists)
                LoadConfigFile();
            else
                CreateNew();
        }

        #endregion

        #region Methods
        private void LoadConfigFile()
        {
            FileIniDataParser parser = new FileIniDataParser();
            _data = parser.ReadFile(_configFile.FullName);
            Database = new Database(_data.Sections.GetSectionData("Database"));
            TempDatabase = new TempDatabase(_data.Sections.GetSectionData(Database.TempStorage));
            Parameters = new Parameters(_data.Sections.GetSectionData("Parameters"));

            _iniSections = new IniSectionWrapper[] { Database, TempDatabase, Parameters };
            
        }

        private void CreateNew()
        {
            _data = new IniData();
            Database = new Database(new SectionData("Database"));
            Database.TempStorage = "TempDatabase";
            TempDatabase = new TempDatabase(new SectionData(Database.TempStorage));
            Parameters = new Parameters(new SectionData("Parameters"));
            
            _iniSections = new IniSectionWrapper[] { Database, TempDatabase, Parameters };

            foreach (var sec in _iniSections)
                _data.Sections.Add(sec.SectionData);
        }

        public void SaveConfigFile()
        {
           
            FileIniDataParser parser = new FileIniDataParser();
            parser.WriteFile(_configFile.FullName, _data);
        }
        

        #endregion

    }




}
