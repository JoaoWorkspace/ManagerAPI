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
using ManagerAPI.Application.FileArea.Commands.OpenFile;

using ManagerAPI.Application.FileArea.Commands.CloseFileProcess;
using ManagerAPI.Application.FileArea.Queries.GetAllProcessesUsingPath;
using System.Security.Principal;

namespace ManagerAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ICacheService cacheService;
        private readonly IFileService fileService;
        private readonly ILogger<TorrentController> _logger;
        public FileController(ICacheService cacheService, IFileService fileService, ILogger<TorrentController> logger)
        {
            this.cacheService = cacheService;
            this.fileService = fileService;
            _logger = logger;
        }

        private bool IsBeingRunAsAdministrator()
        {
            return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
        }

        /// <summary>
        /// Creates a cached JSON file containing the FileSystem structure FolderDto for each selected Drive (if exists)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A simple list of selected drives, and if their JSON file was created.</returns>
        [HttpPost("CreateCachedDriverFolderJson")]
        public async Task<FileManagerOutput> CreateDriveFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;

            List<string> result = new();
            foreach (StorageDrive drive in request.Distinct())
            {
                try
                {
                    List<string> strings = new() { $"{drive}:/" };
                    FileOrFolder driveFolder = await fileService.CreateDriveFolderJson(new CreateDriveFolderJsonCommand(strings), cancellationToken);
                    result.Add(await cacheService.SerializeDriveToCache(drive, driveFolder, cancellationToken));

                }
                catch (Exception ex)
                {
                    ManagerConsole.WriteException("CreateCachedDriverFolderJson", $"Failed to create a DriveFolderJson on cache on drive {drive}", ex);
                    result.Add($"Drive {drive} not written to cache.");
                }
            }
            return new FileManagerOutput(Ok(result), start);
        }

        [HttpPut("GetCachedDriveFolderJson")]
        public async Task<FileManagerOutput> GetCachedDriveFolderJson([FromForm] List<StorageDrive> request, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;

            List<FileOrFolder> driveFolders = new List<FileOrFolder>();
            foreach (StorageDrive drive in request.Distinct())
            {
                FileOrFolder? toAdd = await cacheService.DeserializeDriveFromCache(drive, cancellationToken);
                if (toAdd != null) driveFolders.Add(toAdd);
            }

            return new FileManagerOutput(Ok(driveFolders), start);
        }

        [HttpPost("CreateFolderJson")]
        public async Task<FileManagerOutput> CreateFolderJson([FromBody] List<FolderRequest> request, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;

            List<string> result = new();
            foreach (FolderRequest folderRequest in request)
            {
                try
                {
                    List<string> strings = new() { folderRequest.FolderPath };
                    var response = await fileService.CreateFolderJson(new CreateFolderJsonCommand(strings, folderRequest.SavePath, folderRequest.MaximumFolderDepth), cancellationToken);
                    result.Add(response);
                }
                catch (Exception ex)
                {
                    ManagerConsole.WriteException("CreateFolderJson", $"Failed to create a FolderJson for {folderRequest.FolderPath} on {folderRequest.SavePath}", ex);
                }
            }
            return new FileManagerOutput(Ok(result), start);
        }

        [HttpPut("OpenFile")]
        public async Task<FileManagerOutput> OpenFile(string fullPath, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;

            try
            {
                var result = await fileService.OpenFileAsync(new OpenFileCommand(fullPath), cancellationToken);
                return new FileManagerOutput(Ok(result), start);
            }
            catch(Exception ex) 
            {
                ManagerConsole.WriteException("CloseFileProcessIfExists", $"Failed to close a proocess including the file {fullPath}.", ex);
                return new FileManagerOutput(BadRequest(ex.Message), start);
            }
        }

        [HttpGet("GetAllProcessesUsingPath")]
        public async Task<FileManagerOutput> GetAllProcessesUsingPath(string path, CancellationToken cancellationToken)
        {
            if (!IsBeingRunAsAdministrator()) { return new FileManagerOutput(BadRequest("The running process isn't using elevated rights, needed for the execution of this endpoint.")); }

            DateTime start = DateTime.UtcNow;
            try
            {
                
                var result = await fileService.GetAllProcessesUsingPathAsync(new GetAllProcessesUsingPathQuery(await cacheService.GetAllDrives(cancellationToken), path), cancellationToken);
                return new FileManagerOutput(Ok(result), start);

            }
            catch (Exception ex)
            {
                return new FileManagerOutput(BadRequest(ex.Message), start);

            }
        }

        [HttpDelete("CloseFileProcess")]
        public async Task<FileManagerOutput> CloseFileProcess(int processId, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.UtcNow;
            try
            {
                var result = await fileService.CloseFileProcessAsync(new CloseFileProcessCommand(processId), cancellationToken);
                return new FileManagerOutput(Ok(result), start);
            }
            catch (Exception ex)
            {
                ManagerConsole.WriteException("CloseFileProcessIfExists", $"Failed to close process {processId}.", ex);
                return new FileManagerOutput(BadRequest(ex.Message), start);
            }
        }
    }
}