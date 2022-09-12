using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.FileArea.Commands.CreateFolderJson;

public class CreateFolderJsonCommand : FileCommand<FileOrFolder>
{
    public int MaxDepth { get; set; } = 0;
    public string PathToSaveJson { get; set; }
    public CreateFolderJsonCommand(List<string> drivesToMap, string pathToSaveJson, int maxDepth = 0)
        : base(ManagedAction.Create, drivesToMap)
    {
        MaxDepth = maxDepth;
        PathToSaveJson = pathToSaveJson;
    }
}