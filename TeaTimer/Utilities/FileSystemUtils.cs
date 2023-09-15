using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer.Utilities
{
    public static class FileSystemUtils
    {
        public static async Task CopyBundleAppDataResource(string targetFile)
        {
            try
            {
                string resourceName = Path.GetFileName(targetFile);
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                using (Stream readStream = await FileSystem.OpenAppPackageFileAsync(resourceName).ConfigureAwait(false))
                {
                    using (Stream writeStream = File.Create(targetFile))
                    {
                        await readStream.CopyToAsync(writeStream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

