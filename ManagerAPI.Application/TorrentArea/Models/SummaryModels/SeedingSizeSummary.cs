using ManagerAPI.Application.FileArea;
using ManagerApplication.FileArea.Models;
using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class SeedingSizeSummary
{
    private long TotalBytesInTorrentClient { get; set; }
    public string TotalSeedsizeInTorrentClient { get; set; }
    public Dictionary<StorageDrive, string> SeedSizePerDrive { get; set; } = new();
    private Dictionary<StorageDrive, long> SeedSizeBytesPerDrive { get; set; } = new();
    public Dictionary<string, string> SeedSizeByCategory { get; set; } = new();
    public Dictionary<string, string> SeedSizeByTracker { get; set; } = new();
    public SeedingSizeSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<StorageDrive> allDrives, List<TorrentTrackerInfo> allTrackers)
    {
        TotalBytesInTorrentClient = allTorrents.Sum(t => t.TotalSize) ?? 0L;
        TotalSeedsizeInTorrentClient = FileUtils.FileSizeFormatter(TotalBytesInTorrentClient);
        SetSeedsizePerDrive(allTorrents, allDrives);
    }

    public void SetSeedsizePerDrive(List<TorrentInfo> allTorrents, List<StorageDrive> allDrives)
    {

        foreach (StorageDrive drive in allDrives)
        {
            DriveInfo driveInfo = new DriveInfo(drive.ToString());
            List<TorrentInfo> driveTorrents = allTorrents
                .FindAll(torrent => torrent.SavePath.StartsWith(drive.ToString()))
                .DistinctBy(torrent => new { torrent.Name, torrent.TotalSize })
                .ToList();
            long totalSeededBytesInDrive = driveTorrents.Sum(t => t.CompletedSize) ?? 0L;
            SeedSizeBytesPerDrive.Add(drive, totalSeededBytesInDrive);
            SeedSizePerDrive.Add(drive, $"{FileUtils.FileSizeFormatter(totalSeededBytesInDrive)} " +
                $"({string.Format("{0:n2}", (double.Parse(totalSeededBytesInDrive.ToString()) / double.Parse(TotalBytesInTorrentClient.ToString())) * 100.0)}%)");
        }
        SeedSizePerDrive = SeedSizePerDrive
            .OrderByDescending(pair => TorrentUtils.GetInnerPercentage(pair.Value))
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    /// <summary>
    /// The Regex will match the % number inside parenthesis (x%) and order the inner dictionary.
    /// </summary>
    /// <param name="categories"></param>
    public void SetSeedSizeByCategory(List<TorrentInfo> allTorrents, List<string> categories)
    {
        foreach (string category in categories)
        {
            long categorySize = allTorrents.Where(torrent => torrent.Category == category).Sum(torrent => torrent.CompletedSize) ?? 0L;
            SeedSizeByCategory[category] = $"{FileUtils.FileSizeFormatter(categorySize)} " +
                $"({string.Format("{0:n2}", (double.Parse(categorySize.ToString()) / double.Parse(TotalBytesInTorrentClient.ToString())) * 100.0)}%)";
        }
    }

    public void SetSeedSizeByTrackerPerDrive(List<TorrentInfo> allTorrents, List<TorrentTrackerInfo> trackers)
    {
        foreach (string trackerSite in trackers.Select(tracker => tracker.Site))
        {
            long trackerSize = allTorrents.Where(torrent => torrent.CurrentTracker.Contains(trackerSite)).Sum(torrent => torrent.CompletedSize) ?? 0L;
            SeedSizeByCategory[trackerSite] = $"{FileUtils.FileSizeFormatter(trackerSize)} " +
                $"({string.Format("{0:n2}", (double.Parse(trackerSize.ToString()) / double.Parse(TotalBytesInTorrentClient.ToString())) * 100.0)}%)";
        }
    }
}