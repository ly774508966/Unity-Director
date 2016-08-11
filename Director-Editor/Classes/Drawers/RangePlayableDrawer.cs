using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal.Drawers
{
    [CustomPlayableDrawer(typeof(TDRangeEvent))]
    public class RangePlayableDrawer : EventDrawer
    {
        public override void OnGUI(Rect drawRect, Rect totalRect)
        {
            GUIStyle eventStyle = "flow node 3";
            
            HandleDrag(drawRect, totalRect);
            if (Event.current.type == EventType.repaint)
            {
                eventStyle.Draw(drawRect, GUIContent.none, id);
            }
        }
    }
}
