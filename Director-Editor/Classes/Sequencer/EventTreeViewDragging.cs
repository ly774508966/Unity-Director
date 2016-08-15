using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventTreeViewDragging : TreeViewDragging
    {
        private List<GameObject> dragObjects = new List<GameObject>();

        private SequencerEditorWindow window;

        public EventTreeViewDragging(TreeView tree, SequencerEditorWindow window) : base(tree)
        {
            this.window = window;
        }

        public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
        {
            
        }

        public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, DropPosition dropPosition)
        {
            if (targetItem is SequencerCategoryTreeItem && perform)
            {
                dragObjects.Clear();

                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    Object o = DragAndDrop.objectReferences[i];
                    if (o is GameObject)
                    {
                        dragObjects.Add(o as GameObject);
                    }
                }

                if (dragObjects.Count > 0)
                {
                    var treeItem = targetItem as SequencerCategoryTreeItem;
                    return window.DoDrag(dragObjects.ToArray(), treeItem.target);
                }
            }
            return DragAndDropVisualMode.Move;
        }
    }
}

