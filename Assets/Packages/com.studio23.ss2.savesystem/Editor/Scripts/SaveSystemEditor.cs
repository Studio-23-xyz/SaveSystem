using Cysharp.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    [CustomEditor(typeof(Core.SaveSystem))]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private bool _showDebugTools = true;
        private bool _showFileKeys = true;
        private int _slotIndex; // Default slot index
        private string _slotDescription;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var saveSystem = (Core.SaveSystem)target;

            // Start Debug Tools foldout section
            _showDebugTools = EditorGUILayout.BeginFoldoutHeaderGroup(_showDebugTools, "Debug Tools");

            if (_showDebugTools)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUI.backgroundColor = Color.blue;
                if (GUILayout.Button("Initialize Save System")) saveSystem.Initialize().Forget();

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Select Last Selected Slot")) _ = saveSystem.SelectLastSelectedSlot();

                GUI.backgroundColor = Color.white;
                _slotDescription = EditorGUILayout.TextField("Description", _slotDescription);

                EditorGUILayout.BeginHorizontal();

             
                _slotIndex = EditorGUILayout.IntField("Slot Index", _slotIndex);


                if (GUILayout.Button("Select Slot")) _ = saveSystem.SelectSlot(_slotIndex);


                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginVertical();


                var _selectedSlot = saveSystem._slotProcessor._selectedSlot;


                if (_selectedSlot != null)
                {
                    GUI.backgroundColor = Color.black;

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.LabelField("Selected Slot Information", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Name: {_selectedSlot.Name}");
                    EditorGUILayout.LabelField($"Seed: {_selectedSlot.Seed}");
                    EditorGUILayout.LabelField($"Description: {_selectedSlot.Description}");
                    EditorGUILayout.LabelField($"TimeStamp: {_selectedSlot.TimeStamp}");
                    EditorGUILayout.LabelField($"BackupStamp: {_selectedSlot.BackupStamp}");
                    EditorGUILayout.LabelField($"HasBackup: {_selectedSlot.HasBackup}");

                    EditorGUILayout.Space();
                   

                    _showFileKeys = GUILayout.Toggle(_showFileKeys, "Show File Details");
                    var fileCount = 0;
                    if (_selectedSlot.FileKeys != null && _showFileKeys)
                    {
                        foreach (var fileKey in _selectedSlot.FileKeys)
                            EditorGUILayout.LabelField(
                                $"{++fileCount})File: {fileKey.Key}( Size: {fileKey.Value} Bytes)");

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"{fileCount} Files in the meta");
                        EditorGUILayout.LabelField($"Slot is Empty : {_selectedSlot.IsEmpty}");
                        EditorGUILayout.LabelField($"Slot Size : {_selectedSlot.TotalSize} Bytes");
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Save All")) saveSystem.Save($"Editor: {_slotDescription}").Forget();

                if (GUILayout.Button("Save Dirty")) saveSystem.Save($"Editor: {_slotDescription}", true).Forget();


                GUI.backgroundColor = Color.white; // Reset color


                GUI.backgroundColor = Color.yellow;
                if (GUILayout.Button("Sync Selected Slot Data")) saveSystem.SyncSelectedSlotData().Forget();

                if (GUILayout.Button("Load")) saveSystem.Load().Forget();
                GUI.backgroundColor = Color.white; // Reset color

                if (GUILayout.Button("Restore Backup")) saveSystem.RestoreBackup().Forget();

                GUI.backgroundColor = Color.red; // Reset color
                if (GUILayout.Button("Clear Selected Slot")) saveSystem.ClearSelectedSlot().Forget();

                if (GUILayout.Button("Clear Selected Slot From Cloud")) saveSystem.ClearSelectedSlotCloud().Forget();


                if (GUILayout.Button("Clear All Slots")) saveSystem.ClearAllSlots().Forget();

                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Open Save Directory"))
                {
                    var saveFolderPath = Path.Combine(Application.persistentDataPath);
                    OpenInFileExplorer(saveFolderPath);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }

            // End Debug Tools foldout section
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void OpenInFileExplorer(string path)
        {
            path = path.Replace(@"/", @"\"); // Replaces slashes with backslashes
            Process.Start("explorer.exe", "/select," + path); // Opens file explorer with the specified path selected
        }
    }
}