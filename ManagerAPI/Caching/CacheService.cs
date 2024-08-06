using ManagerAPI.Application.MusicArea.Models;
using ManagerAPI.Application.TorrentArea.Models;
using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerAPI.Response;
using ManagerApplication.FileArea.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace ManagerAPI.Caching;
public class CacheService : ICacheService
{
    private ICache Cache { get; set; }
    public CacheService(ICache cache)
    {
        this.Cache = cache;
    }

    public async Task<FileOrFolder?> DeserializeDriveFromCache(StorageDrive storageDrive, CancellationToken cancellationToken)
    {
        DateTime start = DateTime.UtcNow;

        FileOrFolder? driveFolder = null;
        try
        {
            //Open the stored Cache to read
            if (Cache.DRIVE_FOLDER.TryGetValue(storageDrive, out var cachePath))
            {
                using (StreamReader r = new StreamReader(cachePath))
                {
                    string json = r.ReadToEnd();
                    r.BaseStream.Position = 0;
                    driveFolder = await JsonSerializer.DeserializeAsync<FileOrFolder>(r.BaseStream, cancellationToken: cancellationToken);
                }
            }
            return driveFolder;
        }
        catch (Exception ex)
        {
            ManagerConsole.WriteException("DeserializeDriveFromCache", $"Failed to read {storageDrive}.json from {Cache.CacheFolder}", ex);
            return null;
        }
    }

    public async Task<string> SerializeDriveToCache(StorageDrive storageDrive, FileOrFolder driveFolder, CancellationToken cancellationToken)
    {
        try
        {
            //Open CacheFile to write
            Directory.CreateDirectory(Cache.CacheFolder);
            string cachePath = $"{Cache.CacheFolder}/{storageDrive}.json";
            await using FileStream createStream = File.Create(cachePath);
            //Write the serialized json to file
            await JsonSerializer.SerializeAsync(createStream, driveFolder, cancellationToken: cancellationToken);
            Console.WriteLine($"Written {driveFolder.Name} inside {Cache.CacheFolder}");
            //Update cache
            Cache.DRIVE_FOLDER.TryAdd(storageDrive, cachePath);
            return $"Drive {storageDrive} successfully written to cache.";
        } catch (Exception ex)
        {
            ManagerConsole.WriteException("SerializeDriveToCache", $"Failed to write {driveFolder.Name} inside {Cache.CacheFolder}\n\tReason: {ex}");
            return $"Drive {storageDrive} not written to cache.";
        }
    }

    public async Task<Dictionary<StorageDrive, FileOrFolder?>> GetAllDrives(CancellationToken cancellationToken)
    {
        Dictionary<StorageDrive, FileOrFolder?> result = new();
        foreach (var key in Cache.DRIVE_FOLDER.Keys)
        {
            FileOrFolder? fileOrFolder = await DeserializeDriveFromCache(key, cancellationToken);
            result.Add(key, fileOrFolder);
        }
        return result;
    }
}
