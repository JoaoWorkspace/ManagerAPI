using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Request;
using ManagerApplication.FileArea.Models;

namespace ManagerAPI.Caching;
public interface ICacheService
{
    public Task<FileOrFolder?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken);
    public Task<bool> SerializeDriveToCache(StorageDrive storageDrive, FileOrFolder driveFolder, CancellationToken cancellationToken);
}
