namespace ManagerAPI.Settings
{
    public class CacheSettings
    {
        public static string SectionKey = "CacheSettings";
        public string Folder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
    }

    
}
