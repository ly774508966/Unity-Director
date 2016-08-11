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
}
