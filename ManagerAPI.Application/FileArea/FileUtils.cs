using ManagerAPI.Application.ExceptionHandling;
using ManagerApplication.FileArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ManagerAPI.Application.FileArea
{
    public static class FileUtils
    {
        /// <summary>
        /// Matches the first letter of the string's fullPath
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ExtractDriveLetter(string fullPath)
        {
            try
            {
                Regex extractDriveLetter = new(@"[a-zA-Z](?=:)");
                return extractDriveLetter.Match(fullPath).Value;
            }catch(Exception ex)
            {
                ManagerApplicationConsole.WriteException("FileUtils.ExtractDriveLetter", $"Wasn't able to extract drive letter from {fullPath}", ex);
                throw;
            }
        }

        public static StorageDrive? ToStorageDrive(string driveLetter)
        {
            switch (driveLetter.ToUpper())
            {
                case "A": return StorageDrive.A;
                case "B": return StorageDrive.B;
                case "C": return StorageDrive.C;
                case "D": return StorageDrive.D;
                case "E": return StorageDrive.E;
                case "F": return StorageDrive.F;
                case "G": return StorageDrive.G;
                case "H": return StorageDrive.H;
                case "I": return StorageDrive.I;
                case "J": return StorageDrive.J;
                case "K": return StorageDrive.K;
                case "L": return StorageDrive.L;
                case "M": return StorageDrive.M;
                case "N": return StorageDrive.N;
                case "O": return StorageDrive.O;
                case "P": return StorageDrive.P;
                case "Q": return StorageDrive.Q;
                case "R": return StorageDrive.R;
                case "S": return StorageDrive.S;
                case "T": return StorageDrive.T;
                case "U": return StorageDrive.U;
                case "V": return StorageDrive.V;
                case "W": return StorageDrive.W;
                case "X": return StorageDrive.X;
                case "Y": return StorageDrive.Y;
                case "Z": return StorageDrive.Z;
            }
            return null;
        }

        /// <summary>
        /// Formats into smaller numbers the Bytes (and adds the correct suffix)
        /// </summary>
        /// <param name="bytes">File Size</param>
        /// <returns>Abbreviated File Size</returns>
        public static string FileSizeFormatter(long bytes)
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
            return string.Format("{0:n2}{1}", number, suffixes[counter]);
        }
    }
}
