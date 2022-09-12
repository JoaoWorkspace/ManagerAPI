using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;

public class GetLessDetailedTorrentCommand : TorrentCommand<List<SimpleTorrentInfo>>
{
    public List<string> Hashes { get; set; }

    public GetLessDetailedTorrentCommand(List<string> hashes)
        : base(ManagedAction.Search)
    {
        Hashes = hashes;
    }
}
