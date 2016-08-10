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
            m_RootItem = new TreeViewItem("Assets".GetHashCode(), 0, null, "Root");
            DirectorData data = window.data;
            if (data)
            {
                for (int i = 0; i < data.playableList.Count; i++)
                {
                    Playable p = data.playableList[i];
                    TreeViewItem item = new TreeViewItem(p.GetInstanceID(), 1, m_RootItem, p.name);
                    m_RootItem.AddChild(item);
                }
            }
        }
    }
}
