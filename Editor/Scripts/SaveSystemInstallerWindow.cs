using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    public class SaveSystemInstallerWindow : EditorWindow
    {
        private int slotCount = 3;
        private bool enableEncryption;
        private string encryptionKey = "1234567891234567"; // 16-byte string
        private string encryptionIV = "0234567891234567"; // 16-byte string
        private string saveRootFolder = "SaveData";


        private Texture _header;

        private bool isValid = true;

        [MenuItem("Studio-23/Save System/Installer")]
        public static void ShowWindow()
        {
           
            SaveSystemInstallerWindow window = GetWindow<SaveSystemInstallerWindow>("Save System Installer");
            window.minSize = new Vector2(400, 200);
        }

        private void OnEnable()
        {
            _header = Resources.Load<Texture>("Header");
        }


        private void OnGUI()
        {

            GUILayout.Box(_header, GUILayout.Height(200), GUILayout.ExpandWidth(true));

            isValid = true;
            GUILayout.Label("Save System Setup Wizard", EditorStyles.boldLabel);

            slotCount = EditorGUILayout.IntField("Slot Count", slotCount);
            enableEncryption = EditorGUILayout.Toggle("Enable Encryption", enableEncryption);
            encryptionKey = EditorGUILayout.TextField("Encryption Key", encryptionKey);


            if (enableEncryption && encryptionKey.Length != 16)
            {
                EditorGUILayout.HelpBox("Must be a 16 byte string", MessageType.Error);
                isValid = false;
            }

            encryptionIV = EditorGUILayout.TextField("Encryption IV", encryptionIV);


            if (enableEncryption && encryptionIV.Length != 16)
            {
                EditorGUILayout.HelpBox("Must be a 16 byte string", MessageType.Error);
                isValid = false;
            }

            saveRootFolder = EditorGUILayout.TextField("Save Root Folder", saveRootFolder);


            if (!isValid) return;

            if (GUILayout.Button("Install SaveSystem"))
            {

                GameObject saveSystemObject = new GameObject("SaveSystem");
                Core.SaveSystem newSaveSystem = saveSystemObject.AddComponent<Core.SaveSystem>();


                newSaveSystem._slotCount = slotCount;
                newSaveSystem._enableEncryption = enableEncryption;
                newSaveSystem._encryptionKey = encryptionKey;
                newSaveSystem._encryptionIV = encryptionIV;
                newSaveSystem._saveRootFolderName = saveRootFolder;

                // Mark the object as dirty to save changes
                EditorUtility.SetDirty(saveSystemObject);
            }
        }
    }
}