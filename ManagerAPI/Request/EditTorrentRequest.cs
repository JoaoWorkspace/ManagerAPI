namespace ManagerAPI.Request;

public class EditTorrentRequest
{
    public string NewCategory { get; set; } = string.Empty;
    public string NewDestinationFolder { get; set; } = string.Empty;
    public List<string> TorrentHashes { get; set; } = new List<string>();
}
