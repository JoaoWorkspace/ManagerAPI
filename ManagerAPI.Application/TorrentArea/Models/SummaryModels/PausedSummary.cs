using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class PausedSummary
{
    private int TotalTorrentsCount { get; set; }
    private int TotalPausedCount { get; set; }
    public string SummaryMessage { get; set; }
    public Dictionary<string, string> PausedByCategory { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> PausedByTracker { get; set; } = new Dictionary<string, string>();
    private List<string> PausedTorrentHashes { get; set; } = new List<string>();

    public PausedSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<TorrentTrackerInfo> allTrackers)
    {
        var pausedTorrents = allTorrents.Where(t =>
            t.State.Equals(TorrentState.PausedDownload)
            || t.State.Equals(TorrentState.PausedUpload)).ToList();
        TotalTorrentsCount = allTorrents.Count();
        TotalPausedCount = pausedTorrents.Count();
        SummaryMessage = $"{TotalPausedCount} ({(double)(TotalPausedCount/TotalTorrentsCount)}%) of the {TotalTorrentsCount} torrents are paused";

        SetPausedByCategory(allTorrents, pausedTorrents, allCategories);
        SetPausedByTracker(allTorrents, pausedTorrents, allTrackers);
        PausedTorrentHashes = pausedTorrents.Select(t => t.Hash).ToList();
    }

    public void SetPausedByCategory(List<TorrentInfo> allTorrents, List<TorrentInfo> pausedTorrents, List<string> categories)
    {
        foreach (string category in categories)
        {
            int pausedCount = pausedTorrents.Count(t => t.Category == category);
            int categoryCount = allTorrents.Count(t => t.Category == category);
            PausedByCategory[category] = $"{string.Format("{0:n2}", (double.Parse(pausedCount.ToString()) / double.Parse(categoryCount.ToString())) * 100.0)}%";
        }
        PausedByCategory = PausedByCategory
            .OrderBy(pair => pair.Key)
            .OrderByDescending(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetPausedByTracker(List<TorrentInfo> allTorrents, List<TorrentInfo> pausedTorrents, List<TorrentTrackerInfo> trackers)
    {
        foreach (string trackerSite in trackers.Select(t => t.Site))
        {
            int pausedCount = pausedTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            int trackerCount = allTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            PausedByTracker[trackerSite] = $"{string.Format("{0:n2}", (double.Parse(pausedCount.ToString()) / double.Parse(trackerCount.ToString())) * 100.0)}%";
        }
        PausedByTracker = PausedByTracker
            .OrderBy(pair => pair.Key)
            .OrderByDescending(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public List<string> GetPausedTorrentHashes()
    {
        return PausedTorrentHashes;
    }
}
