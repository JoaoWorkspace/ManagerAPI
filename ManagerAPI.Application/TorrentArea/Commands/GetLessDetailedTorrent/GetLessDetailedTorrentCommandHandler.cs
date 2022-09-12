using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;

public class GetLessDetailedTorrentCommandHandler : IRequestHandler<GetLessDetailedTorrentCommand, List<SimpleTorrentInfo>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public GetLessDetailedTorrentCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<List<SimpleTorrentInfo>> Handle(GetLessDetailedTorrentCommand request, CancellationToken cancellationToken)
    {
        return await GetQbitTorrents(request, cancellationToken);
    }

    public async Task<List<SimpleTorrentInfo>> GetQbitTorrents(GetLessDetailedTorrentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> allTorrents = new();
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Hashes = request.Hashes, SortBy = "category" }, cancellationToken);
            return SimplifyTorrentInfo(torrentList.ToList());
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetLessDetailedTorrentCommandHandler.GetQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }

    public List<SimpleTorrentInfo> SimplifyTorrentInfo(List<TorrentInfo> torrents)
    {
        List<SimpleTorrentInfo> simplifiedTorrents = new List<SimpleTorrentInfo>();
        foreach (var torrent in torrents)
        {
            simplifiedTorrents.Add(new SimpleTorrentInfo(torrent.Hash, torrent.Name, torrent.CurrentTracker, torrent.Category, torrent.SavePath));
        }
        return simplifiedTorrents;
    }
}
