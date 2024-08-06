using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.MusicArea.Commands.GetFilesFromFolder;

public class GetFilesFromFolderCommand : FileCommand<List<string>>
{
    public List<string> FileExtensions { get; set; } = new();

    public GetFilesFromFolderCommand(List<string> foldersToSearch, List<string> fileExtensions)
        : base(ManagedAction.Search, foldersToSearch)
    {
        FileExtensions = fileExtensions;
    }
}
