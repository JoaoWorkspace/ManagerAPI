using AutoMapper;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.FileArea.Models.Enums;
using ManagerApplication.FileArea.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ManagerAPI.Application.FileArea.Queries.GetAllProcessesUsingPath
{
    public class GetAllProcessesUsingPathQueryHandler : IRequestHandler<GetAllProcessesUsingPathQuery, Dictionary<string, List<RunningProcess>>>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        public GetAllProcessesUsingPathQueryHandler(
            IMediator mediator,
            IMapper mapper
            )
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Dictionary<string, List<RunningProcess>>> Handle(GetAllProcessesUsingPathQuery query, CancellationToken cancellationToken)
        {
            FileOrFolder? cachedDrive = query.CachedDrives[FileUtils.ToStorageDrive(query.Path.Substring(0, 1)).Value];
            FileOrFolder? pathFileOrFolder = cachedDrive?.GetInnerFileOrFolder(query.Path);
            return pathFileOrFolder == null ? new() { { "Path doesn't exist in any mapped drive.", new() } } : await GetAllRunningProcessesForAllPathFiles(pathFileOrFolder);
        }

        public async Task<Dictionary<string, List<RunningProcess>>> GetAllRunningProcessesForAllPathFiles(FileOrFolder fileOrFolder)
        {
            if (fileOrFolder.FileFolderSwitch.Equals(FileFolderSwitch.File))
            {
                return new() { { fileOrFolder.Name, await GetAllRunningProcessesForFile(fileOrFolder) } };
            }
            else
            {
                Dictionary<string, List<RunningProcess>> result = new();
                foreach (FileOrFolder file in fileOrFolder.GetInnerFiles(true))
                {
                    result.Add(file.Name, await GetAllRunningProcessesForFile(file));
                }
                return result;
            }
        }

        public async Task<List<RunningProcess>> GetAllRunningProcessesForFile(FileOrFolder file)
        {
            var allProcesses = FileUtils.WhoIsLocking(file.FullPath);
            var mappedProcesses = this.mapper.Map<List<RunningProcess>>(allProcesses);
            return mappedProcesses;
        }

    }
}
