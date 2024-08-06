using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.MusicArea;
using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Application.TorrentArea.Models.Enum;
using ManagerAPI.Application.TorrentArea.Models.SummaryModels;
using ManagerApplication.FileArea.Models;
using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;

public class GetTorrentClientSummaryHandler : IRequestHandler<GetTorrentClientSummaryCommand, TorrentSummaryInfo>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public GetTorrentClientSummaryHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
        //ITorrentRepository torrentRepository
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<TorrentSummaryInfo> Handle(GetTorrentClientSummaryCommand request, CancellationToken cancellationToken)
    {
        var allTorrents = await client.GetTorrentListAsync(new TorrentListQuery { Filter = TorrentListFilter.All }, cancellationToken);
        var allCategories = allTorrents.Select(torrent => torrent.Category).Distinct().ToList();
        var allTrackers = allTorrents.Select(torrent => torrent.CurrentTracker).Distinct().ToList();
        var allTrackersInfo = allTrackers.Select(tracker => TorrentUtils.TransformTrackerURL(tracker)).ToList();
        var allDriveLetters = allTorrents.Select(torrent => FileUtils.ExtractDriveLetter(torrent.SavePath)).Distinct().ToList();
        var allDrives = new List<StorageDrive>();
        allDriveLetters.ForEach(driveLetter => allDrives.Add(FileUtils.ToStorageDrive(driveLetter).Value));
        return GetSummary(allTorrents.ToList(), allCategories, allDrives, allTrackersInfo, request, cancellationToken);
    }

    public TorrentSummaryInfo GetSummary(List<TorrentInfo> allTorrents, List<string> allCategories, List<StorageDrive> allDrives, 
        List<TorrentTrackerInfo> allTrackers, GetTorrentClientSummaryCommand request, CancellationToken cancellationToken)
    {
        TorrentSummaryInfo summary = new TorrentSummaryInfo();
        foreach(TorrentClientStats stats in request.ShowStats.Distinct())
        {
            switch (stats)
            {
                case TorrentClientStats.SeedingSummary:
                    summary.SeedingSummary = new(allTorrents, allCategories, allTrackers);
                    break;
                case TorrentClientStats.LeechingSummary:
                    summary.LeechingSummary = new(allTorrents, allCategories, allTrackers);
                    break;
                case TorrentClientStats.PausedSummary:
                    summary.PausedSummary = new(allTorrents, allCategories, allTrackers);
                    break;
                case TorrentClientStats.UnregisteredSummary:
                    summary.UnregisteredSummary = new(allTorrents, allCategories, allTrackers);
                    break;
                case TorrentClientStats.UploadSpeedSummary:
                    summary.UploadSpeedSummary = new(allTorrents);
                    break;
                case TorrentClientStats.DownloadSpeedSummary:
                    summary.DownloadSpeedSummary = new(allTorrents);
                    break;
                case TorrentClientStats.CategorySummary:
                    summary.CategorySummary = new(allTorrents, allCategories);
                    break;
                case TorrentClientStats.TrackerSummary:
                    summary.TrackerSummary = new(allTorrents, allTrackers);
                    break;
                case TorrentClientStats.DriveSummary:
                    summary.DriveSummary = new(allTorrents, allCategories, allDrives, allTrackers);
                    break;
                case TorrentClientStats.SeedingSizeSummary:
                    summary.SeedingSizeSummary = new(allTorrents, allCategories, allDrives, allTrackers);
                    break;
                case TorrentClientStats.SessionRatioSummary:
                    summary.SessionRatioSummary = new(allTorrents, allCategories, allDrives, allTrackers);
                    break;
            }
        }
        return summary;
    }
}
