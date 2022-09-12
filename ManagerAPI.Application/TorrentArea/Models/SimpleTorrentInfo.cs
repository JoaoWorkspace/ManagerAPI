using ManagerAPI.Application.FileArea.Models;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Models;
public class SimpleTorrentInfo
{
    public string Hash { get; set; }
    public string Name { get; set; }
    public string Tracker { get; set; }
    public string Category { get; set; }
    public string DestinationFolder { get; set; }
    public FileOrFolder? TorrentFile { get; set; }
    public FileOrFolder? ContentFolderOrFile { get; set; }
    public SimpleTorrentInfo(string hash, string name, string tracker, string category, string destinationFolder)
    {
        Hash = hash;
        Name = name;
        Tracker = TorrentUtils.TransformTrackerURL(tracker).Site;
        Category = category;
        DestinationFolder = destinationFolder;
    }
}
