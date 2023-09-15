namespace ManagerAPI.Persistence.Settings
{
    /// <summary>
    ///    The configuration model to setup RavenDB-related Settings
    /// </summary>
    public class RavenDB
    {
        /// <summary>
        ///     This application has many different Database setups available to be configured as IdentityServer or DataServer
        ///     A flag is needed in order to filter off the databases we haven't setup on the local system
        ///     <code>Returns <returns><value>false</value> by default</returns></code>
        /// </summary>
        public bool IsSetup { get; private set; } = false;

        /// <summary>
        ///     URL of all the nodes present in the cluster (by default I'm calling my main one 'Main' with project name 'ManagerAPI')
        ///     <code>Returns <returns><value>{ "https://main.managerapi.ravendb.community/" }</value> by default</returns></code>
        /// </summary>
        public string[] ServerNodeUrls { get; private set; } = { "https://main.managerapi.ravendb.community/" };

        /// <summary>
        ///     The Database name
        ///     <code>Returns <returns><value>"IdentityDatabase"</value> by default</returns></code>
        /// </summary>
        public string DatabaseName { get; private set; } = "IdentityDatabase";

        /// <summary>
        ///     The path for Base64-encoded RavenDB certificate.
        ///		Certificate is NOT required for non-secure connections (e.g. a local instance)
        ///		<code>Returns <returns><value>null</value> by default</returns></code>
        /// </summary>
        public string Certificate { get; private set; }

        /// <summary>
        ///     Flag triggering updating the indexes if 'true'.
        ///		Ideally, it shouldn't be set in PROD as updating indexes is a migration concern,
        ///		but setting it in dev environment makes life a bit easier by applying index updates on a start-up
        ///		<code>Returns <returns><value>false</value> by default</returns></code>
        /// </summary>
        public bool UpdateIndexes { get; private set; } = false;

        /// <summary>
        ///     Defines the maximum number of requests per session.
        ///     <code>Returns <returns><value>30</value> by default</returns></code>
        /// </summary>
        public int MaxNumberOfRequestsPerSession { get; private set; } = 30;
    }
}
