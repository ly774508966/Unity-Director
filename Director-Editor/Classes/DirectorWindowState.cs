using Tangzx.Director;
using UnityEditor;

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

        public DirectorData data
        {
            get { return window.data; }
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
            data.Remove(evt);
            refreshType = RefreshType.All;
        }

    }
}
