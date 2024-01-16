using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Editor
{
    public class SaveSystemInstallerWindow : EditorWindow
    {
       


        private Texture _header;


        [MenuItem("Studio-23/Save System/Installer")]
        public static void ShowWindow()
        {
           
            SaveSystemInstallerWindow window = GetWindow<SaveSystemInstallerWindow>("Save System Installer");
            window.minSize = new Vector2(400, 200);
        }

        private void OnEnable()
        {
            _header = Resources.Load<Texture>("SaveSystem/SaveSystemHeader");
        }


        private void OnGUI()
        {

            GUILayout.Box(_header, GUILayout.Height(200), GUILayout.ExpandWidth(true));

            GUILayout.Label("Save System Setup Wizard", EditorStyles.boldLabel);

          

            if (GUILayout.Button("Install SaveSystem"))
            {

                GameObject saveSystemObject = Resources.Load<GameObject>("SaveSystem/SaveSystem");
                Instantiate(saveSystemObject);
            }
        }
    }
}