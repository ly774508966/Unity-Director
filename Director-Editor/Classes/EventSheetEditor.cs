using Tangzx.Director;
using TangzxInternal.Data;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventSheetEditor : TimeArea, ISheetEditor
    {
        //owner
        private DirectorWindowState windowState;
        //当前正在拖动的
        private ISheetRowDrawer currentDraggingEvent;
        //当前选中的
        private TDEvent currentSelectedEvent;
        //行与行的间隔
        private float rowGap = 1f;

        private float scrollY;

        public EventSheetEditor(DirectorWindowState state) : base(false)
        {
            windowState = state;
        }

        public void OnGUI(Rect position, float scrollPositionY)
        {
            rect = position;
            scrollY = scrollPositionY;

            //除去下面滚动条的高
            position.yMax -= 15;
            
            GUIClip.Push(position, new Vector2(0, scrollPositionY), Vector2.zero, false);
            position.x = 0;

            //画格子，格子的Y坐标不随滚动而动
            position.y = -scrollPositionY;
            OnGridGUI(position);

            //画正式内容元素
            position.y = 0;
            OnGUI(position);

            GUIClip.Pop();
        }

        public void OnGUI(Rect rect)
        {
            TreeRootItem treeData = windowState.treeData;

            if (treeData != null)
            {
                //Rect rowRect = rect;
                //纪录拖动的对象所在的索引
                //int selectedIndex = -1;
                
                OnItemGUI(rect, treeData, 0, false, true);

                //拖动的最后画
                //if (selectedIndex != -1)
                //{
                //    rowRect.y = selectedIndex * windowState.rowHeight;
                //    rowRect.height = windowState.rowHeight;
                //    OnPlayableGUI(currentSelectedEvent, rowRect);
                //}
            }
        }

        protected virtual int OnItemGUI(Rect rect, TreeItem item, int row, bool show, bool showChildren)
        {
            Rect rowRect = rect;
            if (show)
            {
                rowRect.y = row * windowState.rowHeight;
                rowRect.height = windowState.rowHeight;
                OnPlayableGUI(item, rowRect);
                row++;
            }

            if (windowState.treeViewState.expandedIDs.Contains(item.id))
            //先画完所有不拖动的事件
            for (int i = 0; i < item.list.Count; i++)
            {
                TreeItem subItem = item.list[i];
                row = OnItemGUI(rect, subItem, row, true, true);
            }

            return row;
        }

        void OnGridGUI(Rect rect)
        {
            TimeRuler(rect, frameRate, false, true, 0.2f);
        }

        protected virtual void OnPlayableGUI(TreeItem item, Rect rect)
        {
            rect.xMin += rowGap;
            rect.width -= rowGap;
            rect.yMin += rowGap * 0.5f;
            rect.height -= rowGap;

            ISheetRowDrawer rowDrawer = item.GetDrawer();
            if (rowDrawer != null)
            {
                rowDrawer.OnSheetRowGUI(this, rect);
            }
        }

        /// <summary>
        /// 通过时间来垂直画线
        /// </summary>
        /// <param name="time"></param>
        /// <param name="color"></param>
        public void DrawVerticalLine(float time, Color color)
        {
            Rect r = rect;
            r.yMin = 0;
            DrawVerticalLine(TimeToPixel2(time), -scrollY, r.yMax - scrollY, color);
        }
        
        public void OnDragStart(ISheetRowDrawer drawer)
        {
            currentDraggingEvent = drawer;
        }

        public void OnDragEnd(ISheetRowDrawer drawer)
        {
            currentDraggingEvent = null;
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
            windowState.window.Repaint();
        }

        public TDEvent selected
        {
            get
            {
                if (currentSelectedEvent != null)
                    return currentSelectedEvent;
                else
                    return null;
            }
            set { currentSelectedEvent = value; }
        }

        public int frameRate { get; set; }
    }
}
