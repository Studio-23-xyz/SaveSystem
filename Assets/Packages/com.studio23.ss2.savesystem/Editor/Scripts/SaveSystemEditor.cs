using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    [CustomEditor(typeof(Core.SaveSystem))]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private int slotIndex = 0; // Default slot index
        private bool showDebugTools = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Core.SaveSystem saveSystem = (Core.SaveSystem)target;

            // Start Debug Tools foldout section
            showDebugTools = EditorGUILayout.BeginFoldoutHeaderGroup(showDebugTools, "Debug Tools");

            if (showDebugTools)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginHorizontal();

                // Add an integer input field for slot index
                slotIndex = EditorGUILayout.IntField("Slot Index", slotIndex);


                if (GUILayout.Button("Select Slot"))
                {
                    saveSystem.SelectSlot(slotIndex).Forget();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();

                // Set the button color to green
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Save"))
                {
                    saveSystem.Save().Forget();
                }
                GUI.backgroundColor = Color.white; // Reset color

                // Set the button color to red
                GUI.backgroundColor = Color.red;
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
