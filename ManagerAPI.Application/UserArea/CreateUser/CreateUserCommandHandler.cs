﻿using AutoMapper;
using Cqrs.Domain;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Repositories;
using MediatR;
using Newtonsoft.Json.Linq;
using QBittorrent.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ManagerAPI.Application.TorrentArea.Commands.EditTorrent;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, List<string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    private readonly IDocumentStore documentStore;
    private readonly IUserRepository userRepository;
    public CreateUserCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client,
        IDocumentStore documentStore,
        IUserRepository userRepository
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
        this.documentStore = documentStore;
        this.userRepository = userRepository;
    }

    public async Task<List<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = "";
        using(var session = documentStore.OpenAsyncSession())
        {

        }
        var torrents = await EditQbitTorrents(request, cancellationToken);
        return torrents;
    }

    public async Task<List<string>> EditQbitTorrents(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> allTorrents = new();
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Filter = TorrentListFilter.All, Hashes = request.Hashes }, cancellationToken);
            await ModifyTorrents(torrentList.ToList(), request, cancellationToken);
            return torrentList.Select(x => x.Name).ToList();
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }

    /// <summary>
    /// Used to print out additional information like: max_ratio, availability, max_seeding_time, seeding_time_limit and trackers_count.
    /// </summary>
    /// <param name="data"></param>
    public async Task ModifyTorrents(List<TorrentInfo> data, CreateUserCommand request, CancellationToken cancellationToken)
    {
        foreach (var torrent in data)
        {
            //Will only modify Category if there's a value set
            if (!string.IsNullOrEmpty(request.NewCategory))
            {
                ManagerApplicationConsole.WriteInformation("EditTorrentCommandHandler.ModifyTorrents",
                    $"Going to modify torrent {torrent.Name}|{torrent.Hash}...\n" +
                    $"Target Category: {request.NewCategory}\n" +
                    $"Target Folder: {request.NewDestinationFolder}");

                if (torrent.Category.Equals(request.NewCategory))
                {
                    ManagerApplicationConsole.WriteInformation("EditTorrentCommandHandler.ModifyTorrents",
                        $"The torrent {torrent.Name}|{torrent.Hash} already has the target category: {request.NewCategory}");
                }
                else
                {
                    await client.SetTorrentCategoryAsync(torrent.Hash, request.NewCategory, cancellationToken);
                }
            }
            //Will only modify the destination folder if there's a value set
            if (!string.IsNullOrEmpty(request.NewDestinationFolder))
            {
                if (torrent.SavePath.Equals(request.NewDestinationFolder))
                {
                    ManagerApplicationConsole.WriteInformation("EditTorrentCommandHandler.ModifyTorrents",
                        $"The torrent {torrent.Name}|{torrent.Hash} already has the target destination folder: {request.NewDestinationFolder}");
                }
                else
                {
                    await client.SetLocationAsync(torrent.Hash, request.NewDestinationFolder, cancellationToken);
                }
            }
        }
    }
}
