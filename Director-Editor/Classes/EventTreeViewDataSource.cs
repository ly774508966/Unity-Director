using System;
using UnityEditor;

namespace TangzxInternal
{
    class EventTreeViewDataSource : TreeViewDataSource
    {
        public EventTreeViewDataSource(TreeView tree) : base(tree)
        {

        }

        public override void FetchData()
        {
            m_RootItem = new TreeViewItem("Assets".GetHashCode(), 0, null, "test");
            m_RootItem.AddChild(new TreeViewItem("AAA".GetHashCode(), 0, m_RootItem, "AA"));
            m_RootItem.AddChild(new TreeViewItem("AAA".GetHashCode(), 0, m_RootItem, "AA"));
            m_RootItem.AddChild(new TreeViewItem("AAA".GetHashCode(), 0, m_RootItem, "AA"));
        }
    }
}
