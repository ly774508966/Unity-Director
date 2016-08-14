using System.Collections.Generic;
using Tangzx.Director;
using UnityEngine;
using UnityEditor;
using TangzxInternal;

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

        public virtual IRowDrawer GetDrawer()
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

        public virtual void OnTreeRowGUI(EventTreeViewGUI gui, Rect rect, int row, bool selected, bool focused, bool useBoldFont)
        {
            gui.OnNodeGUI(rect, row, this, selected, focused, useBoldFont);
            if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
            {
                OnContextMenu();
            }
        }

        protected virtual void OnContextMenu()
        {
            
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

        public override IRowDrawer GetDrawer()
        {
            if (drawer == null)
                drawer = AttributeTool.GetEventDrawer(target);
            drawer.target = target;
            return drawer;
        }
    }

    class SeqAffectedItem : TreeItem
    {

    }
}
