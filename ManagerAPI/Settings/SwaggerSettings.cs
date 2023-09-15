namespace ManagerAPI.Settings
{
    /// <summary>
    ///     The configuration model to setup Swagger-related Settings
    /// </summary>
    public class SwaggerSettings
    {
        /// <summary>
        ///     The static section-key in 'appsettings.json' to map to this model
        /// </summary>
        public readonly static string SectionKey = "SwaggerSettings";

        /// <summary>
        ///     Glag that adds styling to response application/json when set to true.
        ///     For optimization purposes, we leave it at false, large ResultSets may take minutes before being shown.
        ///     <code>Returns <returns><value>false</value> by default</returns></code>
        /// </summary>
        public bool UseSyntaxHighlight { get; set; } = false;
    }
}
