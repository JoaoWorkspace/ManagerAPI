using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using QBittorrent.Client;
using System.Net;
using System.Web.Http;

namespace ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;

public class AddTorrentsFromFileCommandHandler : IRequestHandler<AddTorrentsFromFileCommand, bool>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public AddTorrentsFromFileCommandHandler(   
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<bool> Handle(AddTorrentsFromFileCommand request, CancellationToken cancellationToken)
    {
        AddTorrentFilesRequest addTorrentsRequest = new AddTorrentFilesRequest(request.FileOrFolderPaths)
        {
            ContentLayout = TorrentContentLayout.Original,
            Category = request.Category,
            DownloadFolder = request.SavePath,
            Paused = !request.StartTorrent
        };
        await client.AddTorrentsAsync(addTorrentsRequest, cancellationToken);
        return true;
    }
}
