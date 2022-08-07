using ManagerAPI.Application.TorrentArea;
using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Caching;
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
        private readonly ICacheService cacheService;
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
        [Produces("application/json")]
        public async Task<TorrentManagerOutput> CreateDriveFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            List<Tuple<StorageDrive, bool>> result = new List<Tuple<StorageDrive, bool>>();
            foreach (StorageDrive drive in request.Distinct())
            {
                try
                {
                    List<string> strings = new List<string>();
                    strings.Add($"{drive}:/");
                    FolderDto driveFolder = await torrentService.CreateDriveFolderJson(new CreateDriveFolderJsonCommand(strings), cancellationToken);
                    result.Add(new Tuple<StorageDrive, bool>(drive, true));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    result.Add(new Tuple<StorageDrive, bool>(drive, false));
                }
            }
            return new TorrentManagerOutput(Ok(result));
        }

        [HttpPost("GetCachedDriverFolderJson")]
        public async Task<TorrentManagerOutput> GetCachedDriverFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            List<FolderDto> driveFolders = new List<FolderDto>();
            foreach (StorageDrive drive in request.Distinct())
            {
                FolderDto? toAdd = await cacheService.DeserializeDriveFromCache(drive, cancellationToken);
                if (toAdd != null) driveFolders.Add(toAdd);
            }
            return new TorrentManagerOutput(Ok(driveFolders));
        }

        //[HttpGet("GetdDriverFolderJson")]
        //public async Task<TorrentManagerOutput> GetCachedDriverFolderJson([FromForm] List<StorageDrive> request)
        //{
        //    List<FolderDto> driveFolders = new List<FolderDto>();
        //    foreach (StorageDrive drive in request.Distinct())
        //    {
        //        driveFolders.Add(await torrentService.GetCachedDriveFolderJson($"{drive}:/"));
        //    }
        //    return new TorrentManagerOutput(Ok(driveFolders));
        //}

        //[HttpPost("ProcessTorrent")]
        //public async Task<TorrentManagerOutput> ProcessTorrentCommand([FromForm] TorrentRequest request, CancellationToken cancellationToken)
        //{
        //    var result = torrentService.ProcessTorrentCommand(
        //        new TorrentCommand(request.Action, request.ActionTarget, request.ActionConnector, request.FolderOrFilePaths, new BinaryData(request.Data))
        //        , cancellationToken);
        //    return new TorrentManagerOutput(Ok(null));
        //}


    }
}