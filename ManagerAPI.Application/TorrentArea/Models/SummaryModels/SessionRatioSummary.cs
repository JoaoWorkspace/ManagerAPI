using ManagerAPI.Application.FileArea;
using ManagerApplication.FileArea.Models;
using QBittorrent.Client;
using System;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class SessionRatioSummary
{
    public Dictionary<string, double> RatioPerCategory { get; set; } = new();
    public Dictionary<StorageDrive, double> RatioPerDrive { get; set; } = new();   
    public Dictionary<string, double> RatioPerTracker { get; set; } = new();
    public SessionRatioSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<StorageDrive> allDrives, List<TorrentTrackerInfo> allTrackers)
    {
        SetRatioPerCategory(allTorrents, allCategories);
        SetRatioPerDrive(allTorrents, allDrives);
        SetRatioPerTracker(allTorrents, allTrackers);
    }

    public void SetRatioPerCategory(List<TorrentInfo> allTorrents, List<string> allCategories)
    {
        foreach (string category in allCategories)
        {
            List<TorrentInfo> categoryTorrents = allTorrents
                .FindAll(torrent => torrent.Category.Equals(category))
                .ToList();
            RatioPerCategory[category] = Math.Round(categoryTorrents.Select(torrent => torrent.Ratio).Sum(), 2);
        }
    }

    public void SetRatioPerDrive(List<TorrentInfo> allTorrents, List<StorageDrive> allDrives)
    {
        foreach (StorageDrive drive in allDrives)
        {
            List<TorrentInfo> driveTorrents = allTorrents
                .FindAll(torrent => torrent.SavePath.StartsWith(drive.ToString()))
                .ToList();
            RatioPerDrive[drive] = Math.Round(driveTorrents.Select(torrent => torrent.Ratio).Sum(), 2);
        }
    }

    public void SetRatioPerTracker(List<TorrentInfo> allTorrents, List<TorrentTrackerInfo> allTrackers)
    {
        foreach (string trackerSite in allTrackers.Select(tracker => tracker.Site))
        {
            List<TorrentInfo> trackerTorrents = allTorrents
                .FindAll(torrent => torrent.CurrentTracker.Contains(trackerSite))
                .ToList();
            RatioPerTracker[trackerSite] = Math.Round(trackerTorrents.Select(torrent => torrent.Ratio).Sum(), 2);
        }
    }
}
