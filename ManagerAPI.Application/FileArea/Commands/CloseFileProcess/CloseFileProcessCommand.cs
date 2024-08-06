using MediatR;
using System.Diagnostics;

namespace ManagerAPI.Application.MusicArea.Commands.CloseFileProcess;

public class CloseFileProcessCommand : IRequest<bool>
{
    public int ProcessId { get; set; }
    public CloseFileProcessCommand(int processId)
    {
        ProcessId = processId;
    }
}