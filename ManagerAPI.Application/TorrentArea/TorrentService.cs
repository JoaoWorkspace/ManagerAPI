using ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;
using ManagerAPI.Application.TorrentArea.Commands.EditTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;
using ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;
using ManagerAPI.Application.TorrentArea.Commands.RemoveTorrentAndDeleteContent;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Models;

using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea;

public class TorrentService : ITorrentService
{
    public IMediator mediator;
    public IQBittorrentClient client;
    public TorrentService(IMediator mediator, IQBittorrentClient client)
    {
        this.mediator = mediator;
        this.client = client;
    }

    public async Task<TorrentSummaryInfo> GetTorrentClientSummaryAsync(GetTorrentClientSummaryCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetAllActiveTorrents(SearchTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetAllInactiveTorrents(SearchTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetAllSeedingTorrents(SearchTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetAllPausedTorrents(SearchTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetAllErroredTorrents(SearchTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetUnregisteredTorrents(GetUnregisteredTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<TorrentInfo>> GetDetailedTorrents(GetDetailedTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
    public async Task<List<SimpleTorrentInfo>> GetTorrents(GetLessDetailedTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<SimpleTorrentInfo>> GetTorrentsAndTorrentFile(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<bool> AddTorrentsFromFile(AddTorrentsFromFileCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> EditCategoryForTorrents(EditTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> EditDestinationFolderForTorrents(EditTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> EditTorrentState(EditTorrentCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<Dictionary<string, string>> RemoveTorrentContent(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken)
    {
        var torrentsToDelete = await mediator.Send(command, cancellationToken);
        return await mediator.Send(new RemoveTorrentAndDeleteContentCommand(torrentsToDelete, false), cancellationToken);
    }

    public async Task<Dictionary<string, string>> RemoveTorrentContentAndTorrentFile(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken)
    {
        var torrentsToDelete = await mediator.Send(command, cancellationToken);
        return await mediator.Send(new RemoveTorrentAndDeleteContentCommand(torrentsToDelete, true), cancellationToken);
    }
}
