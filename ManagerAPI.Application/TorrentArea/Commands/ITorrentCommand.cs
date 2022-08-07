using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;

namespace ManagerAPI.Application.TorrentArea.Commands;

public interface ITorrentCommand
{
    public ManagedAction Action { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public ActionConnector ActionConnector { get; set; }
    public List<string> FileOrFolderPaths { get; set; }
    public BinaryData? Data { get; set; }
}
