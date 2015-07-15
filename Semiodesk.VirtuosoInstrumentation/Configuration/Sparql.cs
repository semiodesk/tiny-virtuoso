using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Semiodesk.VirtuosoInstrumentation.Configuration
{
    /// <summary>
    /// The SPARQL section sets parameters and limits for SPARQL query protocol web service service. This section should stay commented out as long as SPARQL is not in use.
    /// Section RDF Data Access and Data Management contains detailed description of this functionality.
    /// </summary>
    public class Sparql
    {

        /// <summary>
        /// This controls processing of the "query-uri" parameter of the SPARQL query protocol webservice, means enable 1 or prohibited 0.
        /// </summary>
        public bool? ExternalQuerySource { get; set; }

        /// <summary>
        /// MinExpiration = 86400
        /// Sponger caching parameter in seconds. It will cause sponger to use this value as minimal expiration of the pages, which would help in cases where source document's server do not report expiration or it reports no caching at all.
        /// </summary>
        public int? MinExpiration { get; set; }

        /// <summary>
        /// Cache Expiration time in seconds that overrides Sponger's default cache invalidation.
        /// </summary>
        public int? MaxCacheExpiration { get; set; }

        /// <summary>
        /// MaxDataSourceSize = 20971520
        /// Controls the max size that can be sponged. Default is 20 MB.
        /// </summary>
        public int? MaxDataSourceSize { get; set; }

        /*
        ExternalXsltSource = 1 or 0

        This controls processing of the "xslt-uri" parameter of the SPARQL query protocol webservice, means enable 1 or prohibited 0.
        ResultSetMaxRows = number

        This setting is used to limit the number of the rows in the result. The effective limit will be the lowest of this setting, SPARQL query 'LIMIT' clause value (if present), and SPARQL Endpoint request URI &maxrows parameter value (if present).
        DefaultGraph = IRI

        IRI of the default graph to be used if no "default-graph-uri" parameter is specified.
        MaxQueryCostEstimationTime = seconds

        This setting is used to limit the estimate time cost of the query to certain number of seconds, the default is no limit.
        MaxQueryExecutionTime = seconds

        This setting is used to set the transaction execution timeout to certain limit in number of seconds, the default is no limit.
        ImmutableGraphs = URI

        IRI of graphs over which the sponger not to be able able to write.
        PingService = URI

        IRI of notification service to which the sponger results will be send.
        DefaultQuery = SPARQL Query

        Default SPARQL Query.
        DeferInferenceRulesInit = 1

        Defer Loading of inference rules at start up.
        ShortenLongURIs = 1

        Shorten extremely long URIs in datasets when loading with the RDF Bulk Loader. Default is 0.

        Note: This parameter is only in the Virtuoso 06.03.3131+ commercial builds, at the time of writing it is not included in the open source 6.1.4 archives but will be in the next 6.1.5 release. A patch to enable this feature is however available from the Virtuso patches page on source forge, which can be applied to a 6.1.4 archive from source forge and the Virtuoso server binary rebuilt.
        MaxMemInUse = 0

        Maximum amount of memory that is allowed for temporary storing parts of a SPARQL query result. Default is zero for no limit. ResultSetMaxRows is maximum length of a SPARQL query result. Default is zero for no limit. These two options may be useful for protecting public web service endpoints against DOS attacks, but at the same time it may cause returning incomplete results without reporting errors. When used, it is strongly advised to set the value orders of magnitude larger than the expected size of longest reply. As a rule of thumb, timeout should happen before this limit has reached. Values less than 1000000 bytes are impractical in all cases.
    */
    }

}
