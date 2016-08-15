using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventTreeItem : TreeItem, IRenameableTreeItem
    {
        public DirectorEvent target;

        private EventDrawer drawer;

        public EventTreeItem(DirectorEvent evt)
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

        public void RenameEnded(string name)
        {
            displayName = name;
            target.name = name;
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
