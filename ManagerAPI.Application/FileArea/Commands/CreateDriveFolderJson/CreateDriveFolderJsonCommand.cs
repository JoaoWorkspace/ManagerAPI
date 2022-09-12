using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.TorrentArea.Models.Enum;
using ManagerAPI.Domain.Models.Enum;

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