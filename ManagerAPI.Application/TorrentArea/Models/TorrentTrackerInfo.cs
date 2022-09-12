using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models;
public class TorrentTrackerInfo
{
    public string Site { get; set; }
    private string? Port { get; set; }
    private string? PersonalKey { get; set; }
    private string FullTrackerURL { get; set; }
    public TorrentTrackerInfo(string fullTrackerURL, string site, string? port, string? personalKey)
    {
        Site = site;
        Port = port;
        PersonalKey = personalKey;
        FullTrackerURL = fullTrackerURL;
    }
}
