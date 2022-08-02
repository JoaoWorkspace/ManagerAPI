using ManagerAPI.Application.TorrentArea.Commands;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using MediatR;
using Newtonsoft.Json.Linq;

namespace ManagerAPI.Application.TorrentArea;

public class TorrentService : ITorrentService
{
    public IMediator mediator;
    public TorrentService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    
    public async Task ProcessTorrentCommand(TorrentCommand command, CancellationToken cancellationToken)
    {
        switch (command.Action)
        {
            case ManagedAction.Search:
                this.mediator.Send(command, cancellationToken);
                break;
            case ManagedAction.Compare:

                break;
            case ManagedAction.Create:

                break;
            case ManagedAction.Validate: 

                break;
            case ManagedAction.Remove: 

                break;
        }
    }

    #region auxTools
    public static string GetDirectoryAsJson(string path)
    {
        return GetDirectoryAsJObject(new DirectoryInfo(path)).ToString();
    }

    public static JObject GetDirectoryAsJObject(DirectoryInfo directory)
    {
        JObject obj = new();
        foreach (DirectoryInfo d in directory.EnumerateDirectories())
        {
            obj.Add(d.Name, GetDirectoryAsJObject(d));
        }
        foreach (FileInfo f in directory.GetFiles())
        {
            obj.Add(f.Name, JValue.CreateNull());
        }
        return obj;
    }
    #endregion auxTools
}
