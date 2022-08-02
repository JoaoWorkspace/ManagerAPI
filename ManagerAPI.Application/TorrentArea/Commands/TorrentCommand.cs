using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace ManagerAPI.Application.TorrentArea.Commands
{
    public class TorrentCommand
    {

        public ManagedAction Action { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionTarget ActionTarget { get; set; }
        public string PathToFile1 { get; set; }
        public string PathToFolder1 { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionConnector ActionConnector { get; set; }
        public BinaryData Data { get; set; }
        public string PathToFile2 { get; set; }
        public string PathToFolder2 { get; set; }
    }
}
