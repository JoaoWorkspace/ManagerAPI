using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Request;
using ManagerApplication.FileArea.Models;

namespace ManagerAPI.Caching;
public interface ICacheService
{
    public Task<FileOrFolder?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken);
    public Task<string> SerializeDriveToCache(StorageDrive storageDrive, FileOrFolder driveFolder, CancellationToken cancellationToken);
    public Task<Dictionary<StorageDrive, FileOrFolder?>> GetAllDrives(CancellationToken cancellationToken);
}
