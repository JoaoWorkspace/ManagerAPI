using AutoMapper;
using BencodeNET.Parsing;
using BencodeNET.Torrents;
using ManagerAPI.Application.ExceptionHandling;
using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.OpenTorrentFiles;

public class OpenTorrentFilesCommandHandler : IRequestHandler<OpenTorrentFilesCommand, List<BencodeNET.Torrents.Torrent>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    private readonly IBencodeParser torrentParser;
    public OpenTorrentFilesCommandHandler(   
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client,
        IBencodeParser torrentParser
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
        this.torrentParser = torrentParser;
    }

    public async Task<List<BencodeNET.Torrents.Torrent>> Handle(OpenTorrentFilesCommand request, CancellationToken cancellationToken)
    {
        List<BencodeNET.Torrents.Torrent> openedTorrents = new List<BencodeNET.Torrents.Torrent>();
        foreach (int batch in Enumerable.Range(0, request.FileOrFolderPaths.Count()))
        {
            var torrentFilePath = request.FileOrFolderPaths.Take(1).Single();
            request.FileOrFolderPaths.Remove(torrentFilePath);

            BencodeNET.Torrents.Torrent torrent = this.torrentParser.Parse<BencodeNET.Torrents.Torrent>(torrentFilePath);

            //torrent.Files.ForEach(file => file.FullPath); //Search in already mapped drive to confirm if exists?
            //
            openedTorrents.Add(torrent);
            Task print = Task.Run(() => ManagerApplicationConsole.WriteInformation("OpenTorrentFilesCommandHandler", $"Opened torrent #{batch+1}: {torrentFilePath}."));
            print.Wait();
        }
        return openedTorrents;
    }
}
