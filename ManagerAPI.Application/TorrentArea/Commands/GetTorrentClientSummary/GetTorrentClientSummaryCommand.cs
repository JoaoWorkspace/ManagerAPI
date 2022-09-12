using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;

public class GetTorrentClientSummaryCommand : TorrentCommand<TorrentSummaryInfo>
{
    public List<TorrentClientStats> ShowStats { get; set; }
    public GetTorrentClientSummaryCommand(List<TorrentClientStats> showStats)
        : base(ManagedAction.Search)
    {
        ShowStats = showStats;
    }
}
