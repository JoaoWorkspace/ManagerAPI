using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.FileArea.Commands.GetTorrentsFromFolder;

public class GetTorrentsFromFolderCommand : FileCommand<List<string>>
{
    public List<string> FoldersToSearch { get; set; } = new();

    public GetTorrentsFromFolderCommand(List<string> foldersToSearch)
        : base(ManagedAction.Search, foldersToSearch)
    {
        FoldersToSearch = foldersToSearch;
    }
}
