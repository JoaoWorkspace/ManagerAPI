using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.RemoveTorrentAndDeleteContent;

public class RemoveTorrentAndDeleteContentCommand : IRequest<Dictionary<string, string>>
{
    public List<SimpleTorrentInfo> TorrentsToDelete { get; set; }
    public bool AlsoDeleteTorrentFile { get; set; }

    public RemoveTorrentAndDeleteContentCommand(List<SimpleTorrentInfo> torrentsToDelete, bool alsoDeleteTorrentFile)
    {
        TorrentsToDelete = torrentsToDelete;
        AlsoDeleteTorrentFile = alsoDeleteTorrentFile;
    }
}
