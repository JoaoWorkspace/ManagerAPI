using ManagerAPI.Domain.Models.Enum;
using MediatR;

namespace ManagerAPI.Application.TorrentArea.Commands.EditTorrent;

public class EditTorrentCommand : IRequest<List<string>>
{
    public List<string> Hashes { get; set; }
    public string NewCategory { get; set; }
    public string NewDestinationFolder { get; set; }
   
    public EditTorrentCommand(List<string> torrentHashes, string newDestinationFolder = "", string newCategory = "")
    {
        Hashes = torrentHashes;
        NewCategory = newCategory;
        NewDestinationFolder = newDestinationFolder;
    }
}
