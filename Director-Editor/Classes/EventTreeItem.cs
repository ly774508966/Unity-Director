using Tangzx.Director;
using UnityEditor;

namespace TangzxInternal
{
    class EventTreeItem : TreeViewItem
    {
        private TDEvent _evt;

        public EventTreeItem(TDEvent evt, int depth, TreeViewItem parent) : base(evt.GetInstanceID(), depth, parent, evt.name)
        {
            _evt = evt;
        }

        public TDEvent target { get { return _evt; } }
    }

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
