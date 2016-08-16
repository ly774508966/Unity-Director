using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    [CustomEditor(typeof(SequencerData))]
    public class SequencerDataInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            SequencerData sd = target as SequencerData;

            GUI.enabled = false;
            for (int i = 0; i < sd.categories.Count; i++)
            {
                EditorGUILayout.TextField("Index" + i, sd.categories[i].categoryName);
            }
            
            if (sd.defaultCategory != null)
            {
                EditorGUILayout.TextField("Default", sd.defaultCategory.categoryName);
            }
            GUI.enabled = true;
        }
    }
}