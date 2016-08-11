using System;
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
        public bool isSelected { get { return eventSheetEditor.selectedDrawer == this; } }

        internal EventSheetEditor eventSheetEditor;

        // is current dragging ?
        private bool isDragging;
        // obj id
        protected int id;

        private int dragId;
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

        /// <summary>
        /// 处理整体的拖动
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="totalRect"></param>
        protected void HandleDrag(Rect drawRect, Rect totalRect)
        {
            if (isSelected)
            {
                eventSheetEditor.DrawVerticalLine(target.time, Color.red);
            }
            HandleDrag(drawRect, 1,
                () => {
                    timeDragStart = target.time;
                    eventSheetEditor.SetSelected(this);
                    eventSheetEditor.OnDragStart(this);
                },
                () => { eventSheetEditor.OnDragEnd(this); },
                (float offset) => {
                    float dt = eventSheetEditor.PixelToTime2(offset);
                    UpdateTime(Mathf.Max(0, timeDragStart + dt));
                },
                () => {
                    if (isSelected)
                        eventSheetEditor.SetSelected(null);
                });
        }

        protected void HandleDrag(Rect drawRect, int id, Action onStart, Action onEnd, Action<float> onDrag, Action onMouseDownOutside)
        {
            Event evt = Event.current;
            if (evt.isMouse)
            {
                if (evt.type == EventType.MouseDown)
                {
                    if (drawRect.Contains(evt.mousePosition))
                    {
                        positionDragStart = evt.mousePosition.x;
                        isDragging = true;
                        evt.Use();
                        dragId = id;
                        if (onStart != null) onStart();
                    }
                    else
                    {
                        if (onMouseDownOutside != null)
                            onMouseDownOutside();
                    }
                }
                else if (evt.type == EventType.MouseUp)
                {
                    if (isDragging && id == dragId)
                    {
                        isDragging = false;
                        if (onEnd != null)
                            onEnd();
                        evt.Use();
                    }
                }
                else if (evt.type == EventType.MouseDrag)
                {
                    if (isDragging && id == dragId)
                    {
                        if (onDrag != null)
                        {
                            float offset = evt.mousePosition.x - positionDragStart;
                            onDrag(offset);
                        }
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
            eventSheetEditor.Repaint();
        }

        protected virtual void UpdateTime(float v)
        {
            target.time = v;
        }
    }
}