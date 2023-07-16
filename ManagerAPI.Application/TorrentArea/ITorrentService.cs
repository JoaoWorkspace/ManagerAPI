using ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;
using ManagerAPI.Application.TorrentArea.Commands.EditTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;
using ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Models;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea;

public interface ITorrentService
{
    Task<TorrentSummaryInfo> GetTorrentClientSummaryAsync(GetTorrentClientSummaryCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetAllActiveTorrents(SearchTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetAllInactiveTorrents(SearchTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetAllSeedingTorrents(SearchTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetAllPausedTorrents(SearchTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetAllErroredTorrents(SearchTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetUnregisteredTorrents(GetUnregisteredTorrentCommand command, CancellationToken cancellationToken);
    Task<List<TorrentInfo>> GetDetailedTorrents(GetDetailedTorrentCommand command, CancellationToken cancellationToken);
    Task<List<SimpleTorrentInfo>> GetTorrents(GetLessDetailedTorrentCommand command, CancellationToken cancellationToken);
    Task<List<SimpleTorrentInfo>> GetTorrentsAndTorrentFile(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken);
    Task<bool> AddTorrentsFromFile(AddTorrentsFromFileCommand command, CancellationToken cancellationToken);
    Task<List<string>> EditCategoryForTorrents(EditTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> EditDestinationFolderForTorrents(EditTorrentCommand command, CancellationToken cancellationToken);
    Task<List<string>> EditTorrentState(EditTorrentCommand command, CancellationToken cancellationToken);
    Task<Dictionary<string, string>> RemoveTorrentContent(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken);
    Task<Dictionary<string, string>> RemoveTorrentContentAndTorrentFile(GetTorrentsAndTorrentFileCommand command, CancellationToken cancellationToken);
}
