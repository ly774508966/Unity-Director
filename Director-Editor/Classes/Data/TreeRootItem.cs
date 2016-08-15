using UnityEngine;

namespace TangzxInternal
{
    class TreeRootItem : TreeItem
    {
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