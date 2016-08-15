using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventInspector
    {
        private Editor _eventInspectorEditor;
        private Object _last;

        public void OnGUI(Object evt)
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
