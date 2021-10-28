using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

using PTASImageManipulation;

namespace PTASCreateFileSizes
{
    internal class Program
    {
        private static IConfigurationRoot GetConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables()
                          .AddCommandLine(args)
                          .Build();
        }

        private static CloudFileDirectory GetRootDir(string StorageConnection, string ShareName)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(StorageConnection);
            CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();

            CloudFileShare cloudFileShare =
            cloudFileClient.GetShareReference(ShareName);
            CloudFileDirectory rootDir =
cloudFileShare.GetRootDirectoryReference();
            return rootDir;
        }

        private static IEnumerable<(string fileName, Stream stream)> GetSVGStreams(Stream svg, string fileName, bool needsSmall, bool needsMed)
        {
            var extension = Path.GetExtension(fileName);
            if (needsSmall)
            {
                var m = new MemoryStream();
                svg.CopyTo(m);
                yield return (fileName.Replace(extension, "-small" + extension), m);
            }
            if (needsMed)
            {
                var m = new MemoryStream();
                svg.CopyTo(m);
                yield return (fileName.Replace(extension, "-med" + extension), m);
            }
        }

        private static IEnumerable<(string fileName, Stream stream)> GetStreams(Image img, string fileName, bool needsSmall, bool needsMed)
        {
            var extension = Path.GetExtension(fileName);
            if (needsSmall)
            {
                var s = ImageUtilities.GetImageStream(img, ImageUtilities.SmallWidth, extension);
                yield return (fileName.Replace(extension, "-small" + extension), s);
            }
            if (needsMed)
            {
                var s = ImageUtilities.GetImageStream(img, ImageUtilities.MedWidth, extension);
                yield return (fileName.Replace(extension, "-med" + extension), s);
            }
        }

        private static bool HasSize(CloudFile f)
        {
            string path = f.Uri.LocalPath.ToLower();
            return path.Contains("-small") || path.Contains("-med");
        }

        private static void Main(string[] args)
        {
            IConfiguration Configuration = GetConfiguration(args);
            string shareName = Configuration["MediaShare"];
            string startAt = Configuration["StartAt"] ?? "/";
            CloudFileDirectory rootDir = GetRootDir(
                Configuration["StorageConnection"],
                shareName);
            _ = ProcessDir(rootDir, 0, $"/{shareName}{startAt}");
            Console.ReadLine();
        }

        private static async Task ProcessDir(CloudFileDirectory currentDir, int depth, string startAt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(new string('.', depth * 1));
            Console.WriteLine("DIRECTORY: " + currentDir.StorageUri.PrimaryUri.LocalPath);
            if (!NeedsProcessing(startAt, currentDir.Uri.LocalPath))
            {
                return;
            }
            FileContinuationToken continuationToken = null;
            var allFiles = new List<IListFileItem>();
            do
            {
                var response = await currentDir.ListFilesAndDirectoriesSegmentedAsync(continuationToken);
                allFiles.AddRange(response.Results);
                continuationToken = response.ContinuationToken;
            } while (continuationToken != null);

            var directories = allFiles
                .Select(l => l as CloudFileDirectory)
                .Where(l => !(l is null));
            string localPath = currentDir.Uri.LocalPath;
            var files = allFiles
                .Select(l => l as CloudFile)
                .Where(l => !(l is null));

            var originalFiles = files.Where(itm => !HasSize(itm));
            var withSizeFiles = files
                .Where(itm => HasSize(itm));

            var withAttachedFiles = originalFiles.Select(rsf =>
            {
                var match = rsf.Uri.LocalPath.ToLower();
                var items = withSizeFiles
                    .Where(wsf => match.Equals(wsf.Uri.LocalPath.ToLower().Replace("-small", "").Replace("-med", "")));
                return new { rsf, items };
            })
                .Where(x => x.items.Count() < 2)
                .Select(itm => new
                {
                    itm.rsf,
                    needsMed = !itm.items.Any(other => other.Uri.LocalPath.ToLower().Contains("-med")),
                    needsSmall = !itm.items.Any(other => other.Uri.LocalPath.ToLower().Contains("-small")),
                });
            await Task.WhenAll(withAttachedFiles
                .Select(f => ProcessFile(
                    currentDir,
                    f.rsf,
                    f.needsSmall,
                    f.needsMed,
                    depth)));
            foreach (var d in directories)
            {
                await ProcessDir(d, depth + 1, startAt);
            }
            Console.WriteLine("Done:" + localPath);
        }

        private static bool NeedsProcessing(string startAt, string localPath)
        {
            var l = Math.Min(startAt.Length, localPath.Length);
            localPath = localPath.Substring(0, l);
            startAt = startAt.Substring(0, l);
            bool needsProcessing = string.Compare(localPath, startAt, true) >= 0;
            return needsProcessing;
        }

        private static async Task ProcessFile(CloudFileDirectory currentDir, CloudFile item, bool needsSmall, bool needsMed, int depth)
        {
            if (!item.Uri.LocalPath.IsImageExtension())
            {
                return;
            }
            var fileName = item.Name;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string('.', depth * 1));
            Console.WriteLine($"{ToCheck(needsSmall)}{ToCheck(needsMed)}-{fileName}");

            try
            {
                if (item.Uri.LocalPath.ToLower().Contains("wmf"))
                {
                    Console.WriteLine(fileName);
                }
                if (item.Uri.LocalPath.IsImageExtension())
                {
                    await CreateImageSizes(currentDir, item, needsSmall, needsMed, depth, fileName);
                }
                else if (item.Uri.LocalPath.IsSVGExtension())
                {
                    await CreateSVGs(currentDir, item, needsSmall, needsMed, depth, fileName);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"Failed on {fileName}. {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task CreateImageSizes(CloudFileDirectory currentDir, CloudFile item, bool needsSmall, bool needsMed, int depth, string fileName)
        {
            var s = await item.OpenReadAsync();
            var m = new MemoryStream();
            await s.CopyToAsync(m);
            m.Seek(0, 0);
            Image img = Image.FromStream(m);
            var streams = GetStreams(img, fileName, needsSmall, needsMed).ToList();

            var tasks = streams.Select(streamInfo =>
            {
                var fileRef = currentDir.GetFileReference(streamInfo.fileName);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(new string('.', depth * 1) + streamInfo.fileName);
                return fileRef.UploadFromStreamAsync(streamInfo.stream);
            });
            await Task.WhenAll(tasks);
        }

        private static async Task CreateSVGs(CloudFileDirectory currentDir, CloudFile item, bool needsSmall, bool needsMed, int depth, string fileName)
        {
            var svgStream = await item.OpenReadAsync();
            var streams = GetSVGStreams(svgStream, fileName, needsSmall, needsMed).ToList();

            var tasks = streams.Select(streamInfo =>
            {
                var fileRef = currentDir.GetFileReference(streamInfo.fileName);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(new string('.', depth * 1) + streamInfo.fileName);
                return fileRef.UploadFromStreamAsync(streamInfo.stream);
            });
            await Task.WhenAll(tasks);
        }

        private static string ToCheck(bool needsMed) => needsMed ? "Y" : "N";
    }
}