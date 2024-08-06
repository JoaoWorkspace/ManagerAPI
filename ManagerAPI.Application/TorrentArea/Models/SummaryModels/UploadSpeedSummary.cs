using ManagerAPI.Application.MusicArea;
using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class UploadSpeedSummary
{
    public Dictionary<string, string> TorrentsUploadSpeed { get; set; } = new();
    public UploadSpeedSummary(List<TorrentInfo> allTorrents)
    {
        allTorrents.Where(torrent => torrent.UploadSpeed > 0)
            .OrderByDescending(torrent => torrent.UploadSpeed).ToList()
            .ForEach(torrent => {
                TorrentsUploadSpeed[torrent.Name] = $"{FileUtils.FileSizeFormatter(torrent.UploadSpeed)}/s";
                });
    }
}
