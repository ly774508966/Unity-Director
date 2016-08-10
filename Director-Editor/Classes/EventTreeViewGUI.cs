using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace TangzxInternal
{
    class EventTreeViewGUI : TreeViewGUI
    {
        public EventTreeViewGUI(TreeView treeView) : base(treeView)
        {

        }

        public override bool BeginRename(TreeViewItem item, float delay)
        {
            return false;
            //return base.BeginRename(item, delay);
        }
    }
}
