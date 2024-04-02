using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    public abstract class ArchiverBase : ScriptableObject
    {
        public abstract UniTask ArchiveFiles(string archiveFilePath, string FolderToArchive);

        public abstract UniTask ExtractFiles(string archiveFilePath, string extractDirectory,
            bool cleanDirectory = true);
    }
}