using TangzxInternal.Data;
using UnityEngine;

namespace TangzxInternal
{
    public interface IRowDrawer
    {
        void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect);
    }
}
