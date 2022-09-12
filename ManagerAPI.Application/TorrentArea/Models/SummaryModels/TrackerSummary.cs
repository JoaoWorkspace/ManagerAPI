using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class TrackerSummary
{
    public string SummaryMessage { get; set; }
    public Dictionary<string, int> TotalTorrentsByTracker { get; set; } = new();
    public Dictionary<string, string> TorrentsByTracker { get; set; } = new();
    private Dictionary<string, int> SeedingTorrentsByTracker { get; set; } = new();
    private Dictionary<string, int> LeechingTorrentsByTracker { get; set; } = new();
    private Dictionary<string, int> PausedTorrentsByTracker { get; set; } = new();
    private Dictionary<string, int> UnregisteredTorrentsByTracker { get; set; } = new();
    private Dictionary<string, List<string>> TrackerTorrentHashes { get; set; } = new();

    public TrackerSummary(List<TorrentInfo> allTorrents, List<TorrentTrackerInfo> allTrackers)
    {
        SetTorrentsByTracker(allTorrents, allTrackers);
        SummaryMessage = $"There are {allTrackers.Count()} categories holding {allTorrents.Count()} torrents " +
            $"with {(SeedingTorrentsByTracker.Values.Sum() / allTorrents.Count()) * 100.0} being seeded, " +
            $"{(LeechingTorrentsByTracker.Values.Sum() / allTorrents.Count()) * 100.0} being leeched " +
            $"{(PausedTorrentsByTracker.Values.Sum() / allTorrents.Count()) * 100.0} being leeched " +
            $"and {(UnregisteredTorrentsByTracker.Values.Sum() / allTorrents.Count()) * 100.0} being unregistered " +
            $"making {(SeedingTorrentsByTracker.Values.Sum() + LeechingTorrentsByTracker.Values.Sum() + PausedTorrentsByTracker.Values.Sum() + UnregisteredTorrentsByTracker.Values.Sum() / allTorrents.Count()) * 100.0} of the torrents being accounted for";
    }

    public void SetTorrentsByTracker(List<TorrentInfo> allTorrents, List<TorrentTrackerInfo> allTrackers)
    {
        foreach (string trackerSite in allTrackers.Select(tracker => tracker.Site).OrderBy(site => site))
        {
            var trackerTorrents = allTorrents.Where(torrent => torrent.CurrentTracker.Contains(trackerSite)).ToList();
            TotalTorrentsByTracker[trackerSite] = trackerTorrents.Count();
            var seedingTrackerTorrents = trackerTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.CheckingUpload)
                || torrent.State.Equals(TorrentState.ForcedUpload)
                || torrent.State.Equals(TorrentState.QueuedUpload)
                || torrent.State.Equals(TorrentState.StalledUpload)
                || torrent.State.Equals(TorrentState.Uploading)).ToList();
            SeedingTorrentsByTracker[trackerSite] = seedingTrackerTorrents.Count();
            var leechingTrackerTorrents = trackerTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.CheckingDownload)
                || torrent.State.Equals(TorrentState.ForcedDownload)
                || torrent.State.Equals(TorrentState.QueuedDownload)
                || torrent.State.Equals(TorrentState.StalledDownload)
                || torrent.State.Equals(TorrentState.Downloading)).ToList();
            LeechingTorrentsByTracker[trackerSite] = leechingTrackerTorrents.Count();
            var pausedTrackerTorrents = trackerTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.PausedDownload)
                || torrent.State.Equals(TorrentState.PausedUpload)).ToList();
            PausedTorrentsByTracker[trackerSite] = pausedTrackerTorrents.Count();
            var unregisteredTrackerTorrents = trackerTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.StalledDownload)
                || torrent.State.Equals(TorrentState.StalledUpload)
                && torrent.CurrentTracker == String.Empty).ToList();
            UnregisteredTorrentsByTracker[trackerSite] = unregisteredTrackerTorrents.Count();
            TorrentsByTracker[trackerSite] = $"{string.Format("{0:n2}", (double.Parse(SeedingTorrentsByTracker[trackerSite].ToString()) / double.Parse(TotalTorrentsByTracker[trackerSite].ToString())) * 100.0)}% seeding, " +
                $"{string.Format("{0:n2}", (double.Parse(LeechingTorrentsByTracker[trackerSite].ToString()) / double.Parse(TotalTorrentsByTracker[trackerSite].ToString())) * 100.0)}% leeching, " +
                $"{string.Format("{0:n2}", (double.Parse(PausedTorrentsByTracker[trackerSite].ToString()) / double.Parse(TotalTorrentsByTracker[trackerSite].ToString())) * 100.0)}% paused, " +
                $"{string.Format("{0:n2}", (double.Parse(UnregisteredTorrentsByTracker[trackerSite].ToString()) / double.Parse(TotalTorrentsByTracker[trackerSite].ToString())) * 100.0)}% unregistered";
            TrackerTorrentHashes[trackerSite] = trackerTorrents.Select(torrent => torrent.Hash).ToList();
        }
    }

    public List<string> GetTrackerTorrentHashes(string trackerSite)
    {
        if (TrackerTorrentHashes.ContainsKey(trackerSite))
        {
            return TrackerTorrentHashes[trackerSite];
        }
        else
        {
            return new();
        }
    }
}
