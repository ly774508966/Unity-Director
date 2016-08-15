using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventInspector
    {
        private Editor _eventInspectorEditor;
        private DirectorEvent _last;

        public void OnGUI(DirectorEvent evt)
        {
            if (_last != evt)
            {
                _eventInspectorEditor = Editor.CreateEditor(evt);
                _last = evt;
            }
            
            GUILayout.BeginVertical();
            _eventInspectorEditor.DrawHeader();
            _eventInspectorEditor.OnInspectorGUI();
            GUILayout.EndVertical();
        }
    }
}
