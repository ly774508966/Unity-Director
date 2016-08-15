using System.Collections.Generic;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal.Data
{
    /// <summary>
    /// 每一行的数据基类
    /// </summary>
    class TreeItem : TreeViewItem
    {
        public List<TreeItem> list = new List<TreeItem>();

        public DirectorWindowState state;

        public TreeItem() : base(0, 0, null, null)
        {
            
        }

        public virtual ISheetRowDrawer GetDrawer()
        {
            return null;
        }

        public void Add(TreeItem item, int id, string dispalyName)
        {
            item.id = id;
            item.depth = depth + 1;
            item.displayName = dispalyName;
            item.parent = this;
            AddChild(item);
            list.Add(item);
        }

        public void Remove(TreeItem item)
        {
            if (state != null)
                state.ReloadData();
        }

        public virtual void OnTreeRowGUI(EventTreeViewGUI gui, Rect rect, int row, bool selected, bool focused, bool useBoldFont)
        {
            gui.OnNodeGUI(rect, row, this, selected, focused, useBoldFont);
            if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
            {
                GenericMenu menu = OnContextMenu();
                if (menu != null)
                    menu.ShowAsContext();
            }
        }

        protected virtual GenericMenu OnContextMenu()
        {
            return null;
        }

        public virtual void BuildTree(DirectorWindowState windowState)
        {
            state = windowState;
        }

        public virtual void RemoveChild(TreeItem child)
        {
            Debug.Log("Remove : " + child);
        }
    }

    class EventTreeItem : TreeItem
    {
        public TDEvent target;

        private EventDrawer drawer;

        public EventTreeItem(TDEvent evt)
        {
            target = evt;
        }

        public override ISheetRowDrawer GetDrawer()
        {
            if (drawer == null)
                drawer = AttributeTool.GetEventDrawer(target);
            drawer.target = target;
            return drawer;
        }

        protected override GenericMenu OnContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                TreeItem p = parent as TreeItem;
                p.RemoveChild(this);
            });
            return menu;
        }
    }
}
