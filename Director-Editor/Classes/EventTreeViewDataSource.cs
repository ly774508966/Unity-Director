using Tangzx.Director;
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
            m_RootItem = new TreeViewItem(0, -1, null, "Root");
            m_NeedRefreshVisibleFolders = true;

            showRootNode = false;
            rootIsCollapsable = false;
            SetExpanded(m_RootItem, true);

            //m_RootItem.AddChild(new TopTreeItem(m_RootItem));

            DirectorData data = windowState.data;
            if (data)
            {
                for (int i = 0; i < data.eventList.Count; i++)
                {
                    TDEvent p = data.eventList[i];

                    EventTreeItem item = new EventTreeItem(p, 0, m_RootItem);
                    m_RootItem.AddChild(item);
                }
            }
            m_RootItem.AddChild(new BottomTreeItem(m_RootItem));
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }
}
