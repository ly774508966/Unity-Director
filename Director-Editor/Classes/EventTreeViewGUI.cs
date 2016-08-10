using UnityEditor;

namespace TangzxInternal
{
    class EventTreeViewGUI : TreeViewGUI
    {
        private DirectorWindow window;

        public EventTreeViewGUI(TreeView treeView, DirectorWindow window) : base(treeView)
        {
            this.window = window;
        }

        public override bool BeginRename(TreeViewItem item, float delay)
        {
            return false;
            //return base.BeginRename(item, delay);
        }
    }
}
