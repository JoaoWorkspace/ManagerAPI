using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;

public class CreateDriveFolderJsonCommand : FileCommand<FileOrFolder>
{
    public int MaxDepth { get; set; } = 0;
    public CreateDriveFolderJsonCommand(List<string> drivesToMap, int maxDepth = 0) 
        : base(ManagedAction.Create, drivesToMap)
    {
        MaxDepth = maxDepth;
    }
}