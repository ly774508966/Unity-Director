using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class CreateCategoryWindow : EditorWindow
    {
        string input = "Main";

        public SequencerEditorWindow mainWindow;

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name:");
                input = GUILayout.TextField(input);
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("OK"))
                {
                    mainWindow.CreateCategory(input);
                    Close();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}