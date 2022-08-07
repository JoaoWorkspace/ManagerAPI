using FileTypeChecker;
using FileTypeChecker.Abstracts;
using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NsfwSpyNS;
using System.Text.RegularExpressions;

namespace ManagerAPI.Application.TorrentArea;

public class TorrentService : ITorrentService
{
    public IMediator mediator;
    public TorrentService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<FolderDto> CreateDriveFolderJson(CreateDriveFolderJsonCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send<FolderDto>(command, cancellationToken);
    }
    
    //public async Task ProcessTorrentCommand(TorrentCommand command, CancellationToken cancellationToken)
    //{
    //    switch (command.Action)
    //    {
    //        case ManagedAction.Search:
    //            var searchCommand = new TorrentCommand(command.ActionConnector, command.FileOrFolderPaths, command.Data);
    //            await this.mediator.Send(searchCommand, cancellationToken);
    //            break;

    //        case ManagedAction.Compare:
    //            await this.mediator.Send(command, cancellationToken);
    //            break;

    //        case ManagedAction.Create:
    //            await this.mediator.Send(command, cancellationToken);
    //            break;

    //        case ManagedAction.Validate:
    //            await this.mediator.Send(command, cancellationToken);
    //            break;

    //        case ManagedAction.Remove:
    //            await this.mediator.Send(command, cancellationToken);
    //            break;
    //    }
    //}

    #region auxTools

    
    
    
    #endregion auxTools
}
