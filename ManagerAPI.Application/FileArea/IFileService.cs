using ManagerAPI.Application.MusicArea.Commands;
using ManagerAPI.Application.MusicArea.Commands.CloseFileProcess;
using ManagerAPI.Application.MusicArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.MusicArea.Commands.CreateFolderJson;
using ManagerAPI.Application.MusicArea.Commands.GetFilesFromFolder;
using ManagerAPI.Application.MusicArea.Commands.OpenFile;
using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Application.MusicArea.Queries.GetAllProcessesUsingPath;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;
using System.Diagnostics;

namespace ManagerAPI.Application.MusicArea;

public interface IFileService
{
    Task<FileOrFolder> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken);
    Task<string> CreateFolderJson(CreateFolderJsonCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetFilesFromFolder(GetFilesFromFolderCommand command, CancellationToken cancellationToken);
    Task<ProcessStartInfo?> OpenFileAsync(OpenFileCommand command, CancellationToken cancellationToken);
    Task<Dictionary<string, List<RunningProcess>>> GetAllProcessesUsingPathAsync(GetAllProcessesUsingPathQuery query, CancellationToken cancellationToken);
    Task<bool> CloseFileProcessAsync(CloseFileProcessCommand command, CancellationToken cancellationToken);
}