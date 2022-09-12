using AutoMapper;
using Cqrs.Domain;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;

public class GetDetailedTorrentCommandHandler : IRequestHandler<GetDetailedTorrentCommand, List<TorrentInfo>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public GetDetailedTorrentCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<List<TorrentInfo>> Handle(GetDetailedTorrentCommand request, CancellationToken cancellationToken)
    {
        var torrents = await GetQbitTorrents(request, cancellationToken);
        return torrents;
    }

    public async Task<List<TorrentInfo>> GetQbitTorrents(GetDetailedTorrentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> allTorrents = new();
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Hashes = request.Hashes, SortBy="category" }, cancellationToken);
            return torrentList.ToList();
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetDetailedTorrentCommandHandler.GetQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }
}
