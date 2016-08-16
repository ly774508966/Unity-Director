using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal.Drawers
{
    [CustomEventDrawer(typeof(IRangeEvent))]
    public class RangeEventDrawer : EventDrawer
    {
        static GUIStyle styleResizer;
        static GUIStyle styleBg;
        static GUIStyle stylePlayhead;

        static void InitStyle()
        {
            if (styleResizer == null)
            {
                styleBg = "flow node 3";
                styleResizer = "IN FoldoutStatic";
                stylePlayhead = "MeTransPlayhead";
            }
        }
        
        public override void OnGUI(Rect drawRect, Rect totalRect)
        {
            InitStyle();
            //不能太小
            if (drawRect.width < 10)
                drawRect.width = 10;

            //调整Duration的三角形区域
            Rect rectResizer = new Rect(drawRect.xMax - styleResizer.fixedWidth * 0.5f, drawRect.y, styleResizer.fixedWidth, drawRect.height);

            //Reapint
            if (Event.current.type == EventType.repaint)
            {
                styleBg.Draw(drawRect, GUIContent.none, id);

                //Draw Resizer
                if (isSelected)
                {
                    styleResizer.Draw(rectResizer, GUIContent.none, id);
                }
            }

            //处理拖动
            HandleResize(rectResizer, totalRect);
            HandleDrag(drawRect, totalRect);
        }

        private float dragDuration;
        protected virtual void HandleResize(Rect drawRect, Rect totalRect)
        {
            if (isSelected)
            {
                eventSheetEditor.DrawVerticalLine(target.time + target.duration, Color.yellow);
            }
            HandleDrag(!eventSheetEditor.isPreview, drawRect, 999,
                () => { dragDuration = target.duration; },
                () => { },
                (float offset) => {
                    float dt = eventSheetEditor.PixelToTime2(offset);
                    target.duration = Mathf.Max(0, dragDuration + dt);
                },
                () => { });
        }
    }
}
