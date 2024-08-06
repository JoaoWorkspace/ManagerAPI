using AutoMapper;
using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Application.MusicArea.Models.Enums;
using MediatR;
using System.Diagnostics;

namespace ManagerAPI.Application.MusicArea.Commands.OpenFile;

public class OpenFileCommandHandler : IRequestHandler<OpenFileCommand, ProcessStartInfo?>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public OpenFileCommandHandler(
        IMediator mediator,
        IMapper mapper
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<ProcessStartInfo?> Handle(OpenFileCommand request, CancellationToken cancellationToken)
    {
        var pi = new ProcessStartInfo(request.FullFilePath)
        {
            Arguments = Path.GetFileName(request.FullFilePath),
            UseShellExecute = true,
            WorkingDirectory = Path.GetDirectoryName(request.FullFilePath),
            FileName = request.FullFilePath,
            Verb = "OPEN"
        };
        Process.Start(pi);
        return pi;
    }
}
