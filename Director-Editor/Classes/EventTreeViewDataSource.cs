using Tangzx.Director;
using UnityEditor;

namespace TangzxInternal
{
    class EventTreeViewDataSource : TreeViewDataSource
    {

        private DirectorWindow window;

        public EventTreeViewDataSource(TreeView tree, DirectorWindow window) : base(tree)
        {
            this.window = window;
        }

        public override void FetchData()
        {
            m_RootItem = new TreeViewItem(0, -1, null, "Root");
            m_NeedRefreshVisibleFolders = true;

            showRootNode = false;
            rootIsCollapsable = false;
            SetExpanded(m_RootItem, true);
            
            DirectorData data = window.data;
            if (data)
            {
                for (int i = 0; i < data.eventList.Count; i++)
                {
                    TDEvent p = data.eventList[i];

                    EventTreeItem item = new EventTreeItem(p, 0, m_RootItem);
                    m_RootItem.AddChild(item);
                }
            }
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }
}
