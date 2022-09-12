using System.ComponentModel;

namespace ManagerAPI.Request
{
    public class GetTorrentsAndTorrentFileRequest
    {
        public List<string> Hashes { get; set; }
        [DefaultValue("")]
        public string CategoryName { get; set; } = string.Empty;
        [DefaultValue(null)]
        public List<string>? TorrentDirectoryList { get; set; }

        public GetTorrentsAndTorrentFileRequest(List<string> hashes, string categoryName, List<string>? torrentDirectoryList)
        {
            Hashes = hashes;
            CategoryName = categoryName;
            TorrentDirectoryList = torrentDirectoryList;
        }
    }
}
