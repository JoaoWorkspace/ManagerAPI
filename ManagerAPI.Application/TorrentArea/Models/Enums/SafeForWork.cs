using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ManagerAPI.Application.TorrentArea.Models.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SafeForWork
{
    Safe = 0,
    Sexy = 1,
    Hentai = 2,
    Pornography = 3,
    Unclassified = 4
}
