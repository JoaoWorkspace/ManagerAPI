using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class SeedingSummary
{
    private int TotalTorrentsCount { get; set; }
    private int TotalSeedingCount { get; set; }
    public string SummaryMessage { get; set; }
    public Dictionary<string, string> SeedingByCategory { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> SeedingByTracker { get; set; } = new Dictionary<string, string>();
    private List<string> SeedingTorrentHashes { get; set; } = new List<string>();

    public SeedingSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<TorrentTrackerInfo> allTrackers)
    {
        var seedingTorrents = allTorrents.Where(t =>
            t.State.Equals(TorrentState.CheckingUpload)
            || t.State.Equals(TorrentState.ForcedUpload)
            || t.State.Equals(TorrentState.QueuedUpload)
            || t.State.Equals(TorrentState.StalledUpload)
            || t.State.Equals(TorrentState.Uploading)).ToList();
        TotalTorrentsCount = allTorrents.Count();
        TotalSeedingCount = seedingTorrents.Count();
        SummaryMessage = $"{TotalSeedingCount} " +
            $"({string.Format("{0:n2}", (double.Parse(TotalSeedingCount.ToString()) / double.Parse(TotalTorrentsCount.ToString())) * 100.0)}%) of the {TotalTorrentsCount} torrents are being seeded";

        SetSeedingByCategory(allTorrents, seedingTorrents, allCategories);
        SetSeedingByTracker(allTorrents, seedingTorrents, allTrackers);
        SeedingTorrentHashes = seedingTorrents.Select(t => t.Hash).ToList();
    }

    public void SetSeedingByCategory(List<TorrentInfo> allTorrents, List<TorrentInfo> seedingTorrents, List<string> categories)
    {
        foreach (string category in categories)
        {
            int seedingCount = seedingTorrents.Count(t => t.Category == category);
            int categoryCount = allTorrents.Count(t => t.Category == category);
            SeedingByCategory[category] = $"{string.Format("{0:n2}", (double.Parse(seedingCount.ToString()) / double.Parse(categoryCount.ToString())) * 100.0)}%";
        }
        SeedingByCategory = SeedingByCategory
            .OrderBy(pair => pair.Key)
            .OrderBy(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetSeedingByTracker(List<TorrentInfo> allTorrents, List<TorrentInfo> seedingTorrents, List<TorrentTrackerInfo> trackers)
    {
        foreach (string trackerSite in trackers.Select(t => t.Site))
        {
            int seedingCount = seedingTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            int trackerCount = allTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
            SeedingByTracker[trackerSite] = $"{string.Format("{0:n2}", (double.Parse(seedingCount.ToString()) / double.Parse(trackerCount.ToString())) * 100.0)}%";
        }
        SeedingByTracker = SeedingByTracker
            .OrderBy(pair => pair.Key)
            .OrderBy(pair => double.Parse(pair.Value.Trim('%')))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public List<string> GetLeechingTorrentHashes()
    {
        return SeedingTorrentHashes;
    }
}
