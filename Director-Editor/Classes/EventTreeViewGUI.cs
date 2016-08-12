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
            if (!(item is EventTreeItem))
            {
                return false;
            }

            return base.BeginRename(item, delay);
        }

        protected override void RenameEnded()
        {
            RenameOverlay ro = GetRenameOverlay();
            if (!string.IsNullOrEmpty(ro.name) && ro.name != ro.originalName && ro.userAcceptedRename)
            {
                EventTreeItem item = (EventTreeItem) m_TreeView.data.FindItem(ro.userData);
                if (item != null)
                {
                    item.displayName = ro.name;
                    item.target.name = ro.name;
                }
            }
        }

        protected override void DoNodeGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
        {
            if (item is TopTreeItem)
            {

            }
            else if (item is EventTreeItem)
            {
                EventTreeItem evtItem = (EventTreeItem)item;
                Event current = Event.current;
                if (selected && current.type == EventType.ContextClick && rect.Contains(current.mousePosition))
                {
                    GenericMenu menu = CreateContextMenu(evtItem);
                    menu.ShowAsContext();
                }
                base.DoNodeGUI(rect, row, item, selected, focused, useBoldFont);
            }
            else if (item is BottomTreeItem)
            {
                rect.xMin += 15;
                rect.xMax -= 15;
                rect.yMin += 5;
                rect.yMax -= 5;
                GUI.Button(rect, "Add Event");
            }
        }

        public override Rect GetRowRect(int row, float rowWidth)
        {
            Rect rect = base.GetRowRect(row, rowWidth);
            TreeViewItem item = m_TreeView.data.GetItem(row);
            if (row == 0)
            {
                rect.height = 15;
            }
            else
            {
                rect.yMin -= 15;
            }
            return rect;
        }

        GenericMenu CreateContextMenu(EventTreeItem item)
        {
            GenericMenu menu = new GenericMenu();

            //Remove
            menu.AddItem(new GUIContent("Remove Event"), false, () => { window.state.RemoveEvent(item.target); });

            return menu;
        }
    }
}
