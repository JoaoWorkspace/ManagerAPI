using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class LeechingSummary
{
    private int TotalTorrentsCount { get; set; }
    private int TotalLeechingCount { get; set; }
    public string SummaryMessage { get; set; }
    public Dictionary<string, string> LeechingByCategory { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> LeechingByTracker { get; set; } = new Dictionary<string, string>();
    private List<string> LeechingTorrentHashes { get; set; } = new List<string>();

    public LeechingSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<TorrentTrackerInfo> allTrackers)
    {
        var leechingTorrents = allTorrents.Where(t =>
            t.State.Equals(TorrentState.CheckingDownload)
            || t.State.Equals(TorrentState.ForcedDownload)
            || t.State.Equals(TorrentState.QueuedDownload)
            || t.State.Equals(TorrentState.StalledDownload)
            || t.State.Equals(TorrentState.Downloading)).ToList();
        TotalTorrentsCount = allTorrents.Count();
        TotalLeechingCount = leechingTorrents.Count();
        SummaryMessage = $"{TotalLeechingCount} ({(double)(TotalLeechingCount/TotalTorrentsCount)}%) of the {TotalTorrentsCount} torrents are being leeched";

        SetLeechingByCategory(allTorrents, leechingTorrents, allCategories);
        SetLeechingByTracker(allTorrents, leechingTorrents, allTrackers);
        LeechingTorrentHashes = leechingTorrents.Select(t => t.Hash).ToList();
    }

    public void SetLeechingByCategory(List<TorrentInfo> allTorrents, List<TorrentInfo> leechingTorrents, List<string> categories)
    {
        foreach (string category in categories)
        {
            int leechingCount = leechingTorrents.Count(t => t.Category == category);
            int categoryCount = allTorrents.Count(t => t.Category == category);
            LeechingByCategory[category] = $"{string.Format("{0:n2}", (double.Parse(leechingCount.ToString()) / double.Parse(categoryCount.ToString())) * 100.0)}%";
        }
        LeechingByCategory = LeechingByCategory
            .OrderBy(pair => pair.Key)
            .OrderByDescending(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetLeechingByTracker(List<TorrentInfo> allTorrents, List<TorrentInfo> leechingTorrents, List<TorrentTrackerInfo> trackers)
    {
        foreach (string trackerSite in trackers.Select(t => t.Site))
        {
            int leechingCount = leechingTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            int trackerCount = allTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            LeechingByTracker[trackerSite] = $"{string.Format("{0:n2}", (double.Parse(leechingCount.ToString()) / double.Parse(trackerCount.ToString())) * 100.0)}%";
        }
        LeechingByTracker = LeechingByTracker
            .OrderBy(pair => pair.Key)
            .OrderByDescending(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public List<string> GetLeechingTorrentHashes()
    {
        return LeechingTorrentHashes;
    }
}
