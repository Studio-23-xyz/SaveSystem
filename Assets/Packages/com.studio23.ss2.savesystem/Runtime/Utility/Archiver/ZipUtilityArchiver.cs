using Cysharp.Threading.Tasks;
using System.IO;
using Unity.SharpZipLib.Utils;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    [CreateAssetMenu(fileName = "ZipUtilityArchiver", menuName = "Studio-23/SaveSystem/Archiver/ZipUtility", order = 1)]
    internal class ZipUtilityArchiver : ArchiverBase
    {

        internal override async UniTask ArchiveFiles(string archiveFilePath, string FolderToArchive)
        {
            await UniTask.RunOnThreadPool(() =>
            {
                ZipUtility.CompressFolderToZip(archiveFilePath, null, FolderToArchive);
            });
        }


        internal override async UniTask ExtractFiles(string archiveFilePath, string extractDirectory, bool cleanDirectory = true)
        {
            await UniTask.RunOnThreadPool(() =>
            {
                if (cleanDirectory)
                {
                    Directory.Delete(extractDirectory, true);
                }
                ZipUtility.UncompressFromZip(archiveFilePath, null, extractDirectory);
            });
        }
    }
}