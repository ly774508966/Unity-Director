using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventTreeViewGUI : TreeViewGUI
    {
        private DirectorWindow window;

        public EventTreeViewGUI(TreeView treeView, DirectorWindow window) : base(treeView)
        {
            this.window = window;
            this.k_LineHeight = 30;
        }

        public override bool BeginRename(TreeViewItem item, float delay)
        {
            return false;
        }

        protected override void DoNodeGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
        {
            base.DoNodeGUI(rect, row, item, selected, focused, useBoldFont);

            if (item is EventTreeItem)
            {
                EventTreeItem evtItem = (EventTreeItem)item;
                Event current = Event.current;
                if (selected)
                {
                    //del button
                    Rect delRect = rect;
                    rect.width = 20;
                    if (GUI.Button(rect, "-"))
                    {
                        window.state.RemoveEvent(evtItem.target);
                    }
                }
            }
        }

        GenericMenu CreateContextMenu()
        {

            return null;
        }
    }
}
