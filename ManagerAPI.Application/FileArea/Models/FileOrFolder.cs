using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.MusicArea.Models.Enums;
using ManagerAPI.Application.TorrentArea.Models.Enum;
using NsfwSpyNS;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ManagerAPI.Application.MusicArea.Models;

public class FileOrFolder
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FileFolderSwitch FileFolderSwitch { get; set; } = FileFolderSwitch.Folder;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SafeForWork Classification { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Depth { get; set; } = 0;
    public bool IsBeingSeeded { get; set; } = false;
    public long? Bytes { get; set; }
    public string? FileSize { get { return Bytes > 0 ? FileUtils.FileSizeFormatter(Bytes.Value): "0 Bytes"; } }
    public string? Extension { get; set; }
    public int? FolderCount { get; set; } = 0;
    public List<FileOrFolder>? FilesOrFolders { get; set; }

    /// <summary>
    /// Should never be invoked.
    /// Serves only the purpose of being the binding target for Deserialize. 
    /// </summary>
    public FileOrFolder(){}

    public FileOrFolder(FileFolderSwitch fileFolderSwitch, string folderPath, string folderName, int depth, long? fileSizeBytes=null)
    {
        FullPath = folderPath;
        Name = folderName;
        FileFolderSwitch = fileFolderSwitch;
        Depth = depth;
        switch (fileFolderSwitch)
        {
            case FileFolderSwitch.Folder:
                Bytes = fileSizeBytes ?? 0;
                FilesOrFolders = new List<FileOrFolder>();
                break;
            case FileFolderSwitch.File:
                Bytes = fileSizeBytes ?? 0;
                Extension = Path.GetExtension(Name);
                break;
        }
    }

    #region MainFeatures

    public FileOrFolder? GetInnerFileOrFolder(string path)
    {
        if (this.FullPath.Equals(path)) { 
            return this; 
        }
        else
        {
            if(this.FilesOrFolders?.Count > 0)
            {
                foreach(var file in this.FilesOrFolders)
                {
                    var match = file.GetInnerFileOrFolder(path);
                    if (match != null)
                    {
                        return match;
                    };
                }
            }
        }
        return null;
    }

    public List<FileOrFolder> GetInnerFiles(bool includeInnerFolderFiles)
    {
        List<FileOrFolder> result = new();
        if (this.FileFolderSwitch.Equals(FileFolderSwitch.File))
        {
            result.Add(this);
        }
        else
        {
            var innerFiles = FilesOrFolders?.Where(f => f.FileFolderSwitch.Equals(FileFolderSwitch.File)).ToList();
            var innerFolders = FilesOrFolders?.Where(f => f.FileFolderSwitch.Equals(FileFolderSwitch.Folder)).ToList();
            foreach (var file in innerFiles) { result.Add(file); }
            if (includeInnerFolderFiles)
            {
                foreach(var folder in innerFolders) { result.AddRange(folder.GetInnerFiles(includeInnerFolderFiles)); }
            }
        }
        return result;
    }

    #endregion;

    #region ExtraFeatures

    /// <summary>
    /// Classifies a File as Porn, Hentai, Sexy, or Safe for Work
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private SafeForWork Classify()
    {
        try
        {
            SafeForWork result;
            var nsfwSpy = new NsfwSpy();
            var fileBytes = File.ReadAllBytes(FullPath);
            var info = new FileInfo(FullPath);
            var matches = Regex.Match(Extension, @"\(([^)]+)\)");

            //Captures[1] contains the value between the parentheses
            switch (matches.Groups[1].Value)
            {
                //case "wmv":
                //case "mp4":
                //case "avi":
                //case "webm":
                //case "mkv":
                //    var evalVideo = nsfwSpy.ClassifyVideo(path);
                //    result =
                //        evalVideo.Frames.Any(x => x.Value.Pornography > 0.5) ? SafeForWork.Pornography
                //        : evalVideo.Frames.Any(x => x.Value.Hentai > 0.5) ? SafeForWork.Hentai
                //        : evalVideo.Frames.Any(x => x.Value.Sexy > 0.5) ? SafeForWork.Sexy
                //        : SafeForWork.Safe;
                //    break;

                //case "gif":
                //    var evalGif = nsfwSpy.ClassifyGif(path);
                //    result =
                //        evalGif.Frames.Any(x => x.Value.Pornography > 0.5) ? SafeForWork.Pornography
                //        : evalGif.Frames.Any(x => x.Value.Hentai > 0.5) ? SafeForWork.Hentai
                //        : evalGif.Frames.Any(x => x.Value.Sexy > 0.5) ? SafeForWork.Sexy
                //        : SafeForWork.Safe;
                //    break;

                case "png":
                case "jpg":
                case "webp":
                    var evalImage = nsfwSpy.ClassifyImage(FullPath);
                    result =
                        evalImage.Pornography > 0.85 ? SafeForWork.Pornography
                        : evalImage.Hentai > 0.85 ? SafeForWork.Hentai
                        : evalImage.Sexy > 0.85 ? SafeForWork.Sexy
                        : SafeForWork.Safe;
                    break;
                default:
                    result = SafeForWork.Unclassified;
                    break;
            }
            return result;
        }
        catch (Exception ex)
        {
            ManagerApplicationConsole.WriteException("FolderDto.Classify", $"Wasn't able to classify the file at {FullPath}.", ex);
            return SafeForWork.Unclassified;
        }

    }
    #endregion;
}
