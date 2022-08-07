using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;

public class CreateDriveFolderJsonCommand : TorrentCommand<FolderDto>
{
    public int maxDepth { get; set; } = 0;
    public CreateDriveFolderJsonCommand(List<string> drivesToMap, int maxDepth = 0) 
        : base(ManagedAction.Create, ActionTarget.File, ActionConnector.Inside, drivesToMap)
    {
        this.maxDepth = maxDepth;
    }
}