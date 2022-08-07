namespace ManagerAPI.Application.TorrentArea.Dtos.Enums;

public enum ActionConnector
{
    Inside = 0, //When searching inside a set of folders/files
    Contains = 1, //When want to search on paths if they contain a file
    Compare = 2, //When trying to compare folders/files and get the differences between them
    Equals = 3 //When trying to check if two files/folders are the exact same
}
