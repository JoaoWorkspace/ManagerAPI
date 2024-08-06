using ManagerAPI.Application.MusicArea.Commands.OpenFile;
using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Application.TorrentArea.Commands.OpenTorrentFiles;

namespace ManagerAPI.Application.MusicArea;

public interface IMusicService
{
    Task<FileOrFolder> PlaySongAsync(OpenFileCommand openFileCommand, CancellationToken cancellationToken);
}