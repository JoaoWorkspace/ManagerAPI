using ManagerAPI.Application.FileArea.Models;
using ManagerApplication.FileArea.Models;
using MediatR;
using System.Diagnostics;

namespace ManagerAPI.Application.FileArea.Queries.GetAllProcessesUsingPath
{
    public class GetAllProcessesUsingPathQuery : IRequest<Dictionary<string, List<RunningProcess>>>
    {
        public Dictionary<StorageDrive ,FileOrFolder?> CachedDrives { get; set; }
        public string Path { get; set; }
        public GetAllProcessesUsingPathQuery(Dictionary<StorageDrive, FileOrFolder?> cachedDrives, string path)
        {
            CachedDrives = cachedDrives;
            Path = path;
        }
    }
}
