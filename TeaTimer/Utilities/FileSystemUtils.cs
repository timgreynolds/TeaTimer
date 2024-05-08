using System;
using System.IO;
using System.Reflection;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer.Utilities
{
    public static class FileSystemUtils
    {
        public static readonly string AppDataPath = Path.Combine(FileSystem.Current.AppDataDirectory, "Application Support", Assembly.GetExecutingAssembly().GetName().Name);

        public static bool AppDataFileExists(string fileName)
        {
            return File.Exists(Path.Combine(AppDataPath, fileName));
        }

        public static string GetAppDataFileFullName(string fileName)
        {
            return Path.Combine(AppDataPath, fileName);
        }

        public static void CopyBundleAppDataResource(string targetFile)
        {
            try
            {
                Directory.CreateDirectory(AppDataPath);
                using (Stream readStream = FileSystem.Current.OpenAppPackageFileAsync(targetFile).Result)
                {
                    using Stream writeStream = File.Create(Path.Combine(AppDataPath, targetFile));
                    readStream.CopyTo(writeStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

