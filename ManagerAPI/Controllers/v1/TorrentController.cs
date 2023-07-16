using ManagerAPI.Application.TorrentArea;
using ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;
using ManagerAPI.Application.TorrentArea.Commands.EditTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;
using ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Models;

using ManagerAPI.Caching;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerAPI.Response;
using Microsoft.AspNetCore.Mvc;
using QBittorrent.Client;
using ManagerAPI.Application.FileArea;
using ManagerAPI.Application.FileArea.Commands.GetFilesFromFolder;
using ManagerAPI.Application.TorrentArea.Models.Enum;

namespace ManagerAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TorrentController : ControllerBase
    {
        private readonly ICacheService cacheService;
        private readonly IFileService fileService;
        private readonly ITorrentService torrentService;
        private readonly ILogger<TorrentController> _logger;
        public TorrentController(ICacheService cacheService, IFileService fileService, ITorrentService torrentService, ILogger<TorrentController> logger)
        {
            this.cacheService = cacheService;
            this.fileService = fileService;
            this.torrentService = torrentService;
            _logger = logger;
        }

        [HttpPut("GetTorrentClientSummary")]
        public async Task<TorrentManagerOutput> GetTorrentClientSummary([FromForm] List<TorrentClientStats> showStats, CancellationToken cancellationToken)
        {
            TorrentSummaryInfo summary = await torrentService.GetTorrentClientSummaryAsync(new GetTorrentClientSummaryCommand(showStats), cancellationToken);
            return new TorrentManagerOutput(Ok(summary));
        }

        [HttpGet("GetActiveTorrents")]
        public async Task<TorrentManagerOutput> GetActiveTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetAllActiveTorrents(new SearchTorrentCommand(TorrentListFilter.Active), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetActiveTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpGet("GetInactiveTorrents")]
        public async Task<TorrentManagerOutput> GetInactiveTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetAllInactiveTorrents(new SearchTorrentCommand(TorrentListFilter.Inactive), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetInactiveTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpGet("GetSeedingTorrents")]
        public async Task<TorrentManagerOutput> GetSeedingTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetAllSeedingTorrents(new SearchTorrentCommand(TorrentListFilter.Seeding), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetSeedingTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpGet("GetPausedTorrents")]
        public async Task<TorrentManagerOutput> GetPausedTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetAllActiveTorrents(new SearchTorrentCommand(TorrentListFilter.Paused), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetPausedTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpGet("GetErroredTorrents")]
        public async Task<TorrentManagerOutput> GetErroredTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetAllActiveTorrents(new SearchTorrentCommand(TorrentListFilter.Errored), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetErroredTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpGet("GetUnregisteredTorrents")]
        public async Task<TorrentManagerOutput> GetUnregisteredTorrents(CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetUnregisteredTorrents(new GetUnregisteredTorrentCommand(), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetUnregisteredTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPost("GetDetailedTorrents")]
        public async Task<TorrentManagerOutput> GetDetailedTorrents([FromBody] List<string> torrentHashes, CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetDetailedTorrents(new GetDetailedTorrentCommand(torrentHashes), cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetDetailedTorrents", $"Failed to obtain detailed torrents .", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPost("GetTorrents")]
        public async Task<TorrentManagerOutput> GetTorrents([FromBody] List<string> hashes, CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetTorrents(new GetLessDetailedTorrentCommand(hashes),cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetTorrents", $"Failed to obtain torrents.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPost("GetTorrentsAndTorrentFile")]
        public async Task<TorrentManagerOutput> GetTorrentsAndSearchTorrentFile([FromBody] GetTorrentsAndTorrentFileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetTorrentsAndTorrentFile(
                    new GetTorrentsAndTorrentFileCommand(request.Hashes, request.CategoryName, request.TorrentDirectoryList),
                    cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("GetTorrentsAndSearchTorrentFile", $"Failed the search.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPost("AddTorrentsFromFile")]
        public async Task<TorrentManagerOutput> AddTorrentsFromFile([FromForm] AddTorrentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await torrentService.AddTorrentsFromFile(new AddTorrentsFromFileCommand(
                    request.Paths, 
                    request.Category, 
                    request.DestinationFolder,
                    request.StartTorrent), cancellationToken);
                return new TorrentManagerOutput(Ok($"Success = {result}"));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("AddTorrentsFromFile", $"Failed to add new torrents in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPost("AddTorrentsFromFolder")]
        public async Task<TorrentManagerOutput> AddTorrentsFromFolder([FromForm] AddTorrentRequest request, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;
            try
            {
                var torrentFilesToAdd = await fileService.GetFilesFromFolder(new GetFilesFromFolderCommand(request.Paths, new() { ".torrent" }), cancellationToken);
                var result = await torrentService.AddTorrentsFromFile(new AddTorrentsFromFileCommand(
                    torrentFilesToAdd,
                    request.Category,
                    request.DestinationFolder,
                    request.StartTorrent), cancellationToken);
                return new TorrentManagerOutput(Ok($"Success = {result}"));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("AddTorrentsFromFile", $"Failed to add new torrents in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPut("EditCategoryForTorrentsForm")]
        public async Task<TorrentManagerOutput> EditCategoryForTorrentsForm([FromForm] EditTorrentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var editTorrents = await torrentService.EditCategoryForTorrents(new EditTorrentCommand(request.TorrentHashes, newCategory:request.NewCategory), cancellationToken);
                return new TorrentManagerOutput(Ok(editTorrents));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("EditCategoryForTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpPut("EditCategoryForTorrentsBody")]
        public async Task<TorrentManagerOutput> EditCategoryForTorrentsBody([FromBody] EditTorrentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var editTorrents = await torrentService.EditCategoryForTorrents(new EditTorrentCommand(request.TorrentHashes, newCategory: request.NewCategory), cancellationToken);
                return new TorrentManagerOutput(Ok(editTorrents));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("EditCategoryForTorrents", $"Failed to obtain all active torrents running in your qbitclient.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpDelete("RemoveTorrentContent")]
        public async Task<TorrentManagerOutput> RemoveTorrentContent([FromBody] GetTorrentsAndTorrentFileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetTorrentsAndTorrentFile(
                    new GetTorrentsAndTorrentFileCommand(request.Hashes, request.CategoryName, request.TorrentDirectoryList),
                    cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("RemoveTorrentContent", $"Deletion Cancelled.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }

        [HttpDelete("RemoveTorrentContentAndTorrentFile")]
        public async Task<TorrentManagerOutput> RemoveTorrentContentAndTorrentFile([FromBody] GetTorrentsAndTorrentFileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var torrentList = await torrentService.GetTorrentsAndTorrentFile(
                    new GetTorrentsAndTorrentFileCommand(request.Hashes, request.CategoryName, request.TorrentDirectoryList),
                    cancellationToken);
                return new TorrentManagerOutput(Ok(torrentList));
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("RemoveTorrentContentAndTorrentFile", $"Deletion Cancelled.", ex);
                return new TorrentManagerOutput(BadRequest(ex.Message));
            }
        }
    }
}