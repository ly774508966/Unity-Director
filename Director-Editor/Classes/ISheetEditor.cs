using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
    public interface ISheetEditor
    {
        TDEvent selected { get; set; }

        float PixelToTime2(float pixel);

        float TimeToPixel2(float time);

        void DrawVerticalLine(float time, Color color);

        void Repaint();
        
        void OnDragStart(IRowDrawer eventDrawer);

        void OnDragEnd(IRowDrawer eventDrawer);
    }
}
