using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ISavableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var monoBehaviour = (MonoBehaviour)target;

            if (monoBehaviour is ISaveable)
            {
                EditorGUILayout.Space();
                

                GUI.backgroundColor = Color.green;

                if (GUILayout.Button(
                        new GUIContent("Save",
                            "Only Works if Save System has been initialized and a slot is selected"),
                        GUILayout.ExpandWidth(true)))
                {
                   (monoBehaviour as ISaveable).SaveSelf().Forget();
                }

                GUI.backgroundColor = Color.blue;

                if (GUILayout.Button(
                        new GUIContent("Load",
                            "Only Works if Save System has been initialized and a slot is selected"),
                        GUILayout.ExpandWidth(true)))
                {
                    (monoBehaviour as ISaveable).LoadSelf().Forget();
                }


                GUI.backgroundColor = Color.red;

                if (GUILayout.Button(
                        new GUIContent("Delete Local",
                            "Only Works if Save System has been initialized and a slot is selected"),
                        GUILayout.ExpandWidth(true)))
                {
                    var key = (monoBehaviour as ISaveable).GetUniqueID();
                    Core.SaveSystem.Instance.DeleteKeyFromSelectedSlot(key).Forget();
                }



            }
        }
    }
}