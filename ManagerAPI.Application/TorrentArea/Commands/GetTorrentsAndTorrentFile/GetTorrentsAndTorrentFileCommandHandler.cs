using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Application.TorrentArea.Models.Enums;
using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;

public class GetTorrentsAndTorrentFileCommandHandler : IRequestHandler<GetTorrentsAndTorrentFileCommand, List<SimpleTorrentInfo>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public GetTorrentsAndTorrentFileCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<List<SimpleTorrentInfo>> Handle(GetTorrentsAndTorrentFileCommand request, CancellationToken cancellationToken)
    {
        var filteredTorrents =  await GetFilteredQbitTorrents(request, cancellationToken);
        var torrentFiles = TorrentUtils.GetAllTorrentFilesFromTorrentDirectoryList(request.FileOrFolderPaths);
        TorrentUtils.MatchQbitTorrentsWithFileTorrents(filteredTorrents, torrentFiles);
        return filteredTorrents;
    }

    public async Task<List<SimpleTorrentInfo>> GetFilteredQbitTorrents(GetTorrentsAndTorrentFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> allTorrents = new();
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Hashes = request.Hashes, SortBy = "category" }, cancellationToken);
            return TorrentUtils.SimplifyTorrentInfo(torrentList.ToList(), request.CategoryName);
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetLessDetailedTorrentCommandHandler.GetQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }




    

    
}
