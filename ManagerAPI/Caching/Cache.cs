using ManagerAPI.Request;

namespace ManagerAPI.Caching;
/// <summary>
/// Cache for DriveJson, storing the entire folder structure as an object for each System Drive.
/// The value for each drive is null if there's no cached JSON
/// When it exists, each letter has the string path to the JSON file storing it's structure
/// </summary>
public class Cache : ICache
{
    public Dictionary<StorageDrive, string> DRIVE_FOLDER { get; set; } = new Dictionary<StorageDrive, string>();
}
