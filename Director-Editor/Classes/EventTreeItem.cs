using Tangzx.Director;
using UnityEditor;

namespace TangzxInternal
{
    class BottomTreeItem : TreeViewItem
    {
        public BottomTreeItem(TreeViewItem parent) : base(0, 0, parent, null)
        {

        }
    }

    class TopTreeItem : TreeViewItem
    {
        public TopTreeItem(TreeViewItem parent) : base(0, 0, parent, null)
        {

        }
    }
}
