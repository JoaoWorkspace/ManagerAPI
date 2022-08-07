using ManagerAPI.Application.TorrentArea.Dtos.Enums;

namespace ManagerAPI.Request;

public class TorrentRequest
{

    public ManagedAction Action { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public ActionConnector ActionConnector { get; set; }
    public IFormFile Data { get; set; }
    public List<string> FolderOrFilePaths { get; set; } = new List<string>();
}
