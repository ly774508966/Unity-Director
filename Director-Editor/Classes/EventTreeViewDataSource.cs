using UnityEditor;

namespace TangzxInternal
{
    class EventTreeViewDataSource : TreeViewDataSource
    {
        private DirectorWindowState windowState;

        public EventTreeViewDataSource(TreeView tree, DirectorWindowState windowState) : base(tree)
        {
            this.windowState = windowState;
        }

        public override void FetchData()
        {
            TreeRootItem data = windowState.treeRootItem;
            if (data == null)
                m_RootItem = new TreeViewItem(0, -1, null, "Root");
            else
            {
                data.state = windowState;
                data.BuildTree(windowState);
                m_RootItem = data;
            }

            m_NeedRefreshVisibleFolders = true;

            showRootNode = false;
            rootIsCollapsable = false;
            SetExpanded(m_RootItem, true);

            m_RootItem.AddChild(windowState.window.CreateBottomTreeItem(m_RootItem));
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }
}