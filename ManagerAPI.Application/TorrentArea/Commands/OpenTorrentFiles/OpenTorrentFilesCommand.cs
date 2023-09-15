using MediatR;

namespace ManagerAPI.Application.TorrentArea.Commands.OpenTorrentFiles;

public class OpenTorrentFilesCommand : IRequest<List<BencodeNET.Torrents.Torrent>>
{
    public List<string> FileOrFolderPaths { get; set; }

    public OpenTorrentFilesCommand(List<string> torrentsToAdd)
    {
        FileOrFolderPaths = torrentsToAdd;

    }
}
