using System;
using UnityEngine;

namespace TangzxInternal
{
    public class Draggable
    {
        // is current dragging ?
        private bool _isDragging;
        // 当前拖动的ID
        private int _dragId;
        // 开始拖动时的鼠标位置纪录
        private float _xWhenDragStart;

        protected void HandleDrag(bool canDrag, Rect hotAreaRect, int id, Action onStart, Action onEnd, Action<float> onDrag, Action onMouseDownOutside)
        {
            Event evt = Event.current;
            if (evt.isMouse)
            {
                if (evt.type == EventType.MouseDown)
                {
                    //BUG FIX 拖出屏幕，再来拖一次？ MouseUpOutside
                    if (_isDragging && id == _dragId)
                    {
                        _isDragging = false;
                        if (onEnd != null)
                            onEnd();
                    }
                    //BUG FIX

                    if (hotAreaRect.Contains(evt.mousePosition))
                    {
                        _xWhenDragStart = evt.mousePosition.x;
                        _isDragging = true;
                        evt.Use();
                        _dragId = id;
                        if (onStart != null) onStart();
                    }
                    else
                    {
                        if (onMouseDownOutside != null)
                            onMouseDownOutside();
                    }
                }
                else if (evt.type == EventType.MouseDrag)
                {
                    if (_isDragging && id == _dragId && canDrag)
                    {
                        if (onDrag != null)
                        {
                            float offset = evt.mousePosition.x - _xWhenDragStart;
                            onDrag(offset);
                        }
                        evt.Use();
                        Repaint();
                    }
                }
                else
                {
                    if (_isDragging && id == _dragId)
                    {
                        _isDragging = false;
                        if (onEnd != null)
                            onEnd();
                        evt.Use();
                    }
                }
            }
        }

        protected virtual void Repaint()
        {

        }

        protected bool isDragging { get { return _isDragging; } }
    }
}
