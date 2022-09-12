using ManagerAPI.Application.TorrentArea.Dtos.SummaryModels;
using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models;
public class TorrentSummaryInfo
{
    public SeedingSummary? SeedingSummary { get; set; }
    public LeechingSummary? LeechingSummary { get; set; }
    public PausedSummary? PausedSummary { get; set; }
    public UnregisteredSummary? UnregisteredSummary { get; set; }
    public CategorySummary? CategorySummary { get; set; }
    public TrackerSummary? TrackerSummary { get; set; }
    public DriveSummary? DriveSummary { get; set; }
    public UploadSpeedSummary? UploadSpeedSummary { get; set; }
    public DownloadSpeedSummary? DownloadSpeedSummary { get; set; }
    public SeedingSizeSummary? SeedingSizeSummary { get; set; }
    public SessionRatioSummary? SessionRatioSummary { get; set; }
    public TorrentSummaryInfo()
    {
    }
}
