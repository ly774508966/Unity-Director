using System.Collections.Generic;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventSheetEditor : TimeArea
    {
        //owner
        private DirectorWindow owner;
        //缓存
        private Dictionary<TDEvent, EventDrawer> drawerList = new Dictionary<TDEvent, EventDrawer>();
        //当前正在拖动的
        private TDEvent currentDraggingEvent;
        //当前选中的
        private EventDrawer currentSelectedDrawer;
        //行与行的间隔
        private int rowGap = 1;

        public EventSheetEditor(DirectorWindow owner) : base(false)
        {
            this.owner = owner;
        }

        public void OnGUI(Rect position, Vector2 scrollPosition)
        {
            rect = position;

            //除去下面滚动条的高
            position.yMax -= 15;

            GUI.BeginClip(position);
            position.x = 0;
            position.y = 0;
            OnGridGUI(position);
            OnGUI(position);
            GUI.EndClip();
        }

        public void OnGUI(Rect rect)
        {
            DirectorData data = owner.data;
            if (data)
            {
                Rect rowRect = rect;
                //纪录拖动的对象所在的索引
                int draggingIndex = -1;

                //先画完所有不拖动的事件
                for (int i = 0; i < data.eventList.Count; i++)
                {
                    TDEvent p = data.eventList[i];
                    rowRect.y = i * 30;
                    rowRect.height = 30;
                    if (p == currentDraggingEvent)
                    {
                        draggingIndex = i;
                    }
                    else
                    {
                        OnPlayableGUI(p, rowRect);
                    }
                }

                //拖动的最后画
                if (draggingIndex != -1)
                {
                    rowRect.y = draggingIndex * 30;
                    rowRect.height = 30;
                    OnPlayableGUI(currentDraggingEvent, rowRect);
                }
            }
        }

        void OnGridGUI(Rect rect)
        {
            TimeRuler(rect, frameRate, false, true, 0.2f);
        }

        void OnPlayableGUI(TDEvent p, Rect rect)
        {
            rect.xMin += rowGap;
            rect.width -= rowGap * 2;
            rect.yMin += rowGap;
            rect.height -= rowGap * 2;
            //BG
            GUI.Box(rect, GUIContent.none);

            Rect evtDrawRect = new Rect(rect);
            evtDrawRect.xMin = TimeToPixel2(p.time);
            evtDrawRect.xMax = TimeToPixel2(p.time + p.duration);

            EventDrawer drawer = GetDrawer(p);
            drawer.Reset();
            drawer.target = p;
            drawer.eventSheetEditor = this;
            drawer.OnGUI(evtDrawRect, rect);
        }

        EventDrawer GetDrawer(TDEvent p)
        {
            EventDrawer drawer = null;
            drawerList.TryGetValue(p, out drawer);
            if (drawer == null)
            {
                drawer = AttributeTool.GetDrawer(p);
                drawerList[p] = drawer;
            }

            return drawer;
        }

        /// <summary>
        /// 通过时间来垂直画线
        /// </summary>
        /// <param name="time"></param>
        /// <param name="color"></param>
        public void DrawVerticalLine(float time, Color color)
        {
            Rect r = rect;
            color.a = 0.4f;
            DrawVerticalLine(TimeToPixel2(time), r.yMin - 16, r.yMax, color);
        }
        
        public void OnDragStart(EventDrawer drawer)
        {
            currentDraggingEvent = drawer.target;
        }

        public void OnDragEnd(EventDrawer drawer)
        {
            currentDraggingEvent = null;
        }

        /// <summary>
        /// 设置当前选中
        /// </summary>
        /// <param name="drawer"></param>
        public void SetSelected(EventDrawer drawer)
        {
            currentSelectedDrawer = drawer;
        }

        public float TimeToPixel2(float time)
        {
            return ((time - shownArea.x) / shownArea.width) * rect.width;
        }

        public float PixelToTime2(float pixel)
        {
            return (pixel / rect.width) * shownArea.width;
        }

        public void Repaint()
        {
            owner.Repaint();
        }

        public TDEvent selected
        {
            get
            {
                if (currentSelectedDrawer != null)
                    return currentSelectedDrawer.target;
                else
                    return null;
            }
        }

        public EventDrawer selectedDrawer
        {
            get { return currentSelectedDrawer; }
        }

        public int frameRate { get; set; }
    }
}
