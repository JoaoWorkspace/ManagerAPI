using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
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
