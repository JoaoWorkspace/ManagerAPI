using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;

public class GetTorrentsAndTorrentFileCommand : TorrentCommand<List<SimpleTorrentInfo>>
{
    public List<string> Hashes { get; set; }
    public string CategoryName { get; set; }
    public GetTorrentsAndTorrentFileCommand(List<string> hashes, string categoryName, List<string>? torrentFolderPath = null)
        : base(ManagedAction.Search, torrentFolderPath)
    {
        Hashes = hashes;
        CategoryName = categoryName;
    }
}
