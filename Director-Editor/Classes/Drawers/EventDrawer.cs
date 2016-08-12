using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
    [CustomPlayableDrawer(typeof(TDEvent))]
    public class EventDrawer : Draggable
    {
        static int count = 0;
        /// <summary>
        /// event
        /// </summary>
        public TDEvent target;
        /// <summary>
        /// 是否被选中的
        /// </summary>
        public bool isSelected { get { return eventSheetEditor.selected == target; } }

        internal EventSheetEditor eventSheetEditor;

        // obj id
        protected int id;

        //开始拖动时的时间纪录
        private float timeDragStart;

        public EventDrawer()
        {
            id = count++;
        }

        public virtual void OnGUI(Rect drawRect, Rect totalRect)
        {
            GUIStyle eventStyle = "Dopesheetkeyframe";

            drawRect.width = 9;
            drawRect.x -= drawRect.width / 2 + 0.5f;
            drawRect.x = Mathf.CeilToInt(drawRect.x);

            HandleDrag(drawRect, totalRect);
            if (Event.current.type == EventType.repaint)
            {
                eventStyle.Draw(drawRect, GUIContent.none, id);
            }
        }

        /// <summary>
        /// 处理整体的拖动
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="totalRect"></param>
        protected void HandleDrag(Rect drawRect, Rect totalRect)
        {
            if (isSelected)
            {
                eventSheetEditor.DrawVerticalLine(target.time, Color.yellow);
            }
            HandleDrag(drawRect, 1,
                () =>
                {
                    timeDragStart = target.time;
                    eventSheetEditor.SetSelected(this);
                    eventSheetEditor.OnDragStart(this);
                },
                () => { eventSheetEditor.OnDragEnd(this); },
                (float offset) =>
                {
                    float dt = eventSheetEditor.PixelToTime2(offset);
                    UpdateTime(Mathf.Max(0, timeDragStart + dt));
                },
                () =>
                {
                    if (isSelected)
                        eventSheetEditor.SetSelected(null);
                });
        }

        public virtual void Reset()
        {

        }

        protected override void Repaint()
        {
            eventSheetEditor.Repaint();
        }

        protected virtual void UpdateTime(float v)
        {
            target.time = v;
        }
    }
}