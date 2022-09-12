using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class UnregisteredSummary
{
    private int TotalTorrentsCount { get; set; }
    private int TotalUnregisteredCount { get; set; }
    public string SummaryMessage { get; set; }
    public Dictionary<string, string> UnregisteredByCategory { get; set; } = new Dictionary<string, string>();
    private List<string> UnregisteredTorrentHashes { get; set; } = new List<string>();

    public UnregisteredSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<TorrentTrackerInfo> allTrackers)
    {
        var unregisteredTorrents = allTorrents.Where(t =>
            t.State.Equals(TorrentState.StalledDownload)
            || t.State.Equals(TorrentState.StalledUpload)
            && t.CurrentTracker == String.Empty).ToList();
        TotalTorrentsCount = allTorrents.Count();
        TotalUnregisteredCount = unregisteredTorrents.Count();
        SummaryMessage = $"{TotalUnregisteredCount} ({(double)(TotalUnregisteredCount/TotalTorrentsCount)}%) of the {TotalTorrentsCount} torrents are unregistered";

        SetUnregisteredByCategory(allTorrents, unregisteredTorrents, allCategories);
        UnregisteredTorrentHashes = unregisteredTorrents.Select(t => t.Hash).ToList();
    }

    public void SetUnregisteredByCategory(List<TorrentInfo> allTorrents, List<TorrentInfo> unregisteredTorrents, List<string> categories)
    {
        Dictionary<string, string> summary = new Dictionary<string, string>();
        foreach (string category in categories)
        {
            int unregisteredCount = unregisteredTorrents.Count(t => t.Category == category);
            int categoryCount = allTorrents.Count(t => t.Category == category);
            UnregisteredByCategory[category] = $"{string.Format("{0:n2}", (double.Parse(unregisteredCount.ToString()) / double.Parse(categoryCount.ToString())) * 100.0)}%";
        }
        UnregisteredByCategory = UnregisteredByCategory
            .OrderBy(pair => pair.Key)
            .OrderByDescending(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public List<string> GetUnregisteredTorrentHashes()
    {
        return UnregisteredTorrentHashes;
    }
}
