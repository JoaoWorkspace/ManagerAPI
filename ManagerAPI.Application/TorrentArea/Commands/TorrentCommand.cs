using Cqrs.Commands;
using Cqrs.Messages;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands;

public class TorrentCommand<TResponse> : IRequest<TResponse>
{
    public ManagedAction Action { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public ActionConnector ActionConnector { get; set; }
    public List<string> FileOrFolderPaths { get; set; } = new List<string>();
    public BinaryData? Data { get; set; }

    public TorrentCommand(ManagedAction action, ActionTarget actionTarget, ActionConnector actionConnector, List<string> fileOrFolderPaths, BinaryData? data = null)
    {
        Action = action;
        ActionTarget = actionTarget;
        ActionConnector = actionConnector;
        FileOrFolderPaths = fileOrFolderPaths;
        Data = data;
    }
}
