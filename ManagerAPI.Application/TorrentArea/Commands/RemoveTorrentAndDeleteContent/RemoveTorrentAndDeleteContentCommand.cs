using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.RemoveTorrentAndDeleteContent;

public class RemoveTorrentAndDeleteContentCommand : TorrentCommand<Dictionary<string, string>>
{
    public List<SimpleTorrentInfo> TorrentsToDelete { get; set; }
    public bool AlsoDeleteTorrentFile { get; set; }

    public RemoveTorrentAndDeleteContentCommand(List<SimpleTorrentInfo> torrentsToDelete, bool alsoDeleteTorrentFile)
        : base(ManagedAction.Remove)
    {
        TorrentsToDelete = torrentsToDelete;
        AlsoDeleteTorrentFile = alsoDeleteTorrentFile;
    }
}
