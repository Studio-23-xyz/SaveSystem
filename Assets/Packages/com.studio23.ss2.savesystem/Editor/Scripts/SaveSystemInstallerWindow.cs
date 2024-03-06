using System.IO;
using Studio23.SS2.SaveSystem.Utilities;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    public class SaveSystemInstallerWindow : EditorWindow
    {
        [MenuItem("Studio-23/Save System/Install")]
        private static void CreateSaveSystem()
        {
            var saveSystemObject = Resources.Load<GameObject>("SaveSystem/SaveSystem");

            Instantiate(saveSystemObject).name = "SaveSystem";
        }


        [MenuItem("Studio-23/Save System/Encryptors/AES")]
        private static void CreateAESEncryptor()
        {
            var providerSettings = CreateInstance<AESEncryptor>();

            // Create the resource folder path
            var resourceFolderPath = "Assets/Resources/SaveSystem/Encryptors";

            if (!Directory.Exists(resourceFolderPath)) Directory.CreateDirectory(resourceFolderPath);

            // Create the ScriptableObject asset in the resource folder
            var assetPath = resourceFolderPath + "/AESEncryptor.asset";
            AssetDatabase.CreateAsset(providerSettings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("AES Encryptor created at: " + assetPath);
        }


        [MenuItem("Studio-23/Save System/Archiver/ZipUtility")]
        private static void CreateZipUtilityArchiver()
        {
            var providerSettings = CreateInstance<ZipUtilityArchiver>();

            // Create the resource folder path
            var resourceFolderPath = "Assets/Resources/SaveSystem/Archivers";

            if (!Directory.Exists(resourceFolderPath)) Directory.CreateDirectory(resourceFolderPath);

            // Create the ScriptableObject asset in the resource folder
            var assetPath = resourceFolderPath + "/ZipUtility.asset";
            AssetDatabase.CreateAsset(providerSettings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ZipUtility Archiver created at: " + assetPath);
        }
    }
}