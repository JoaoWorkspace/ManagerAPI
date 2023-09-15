using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea;
using ManagerApplication.FileArea.Models;
using QBittorrent.Client;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class DriveSummary
{
    private int TotalTorrentsCount { get; set; }
    private int TotalDriveCount { get; set; }
    private long TotalBytesInTorrentClient { get; set; }
    public string TotalSeedsizeInTorrentClient { get; set; }
    public string SummaryMessage { get; set; }
    private Dictionary<StorageDrive, List<TorrentInfo>> DriveTorrents { get; set; } = new();
    public Dictionary<StorageDrive, int> TorrentsPerDrive { get; set; } = new();
    public Dictionary<StorageDrive, string> SeedSizePerDrive { get; set; } = new();
    private Dictionary<StorageDrive, long> SeedSizeBytesPerDrive { get; set; } = new();
    public Dictionary<StorageDrive, Dictionary<string, string>> TorrentsByCategoryPerDrive { get; set; } = new();
    public Dictionary<StorageDrive, Dictionary<string, string>> TorrentsByTrackerPerDrive { get; set; } = new();
    public Dictionary<StorageDrive, Dictionary<string, string>> SeedSizeByCategoryPerDrive { get; set; } = new();
    public Dictionary<StorageDrive, Dictionary<string, string>> SeedSizeByTrackerPerDrive { get; set; } = new();
    private Dictionary<StorageDrive, List<string>> DriveTorrentHashes { get; set; } = new();

    public DriveSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<StorageDrive> allDrives, List<TorrentTrackerInfo> allTrackers)
    {
        TotalTorrentsCount = allTorrents.Count();
        TotalDriveCount = allDrives.Count();
        TotalBytesInTorrentClient = allTorrents.Sum(t => t.TotalSize) ?? 0L;
        TotalSeedsizeInTorrentClient = FileUtils.FileSizeFormatter(TotalBytesInTorrentClient);
        SummaryMessage = $"There are {TotalDriveCount} drives in your client holding the content of {TotalTorrentsCount} torrents, " +
            $"total of {TotalSeedsizeInTorrentClient}";

        SetTorrentsAndSeedsizePerDrive(allTorrents, allDrives);
        SetTorrentsByCategoryPerDrive(allCategories);
        SetTorrentsByTrackerPerDrive(allTrackers);
        SetSeedSizeByCategoryPerDrive(allCategories);
        SetSeedSizeByTrackerPerDrive(allTrackers);
    }

    public void SetTorrentsAndSeedsizePerDrive(List<TorrentInfo> allTorrents, List<StorageDrive> allDrives)
    {
        foreach (StorageDrive drive in allDrives)
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(drive.ToString());
                List<TorrentInfo> driveTorrents = allTorrents
                    .FindAll(torrent => torrent.SavePath.StartsWith(drive.ToString()))
                    .DistinctBy(torrent => new { torrent.Name, torrent.TotalSize })
                    .ToList();
                DriveTorrents.Add(drive, driveTorrents);
                TorrentsPerDrive.Add(drive, driveTorrents.Count());
                long totalSeededBytesInDrive = driveTorrents.Sum(t => t.CompletedSize) ?? 0L;
                SeedSizeBytesPerDrive.Add(drive, totalSeededBytesInDrive);
                SeedSizePerDrive.Add(drive, $"{FileUtils.FileSizeFormatter(totalSeededBytesInDrive)} " +
                    $"({string.Format("{0:n2}", (double.Parse(totalSeededBytesInDrive.ToString()) / double.Parse(TotalBytesInTorrentClient.ToString())) * 100.0)}%) " +
                    $"which is {string.Format("{0:n2}", (double.Parse(totalSeededBytesInDrive.ToString()) / double.Parse((driveInfo.TotalSize - driveInfo.AvailableFreeSpace).ToString())) * 100.0)}% " +
                    $"of the {FileUtils.FileSizeFormatter(driveInfo.TotalSize - driveInfo.AvailableFreeSpace)} occupied drive space, " +
                    $"leaving only {FileUtils.FileSizeFormatter(driveInfo.AvailableFreeSpace)} of space for new torrents");
                DriveTorrentHashes.Add(drive, driveTorrents.Select(torrent => torrent.Hash).ToList());
                SeedSizePerDrive = SeedSizePerDrive
                    .OrderByDescending(pair => TorrentUtils.GetInnerPercentage(pair.Value))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }catch(Exception ex)
            {
                ManagerApplicationConsole.BuildExceptionMessage($"There was an error getting a summary for drive {drive}", ex);
            }
        }
    }


    public void SetTorrentsByCategoryPerDrive(List<string> categories)
    {
        foreach (StorageDrive driveLetter in TorrentsPerDrive.Keys)
        {
            Dictionary<string, string> categorySummary = new Dictionary<string, string>();
            List<TorrentInfo> driveTorrents = DriveTorrents[driveLetter];
            foreach (string category in categories)
            {
                int categoryCount = driveTorrents.Count(t => t.Category == category);
                categorySummary[category] = $"{categoryCount} " +
                    $"({string.Format("{0:n2}", (double.Parse(categoryCount.ToString())/double.Parse(TorrentsPerDrive[driveLetter].ToString()))*100.0)}%)";
            }
            TorrentsByCategoryPerDrive[driveLetter] = categorySummary
                .Where(pair => !pair.Value.StartsWith("0"))
                .OrderByDescending(pair => int.Parse(pair.Value.Split(" ")[0]))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    public void SetTorrentsByTrackerPerDrive(List<TorrentTrackerInfo> trackers)
    {
        foreach (StorageDrive driveLetter in TorrentsPerDrive.Keys)
        {
            Dictionary<string, string> trackerSummary = new();
            List<TorrentInfo> driveTorrents = DriveTorrents[driveLetter];
            foreach (string trackerSite in trackers.Select(tracker => tracker.Site).Where(tracker => !string.IsNullOrEmpty(tracker)))
            {
                int trackerCount = driveTorrents.Count(t => t.CurrentTracker.Contains(trackerSite));
                trackerSummary[trackerSite] = $"{trackerCount} " +
                    $"({string.Format("{0:n2}", (double.Parse(trackerCount.ToString())/double.Parse(TorrentsPerDrive[driveLetter].ToString()))*100.0)}%)";
            }
            TorrentsByTrackerPerDrive[driveLetter] = trackerSummary
                .Where(pair => !pair.Value.StartsWith("0"))
                .OrderByDescending(pair => int.Parse(pair.Value.Split(" ")[0]))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    /// <summary>
    /// The Regex will match the % number inside parenthesis (x%) and order the inner dictionary.
    /// </summary>
    /// <param name="categories"></param>
    public void SetSeedSizeByCategoryPerDrive(List<string> categories)
    {
        foreach (StorageDrive driveLetter in TorrentsPerDrive.Keys)
        {
            Dictionary<string, string> categorySummary = new();
            List<TorrentInfo> driveTorrents = DriveTorrents[driveLetter];
            foreach (string category in categories)
            {
                long categorySize = driveTorrents.Where(torrent => torrent.Category == category).Sum(torrent => torrent.CompletedSize) ?? 0L;
                categorySummary[category] = $"{FileUtils.FileSizeFormatter(categorySize)} " +
                    $"({string.Format("{0:n2}", (double.Parse(categorySize.ToString())/double.Parse(SeedSizeBytesPerDrive[driveLetter].ToString()))*100.0)}%)";
            }
            SeedSizeByCategoryPerDrive[driveLetter] = categorySummary
                .Where(pair => !pair.Value.StartsWith("0"))
                .OrderByDescending(pair => TorrentUtils.GetInnerPercentage(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    public void SetSeedSizeByTrackerPerDrive(List<TorrentTrackerInfo> trackers)
    {
        foreach (StorageDrive driveLetter in TorrentsPerDrive.Keys)
        {
            Dictionary<string, string> trackerSummary = new();
            List<TorrentInfo> driveTorrents = DriveTorrents[driveLetter];
            foreach (string trackerSite in trackers.Select(tracker => tracker.Site).Where(tracker => !string.IsNullOrEmpty(tracker)))
            {
                long trackerSize = driveTorrents.Where(torrent => torrent.CurrentTracker.Contains(trackerSite)).Sum(torrent => torrent.CompletedSize) ?? 0L;
                trackerSummary[trackerSite] = $"{FileUtils.FileSizeFormatter(trackerSize)} " +
                    $"({string.Format("{0:n2}", (double.Parse(trackerSize.ToString()) / double.Parse(SeedSizeBytesPerDrive[driveLetter].ToString()))*100.0)}%)";
            }
            SeedSizeByTrackerPerDrive[driveLetter] = trackerSummary
                .Where(pair => !pair.Value.StartsWith("0"))
                .OrderByDescending(pair => TorrentUtils.GetInnerPercentage(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    public List<string> GetDriveTorrentHashes(StorageDrive drive)
    {
        if (DriveTorrentHashes.ContainsKey(drive))
        {
            return DriveTorrentHashes[drive];
        }
        else
        {
            return new();
        }
    }
}
