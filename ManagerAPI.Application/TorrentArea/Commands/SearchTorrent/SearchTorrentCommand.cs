using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Domain.Models.Enum;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;

public class SearchTorrentCommand : TorrentCommand<List<string>>
{
    public TorrentListFilter TorrentFilter { get; set; } = TorrentListFilter.All;
    public string? Category { get; set; } = null;
    public TorrentState? TorrentState { get; set; }

    public SearchTorrentCommand(TorrentListFilter torrentFilter, string? category = null, TorrentState? torrentState = null, List<string>? paths = null, List<BinaryData>? data = null)
        : base(ManagedAction.Search, paths, data)
    {
        TorrentFilter = torrentFilter;
        Category = category;
        TorrentState = torrentState;
    }
}
