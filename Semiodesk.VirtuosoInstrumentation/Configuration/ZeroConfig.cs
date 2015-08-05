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
using System.Linq;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation.Configuration
{

    public class ZeroConfig
    {
        /// <summary>
        /// ServerName
        /// Name used to advertise the Virtuoso ODBC service details in ZeroConfig. This is the name that will be shown to clients amongst other ZeroConfig datasources.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// ServerDSN
        /// An ODBC style connect string to preset the values of the parameters when the ODBC service offered by this server is selected by the Virtuoso ZeroConfig enabled clients.
        /// </summary>
        public string ServerDSN { get; set; }

        /// <summary>
        /// SSLServerName
        /// Name used to advertise the Virtuoso ODBC SSL encrypted service details in ZeroConfig.
        /// </summary>
        public string SSLServerName { get; set; }

        /// <summary>
        /// SSLServerDSN
        /// An ODBC style connect string to preset the values of the parameters when the ODBC SSL encrypted service offered by this server is selected by the Virtuoso ZeroConfig enabled clients.
        /// </summary>
        public string SSLServerDSN { get; set; }
    }

}
