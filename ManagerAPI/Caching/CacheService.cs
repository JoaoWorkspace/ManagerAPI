using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Request;
using System.Text.Json;

namespace ManagerAPI.Caching;
public class CacheService : ICacheService
{
    private ICache _cache;
    public CacheService(ICache cache)
    {
        _cache = cache;
    }

    public async Task<FolderDto?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken)
    {
        try
        {
            //Open the stored Cache to read
            string cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{storageDrive}.json";
            string json = await File.ReadAllTextAsync(cachePath, cancellationToken);
            //Deserialize the json into a FolderDto
            FolderDto? driveFolder = JsonSerializer.Deserialize<FolderDto>(json);
            return driveFolder;
        }catch(Exception ex)
        {
            Console.WriteLine($"[ERROR]\tFailed to read {storageDrive}.json from {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"\tReason: {ex.Message}");
            return null;
        }
        
    }

    public async Task<bool> SerializeDriveToCache(StorageDrive storageDrive, FolderDto driveFolder, CancellationToken cancellationToken)
    {
        try
        {
            //Open CacheFile to write
            string cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{driveFolder.Name}.json";
            await using FileStream createStream = File.Create(cachePath);
            //Write the serialized json to file
            await JsonSerializer.SerializeAsync(createStream, driveFolder, cancellationToken: cancellationToken);
            Console.WriteLine($"Written {driveFolder.Name} inside {AppDomain.CurrentDomain.BaseDirectory}");
            //Update cache
            _cache.DRIVE_FOLDER.TryAdd(storageDrive, cachePath);
            return true;
        }catch(Exception ex)
        {
            Console.WriteLine($"[ERROR]\tFailed to write {driveFolder.Name} inside {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"\tReason: {ex.Message}");
            return false;
        }
        
    }
}
