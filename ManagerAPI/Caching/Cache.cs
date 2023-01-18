using ManagerAPI.ExceptionHandling;
using ManagerAPI.Request;
using ManagerAPI.Settings;
using ManagerApplication.FileArea.Models;
using Microsoft.Extensions.Options;

namespace ManagerAPI.Caching;
/// <summary>
/// Cache for DriveJson, storing the entire folder structure as an object for each System Drive.
/// The value for each drive is null if there's no cached JSON
/// When it exists, each letter has the string path to the JSON file storing it's structure
/// </summary>
public class Cache : ICache
{
    public string CacheFolder { get; set; }
    public Dictionary<StorageDrive, string> DRIVE_FOLDER { get; set; } = new Dictionary<StorageDrive, string>();
    public Cache(IConfiguration configuration, IOptions<CacheSettings> settings)
    {
        CacheFolder = settings.Value.Folder;
        InitializeCache();
    }
    /// <summary>
    /// Fills Cache Dictionaries with Initialized Values if the cacheData exists
    /// </summary>
    public void InitializeCache()
    {
        InitializeDriveCache();
    }

    public void InitializeDriveCache()
    {
        foreach (var driveLetter in Enum.GetValues(typeof(StorageDrive)))
        {
            if (!DRIVE_FOLDER.TryGetValue((StorageDrive)driveLetter, out var cachePath))
            {
                string pathToJson = $"{CacheFolder}{(StorageDrive)driveLetter}.json";
                if (File.Exists(pathToJson))
                {
                    DRIVE_FOLDER.TryAdd((StorageDrive)driveLetter, pathToJson);
                    ManagerConsole.WriteInformation("InitializeCache", $"Successfully found {driveLetter}.json on {CacheFolder}");
                }
                else
                {
                    ManagerConsole.WriteWarning("InitializeCache", $"Didn't find {driveLetter}.json from {CacheFolder}");
                }
            }
        }
    }
}
