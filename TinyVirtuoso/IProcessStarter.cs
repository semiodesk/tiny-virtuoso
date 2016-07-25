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

namespace Semiodesk.VirtuosoInstrumentation
{
    /// <summary>
    /// Start a process independent from the target platform
    /// </summary>
    public interface IProcessStarter: IDisposable
    {
        #region Members
        string Executable { get; set; }
        string Parameter { get; set; }
        bool ProcessRunning { get; }
        bool Started { get; }
        int Port { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Start the process
        /// </summary>
        /// <param name="waitOnStartup">If true, blocks until the process is started.</param>
        /// <param name="timeout">Timeout if blocking</param>
        /// <returns>Returns true if the process was started successfully</returns>
        bool Start(bool waitOnStartup = true, TimeSpan? timeout = null);

        /// <summary>
        /// Stops the process. If not responding, can also kill the process.
        /// </summary>
        /// <param name="force">Kills the process, if neccessary.</param>
        /// <returns></returns>
        bool Stop(bool force = false);
        #endregion

    }
}
