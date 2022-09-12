using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea.Models;
using ManagerAPI.Application.FileArea.Models.Enums;
using ManagerAPI.Application.TorrentArea.Models;
using QBittorrent.Client;
using System.Text.RegularExpressions;

namespace ManagerAPI.Application.TorrentArea;

public static class TorrentUtils
{
    public static List<SimpleTorrentInfo> SimplifyTorrentInfo(List<TorrentInfo> torrents, string categoryName = "")
    {
        List<SimpleTorrentInfo> simplifiedTorrents = new List<SimpleTorrentInfo>();
        foreach (var torrent in FilterTorrents(torrents, categoryName))
        {
            simplifiedTorrents.Add(new SimpleTorrentInfo(torrent.Hash, torrent.Name, torrent.CurrentTracker, torrent.Category, torrent.SavePath));
        }
        return simplifiedTorrents;
    }

    public static List<TorrentInfo> FilterTorrents(List<TorrentInfo> torrents, string categoryName = "")
    {
        return torrents.Where(t => t.Category == categoryName || categoryName == "").ToList();
    }

    public static void MatchQbitTorrentsWithFileTorrents(List<SimpleTorrentInfo> filteredTorrents, List<FileOrFolder> torrentFiles)
    {
        foreach (var torrent in filteredTorrents)
        {
            ManagerApplicationConsole.WriteInformation("TorrentUtils.MatchQbitTorrentsWithFileTorrents",
                $"Trying to match any file to the torrent:\n" +
                $"Name={torrent.Name} Hash ={torrent.Hash} DestinationFolder={torrent.DestinationFolder}");
            var matchingFile = torrentFiles.Where(f => f.Name.ToLower().Contains(torrent.Name.ToLower())).FirstOrDefault();
            if (matchingFile != null)
            {
                ManagerApplicationConsole.WriteInformation("TorrentUtils.MatchQbitTorrentsWithFileTorrents",
               $"Found a match!\n" +
               $"Name={matchingFile.Name} Hash={matchingFile.Hash} Extension={matchingFile.Extension}");
            }
            torrent.TorrentFile = matchingFile;
        }
    }

    /// <summary>
    /// Examples: https://web.site:port/MyKey123/announce | udp://web.site:port/announce | http://web.site.without.port/MyKey123/announce
    /// Getting Key: (?<=.*:\/\/.*:?.*\/).*(?=\/)
    /// (?<= .*:\/\/.*:?.*\/ ) will get "anything[://]anything+optional(:)+anything+[/]" and removes it from beginning of the trackerURL leaving MyKey123/announce
    /// .* in the middle is capturing everything after
    /// (?=\/) in the end is capturing everything from the following [/] forward which is "/announce" therefore only leaving MyKey123 in place
    /// Getting Port: (?<= .*:\/\/.*:)\d*
    /// (?<= .*:\/\/.*:) will get "anything[://]anything" and removes it from the beginning of the trackerURL leaving web.site:port/MyKey123/announce
    /// \d* will get the next full sequence of numbers which are the port numbers before the [/] and after the [:]
    /// Getting Site:(?<=.*:\/\/)[a-zA-Z0-9.-]*
    /// (?<= .*:\/\/) will get "anything[://]" and removes it from the beginning of the trackerURL leaving web.site:port/MyKey123/announce
    /// [a-zA-Z0-9.-]* will get the next full sequence of characters on that range, which means we'll be excluding all special characters except [.] and [-] leaving web.site
    /// </summary>
    /// <param name="trackerURL">The full trackerURL</param>
    /// <returns></returns>
    public static TorrentTrackerInfo TransformTrackerURL (string trackerURL)
    {
        Regex extractTrackerSite = new Regex(@"(?<=.*:\/\/)[a-zA-Z0-9.-]*");
        Regex extractTrackerPort = new Regex(@"(?<=.*:\/\/.*:)\d*(?=\/*)");
        Regex extractTrackerPersonalKey = new Regex(@"(?<=.*:\/\/.*:?.*\/).*(?=\/)");
        string site = extractTrackerSite.IsMatch(trackerURL) ? extractTrackerSite.Match(trackerURL).Value : string.Empty;
        string port = extractTrackerPort.IsMatch(trackerURL) ? extractTrackerPort.Match(trackerURL).Value : string.Empty;
        string personalKey = extractTrackerPersonalKey.IsMatch(trackerURL) ? extractTrackerPersonalKey.Match(trackerURL).Value : string.Empty;
        return new TorrentTrackerInfo(trackerURL, site, port, personalKey);
    }

    public static List<FileOrFolder> GetAllTorrentFilesFromTorrentDirectoryList(List<string>? torrentDirectoryList = null)
    {
        List<FileOrFolder> torrentFiles = new List<FileOrFolder>();
        if(torrentDirectoryList != null && torrentDirectoryList.Any())
        {
            foreach(var torrentDirectory in torrentDirectoryList)
            {
                try
                {
                    FileOrFolder directory = GetDirectoryAsFolder(torrentDirectory, 0);
                    torrentFiles = GetAllTorrentsInDirectoryRecursive(directory);
                }catch(Exception ex)
                {
                    ManagerApplicationConsole.WriteException("TorrentUtils.GetAllTorrentFilesFromTorrentDirectoryList", $"{torrentDirectory} couldn't be converted into a FileOrFoler Folder", ex);
                }
                
            }
        }
        return torrentFiles;
    }

    public static List<FileOrFolder> GetAllTorrentsInDirectoryRecursive(FileOrFolder directory)
    {
        List<FileOrFolder> torrentFiles = new List<FileOrFolder>();
        foreach (FileOrFolder file in directory.Files)
        {
            if (file.FileFolderSwitch == FileFolderSwitch.File)
            {
                torrentFiles.Add(file);
            }
            else
            {
                torrentFiles.AddRange(GetAllTorrentsInDirectoryRecursive(file));
            }
        }
        return torrentFiles;
    }

    /// <summary>
    /// Examples: 50TB (50.3%) of my seedSize
    /// Getting Porcentage: (?<=.*\()[0-9.,]*(?=%\))
    /// (?<=.*\() Will get everything up to the first left parenthesis [(] and removes it from the beginning of the summaryIncludingPercentage
    /// [0-9.,]* will catch the entire percentage double
    /// (?=%\)) will catch [%)] which is the percentage symbol and the closing parenthesis
    /// </summary>
    /// <param name="summaryIncludingPercentage"></param>
    /// <returns></returns>
    public static double GetInnerPercentage(string summaryIncludingPercentage)
    {
        Regex extractPorcentage = new Regex(@"(?<=.*\()[0-9.,]*(?=%\))");
        double porcentage = extractPorcentage.IsMatch(summaryIncludingPercentage) ? double.Parse(extractPorcentage.Match(summaryIncludingPercentage).Value) : 0.0;
        return porcentage;
    }

    /// <summary>
    /// Returns a Folder and all it's subsequent sub-Folders down to an estabilished maximumDepth
    /// </summary>
    /// <param name="path">The base folder</param>
    /// <param name="maxDepth">How many subfolder levels we are willing to dig into. 0=Unlimited</param>
    /// <returns></returns>
    public static FileOrFolder GetDirectoryAsFolder(string path, int maxDepth = 0)
    {
        FileOrFolder folder = GetDirectoryAsFolder(
            directory: new DirectoryInfo(path),
            depthLimit: maxDepth);
        return folder;
    }

    /// <summary>
    /// Transforms the whole Folder Structure in the given path into a FileOrFolder
    /// </summary>
    /// <param name="directory">The base folder</param>
    /// <param name="depth">Sets the current folder/file's depth - Should always be 0 when calling this method, since the only time it's not 0 is when the method calls itself recurvsively.</param>
    /// <param name="depthLimit">How many subfolder levels we are willing to dig into. null = No Depth</param>
    /// <returns></returns>
    public static FileOrFolder GetDirectoryAsFolder(DirectoryInfo directory, int depth = 0, int depthLimit = 0)
    {
        FileOrFolder folder = new(FileFolderSwitch.Folder, directory.FullName, directory.Name, depth);
        if (depthLimit == 0 || depthLimit > folder.Depth)
        {
            foreach (DirectoryInfo d in directory.EnumerateDirectories())
            {
                try
                {
                    folder.Files?.Add(GetDirectoryAsFolder(d, folder.Depth + 1, depthLimit));
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
            folder.Files?.Add(new FileOrFolder(FileFolderSwitch.File, f.FullName, f.Name, folder.Depth, fileSizeBytes: f.Length));
        }
        return folder;
    }
}
