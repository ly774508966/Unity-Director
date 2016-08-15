using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
    public interface ISheetEditor
    {
        float PixelToTime2(float pixel);

        float TimeToPixel2(float time);

        void DrawVerticalLine(float time, Color color);

        void Repaint();
        
        void OnDragStart(ISheetRowDrawer eventDrawer);

        void OnDragEnd(ISheetRowDrawer eventDrawer);

        bool IsSelected(DirectorEvent evt);

        void SetSelected(DirectorEvent evt);
    }
}
