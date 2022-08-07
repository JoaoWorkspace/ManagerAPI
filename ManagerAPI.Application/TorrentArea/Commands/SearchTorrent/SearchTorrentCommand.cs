using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;

public class SearchTorrentCommand : TorrentCommand<string>
{
    public SearchTorrentCommand(ActionConnector actionConnector, List<string> paths, BinaryData? data) 
        : base(ManagedAction.Search, ActionTarget.Torrent, ActionConnector.Inside, paths, data)
    {
    }
}
