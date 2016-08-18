using UnityEngine;
using System.Collections;
using UnityEditor;
using Tangzx.Director;

namespace TangzxInternal
{
    [CustomEditor(typeof(DirectorEvent), true)]
    public class DirectorEventEditor : Editor
    {
        private DirectorEvent _evt;
        private bool _hideDuration;

        private void Init()
        {
            if (_evt == null)
            {
                _hideDuration = true;
                _evt = target as DirectorEvent;
                if (_evt is IRangeEvent)
                {
                    DirectorPlayableAttribute attr = AttributeTool.GetAttribute<DirectorPlayableAttribute>(_evt);
                    if (attr != null)
                        _hideDuration = attr.hideDuration;
                    else
                        _hideDuration = false;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            Init();

            SerializedObject obj = serializedObject;

            obj.Update();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (_hideDuration && iterator.propertyPath == "_duration")
                    continue;
                
                EditorGUI.BeginDisabledGroup("m_Script" == iterator.propertyPath);
                EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
                EditorGUI.EndDisabledGroup();
                enterChildren = false;
            }
            obj.ApplyModifiedProperties();
        }
    }
}