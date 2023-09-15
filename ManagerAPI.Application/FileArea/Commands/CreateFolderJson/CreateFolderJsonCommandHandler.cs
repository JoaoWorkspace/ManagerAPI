using AutoMapper;
using Cqrs.Domain;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.FileArea.Models.Enums;
using MediatR;
using System.Web.Http.Results;

namespace ManagerAPI.Application.FileArea.Commands.CreateFolderJson;

public class CreateFolderJsonCommandHandler : IRequestHandler<CreateFolderJsonCommand, string>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public CreateFolderJsonCommandHandler(
        IMediator mediator,
        IMapper mapper
        )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<string> Handle(CreateFolderJsonCommand request, CancellationToken cancellationToken)
    {
        DateTime start = DateTime.UtcNow;

        try
        {
            var result = await GetDirectoryAsFolder(request.FileOrFolderPaths[0], request.MaxDepth);
            await SerializeFolderToSavePath(result, request.PathToSaveJson, cancellationToken);
            var mappedResult = this.mapper.Map<FileOrFolder>(result);
            return "Success";
        }
        catch(Exception ex)
        {
            return "Exception";
        }
    }

    /// <summary>
    /// Returns a Folder and all it's subsequent sub-Folders down to an estabilished maximumDepth
    /// </summary>
    /// <param name="path">The base folder</param>
    /// <param name="depthLimit">How many subfolder levels we are willing to dig into. 0=Unlimited</param>
    /// <returns></returns>
    public async Task<FileOrFolder> GetDirectoryAsFolder(string path, int maxDepth = 0)
    {
        FileOrFolder folder = await GetDirectoryAsFolder(
            directory: new DirectoryInfo(path),
            depthLimit: maxDepth);
        return folder;
    }

    /// <summary>
    /// Transforms the whole Folder Structure in the given path into a FolderDto
    /// </summary>
    /// <param name="directory">The base folder</param>
    /// <param name="depth">Sets the current folder/file's depth - Should always be 0 when calling this method, since the only time it's not 0 is when the method calls itself recurvsively.</param>
    /// <param name="depthLimit">How many subfolder levels we are willing to dig into. 0 = No Depth</param>
    /// <returns></returns>
    public async Task<FileOrFolder> GetDirectoryAsFolder(DirectoryInfo directory, int depth = 0, int depthLimit = 0)
    {
        FileOrFolder folder = new(FileFolderSwitch.Folder, directory.FullName, directory.Name, depth);
        if (depthLimit == 0 || depthLimit > folder.Depth)
        {
            foreach (DirectoryInfo d in directory.EnumerateDirectories())
            {
                try
                {
                    folder.FilesOrFolders?.Add(await GetDirectoryAsFolder(d, folder.Depth + 1, depthLimit));
                    folder.FolderCount++;
                    folder.Bytes += folder.FilesOrFolders.Single(folder => folder.FullPath == d.FullName).Bytes;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }
        foreach (FileInfo f in directory.GetFiles())
        {
            folder.FilesOrFolders?.Add(new FileOrFolder(FileFolderSwitch.File, f.FullName, f.Name, folder.Depth, fileSizeBytes: f.Length));
            folder.Bytes += f.Length;
        }
        return folder;
    }

    public async Task SerializeFolderToSavePath(FileOrFolder driveFolder, string savePath, CancellationToken cancellationToken)
    {
        try
        {
            //Open CacheFile to write
            string path = $"{savePath}/{driveFolder.Name.Substring(0,1)}.json";
            Directory.CreateDirectory(savePath);
            await using FileStream createStream = File.Create(path);
            //Write the serialized json to file
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, driveFolder, cancellationToken: cancellationToken);
            Console.WriteLine($"Written {driveFolder.Name} inside {path}.");
        }
        catch (Exception ex)
        {
            ManagerApplicationConsole.WriteException("SerializeFolderToSavePath", $"Failed to write {driveFolder.Name} inside {savePath}", ex);
            throw ex;
        }
    }
}
