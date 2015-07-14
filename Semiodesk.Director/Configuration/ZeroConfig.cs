using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
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
