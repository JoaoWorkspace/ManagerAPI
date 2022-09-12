using AutoMapper;
using Cqrs.Domain;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;

public class GetUnregisteredTorrentCommandHandler : IRequestHandler<GetUnregisteredTorrentCommand, List<string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public GetUnregisteredTorrentCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<List<string>> Handle(GetUnregisteredTorrentCommand request, CancellationToken cancellationToken)
    {
        return await GetUnregisteredQbitTorrents(cancellationToken);
    }

    public async Task<List<string>> GetUnregisteredQbitTorrents(CancellationToken cancellationToken)
    {
        try
        {
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Filter = TorrentListFilter.Stalled }, cancellationToken);
            return torrentList.Where(ti => ti.CurrentTracker == String.Empty).Select(ti => ti.Hash).ToList();
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetUnregisteredTorrentCommandHandler.GetUnregisteredQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }
}
