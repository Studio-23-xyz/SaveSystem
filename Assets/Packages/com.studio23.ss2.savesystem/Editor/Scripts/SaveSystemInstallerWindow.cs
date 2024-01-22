using Studio23.SS2.SaveSystem.Utilities;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    public class SaveSystemInstallerWindow : EditorWindow
    {

        [MenuItem("Studio-23/Save System/Install")]
        static void CreateDefaultProvider()
        {
            GameObject saveSystemObject = Resources.Load<GameObject>("SaveSystem/SaveSystem");
            Instantiate(saveSystemObject);
        }



        [MenuItem("Studio-23/Save System/Encryptors/AES")]
        static void CreateAESEncryptor()
        {
            AESEncryptor providerSettings = ScriptableObject.CreateInstance<AESEncryptor>();

            // Create the resource folder path
            string resourceFolderPath = "Assets/Resources/SaveSystem/Encryptors";

            if (!Directory.Exists(resourceFolderPath))
            {
                Directory.CreateDirectory(resourceFolderPath);
            }

            // Create the ScriptableObject asset in the resource folder
            string assetPath = resourceFolderPath + "/AESEncryptor.asset";
            AssetDatabase.CreateAsset(providerSettings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("AES Encryptor created at: " + assetPath);
        }


        [MenuItem("Studio-23/Save System/Archiver/ZipUtility")]
        static void CreateZipUtilityArchiver()
        {
            ZipUtilityArchiver providerSettings = ScriptableObject.CreateInstance<ZipUtilityArchiver>();

            // Create the resource folder path
            string resourceFolderPath = "Assets/Resources/SaveSystem/Archivers";

            if (!Directory.Exists(resourceFolderPath))
            {
                Directory.CreateDirectory(resourceFolderPath);
            }

            // Create the ScriptableObject asset in the resource folder
            string assetPath = resourceFolderPath + "/ZipUtility.asset";
            AssetDatabase.CreateAsset(providerSettings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ZipUtility Archiver created at: " + assetPath);
        }


    }
}