using Studio23.SS2.SaveSystem.Core;
using UnityEditor;

namespace Studio23.SS2.SaveSystem.Editor
{
    [CustomEditor(typeof(SaveSlotProcessor))]
    public class SaveSlotProcessorEditor : UnityEditor.Editor
    {
        private bool showSlotConfig = true;

        public override void OnInspectorGUI()
        {
            // Draw the default inspector for the ScriptableObject
            DrawDefaultInspector();

            // Cast the target to your ScriptableObject type
            var slotprocessor = (SaveSlotProcessor)target;

            var slotConfig = slotprocessor._slotConfiguration;

            if (slotprocessor != null)
                if (slotConfig != null)
                {
                    showSlotConfig = EditorGUILayout.BeginFoldoutHeaderGroup(showSlotConfig, "Config Data");
                    //EditorGUILayout.LabelField("", EditorStyles.boldLabel);
                    if (showSlotConfig)
                    {
                        EditorGUILayout.IntField("Slot Count", slotConfig.SlotCount);
                        EditorGUILayout.Toggle("Enable Backups", slotConfig.EnableBackups);
                        EditorGUILayout.TextField("Data Folder Name", slotConfig.SlotDatafolderName);
                        EditorGUILayout.TextField("Data Backup File Name", slotConfig.SlotDataBackupFileName);
                        EditorGUILayout.TextField("Metafile Name", slotConfig.SlotMetafileName);
                        EditorGUILayout.TextField("Save File Extension", slotConfig.SaveFileExtention);
                        EditorGUILayout.TextField("Save Root Folder Name", slotConfig.SaveRootFolderName);
                    }

                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
        }
    }
}