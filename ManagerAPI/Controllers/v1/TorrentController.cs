using ManagerAPI.Application.TorrentArea;
using ManagerAPI.Request;
using ManagerAPI.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ManagerAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TorrentController : ControllerBase
    {
        private readonly TorrentService torrentService;
        private readonly ILogger<TorrentController> _logger;
        public TorrentController(TorrentService torrentService, ILogger<TorrentController> logger)
        {
            this.torrentService = torrentService;
            _logger = logger;
        }

        [HttpPost("ProcessTorrent")]
        public async Task<TorrentManagerOutput> ProcessTorrentCommand([FromForm] TorrentRequest request, CancellationToken cancellationToken)
        {
            return new TorrentManagerOutput
            {
            };
        }
    }
}