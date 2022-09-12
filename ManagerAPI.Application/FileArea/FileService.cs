using ManagerAPI.Application.FileArea.Commands.GetTorrentsFromFolder;
using ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.FileArea.Commands.CreateFolderJson;

using ManagerAPI.Application.FileArea.Models;
using MediatR;
using QBittorrent.Client;

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

    public async Task<FileOrFolder> CreateFolderJson(CreateFolderJsonCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<List<string>> GetTorrentsFromFolder(GetTorrentsFromFolderCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
}
