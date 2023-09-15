using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Models;
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
        foreach (int batch in Enumerable.Range(0, request.FileOrFolderPaths.Count()))
        {
            var torrent = request.FileOrFolderPaths.Take(1).Single();
            request.FileOrFolderPaths.Remove(torrent);
            AddTorrentFilesRequest addTorrentsRequest = new AddTorrentFilesRequest(torrent)
            {
                ContentLayout = TorrentContentLayout.Original,
                Category = request.Category,
                DownloadFolder = request.SavePath,
                Paused = !request.StartTorrent
            };
            Task addToClient = client.AddTorrentsAsync(addTorrentsRequest, cancellationToken);
            Task print = Task.Run(() => ManagerApplicationConsole.WriteInformation("AddTorrentsFromFileCommandHandler", $"Added torrent #{batch}: {torrent} to the Qbittorrent Client."));
            await Task.WhenAll(addToClient, print);
        }
        
        return true;
    }
}
