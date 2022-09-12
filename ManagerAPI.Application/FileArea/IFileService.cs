using ManagerAPI.Application.FileArea.Commands;
using ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.FileArea.Commands.CreateFolderJson;
using ManagerAPI.Application.FileArea.Commands.GetTorrentsFromFolder;


using ManagerAPI.Application.FileArea.Models;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;

namespace ManagerAPI.Application.FileArea;

public interface IFileService
{
    Task<FileOrFolder> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken);
    Task<FileOrFolder> CreateFolderJson(CreateFolderJsonCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetTorrentsFromFolder(GetTorrentsFromFolderCommand command, CancellationToken cancellationToken);
}
