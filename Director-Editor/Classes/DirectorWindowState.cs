﻿using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    /// <summary>
    /// 上下文数据
    /// </summary>
    class DirectorWindowState
    {
        public enum RefreshType
        {
            None,
            All,
        }

        public static int SCROLLBAR_WIDTH = 15;
        public static int SCROLLBAR_HEIGHT = 15;
        public static int TOOLBAR_HEIGHT = 18;

        public DirectorWindow window;

        public TreeViewState treeViewState;

        public EventTreeViewDataSource dataSource;

        /// <summary>
        /// 下次刷新的类型
        /// </summary>
        public RefreshType refreshType;

        /// <summary>
        /// 时间轴的高度
        /// </summary>
        public float timeRulerHeight = TOOLBAR_HEIGHT;
        /// <summary>
        /// 每行的高度
        /// </summary>
        public float rowHeight = 16;
        /// <summary>
        /// 层次树那里最下面的添加事件按钮的高度
        /// </summary>
        public float hierarchyAddEventButtonHeight = 30;

        private bool _isPreview;
        
        public DirectorWindowState(DirectorWindow window)
        {
            this.window = window;
        }

        public TreeRootItem treeRootItem
        {
            get
            {
                return window.treeRootItem;
            }
        }

        public void OnGUI()
        {
            Refresh();
        }

        void Refresh()
        {
            if (refreshType == RefreshType.All)
            {
                dataSource.UpdateData();
            }
            refreshType = RefreshType.None;
        }

        public void ReloadData()
        {
            refreshType = RefreshType.All;
        }

        /// <summary>
        /// 显示创建事件菜单
        /// </summary>
        public GenericMenu ShowCreateEventMenu(GenericMenu.MenuFunction2 HandlerCreate)
        {
            GenericMenu menu = new GenericMenu();

            AttributeTool.EventInfo[] types = AttributeTool.GetAllEventTypes();
            for (int i = 0; i < types.Length; i++)
            {
                AttributeTool.EventInfo evtInfo = types[i];
                menu.AddItem(new GUIContent(evtInfo.eventAttri.category), false, HandlerCreate, evtInfo);
            }

            return menu;
        }

        public bool isPreview
        {
            get { return _isPreview; }
            set { _isPreview = value; }
        }
    }
}
