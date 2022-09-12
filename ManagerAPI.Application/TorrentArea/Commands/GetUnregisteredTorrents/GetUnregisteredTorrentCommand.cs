using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;

public class GetUnregisteredTorrentCommand : TorrentCommand<List<string>>
{
    public GetUnregisteredTorrentCommand()
        : base(ManagedAction.Search)
    {
    }
}
