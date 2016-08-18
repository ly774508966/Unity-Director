﻿using Tangzx.Director;
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
        //行与行的间隔
        private float rowGap = 1f;

        private float scrollY;

        public EventSheetEditor(DirectorWindowState state) : base(false)
        {
            windowState = state;
            ApplySettings();
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
            TreeRootItem treeRootItem = windowState.treeRootItem;

            if (treeRootItem != null)
            {
                OnItemGUI(rect, treeRootItem, 0, treeRootItem is IVisibleRootItem, true);
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

        public int frameRate { get; set; }

        public bool isPreview { get { return windowState.isPreview; } }

        public bool IsSelected(DirectorEvent evt)
        {
            if (evt == null)
                return false;
            return windowState.treeViewState.selectedIDs.Contains(evt.GetInstanceID());
        }

        public void SetSelected(DirectorEvent evt)
        {
            windowState.treeViewState.selectedIDs.Clear();
            if (evt)
            {
                windowState.treeViewState.selectedIDs.Add(evt.GetInstanceID());
                windowState.treeViewState.lastClickedID = evt.GetInstanceID();
            }
        }
    }
}
