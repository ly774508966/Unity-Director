using Tangzx.Director;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangzxInternal
{
    class AreaEventEditor : TimeArea
    {
        private DirectorWindow owner;

        public AreaEventEditor(DirectorWindow owner) : base(false)
        {
            this.owner = owner;
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
            DirectorData data = owner.data;
            if (data)
            {
                Rect rowRect = rect;

                for (int i = 0; i < data.playableList.Count; i++)
                {
                    Playable p = data.playableList[i];
                    rowRect.y = i * 30;
                    rowRect.height = 30;
                    OnPlayableGUI(p, rowRect);
                }
            }
        }

        void OnPlayableGUI(Playable p, Rect rect)
        {
            float x = FrameToPixel(p.time * 60, 60, rect);
            Rect buttonRect = new Rect(rect);
            buttonRect.xMin = x;
            buttonRect.xMax = FrameToPixel((p.time+1) * 60, 60, rect);

            EventDrawer drawer = AttributeTool.GetDrawer(p);
            if (drawer != null)
            {
                drawer.Reset();
                drawer.playable = p;
                drawer.OnGUI(buttonRect);
            }
        }

        public void AddEvent(Playable evt)
        {
            evt.time = Random.value * 10;
        }
    }
}
