using ManagerAPI.Application.FileArea;
using ManagerAPI.Application.FileArea.Commands.CreateDriveFolderJson;
using ManagerAPI.Application.FileArea.Commands.CreateFolderJson;
using ManagerAPI.Application.FileArea.Models;

using ManagerAPI.Caching;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerAPI.Response;
using ManagerApplication.FileArea.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManagerAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ICacheService cacheService;
        private readonly IFileService fileService;
        private readonly ILogger<TorrentController> _logger;
        public FileController(ICacheService cacheService, IFileService torrentService, ILogger<TorrentController> logger)
        {
            this.cacheService = cacheService;
            this.fileService = fileService;
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
                    FileOrFolder driveFolder = await fileService.CreateDriveFolderJson(new CreateDriveFolderJsonCommand(strings), cancellationToken);
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
                    var folder = await fileService.CreateFolderJson(new CreateFolderJsonCommand(strings, folderRequest.SavePath, folderRequest.MaximumFolderDepth), cancellationToken);
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
    }
}