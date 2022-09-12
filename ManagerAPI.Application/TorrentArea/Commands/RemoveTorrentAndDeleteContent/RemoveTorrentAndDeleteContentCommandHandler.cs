using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using QBittorrent.Client;
using System.Net;
using System.Web.Http;

namespace ManagerAPI.Application.TorrentArea.Commands.RemoveTorrentAndDeleteContent;

public class RemoveTorrentAndDeleteContentCommandHandler : IRequestHandler<RemoveTorrentAndDeleteContentCommand, Dictionary<string, string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public RemoveTorrentAndDeleteContentCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<Dictionary<string, string>> Handle(RemoveTorrentAndDeleteContentCommand request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        foreach (var torrent in request.TorrentsToDelete)
        {
            //If we choose to also delete torrent file, then it will only delete the torrent if that TorrentFile exists, otherwise, it'll just pass through regardless if we have or not a torrentFile to delete.
            if (torrent.TorrentFile != null || !request.AlsoDeleteTorrentFile)
            {
                await client.DeleteAsync(torrent.Hash, true, cancellationToken);
                
                if (request.AlsoDeleteTorrentFile)
                {
                    File.Delete(torrent.TorrentFile.FullPath);
                    result.Add($"{torrent.Hash}", $"The torrent {torrent.Name} and the torrentFile {torrent.TorrentFile.Name} in {torrent.TorrentFile.FullPath} was successfully removed and all it's contents permanently deleted.");
                }
                else
                {
                    result.Add($"{torrent.Hash}", $"The torrent {torrent.Name} was successfully removed and all it's contents permanently deleted.");
                }
            }
            //It only passes here if we chose to delete the torrentFile but it wasn't found in any of the directories provided.
            else
            {
                if (request.AlsoDeleteTorrentFile)
                {
                    result.Add($"{torrent.Hash}", $"The torrent {torrent.Name} did not have a TorrentFile on any of the given directories {request.FileOrFolderPaths}, which was set as a requirement before deleting, so it was skipped.");
                }
            }

        }
        return result;
    }
}
