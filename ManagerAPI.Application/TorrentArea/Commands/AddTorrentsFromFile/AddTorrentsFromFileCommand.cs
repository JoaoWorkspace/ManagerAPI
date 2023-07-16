using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;

public class AddTorrentsFromFileCommand : IRequest<bool>
{
    public List<string> FileOrFolderPaths { get; set; }
    public string Category { get; set; }
    public string SavePath { get; set; }
    public bool StartTorrent { get; set; } = false;

    public AddTorrentsFromFileCommand(List<string> torrentsToAdd, string category, string savePath, bool startTorrent)
    {
        FileOrFolderPaths = torrentsToAdd;
        Category = category;
        SavePath = savePath;
        StartTorrent = startTorrent;
    }
}
