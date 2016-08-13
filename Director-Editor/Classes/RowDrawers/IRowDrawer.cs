using TangzxInternal.Data;
using UnityEngine;

namespace TangzxInternal.RowDrawers
{
    public interface IRowDrawer
    {
        void OnGUI(ISheetEditor sheetEditor, Rect rect, VOTreeItem item);
    }
}
