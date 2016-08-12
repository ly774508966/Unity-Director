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

        public DirectorWindow window;

        public TreeViewState treeViewState;

        /// <summary>
        /// 下次刷新的类型
        /// </summary>
        public RefreshType refreshType;

        public EventTreeViewDataSource dataSource;
        
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
