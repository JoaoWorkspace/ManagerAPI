using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;

public class GetDetailedTorrentCommand : TorrentCommand<List<TorrentInfo>>
{
    public List<string> Hashes { get; set; }

    public GetDetailedTorrentCommand(List<string> hashes)
        : base(ManagedAction.Search)
    {
        Hashes = hashes;
    }
}
