using System.IO;
using Cysharp.Threading.Tasks;
using Unity.SharpZipLib.Utils;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    [CreateAssetMenu(fileName = "ZipUtilityArchiver", menuName = "Studio-23/SaveSystem/Archiver/ZipUtility", order = 1)]
    internal class ZipUtilityArchiver : ArchiverBase
    {
        public override async UniTask ArchiveFiles(string archiveFilePath, string FolderToArchive)
        {
            await UniTask.RunOnThreadPool(() =>
            {
                ZipUtility.CompressFolderToZip(archiveFilePath, null, FolderToArchive);
            });
        }


        public override async UniTask ExtractFiles(string archiveFilePath, string extractDirectory,
            bool cleanDirectory = true)
        {
            await UniTask.RunOnThreadPool(() =>
            {
                if (!Directory.Exists(extractDirectory)) Directory.CreateDirectory(extractDirectory);
                if (cleanDirectory) Directory.Delete(extractDirectory, true);
                ZipUtility.UncompressFromZip(archiveFilePath, null, extractDirectory);
            });
        }
    }
}