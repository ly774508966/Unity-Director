using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class PlayHeadDrawer : Draggable
    {
        private DirectorWindow directorWindow;
        private EventSheetEditor sheetEditor;
        private GUIStyle stylePlayhead;

        private float timeWhenDragStart;

        public PlayHeadDrawer(DirectorWindow directorWindow, EventSheetEditor sheetEditor)
        {
            this.directorWindow = directorWindow;
            this.sheetEditor = sheetEditor;
        }

        public void OnGUI(Rect rect, float time)
        {
            if (stylePlayhead == null)
            {
                stylePlayhead = "MeTransPlayhead";
            }

            Rect drawArea = rect;
            drawArea.xMin = sheetEditor.TimeToPixel2(time) - stylePlayhead.fixedWidth * 0.5f - 0.5f;
            drawArea.width = stylePlayhead.fixedWidth;
            drawArea.height = stylePlayhead.fixedHeight;

            //画竖线
            float lineYMin = isDragging ? drawArea.yMax : rect.yMin;
            TimeArea.DrawVerticalLine(sheetEditor.TimeToPixel2(time), lineYMin, rect.yMax, Color.red);

            //播放头标记
            if (Event.current.type == EventType.Repaint && isDragging)
                stylePlayhead.Draw(drawArea, GUIContent.none, 0);

            //拖动处理
            Rect dragArea = rect;
            dragArea.height = 15;
            HandleDrag(true, dragArea, 0,
                () =>
                {
                    timeWhenDragStart = directorWindow.playHeadTime;
                    directorWindow.OnDragPlayHeadStart();
                },
                () =>
                {
                    directorWindow.OnDragPlayHeadEnd();
                },
                (float offset) =>
                {
                    directorWindow.SetPlayHead(timeWhenDragStart + sheetEditor.PixelToTime2(offset));
                },
                null);
        }
    }
}