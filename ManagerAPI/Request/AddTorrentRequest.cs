namespace ManagerAPI.Request;

public class AddTorrentRequest
{
    public List<string> Paths { get; set; } = new List<string>();
    public string Category { get; set; } = string.Empty;
    public string DestinationFolder { get; set; } = string.Empty;
    public bool StartTorrent { get; set; } = false; 

}
