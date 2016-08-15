using TangzxInternal.Data;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventTreeViewGUI : TreeViewGUI
    {
        private DirectorWindowState windowState;

        public EventTreeViewGUI(TreeView treeView, DirectorWindowState windowState) : base(treeView)
        {
            this.windowState = windowState;
            k_TopRowMargin = windowState.timeRulerHeight;
            k_BottomRowMargin = windowState.hierarchyAddEventButtonHeight;
            k_LineHeight = windowState.rowHeight;
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
            if (item is BottomTreeItem)
            {
                rect.xMin += 15;
                rect.xMax -= 15;
                rect.yMin += 5;
                rect.yMax -= 5;
                if (GUI.Button(rect, "Add Event"))
                    windowState.ShowCreateEventMenu();
            }
            else if (item is TreeItem)
            {
                TreeItem ti = item as TreeItem;
                ti.state = windowState;
                ti.OnTreeRowGUI(this, rect, row, selected, focused, useBoldFont);
            }
            else
            {
                base.DoNodeGUI(rect, row, item, selected, focused, useBoldFont);
            }
        }

        public void OnNodeGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
        {
            base.DoNodeGUI(rect, row, item, selected, focused, useBoldFont);
        }

        public override Rect GetRowRect(int row, float rowWidth)
        {
            Rect rect = base.GetRowRect(row, rowWidth);
            TreeViewItem item = m_TreeView.data.GetItem(row);

            if (item is BottomTreeItem)
            {
                rect.height = windowState.hierarchyAddEventButtonHeight;
            }
            else
            {
                rect.height = windowState.rowHeight;
            }
            return rect;
        }
    }
}
