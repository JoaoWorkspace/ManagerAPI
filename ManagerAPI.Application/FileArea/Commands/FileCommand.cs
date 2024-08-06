using Cqrs.Commands;
using Cqrs.Messages;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.MusicArea.Commands;

public class FileCommand<TResponse> : IRequest<TResponse>
{
    public ManagedAction Action { get; set; }
    public List<string> FileOrFolderPaths { get; set; }
    public List<BinaryData>? Data { get; set; }

    public FileCommand(ManagedAction action, List<string>? fileOrFolderPaths = null, List<BinaryData>? data = null)
    {
        Action = action;
        FileOrFolderPaths = fileOrFolderPaths ?? new();
        Data = data;
    }
}
