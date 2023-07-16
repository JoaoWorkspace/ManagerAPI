using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;

public class GetUnregisteredTorrentCommand : IRequest<List<string>>
{
    public GetUnregisteredTorrentCommand()
    {
    }
}
