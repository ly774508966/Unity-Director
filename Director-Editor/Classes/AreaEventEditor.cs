using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class AreaEventEditor : TimeArea
    {
        public AreaEventEditor(EditorWindow owner) : base(false)
        {

        }

        public void OnGUI(Rect position, Vector2 scrollPosition)
        {
            rect = position;

            //排除垂直滚动条的宽
            position.width -= 15;

            GUI.BeginGroup(position);
            OnGUI(position);
            GUI.EndGroup();
        }

        public void OnGUI(Rect rect)
        {

        }
    }
}
