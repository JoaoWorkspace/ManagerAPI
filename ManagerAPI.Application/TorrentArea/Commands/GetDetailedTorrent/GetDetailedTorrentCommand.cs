using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;

public class GetDetailedTorrentCommand : IRequest<List<TorrentInfo>>
{
    public List<string> Hashes { get; set; }

    public GetDetailedTorrentCommand(List<string> hashes)
    {
        Hashes = hashes;
    }
}
