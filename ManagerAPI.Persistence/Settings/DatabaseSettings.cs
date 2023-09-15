namespace ManagerAPI.Persistence.Settings
{
    /// <summary>
    ///     The configuration model to setup Database-related Settings
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        ///     The static section-key in 'appsettings.json' to map to this model
        /// </summary>
        public static string SectionKey = "DatabaseSettings";

        /// <summary>
        ///     The configuration model to setup RavenDB-related Settings
        /// </summary>
        public RavenDB? RavenDB { get; set; }
    }
}
