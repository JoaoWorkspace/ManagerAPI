using ManagerAPI.Application.FileArea.Commands.GetTorrentsFromFolder;
using ManagerAPI.Application.TorrentArea;
using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Commands.AddTorrentsFromFile;
using ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.TorrentArea.Commands.CreateFolderJson;
using ManagerAPI.Application.TorrentArea.Commands.EditTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetLessDetailedTorrent;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentClientSummary;
using ManagerAPI.Application.TorrentArea.Commands.GetTorrentsAndTorrentFile;
using ManagerAPI.Application.TorrentArea.Commands.GetUnregisteredTorrent;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.TorrentArea.Models;

using ManagerAPI.Caching;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerAPI.Response;
using ManagerApplication.FileArea.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using QBittorrent.Client;
using System.Text.Json.Serialization;

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
        public TorrentController(ICacheService cacheService, ITorrentService torrentService, ILogger<TorrentController> logger)
        {
            this.cacheService = cacheService;
            this.torrentService = torrentService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a cached JSON file containing the FileSystem structure FolderDto for each selected Drive (if exists)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A simple list of selected drives, and if their JSON file was created.</returns>
        [HttpPost("CreateCachedDriverFolderJson")]
        public async Task<TorrentManagerOutput> CreateDriveFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            List<Tuple<StorageDrive, bool>> result = new List<Tuple<StorageDrive, bool>>();
            foreach (StorageDrive drive in request.Distinct())
            {
                try
                {
                    List<string> strings = new() { $"{drive}:/" };
                    FileOrFolder driveFolder = await torrentService.CreateDriveFolderJson(new CreateDriveFolderJsonCommand(strings), cancellationToken);
                    bool success = await cacheService.SerializeDriveToCache(drive, driveFolder, cancellationToken);
                    if (success)
                    {
                        result.Add(new Tuple<StorageDrive, bool>(drive, true));
                    }
                    else
                    {
                        result.Add(new Tuple<StorageDrive, bool>(drive, false));
                    }

                }
                catch (Exception ex)
                {
                    ManagerConsole.WriteException("CreateCachedDriverFolderJson", $"Failed to create a DriveFolderJson on cache on drive {drive}", ex);
                    result.Add(new Tuple<StorageDrive, bool>(drive, false));
                }
            }
            return new TorrentManagerOutput(Ok(result));
        }

        [HttpGet("GetCachedDriveFolderJson")]
        public async Task<TorrentManagerOutput> GetCachedDriveFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            List<FileOrFolder> driveFolders = new List<FileOrFolder>();
            foreach (StorageDrive drive in request.Distinct())
            {
                FileOrFolder? toAdd = await cacheService.DeserializeDriveFromCache(drive, cancellationToken);
                if (toAdd != null) driveFolders.Add(toAdd);
            }
            return new TorrentManagerOutput(Ok(driveFolders));
        }

        [HttpPost("CreateFolderJson")]
        public async Task<TorrentManagerOutput> CreateFolderJson([FromBody] List<FolderRequest> request, CancellationToken cancellationToken)
        {
            List<Tuple<string, bool>> result = new List<Tuple<string, bool>>();

            foreach (FolderRequest folderRequest in request)
            {
                try
                {
                    List<string> strings = new() { folderRequest.FolderPath };
                    var folder = await torrentService.CreateFolderJson(new CreateFolderJsonCommand(strings, folderRequest.SavePath, folderRequest.MaximumFolderDepth), cancellationToken);
                    result.Add(new Tuple<string, bool>(folderRequest.SavePath, folder != null));
                }
                catch (Exception ex)
                {
                    ManagerConsole.WriteException("CreateFolderJson", $"Failed to create a FolderJson for {folderRequest.FolderPath} on {folderRequest.SavePath}", ex);
                    result.Add(new Tuple<string, bool>(folderRequest.FolderPath, false));
                }
            }
            return new TorrentManagerOutput(Ok(result));
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
                var torrentList = await torrentService.GetAllActiveTorrents(new SearchTorrentCommand(TorrentListFilter.Active), cancellationToken);
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
        public async Task<TorrentManagerOutput> AddTorrentsFromFile([FromForm] AddTorrentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var torrentFilesToAdd = await fileService.GetAllTorrents(new GetTorrentsFromFolderCommand(request.Paths), cancellationToken);
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