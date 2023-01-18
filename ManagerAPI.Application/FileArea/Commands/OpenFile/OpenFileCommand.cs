using MediatR;
using System.Diagnostics;

namespace ManagerAPI.Application.FileArea.Commands.OpenFile;

public class OpenFileCommand : IRequest<ProcessStartInfo?>
{
    public string FullFilePath { get; set; }
    public OpenFileCommand(string fullFilePath)
    {
        FullFilePath = fullFilePath;
    }
}