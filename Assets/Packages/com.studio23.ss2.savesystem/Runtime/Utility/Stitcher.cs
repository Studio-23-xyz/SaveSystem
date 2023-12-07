using Cysharp.Threading.Tasks;
using Unity.SharpZipLib.Utils;

namespace Studio23.SS2.SaveSystem.Utilities
{
    internal class Stitcher
    {


        internal async UniTask ArchiveFiles(string archiveFilePath, string FolderToArchive)
        {
            
            await UniTask.RunOnThreadPool(() =>
            {
                ZipUtility.CompressFolderToZip(archiveFilePath, null, FolderToArchive);
            });
        }

        internal async UniTask ExtractFiles(string archiveFilePath, string extractDirectory)
        {
           
            await UniTask.RunOnThreadPool(() =>
            {
                ZipUtility.UncompressFromZip(archiveFilePath, null, extractDirectory);
            });
        }


     

    }
}