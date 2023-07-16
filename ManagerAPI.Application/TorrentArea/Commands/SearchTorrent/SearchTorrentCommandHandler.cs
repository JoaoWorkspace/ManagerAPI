using AutoMapper;
using Autofac;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea.Models;
using MediatR;
using QBittorrent.Client;

namespace ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;

public class SearchTorrentCommandHandler : IRequestHandler<SearchTorrentCommand, List<string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IQBittorrentClient client;
    public SearchTorrentCommandHandler(
        IMediator mediator,
        IMapper mapper,
        IQBittorrentClient client
        //ITorrentRepository torrentRepository
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.client = client;
    }

    public async Task<List<string>> Handle(SearchTorrentCommand request, CancellationToken cancellationToken)
    {
        var torrents = await GetQbitTorrents(request, cancellationToken);
        return torrents;
    }

    public async Task<List<string>> GetQbitTorrents(SearchTorrentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<string> allTorrents = new();
            var torrentList = await client.GetTorrentListAsync(new TorrentListQuery { Filter = request.TorrentFilter, Category = request.Category, Hashes = ProcessHashes(request) }, cancellationToken);
            //PrintAdditionalData(torrentList.ToList());
            //SimplifyTorrentInfo(torrentList.ToList());
            var torrents = ApplySearchFilters(request, torrentList.ToList());
            return torrents.Select(t => t.Hash).ToList();
        }catch(Exception ex)
        {
            ManagerApplicationConsole.WriteException("GetQbitTorrents", "There was an issue getting data from the QbitClient", ex);
            throw;
        }
    }

    public List<TorrentInfo> ApplySearchFilters(SearchTorrentCommand request, List<TorrentInfo> torrents)
    {
        if(request.TorrentState != null)
        {
            torrents = torrents.Where(ti => ti.State.Equals(request.TorrentState.Value)).ToList();
        }
        return torrents;
    }

    public List<string> ProcessHashes(SearchTorrentCommand request)
    {
        List<string> hashes = new();

        if (request.FileOrFolderPaths != null && request.FileOrFolderPaths.Any())
        {
            List<FileOrFolder> SourceList = new List<FileOrFolder>();
            foreach (var folderOrFile in request.FileOrFolderPaths)
            {
                FileOrFolder folder = TorrentUtils.GetDirectoryAsFolder(folderOrFile);
                Torrent.Torrent fromFile = Torrent.Torrent.FromFile(folder.FullPath);
                //Properly hash everything to compare with each torrent
                //Compare with each torrent
                //hashes.Add(folderOrFile.Hash);
            }
        }

        return hashes;
    }

    /// <summary>
    /// Used to print out additional information like: max_ratio, availability, max_seeding_time, seeding_time_limit and trackers_count.
    /// </summary>
    /// <param name="data"></param>
    public void PrintAdditionalData(List<TorrentInfo> data)
    {
        foreach(var torrent in data)
        {
            foreach(var key in torrent.AdditionalData.Keys)
            {
                torrent.AdditionalData.TryGetValue(key, out var newData);
                ManagerApplicationConsole.WriteInformation("SearchTorrentCommandHandler.PrintAdditionalData", $"Torrent Data for {torrent.Name}.{torrent.Hash}: {key}={newData}");
                foreach(var token in newData.Children())
                {
                    ManagerApplicationConsole.WriteInformation("SearchTorrentCommandHandler.PrintAdditionalData", $"Additional Torrent Data for {torrent.Name}.{torrent.Hash}: {token}");
                }
            }
        }
    }
}
