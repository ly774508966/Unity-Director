using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventInspector
    {
        private Editor _eventInspectorEditor;
        private Object _last;
        private DirectorWindowState _windowState;

        public EventInspector(DirectorWindowState state)
        {
            _windowState = state;
        }

        public void OnGUI(Object evt)
        {
            if (_last != evt)
            {
                _eventInspectorEditor = Editor.CreateEditor(evt);
                _last = evt;
            }
            
            GUILayout.BeginVertical();
            {
                EditorGUI.BeginDisabledGroup(_windowState.isPreview);
                _eventInspectorEditor.DrawHeader();
                _eventInspectorEditor.OnInspectorGUI();
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }
    }
}
