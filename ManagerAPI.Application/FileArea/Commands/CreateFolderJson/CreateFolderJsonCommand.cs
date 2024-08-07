﻿using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Domain.Models.Enum;

namespace ManagerAPI.Application.MusicArea.Commands.CreateFolderJson;

public class CreateFolderJsonCommand : FileCommand<string >
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