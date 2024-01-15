using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    internal abstract class ArchiverBase : ScriptableObject
    {

        internal abstract  UniTask ArchiveFiles(string archiveFilePath, string FolderToArchive);

        internal abstract  UniTask ExtractFiles(string archiveFilePath, string extractDirectory, bool cleanDirectory = true);
        
    }
}