using FileTypeChecker;
using FileTypeChecker.Abstracts;
using ManagerAPI.Application.ExceptionHandling;
using ManagerAPI.Application.FileArea.Models.Enums;
using ManagerAPI.Application.TorrentArea.Models.Enum;
using NsfwSpyNS;
using System.Text.RegularExpressions;

namespace ManagerAPI.Application.FileArea.Models;

public class FileOrFolder
{
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
                Files = new List<FileOrFolder>();
                break;
            case FileFolderSwitch.File:
                Bytes = fileSizeBytes ?? 0;
                FileSize = FileUtils.FileSizeFormatter(Bytes.Value);
                Extension = TagFileType(FullPath);
                Classification = Classify(folderPath);
                break;
        }
    }
    public string FullPath { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public FileFolderSwitch FileFolderSwitch { get; set; } = FileFolderSwitch.Folder;
    public int Depth { get; set; } = 0;
    public string Hash { get; set; } = string.Empty;
    public bool IsBeingSeeded { get; set; } = false;
    public long? Bytes { get; set; }    
    public string? FileSize { get; set; }
    public string? Extension { get; set; }
    public SafeForWork Classification { get; set; }
    public int? FolderCount { get; set; }
    public List<FileOrFolder>? Files { get; set; }


    

    /// <summary>
    /// Returns a tag(Extension) to the File regarding it's contents (using the magic bytes)
    /// or tags it as an unrecognized (which means Potential Virus or just unable to detect using the magic bytes) therefore unsafe.
    /// </summary>
    /// <param name="path">The pathway to the specific file.</param>
    /// <returns>The extension recognized on the file.</returns>
    public string TagFileType(string path)
    {
        using (var fileStream = File.OpenRead(path))
        {
            var isRecognizableType = FileTypeValidator.IsTypeRecognizable(fileStream);

            if (!isRecognizableType)
            {
                return $"Unrecognizable({Name.Split(".")[Name.Split(".").Length-1]})";
            }

            IFileType fileType = FileTypeValidator.GetFileType(fileStream);
            return $"{fileType.Name}({fileType.Extension.ToLowerInvariant()})";
        }
    }

    /// <summary>
    /// Classifies a File as Porn, Hentai, Sexy, or Safe for Work
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private SafeForWork Classify(string path)
    {
        try
        {
            SafeForWork result;
            var nsfwSpy = new NsfwSpy();
            var fileBytes = File.ReadAllBytes(path);
            var info = new FileInfo(path);
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
                    var evalImage = nsfwSpy.ClassifyImage(path);
                    result =
                        evalImage.Pornography > 0.5 ? SafeForWork.Pornography
                        : evalImage.Hentai > 0.5 ? SafeForWork.Hentai
                        : evalImage.Sexy > 0.5 ? SafeForWork.Sexy
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
            ManagerApplicationConsole.WriteException("FolderDto.Classify", $"Wasn't able to classify the file at {path}.", ex);
            return SafeForWork.Unclassified;
        }

    }
}
