using ManagerAPI.Application.FileArea.Commands.GetFilesFromFolder;
using ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.FileArea.Commands.CreateFolderJson;

using ManagerAPI.Application.FileArea.Models;
using MediatR;
using QBittorrent.Client;
using System.Diagnostics;
using ManagerAPI.Application.FileArea.Commands.CloseFileProcess;
using ManagerAPI.Application.FileArea.Queries.GetAllProcessesUsingPath;
using ManagerAPI.Application.FileArea.Commands.OpenFile;

namespace ManagerAPI.Application.FileArea;

public class FileService : IFileService
{
    public IMediator mediator;
    public IQBittorrentClient client;
    public FileService(IMediator mediator, IQBittorrentClient client)
    {
        this.mediator = mediator;
        this.client = client;
    }

    public async Task<FileOrFolder> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<string> CreateFolderJson(CreateFolderJsonCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetFilesFromFolder(GetFilesFromFolderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<ProcessStartInfo?> OpenFileAsync(OpenFileCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<Dictionary<string, List<RunningProcess>>> GetAllProcessesUsingPathAsync(GetAllProcessesUsingPathQuery query, CancellationToken cancellationToken)
    {
        return await mediator.Send(query, cancellationToken);
    }

    public async Task<bool> CloseFileProcessAsync(CloseFileProcessCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }


}
