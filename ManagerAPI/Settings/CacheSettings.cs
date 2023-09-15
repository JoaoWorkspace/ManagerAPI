namespace ManagerAPI.Settings
{
    /// <summary>
    ///     The configuration model to setup Cache-related Settings
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        ///     The static section-key in 'appsettings.json' to map to this model
        /// </summary>
        public static string SectionKey = "CacheSettings";
        /// <summary>
        ///     Defines the path to which it saves cache to.
        ///     <code>Returns <returns><value>AppDomain.CurrentDomain.BaseDirectory</value> by default</returns></code>
        /// </summary>
        public string Folder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
    }

    
}
