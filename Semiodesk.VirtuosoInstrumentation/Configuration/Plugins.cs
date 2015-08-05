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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation.Configuration
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
