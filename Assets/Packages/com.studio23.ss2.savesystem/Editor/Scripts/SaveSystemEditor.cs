using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Data;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    [CustomEditor(typeof(Core.SaveSystem))]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private int _slotIndex = 0; // Default slot index
        private bool _showDebugTools = true;
        private bool _showFileKeys = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Core.SaveSystem saveSystem = (Core.SaveSystem)target;

            // Start Debug Tools foldout section
            _showDebugTools = EditorGUILayout.BeginFoldoutHeaderGroup(_showDebugTools, "Debug Tools");

            if (_showDebugTools)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUI.backgroundColor = Color.blue;
                if (GUILayout.Button("Initialize Save System"))
                {
                    saveSystem.Initialize().Forget();
                }

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Select Last Selected Slot"))
                {
                    _ = saveSystem.SelectLastSelectedSlot();
                }

                EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = Color.white;
                _slotIndex = EditorGUILayout.IntField("Slot Index", _slotIndex);


                if (GUILayout.Button("Select Slot"))
                {
                    _=saveSystem.SelectSlot(_slotIndex);
                }



                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginVertical();



                SaveSlot _selectedSlot = saveSystem._slotProcessor._selectedSlot;


                if (_selectedSlot != null)
                {

                    EditorGUILayout.LabelField("Selected Slot Information", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Name: {_selectedSlot.Name}");
                    EditorGUILayout.LabelField($"Description: {_selectedSlot.Description}");
                    EditorGUILayout.LabelField($"TimeStamp: {_selectedSlot.TimeStamp}");
                    EditorGUILayout.LabelField($"BackupStamp: {_selectedSlot.BackupStamp}");
                    EditorGUILayout.LabelField($"HasBackup: {_selectedSlot.HasBackup}");

                    EditorGUILayout.Space();


                    _showFileKeys = GUILayout.Toggle(_showFileKeys, "Show File Details");
                    int fileCount = 0;
                    if (_selectedSlot.FileKeys != null && _showFileKeys)
                    {
                        foreach (var fileKey in _selectedSlot.FileKeys)
                        {
                            EditorGUILayout.LabelField($"{++fileCount})File: {fileKey.Key}( Size: {fileKey.Value} Bytes)");
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"{fileCount} Files in the meta");
                    }

                }

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Save All"))
                {
                    saveSystem.Save().Forget();
                }

                if (GUILayout.Button("Save Dirty"))
                {
                    saveSystem.Save(true).Forget();
                }


                GUI.backgroundColor = Color.white; // Reset color

      
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Sync Selected Slot Data"))
                {
                    saveSystem.SyncSelectedSlotData().Forget();
                }

                if (GUILayout.Button("Load"))
                {
                    saveSystem.Load().Forget();
                }
                GUI.backgroundColor = Color.white; // Reset color

                if (GUILayout.Button("Restore Backup"))
                {
                    saveSystem.RestoreBackup().Forget();
                }

                if (GUILayout.Button("Clear Selected Slot"))
                {
                    saveSystem.ClearSelectedSlot().Forget();
                }

                if (GUILayout.Button("Clear All Slots"))
                {
                    saveSystem.ClearAllSlots().Forget();
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }

            // End Debug Tools foldout section
            EditorGUILayout.EndFoldoutHeaderGroup();
        }



    }
}