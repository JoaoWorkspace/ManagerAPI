using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Request;

namespace ManagerAPI.Caching;
public interface ICacheService
{
    public Task<FolderDto?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken);
    public Task<bool> SerializeDriveToCache(StorageDrive storageDrive, FolderDto driveFolder, CancellationToken cancellationToken);
}
