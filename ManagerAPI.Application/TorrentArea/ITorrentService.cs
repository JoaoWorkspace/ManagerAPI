using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Dtos;
using Newtonsoft.Json.Linq;

namespace ManagerAPI.Application.TorrentArea;

public interface ITorrentService
{
    Task<FolderDto> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken);
    //Task ProcessTorrentCommand(TorrentCommand command, CancellationToken cancellationToken);
}
