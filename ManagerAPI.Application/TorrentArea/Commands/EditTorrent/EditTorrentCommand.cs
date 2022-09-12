using ManagerAPI.Application.TorrentArea.Dtos.Enums;

namespace ManagerAPI.Application.TorrentArea.Commands.EditTorrent;

public class EditTorrentCommand : TorrentCommand<List<string>>
{
    public List<string> Hashes { get; set; }
    public string NewCategory { get; set; }
    public string NewDestinationFolder { get; set; }
   
    public EditTorrentCommand(List<string> torrentHashes, string newDestinationFolder = "", string newCategory = "")
        : base(ManagedAction.Update)
    {
        Hashes = torrentHashes;
        NewCategory = newCategory;
        NewDestinationFolder = newDestinationFolder;
    }
}
