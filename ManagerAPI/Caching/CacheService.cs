using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerApplication.FileArea.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ManagerAPI.Caching;
public class CacheService : ICacheService
{
    private ICache cache;
    public CacheService(ICache cache)
    {
        this.cache = cache;
    }

    public async Task<FileOrFolder?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken)
    {
        FileOrFolder? driveFolder = null;
        try
        {
            //Open the stored Cache to read
            if(cache.DRIVE_FOLDER.TryGetValue(storageDrive, out var cachePath))
            {
                using (StreamReader r = new StreamReader(cachePath))
                {
                    string json = r.ReadToEnd();
                    driveFolder = await JsonSerializer.DeserializeAsync<FileOrFolder>(r.BaseStream, cancellationToken: cancellationToken);
                }
            }
            return driveFolder;
        }
        catch (Exception ex)
        {
            ManagerConsole.WriteException("DeserializeDriveFromCache", $"Failed to read {storageDrive}.json from {AppDomain.CurrentDomain.BaseDirectory}",ex);
            return null;
        }
    }

    public async Task<bool> SerializeDriveToCache(StorageDrive storageDrive, FileOrFolder driveFolder, CancellationToken cancellationToken)
    {
        try
        {
            //Open CacheFile to write
            string cachePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{storageDrive}.json";
            await using FileStream createStream = File.Create(cachePath);
            //Write the serialized json to file
            await JsonSerializer.SerializeAsync(createStream, driveFolder, cancellationToken: cancellationToken);
            Console.WriteLine($"Written {driveFolder.Name} inside {AppDomain.CurrentDomain.BaseDirectory}");
            //Update cache
            cache.DRIVE_FOLDER.TryAdd(storageDrive, cachePath);
            return true;
        }catch(Exception ex)
        {
            ManagerConsole.WriteException("SerializeDriveToCache", $"Failed to write {driveFolder.Name} inside {AppDomain.CurrentDomain.BaseDirectory}\n\tReason: {ex}");
            return false;
        }
    }
}
