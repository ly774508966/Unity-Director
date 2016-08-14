using Tangzx.Director;
using UnityEditor;
using TangzxInternal.Data;

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
            TreeRootItem data = windowState.treeData;
            if (data == null)
                m_RootItem = new TreeViewItem(0, -1, null, "Root");
            else
            {
                data.FetchData();
                m_RootItem = data;
            }

            m_NeedRefreshVisibleFolders = true;

            showRootNode = false;
            rootIsCollapsable = false;
            SetExpanded(m_RootItem, true);

            m_RootItem.AddChild(new BottomTreeItem(m_RootItem));
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }
}
