using ManagerAPI.Application.MusicArea;
using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class DownloadSpeedSummary
{
    public Dictionary<string, string> TorrentsDownloadSpeed { get; set; } = new();
    public DownloadSpeedSummary(List<TorrentInfo> allTorrents)
    {
        allTorrents.Where(torrent => torrent.DownloadSpeed > 0)
            .OrderByDescending(torrent => torrent.DownloadSpeed).ToList()
            .ForEach(torrent => {
                TorrentsDownloadSpeed[torrent.Name] = $"{FileUtils.FileSizeFormatter(torrent.DownloadSpeed)}/s";
            });
    }
}
