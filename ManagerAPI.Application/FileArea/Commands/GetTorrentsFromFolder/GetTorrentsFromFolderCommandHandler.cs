using AutoMapper;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using QBittorrent.Client;
using System.Net;
using System.Web.Http;

namespace ManagerAPI.Application.FileArea.Commands.GetTorrentsFromFolder;

public class GetFilesFromFolderCommandHandler : IRequestHandler<GetTorrentsFromFolderCommand, List<string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public GetTorrentsFromFolderCommandHandler(   
        IMediator mediator,
        IMapper mapper
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<List<string>> Handle(GetTorrentsFromFolderCommand request, CancellationToken cancellationToken)
    {
        return new();
    }
}
