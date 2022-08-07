using FileTypeChecker;
using FileTypeChecker.Abstracts;
using ManagerAPI.Application.TorrentArea.Dtos.Enums;
using NsfwSpyNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ManagerAPI.Application.TorrentArea.Dtos
{
    public class FolderDto
    {
        public FolderDto(FileOrFolder fileOrFolder, string folderPath, string folderName, int depth, long? fileSizeBytes=null)
        {
            FullPath = folderPath;
            Name = folderName;
            FileOrFolder = fileOrFolder;
            Depth = depth;
            switch (fileOrFolder)
            {
                case FileOrFolder.Folder:
                    Files = new List<FolderDto>();
                    break;
                case FileOrFolder.File:
                    Bytes = fileSizeBytes ?? 0;
                    FileSize = FileSizeFormatter(Bytes.Value);
                    Extension = TagFileType(FullPath);
                    Classification = Classify(folderPath);
                    break;
            }
        }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public FileOrFolder FileOrFolder { get; set; }
        public int Depth { get; set; }
        public long? Bytes { get; set; }    
        public string? FileSize { get; set; }
        public string? Extension { get; set; }
        public SafeForWork Classification { get; set; }
        public int? FolderCount { get; set; }
        public List<FolderDto>? Files { get; set; }

        /// <summary>
        /// Formats into smaller numbers the Bytes (and adds the correct suffix)
        /// </summary>
        /// <param name="bytes">File Size</param>
        /// <returns>Abbreviated File Size</returns>
        public string FileSizeFormatter(long bytes)
        {
            string[] suffixes =
            { "Bytes", "KB", "MB", "GB", "TB", "PB" };

            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

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
                    return "Unavailable(unrecognized)";
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
            SafeForWork result;
            var nsfwSpy = new NsfwSpy();
            var fileBytes = File.ReadAllBytes(path);
            var info = new FileInfo(path);
            var matches = Regex.Match(Extension, @"\(([^)]+)\)");

            //Captures[1] contains the value between the parentheses
            switch (matches.Captures[0].Value)
            {
                case "wmv":
                case "mp4":
                case "avi":
                case "webm":
                case "mkv":
                    var evalVideo = nsfwSpy.ClassifyVideo(path);
                    result =
                        evalVideo.Frames.Any(x => x.Value.Pornography > 0.5) ?  SafeForWork.Pornography
                        : evalVideo.Frames.Any(x => x.Value.Hentai > 0.5) ? SafeForWork.Hentai
                        : evalVideo.Frames.Any(x => x.Value.Sexy > 0.5) ? SafeForWork.Sexy
                        : SafeForWork.Safe;
                    break;

                case "gif":
                    var evalGif = nsfwSpy.ClassifyGif(path);
                    result =
                        evalGif.Frames.Any(x => x.Value.Pornography > 0.5) ? SafeForWork.Pornography
                        : evalGif.Frames.Any(x => x.Value.Hentai > 0.5) ? SafeForWork.Hentai
                        : evalGif.Frames.Any(x => x.Value.Sexy > 0.5) ? SafeForWork.Sexy
                        : SafeForWork.Safe;
                    break;

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
    }
}
