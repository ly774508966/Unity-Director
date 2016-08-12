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
                //OnNodeGUI(rect, row, evtItem, selected, focused, useBoldFont);
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

        /*
        protected void OnNodeGUI(Rect rect, int row, EventTreeItem item, bool selected, bool focused, bool useBoldFont)
        {
            EditorGUIUtility.SetIconSize(new Vector2(k_IconWidth, k_IconWidth));
            float foldoutIndent = this.GetFoldoutIndent(item);
            int itemControlID = TreeView.GetItemControlID(item);
            bool flag = false;
            if (m_TreeView.dragging != null)
            {
                flag = (m_TreeView.dragging.GetDropTargetControlID() == itemControlID) && this.m_TreeView.data.CanBeParent(item);
            }
            bool flag2 = this.IsRenaming(item.id);
            bool flag3 = this.m_TreeView.data.IsExpandable(item);
            if (flag2 && (Event.current.type == EventType.Repaint))
            {
                float num3 = (item.icon != null) ? k_IconWidth : 0f;
                float num4 = (((foldoutIndent + k_FoldoutWidth) + num3) + this.iconTotalPadding) - 1f;
                this.GetRenameOverlay().editFieldRect = new Rect(rect.x + num4, rect.y, rect.width - num4, rect.height);
            }
            if (Event.current.type == EventType.Repaint)
            {
                string displayName = item.displayName;
                if (flag2)
                {
                    selected = false;
                    displayName = string.Empty;
                }
                if (selected)
                {
                    GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
                    //s_Styles.selectionStyle.Draw(rect, false, false, true, focused);
                }
                if (flag)
                {
                    s_Styles.lineStyle.Draw(rect, GUIContent.none, true, true, false, false);
                }
                this.DrawIconAndLabel(rect, item, displayName, selected, focused, useBoldFont, false);
                if ((this.m_TreeView.dragging != null) && (this.m_TreeView.dragging.GetRowMarkerControlID() == itemControlID))
                {
                    this.m_DraggingInsertionMarkerRect = new Rect((rect.x + foldoutIndent) + this.k_FoldoutWidth, rect.y, rect.width - foldoutIndent, rect.height);
                }
            }
            if (flag3)
            {
                this.DoFoldout(rect, item, row);
            }
            EditorGUIUtility.SetIconSize(Vector2.zero);
        }*/

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

        GenericMenu CreateContextMenu(EventTreeItem item)
        {
            GenericMenu menu = new GenericMenu();

            //Remove
            menu.AddItem(new GUIContent("Remove Event"), false, () => { windowState.RemoveEvent(item.target); });

            return menu;
        }
    }
}
