using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;

public class GetTorrentsAndTorrentFileCommand : IRequest<List<SimpleTorrentInfo>>
{
    public string CategoryName { get; set; }
    public List<string> Hashes { get; set; }
    public List<string> TorrentFolderPaths { get; set; }

    public GetTorrentsAndTorrentFileCommand(List<string> hashes, string categoryName, List<string>? torrentFolderPath = null)
    {
        CategoryName = categoryName;
        Hashes = hashes;
        TorrentFolderPaths = torrentFolderPath;
    }
}
