using AutoMapper;
using Cqrs.Domain;
using ManagerAPI.Application.TorrentArea.Dtos;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;

namespace ManagerAPI.Application.TorrentArea.Commands.CreateDriveFolderJson;

public class CreateDriveFolderJsonCommandHandler : IRequestHandler<CreateDriveFolderJsonCommand, FolderDto>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public CreateDriveFolderJsonCommandHandler(
        IMediator mediator,
        IMapper mapper
        //ITorrentRepository torrentRepository
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<FolderDto> Handle(CreateDriveFolderJsonCommand request, CancellationToken cancellationToken)
    {
        var result = await GetDirectoryAsFolder(request.FileOrFolderPaths[0], request.maxDepth);
        var mappedResult = this.mapper.Map<FolderDto>(result);
        return mappedResult;
    }

    /// <summary>
    /// Returns a Folder and all it's subsequent sub-Folders down to an estabilished maximumDepth
    /// </summary>
    /// <param name="path">The base folder</param>
    /// <param name="depthLimit">How many subfolder levels we are willing to dig into. 0=Unlimited</param>
    /// <returns></returns>
    public async Task<FolderDto> GetDirectoryAsFolder(string path, int maxDepth = 0)
    {
        FolderDto folder = await GetDirectoryAsFolder(
            directory: new DirectoryInfo(path),
            depthLimit: maxDepth);
        return folder;
    }

    /// <summary>
    /// Transforms the whole Folder Structure in the given path into a FolderDto
    /// </summary>
    /// <param name="directory">The base folder</param>
    /// <param name="depthLimit">How many subfolder levels we are willing to dig into. null = No Depth</param>
    /// <returns></returns>
    public async Task<FolderDto> GetDirectoryAsFolder(DirectoryInfo directory, int depth = 0, int depthLimit = 0)
    {
        FolderDto folder = new(FileOrFolder.Folder, directory.FullName, directory.Name, depth);
        if (depthLimit == 0 || depthLimit > folder.Depth)
        {
            foreach (DirectoryInfo d in directory.EnumerateDirectories())
            {
                try
                {
                    folder.Files?.Add(await GetDirectoryAsFolder(d, folder.Depth + 1, depthLimit));
                    folder.FolderCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }
        foreach (FileInfo f in directory.GetFiles())
        {
            folder.Files?.Add(new FolderDto(FileOrFolder.File, f.FullName, f.Name, folder.Depth, fileSizeBytes: f.Length));
        }
        return folder;
    }
}
