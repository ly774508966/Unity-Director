using TangzxInternal.Data;
using UnityEngine;

namespace TangzxInternal
{
    public interface ISheetRowDrawer
    {
        void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect);
    }
}
