using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;

public class AddTorrentsFromFileCommand : TorrentCommand<bool>
{
    public string Category { get; set; }
    public string SavePath { get; set; }
    public bool StartTorrent { get; set; } = false;

    public AddTorrentsFromFileCommand(List<string> torrentsToAdd, string category, string savePath, bool startTorrent)
        : base(ManagedAction.Create)
    {
        FileOrFolderPaths = torrentsToAdd;
        Category = category;
        SavePath = savePath;
        StartTorrent = startTorrent;
    }
}
