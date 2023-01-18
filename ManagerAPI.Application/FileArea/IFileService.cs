using ManagerAPI.Application.FileArea.Commands;
using ManagerAPI.Application.FileArea.Commands.CloseFileProcess;
using ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.FileArea.Commands.CreateFolderJson;
using ManagerAPI.Application.FileArea.Commands.GetFilesFromFolder;
using ManagerAPI.Application.FileArea.Commands.OpenFile;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.FileArea.Queries.GetAllProcessesUsingPath;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;
using System.Diagnostics;

namespace ManagerAPI.Application.FileArea;

public interface IFileService
{
    Task<FileOrFolder> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken);
    Task<string> CreateFolderJson(CreateFolderJsonCommand command, CancellationToken cancellationToken);
    Task<List<string>> GetFilesFromFolder(GetFilesFromFolderCommand command, CancellationToken cancellationToken);
    Task<ProcessStartInfo?> OpenFileAsync(OpenFileCommand command, CancellationToken cancellationToken);
    Task<Dictionary<string, List<RunningProcess>>> GetAllProcessesUsingPathAsync(GetAllProcessesUsingPathQuery query, CancellationToken cancellationToken);
    Task<bool> CloseFileProcessAsync(CloseFileProcessCommand command, CancellationToken cancellationToken);
}