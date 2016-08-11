using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
    [CustomPlayableDrawer(typeof(TDEvent))]
    public class EventDrawer
    {
        static int count = 0;
        /// <summary>
        /// event
        /// </summary>
        public TDEvent target;
        /// <summary>
        /// 是否被选中的
        /// </summary>
        public bool isSelected { get { return areaEventEditor.selectedDrawer == this; } }

        internal AreaEventEditor areaEventEditor;

        // is current dragging ?
        private bool isDragging;
        // obj id
        protected int id;
        //开始拖动时的时间纪录
        private float timeDragStart;
        //开始拖动时的鼠标位置纪录
        private float positionDragStart;

        public EventDrawer()
        {
            id = count++;
        }

        public virtual void OnGUI(Rect drawRect, Rect totalRect)
        {
            GUIStyle eventStyle = "Grad Up Swatch";

            drawRect.width = eventStyle.fixedWidth;
            drawRect.x -= drawRect.width / 2;

            HandleDrag(drawRect, totalRect);
            if (Event.current.type == EventType.repaint)
            {
                eventStyle.Draw(drawRect, GUIContent.none, id);
            }
        }

        protected void HandleDrag(Rect drawRect, Rect totalRect)
        {
            Event evt = Event.current;
            if (evt.isMouse)
            {
                if (evt.type == EventType.MouseDown)
                {
                    if (drawRect.Contains(evt.mousePosition))
                    {
                        timeDragStart = target.time;
                        positionDragStart = evt.mousePosition.x;
                        isDragging = true;
                        evt.Use();
                        areaEventEditor.SetSelected(this);
                        areaEventEditor.OnDragStart(this);
                    }
                    else
                    {
                        //在我以外的区域点击了
                        if (isSelected)
                        {
                            areaEventEditor.SetSelected(null);
                        }
                    }
                }
                else if (evt.type == EventType.MouseUp)
                {
                    if (isDragging)
                    {
                        isDragging = false;
                        areaEventEditor.OnDragEnd(this);
                        evt.Use();
                    }
                }
                else if (evt.type == EventType.MouseDrag)
                {
                    if (isDragging)
                    {
                        float offset = evt.mousePosition.x - positionDragStart;
                        float dt = areaEventEditor.PixelToTime2(offset, totalRect);
                        target.time = Mathf.Max(0, timeDragStart + dt);
                        evt.Use();
                        Repaint();
                    }
                }
            }
        }

        public virtual void Reset()
        {

        }

        public void Repaint()
        {
            areaEventEditor.Repaint();
        }
    }
}