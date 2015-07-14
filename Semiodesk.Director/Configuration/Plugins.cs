﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
{
    public class Plugins
    {
        /// <summary>
        /// LoadPath = /home/virtuoso/hosting
        /// The directory containing shared objects/libraries for use as Virtuoso VSEI plugins.
        /// </summary>
        public DirectoryInfo LoadPath { get; set; }

        /// <summary>
        /// Load<number> = <module type>, <module name>
        /// <number> is the module load number, required and starting with 1. <module type> specifies the type of module that is to be loaded, and hence how Virtuoso is to use it. So far "Hosting" and "attach" types exist. <module name> is the file name of the modules shared library or object.
        /// Example:
        /// Load6 = attach, libphp5.so)
        /// Load7 = Hosting, hosting_php.so)
        /// "Attach" is used for now for the php library. It can be used to load other libraries in future too. The reason is to load PHP5 functionality into virtuoso namespace, so when actually is loaded the hosting plugin, it can bind to the already available symbols for php5.
        /// </summary>
        public List<string> Load { get; set; }
    }
 
}
