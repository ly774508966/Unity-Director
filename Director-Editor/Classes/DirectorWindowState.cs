using Tangzx.Director;
using TangzxInternal.Data;
using UnityEditor;
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
        public float timeRulerHeight = 15;
        /// <summary>
        /// 每行的高度
        /// </summary>
        public float rowHeight = 16;
        /// <summary>
        /// 层次树那里最下面的添加事件按钮的高度
        /// </summary>
        public float hierarchyAddEventButtonHeight = 30;
        
        public DirectorWindowState(DirectorWindow window)
        {
            this.window = window;
        }

        public VOTree treeData
        {
            get
            {
                return window.treeData;
            }
        }

        public void OnGUI()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (refreshType == RefreshType.All)
            {
                dataSource.UpdateData();
            }
            refreshType = RefreshType.None;
        }

        public void RemoveEvent(TDEvent evt)
        {
            //data.Remove(evt);
            refreshType = RefreshType.All;
        }

        public void ShowCreateEventMenu()
        {
            ShowCreateEventMenu(HandlerCreate);
        }

        /// <summary>
        /// 显示创建事件菜单
        /// </summary>
        public void ShowCreateEventMenu(GenericMenu.MenuFunction2 HandlerCreate)
        {
            GenericMenu menu = new GenericMenu();

            AttributeTool.EventInfo[] types = AttributeTool.GetAllEventTypes();
            for (int i = 0; i < types.Length; i++)
            {
                AttributeTool.EventInfo evtInfo = types[i];
                menu.AddItem(new GUIContent(evtInfo.eventAttri.category), false, HandlerCreate, evtInfo);
            }

            menu.ShowAsContext();
        }

        /// <summary>
        /// 处理点击创建事件菜单项
        /// </summary>
        /// <param name="typeData"></param>
        void HandlerCreate(object typeData)
        {
            //AttributeTool.EventInfo evtInfo = (AttributeTool.EventInfo)typeData;
            //TDEvent p = data.Add(evtInfo.eventType);
            // Refresh
            //refreshType = RefreshType.All;
        }
    }
}
