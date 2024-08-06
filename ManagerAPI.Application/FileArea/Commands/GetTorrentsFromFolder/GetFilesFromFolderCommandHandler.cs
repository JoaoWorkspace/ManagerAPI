using AutoMapper;
using MediatR;

namespace ManagerAPI.Application.MusicArea.Commands.GetFilesFromFolder;

public class GetFilesFromFolderCommandHandler : IRequestHandler<GetFilesFromFolderCommand, List<string>>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    public GetFilesFromFolderCommandHandler(   
        IMediator mediator,
        IMapper mapper
    )
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<List<string>> Handle(GetFilesFromFolderCommand request, CancellationToken cancellationToken)
    {
        List<string> filesToReturn = new();
        foreach(string folderPath in request.FileOrFolderPaths)
        {
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            filesToReturn.AddRange(directory.GetFiles().Where(file => request.FileExtensions.Contains(file.Extension)).Select(file => file.FullName));
        }
        return filesToReturn;
    }
}
