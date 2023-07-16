using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.Application.TorrentArea.Models.Enum;
using ManagerAPI.Domain.Models.Enum;
using MediatR;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;

public class GetTorrentClientSummaryCommand : IRequest<TorrentSummaryInfo>
{
    public List<TorrentClientStats> ShowStats { get; set; }
    public GetTorrentClientSummaryCommand(List<TorrentClientStats> showStats)
    {
        ShowStats = showStats;
    }
}
