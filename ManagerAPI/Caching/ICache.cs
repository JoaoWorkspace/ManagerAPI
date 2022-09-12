using ManagerAPI.Request;
using ManagerApplication.FileArea.Models;

namespace ManagerAPI.Caching;
public interface ICache
{
    public Dictionary<StorageDrive, string> DRIVE_FOLDER { get; set; }
}
