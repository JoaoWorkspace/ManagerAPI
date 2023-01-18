using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.FileArea.Models.Enums;
using MediatR;
using System.Diagnostics;

namespace ManagerAPI.Application.FileArea.Commands.CloseFileProcess;

public class CloseFileProcessCommandHandler : IRequestHandler<CloseFileProcessCommand, bool>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public CloseFileProcessCommandHandler(
        IMediator mediator,
        IMapper mapper
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<bool> Handle(CloseFileProcessCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Process process = Process.GetProcessById(request.ProcessId);
            if(process == null)
            {
                throw new Exception($"Process with Id = {request.ProcessId} not found.");
            }
            process.Kill();
            return true;
        }
        catch(Exception ex) 
        { 
            ManagerApplicationConsole.WriteException("CloseFileProcessCommandHandler", $"Was unable to kill process with ID={request.ProcessId}", ex);
            return false;
        }

    }
}
