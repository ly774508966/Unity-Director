using UnityEngine;
using UnityEditor;

namespace TangzxInternal
{
    class TreeRootItem : TreeItem
    {
    }

    class BottomTreeItem : TreeItem
    {
        public BottomTreeItem(TreeViewItem parent)
        {
            this.parent = parent;
        }

        public override void OnTreeRowGUI(EventTreeViewGUI gui, UnityEngine.Rect rect, int row, bool selected, bool focused, bool useBoldFont)
        {
            rect.xMin += 15;
            rect.xMax -= 15;
            rect.yMin += 5;
            rect.yMax -= 5;
            if (GUI.Button(rect, "Add"))
            {
                //windowState.ShowCreateEventMenu();
            }
        }
    }

    interface IRenameableTreeItem
    {
        void RenameEnded(string name);
    }

    interface IInspectorItem
    {
        Object GetInspectorObject();
    }
}