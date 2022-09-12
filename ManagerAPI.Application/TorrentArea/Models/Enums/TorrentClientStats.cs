using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.Enum;
public enum TorrentClientStats
{
    SeedingSummary = 0,
    LeechingSummary = 1,
    PausedSummary = 2,
    UnregisteredSummary = 3,
    CategorySummary = 4,
    TrackerSummary = 5,
    DriveSummary = 6,
    UploadSpeedSummary = 7,
    DownloadSpeedSummary = 8,
    SeedingSizeSummary = 9,
    SessionRatioSummary = 10
}
